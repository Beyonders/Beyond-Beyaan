using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum PlayerType { HUMAN, CPU }
	public class Empire
	{
		#region Member Variables
		private string empireName;
		private int empireID; //used to create unique sprite name
		//private float reserves;
		//private float expenses;
		private Color empireColor;
		private PlayerType type;
		private StarSystem selectedSystem;
		private StarSystem lastSelectedSystem; //the last system selected by the player (can be the current selected system, used for end of turn processing)
		private int planetSelected;
		private int fleetSelected;
		private FleetGroup selectedFleetGroup;
		private List<Fleet> visibleOtherFleets;
		#endregion

		#region Properties
		public string EmpireName
		{
			get { return empireName; }
		}
		public int EmpireID
		{
			get { return empireID; }
		}

		public Color EmpireColor
		{
			get { return empireColor; }
			set 
			{ 
				empireColor = value;
				ConvertedColor = new[]
				{
					empireColor.R / 255.0f,
					empireColor.G / 255.0f,
					empireColor.B / 255.0f,
					empireColor.A / 255.0f
				};
			}
		}

		public float[] ConvertedColor { get; private set; }

		public PlayerType Type
		{
			get { return type; }
		}

		public StarSystem SelectedSystem
		{
			get { return selectedSystem; }
			set { selectedSystem = value; }
		}

		public StarSystem LastSelectedSystem
		{
			get { return lastSelectedSystem; }
			set { lastSelectedSystem = value; }
		}

		public FleetGroup SelectedFleetGroup
		{
			get { return selectedFleetGroup; }
			set { selectedFleetGroup = value; }
		}

		public int PlanetSelected
		{
			get { return planetSelected; }
			set { planetSelected = value; }
		}

		public int FleetSelected
		{
			get { return fleetSelected; }
			set { fleetSelected = value; }
		}

		public PlanetManager PlanetManager { get; private set; }

		public FleetManager FleetManager { get; private set; }

		public TechnologyManager TechnologyManager { get; private set; }

		public ContactManager ContactManager { get; private set; }

		public SitRepManager SitRepManager { get; private set; }

		public List<StarSystem> SystemsUnderInfluence
		{
			get;
			set;
		}

		public List<Fleet> VisibleFleets
		{
			get { return visibleOtherFleets; }
		}

		public float PlanetTotalProduction
		{
			get
			{
				float planetIncome = 0;
				foreach (Planet planet in PlanetManager.Planets)
				{
					planetIncome += planet.TotalProduction;
				}
				return planetIncome;
			}
		}
		public float TradeIncome
		{
			get;
			private set;
		}
		public float ShipMaintenance
		{
			get { return FleetManager.GetExpenses(); }
		}
		public float ShipMaintenancePercentage
		{
			get
			{
				return ShipMaintenance / NetIncome;
			}
		}
		// TODO: Implement the properties with simple getters/setters
		public float BaseMaintenance
		{
			get; 
			private set;
		}
		public float BaseMaintenancePercentage
		{
			get
			{
				return BaseMaintenance / NetIncome;
			}
		}
		public float EspionageExpense
		{
			get;
			private set;
		}
		public float EspionageExpensePercentage
		{
			get
			{
				return EspionageExpense / NetIncome;
			}
		}
		public float SecurityExpense
		{
			get;
			private set;
		}
		public float SecurityExpensePercentage
		{
			get
			{
				return SecurityExpense / NetIncome;
			}
		}
		public float NetProduction
		{
			get
			{
				float production = 0;
				foreach (var planet in PlanetManager.Planets)
				{
					production += planet.TotalProduction;
				}
				return production;
			}
		}
		public float NetIncome
		{
			get
			{
				return PlanetTotalProduction + TradeIncome;
			}
		}
		public float NetExpenses
		{
			get
			{
				return ShipMaintenance + BaseMaintenance + EspionageExpense + SecurityExpense + TaxExpenses;
			}
		}
		public float ExpensesPercentage
		{
			get
			{
				return NetExpenses / NetIncome;
			}
		}

		public float ResearchPoints { get; private set; }
		public float Reserves { get; set; }
		private int _taxRate;
		public int TaxRate
		{
			get
			{
				return _taxRate;
			}
			set
			{
				_taxRate = value;
				UpdateProduction();
			}
		}
		public float TaxExpenses 
		{ 
			get
			{
				if (TaxRate == 0)
				{
					//a bit of optimization here.
					return 0;
				}
				return NetProduction * TaxRate * 0.01f;
			}
		}

		public Race EmpireRace { get; private set; }

		#endregion

		#region Constructors
		public Empire(string emperorName, int empireID, Race race, PlayerType type, Color color, GameMain gameMain) : this()
		{
			Reserves = 0;
			TaxRate = 0;
			this.empireName = emperorName;
			this.empireID = empireID;
			this.type = type;
			EmpireColor = color;
			try
			{
				TechnologyManager.SetComputerTechs(gameMain.MasterTechnologyManager.GetRandomizedComputerTechs());
				TechnologyManager.SetConstructionTechs(gameMain.MasterTechnologyManager.GetRandomizedConstructionTechs());
				TechnologyManager.SetForceFieldTechs(gameMain.MasterTechnologyManager.GetRandomizedForceFieldTechs());
				TechnologyManager.SetPlanetologyTechs(gameMain.MasterTechnologyManager.GetRandomizedPlanetologyTechs());
				TechnologyManager.SetPropulsionTechs(gameMain.MasterTechnologyManager.GetRandomizedPropulsionTechs());
				TechnologyManager.SetWeaponTechs(gameMain.MasterTechnologyManager.GetRandomizedWeaponTechs());
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

			EmpireRace = race;
		}
		public Empire()
		{
			FleetManager = new FleetManager(this);
			TechnologyManager = new TechnologyManager();
			PlanetManager = new PlanetManager();
			SitRepManager = new SitRepManager();
			visibleOtherFleets = new List<Fleet>();
		}
		#endregion

		#region Functions
		public void SetHomeSystem(StarSystem homeSystem, Planet homePlanet)
		{
			selectedSystem = homeSystem;
			lastSelectedSystem = homeSystem;
			PlanetManager.AddOwnedPlanet(homePlanet);
			FleetManager.SetupStarterFleet(homeSystem);
			homePlanet.ShipBeingBuilt = FleetManager.CurrentDesigns[0];
			homePlanet.SetCleanup();
			homePlanet.UpdateOutputs();
		}

		public void SetUpContacts(List<Empire> allEmpires)
		{
			ContactManager = new ContactManager(this, allEmpires);
		}

		public void ClearTurnData()
		{
			//Clears all current turn's temporary data to avoid issues
			selectedFleetGroup = null;
			selectedSystem = lastSelectedSystem;
		}

		public List<StarSystem> CheckExploredSystems(Galaxy galaxy)
		{
			List<StarSystem> exploredSystems = new List<StarSystem>();
			foreach (Fleet fleet in FleetManager.GetFleets())
			{
				if (fleet.TravelNodes == null || fleet.TravelNodes.Count == 0)
				{
					StarSystem systemExplored = fleet.AdjacentSystem;
					if (systemExplored != null && !systemExplored.IsThisSystemExploredByEmpire(this))
					{
						SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, systemExplored, null, new Point(systemExplored.X, systemExplored.Y), systemExplored.Name + " has been explored."));
						systemExplored.AddEmpireExplored(this);
						exploredSystems.Add(systemExplored);
					}
				}
			}
			return exploredSystems;
		}

		public List<Fleet> CheckColonizableSystems(Galaxy galaxy)
		{
			List<Fleet> colonizingFleets = new List<Fleet>();
			foreach (Fleet fleet in FleetManager.GetFleets())
			{
				if (fleet.TravelNodes == null || fleet.TravelNodes.Count == 0)
				{
					if (fleet.AdjacentSystem.Planets[0].Owner != null)
					{
						continue;
					}
					int colonyReq = fleet.AdjacentSystem.Planets[0].ColonyRequirement;
					foreach (Ship ship in fleet.OrderedShips)
					{
						foreach (var special in ship.Specials)
						{
							if (special != null && special.Technology.Colony >= colonyReq)
							{
								colonizingFleets.Add(fleet);
								break;
							}
						}
					}
				}
			}
			return colonizingFleets;
		}

		public void LaunchTransports()
		{
			foreach (var planet in PlanetManager.Planets)
			{
				if (planet.TransferSystem.Key.StarSystem != null)
				{
					Fleet newFleet = new Fleet();
					newFleet.Empire = this;
					newFleet.GalaxyX = planet.System.X;
					newFleet.GalaxyY = planet.System.Y;
					newFleet.AddTransport(planet.Races[0], planet.TransferSystem.Value);
					newFleet.TravelNodes = new List<TravelNode> {planet.TransferSystem.Key };
					planet.RemoveRacePopulation(planet.Races[0], planet.TransferSystem.Value);
					planet.TransferSystem = new KeyValuePair<TravelNode,int>(new TravelNode(), 0);
					newFleet.ResetMove();
					FleetManager.AddFleet(newFleet);
				}
			}
		}

		public void LandTransports()
		{
			List<Fleet> fleetsToRemove = new List<Fleet>();
			foreach (var fleet in FleetManager.GetFleets())
			{
				if (fleet.TransportShips.Count > 0 && (fleet.TravelNodes == null || fleet.TravelNodes.Count == 0) && fleet.AdjacentSystem != null)
				{
					if (fleet.AdjacentSystem.Planets[0].Owner == this)
					{
						foreach (var transport in fleet.TransportShips)
						{
							fleet.AdjacentSystem.Planets[0].AddRacePopulation(transport.raceOnShip, transport.amount);
						}
						fleetsToRemove.Add(fleet);
					}
				}
			}
			foreach (var fleet in fleetsToRemove)
			{
				FleetManager.RemoveFleet(fleet);
			}
		}

		public void CheckForBuiltShips()
		{
			foreach (Planet planet in PlanetManager.Planets)
			{
				int amount;
				Ship result = planet.CheckIfShipBuilt(out amount);
				if (amount > 0 && result != null)
				{
					Fleet newFleet = new Fleet();
					newFleet.Empire = this;
					newFleet.GalaxyX = planet.System.X;
					newFleet.GalaxyY = planet.System.Y;
					newFleet.AdjacentSystem = planet.System;
					newFleet.AddShips(result, amount);
					FleetManager.AddFleet(newFleet);
					SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, planet.System, planet, new Point(planet.System.X, planet.System.Y), planet.Name + " has produced " + amount + " " + result.Name + " ship" + (amount > 1 ? "s." : ".")));
				}
			}
			FleetManager.MergeIdleFleets();
		}

		public void SetVisibleFleets(List<Fleet> fleets)
		{
			visibleOtherFleets = fleets;
		}

		public void UpdateProduction()
		{
			//Take the total trade income, and add them to the planets, proportionally to their percentage of the total production across empire
			float totalProduction = 0;
			foreach (var planet in PlanetManager.Planets)
			{
				planet.ProductionFromTrade = 0; //Don't add extra production from last turn, set it to 0 here
				totalProduction += planet.TotalProduction;
			}
			float netExpenses = NetExpenses; //NetExpenses property dynamically calculates the value, so cache it here
			//Now that we've added up the total production, distribute the trade so we'd have accurate TotalProduction
			foreach (var planet in PlanetManager.Planets)
			{
				float scale = planet.TotalProduction / totalProduction;
				planet.ProductionFromTrade = scale * TradeIncome;
				planet.ProductionLostFromExpenses = scale * netExpenses;
				planet.SetCleanup();
			}
		}

		public void AccureIncome()
		{
			if (TaxRate > 0)
			{
				Reserves += TaxExpenses / 2;
			}
			foreach (var planet in PlanetManager.Planets)
			{
				Reserves += planet.AmountOfBCGeneratedThisTurn;
			}
		}

		public void UpdateResearchPoints()
		{
			ResearchPoints = 0;
			foreach (Planet planet in PlanetManager.Planets)
			{
				ResearchPoints += planet.ResearchAmount * 0.01f * planet.ActualProduction;
			}
		}

		public void HandleAIEmpire()
		{
		}

		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("Empire");
			writer.WriteAttributeString("ID", empireID.ToString());
			writer.WriteAttributeString("Name", empireName);
			writer.WriteAttributeString("Color", empireColor.ToArgb().ToString());
			writer.WriteAttributeString("Race", EmpireRace.RaceName);
			writer.WriteAttributeString("IsHumanPlayer", type == PlayerType.HUMAN ? "True" : "False");
			writer.WriteAttributeString("SelectedSystem", lastSelectedSystem.ID.ToString());
			TechnologyManager.Save(writer);
			FleetManager.Save(writer);
			//sitRepManager.Save(writer);
			writer.WriteEndElement();
		}
		public void Load(XElement empireToLoad, GameMain gameMain)
		{
			empireID = int.Parse(empireToLoad.Attribute("ID").Value);
			empireName = empireToLoad.Attribute("Name").Value;
			EmpireColor = Color.FromArgb(int.Parse(empireToLoad.Attribute("Color").Value));
			EmpireRace = gameMain.RaceManager.GetRace(empireToLoad.Attribute("Race").Value);
			type = bool.Parse(empireToLoad.Attribute("IsHumanPlayer").Value) ? PlayerType.HUMAN : PlayerType.CPU;
			lastSelectedSystem = gameMain.Galaxy.GetStarWithID(int.Parse(empireToLoad.Attribute("SelectedSystem").Value));
			TechnologyManager.Load(empireToLoad, gameMain.MasterTechnologyManager);
			FleetManager.Load(empireToLoad, this, gameMain);
		}
		#endregion
	}
}
