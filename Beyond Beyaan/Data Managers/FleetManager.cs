using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Beyond_Beyaan
{
	public class FleetManager
	{
		#region Variables

		private List<Fleet> _fleets;
		private Empire _empire;
		private int _currentShipDesignID;
		#endregion

		#region Properties
		public Ship LastShipDesign { get; set; }

		public List<Ship> CurrentDesigns { get; private set; }

		//public List<Ship> ObsoleteDesigns { get; private set; }

		#endregion

		public FleetManager(Empire empire)
		{
			this._empire = empire;
			_currentShipDesignID = 0;
			_fleets = new List<Fleet>();
			CurrentDesigns = new List<Ship>();
			//ObsoleteDesigns = new List<Ship>();
		}

		public void SetupStarterFleet(StarSystem homeSystem)
		{
			Technology retroEngine = null;
			Technology titaniumArmor = null;
			Technology laser = null;
			Technology nuclearMissile = null;
			Technology nuclearBomb = null;

			foreach (var tech in _empire.TechnologyManager.ResearchedPropulsionTechs)
			{
				if (tech.Speed == 1)
				{
					retroEngine = tech;
					break;
				}
			}
			foreach (var tech in _empire.TechnologyManager.ResearchedConstructionTechs)
			{
				if (tech.Armor == Technology.TITANIUM_ARMOR)
				{
					titaniumArmor = tech;
					break;
				}
			}
			foreach (var tech in _empire.TechnologyManager.ResearchedWeaponTechs)
			{
				if (tech.TechName == "Laser")
				{
					laser = tech;
				}
				else if (tech.TechName == "Nuclear Missile")
				{
					nuclearMissile = tech;
				}
				else if (tech.TechName == "Nuclear Bomb")
				{
					nuclearBomb = tech;
				}
			}
			Ship scout = new Ship();
			scout.Name = "Scout";
			scout.Owner = _empire;
			scout.Size = Ship.SMALL;
			scout.WhichStyle = 0;
			scout.Armor = new Equipment(titaniumArmor, false);
			foreach (var tech in _empire.TechnologyManager.ResearchedConstructionTechs)
			{
				if (tech.ReserveFuelTanks)
				{
					scout.Specials[0] = new Equipment(tech, false);
					break;
				}
			}
			scout.Engine = new KeyValuePair<Equipment, float>(new Equipment(retroEngine, false), scout.PowerUsed / (retroEngine.Speed * 10));
			scout.DesignID = _currentShipDesignID;
			CurrentDesigns.Add(scout);
			_currentShipDesignID++;

			Ship fighter = new Ship();
			fighter.Name = "Fighter";
			fighter.Owner = _empire;
			fighter.Size = Ship.SMALL;
			fighter.WhichStyle = 1;
			fighter.Weapons[0] = new KeyValuePair<Equipment, int>(new Equipment(laser, false), 1);
			fighter.Armor = new Equipment(titaniumArmor, false);
			fighter.Engine = new KeyValuePair<Equipment, float>(new Equipment(retroEngine, false), fighter.PowerUsed / (retroEngine.Speed * 10));
			CurrentDesigns.Add(fighter);
			_currentShipDesignID++;

			Ship destroyer = new Ship();
			destroyer.Name = "Destroyer";
			destroyer.Owner = _empire;
			destroyer.Size = Ship.MEDIUM;
			destroyer.WhichStyle = 0;
			destroyer.Weapons[0] = new KeyValuePair<Equipment, int>(new Equipment(nuclearMissile, false), 1);
			destroyer.Weapons[1] = new KeyValuePair<Equipment, int>(new Equipment(laser, false), 3);
			destroyer.Armor = new Equipment(titaniumArmor, false);
			destroyer.Engine = new KeyValuePair<Equipment, float>(new Equipment(retroEngine, false), destroyer.PowerUsed / (retroEngine.Speed * 10));
			CurrentDesigns.Add(destroyer);
			_currentShipDesignID++;

			Ship bomber = new Ship();
			bomber.Name = "Bomber";
			bomber.Owner = _empire;
			bomber.Size = Ship.MEDIUM;
			bomber.WhichStyle = 1;
			bomber.Weapons[0] = new KeyValuePair<Equipment, int>(new Equipment(nuclearBomb, false), 2);
			bomber.Weapons[1] = new KeyValuePair<Equipment, int>(new Equipment(laser, false), 2);
			bomber.Armor = new Equipment(titaniumArmor, false);
			bomber.Engine = new KeyValuePair<Equipment, float>(new Equipment(retroEngine, false), bomber.PowerUsed / (retroEngine.Speed * 10));
			CurrentDesigns.Add(bomber);
			_currentShipDesignID++;

			Ship colonyShip = new Ship();
			colonyShip.Name = "Colony Ship";
			colonyShip.Owner = _empire;
			colonyShip.Size = Ship.LARGE;
			colonyShip.WhichStyle = 0;
			colonyShip.Armor = new Equipment(titaniumArmor, false);
			foreach (var tech in _empire.TechnologyManager.ResearchedPlanetologyTechs)
			{
				if (tech.Colony == Technology.STANDARD_COLONY)
				{
					colonyShip.Specials[0] = new Equipment(tech, false);
					break;
				}
			}
			colonyShip.Engine = new KeyValuePair<Equipment, float>(new Equipment(retroEngine, false), colonyShip.PowerUsed / (retroEngine.Speed * 10));
			colonyShip.DesignID = _currentShipDesignID;
			CurrentDesigns.Add(colonyShip);
			_currentShipDesignID++;

			LastShipDesign = new Ship(scout); //Make a copy so we don't accidentally modify the original ship

			Fleet starterFleet = new Fleet();
			starterFleet.GalaxyX = homeSystem.X;
			starterFleet.GalaxyY = homeSystem.Y;
			starterFleet.AdjacentSystem = homeSystem;
			starterFleet.Empire = _empire;
			starterFleet.AddShips(CurrentDesigns[0], 2);
			starterFleet.AddShips(CurrentDesigns[4], 1);
			_fleets.Add(starterFleet);
		}

		public List<Fleet> GetFleets()
		{
			return _fleets;
		}

		public Fleet[] ReturnFleetAtPoint(int x, int y)
		{
			List<Fleet> listOfFleets = new List<Fleet>();
			foreach (Fleet fleet in _fleets)
			{
				if (fleet.AdjacentSystem != null)
				{
					if (fleet.TravelNodes != null && fleet.TravelNodes.Count > 0)
					{
						if (x >= fleet.AdjacentSystem.X - 48 && x < fleet.AdjacentSystem.X - 16 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
						{
							listOfFleets.Add(fleet);
						}
					}
					else
					{
						if (x >= fleet.AdjacentSystem.X + 16 && x < fleet.AdjacentSystem.X + 48 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
						{
							listOfFleets.Add(fleet);
						}
					}
				}
				else
				{
					if (x >= fleet.GalaxyX - 16 && x < fleet.GalaxyX + 16 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
					{
						listOfFleets.Add(fleet);
					}
				}
			}
			return listOfFleets.ToArray();
		}

		public void AddFleet(Fleet fleet)
		{
			_fleets.Add(fleet);
		}

		public void RemoveFleet(Fleet fleet)
		{
			_fleets.Remove(fleet);
		}

		public void AddShipDesign(Ship newShipDesign)
		{
			Ship ship = new Ship(newShipDesign); //Make a copy so it don't get modified by accident
			ship.DesignID = _currentShipDesignID;
			_currentShipDesignID++;
			CurrentDesigns.Add(ship);
		}

		public float ObsoleteShipDesign(Ship shipToObsolete)
		{
			float totalScrappedValue = 0;
			foreach (var fleet in _fleets)
			{
				if (fleet.Ships.ContainsKey(shipToObsolete))
				{
					totalScrappedValue += fleet.Ships[shipToObsolete] * shipToObsolete.Cost * 0.25f;
					fleet.SubtractShips(shipToObsolete, -1);
				}
			}
			ClearEmptyFleets();
			CurrentDesigns.Remove(shipToObsolete);
			//ObsoleteDesigns.Add(shipToObsolete);
			return totalScrappedValue;
		}

		public Ship GetShipWithDesignID(int designID)
		{
			//Iterates through both current and obsolete designs
			foreach (var ship in CurrentDesigns)
			{
				if (ship.DesignID == designID)
				{
					return ship;
				}
			}
			/*foreach (var ship in ObsoleteDesigns)
			{
				if (ship.DesignID == designID)
				{
					return ship;
				}
			}*/
			throw new Exception("Ship Design not found: " + designID);
		}

		public Ship GetNextShipDesign(Ship previousDesign) //For iterating through ship design when clicking on construction button in planet UI
		{
			int iter = CurrentDesigns.IndexOf(previousDesign) + 1; //Even if not found (which results in -1), it will be the next ship
			if (iter >= CurrentDesigns.Count)
			{
				iter = 0; //Start over from beginning
			}
			return CurrentDesigns[iter];
		}

		public int GetShipCount(Ship design)
		{
			int amount = 0;
			//Gets the count of ships with this design in the empire
			foreach (var fleet in _fleets)
			{
				if (fleet.Ships.ContainsKey(design))
				{
					amount += fleet.Ships[design];
				}
			}
			return amount;
		}

		public bool MoveFleets(float frameDeltaTime)
		{
			bool stillHaveMovement = false;
			//This is called during end of turn processing
			foreach (Fleet fleet in _fleets)
			{
				if (fleet.Move(frameDeltaTime))
				{
					stillHaveMovement = true;
				}
				else
				{
					//This refreshes the ETA text
					fleet.TravelNodes = fleet.TravelNodes;
				}
			}
			return stillHaveMovement;
		}

		public void ResetFleetMovements()
		{
			foreach (Fleet fleet in _fleets)
			{
				fleet.ResetMove();
			}
		}

		public void MergeIdleFleets()
		{
			for (int i = 0; i < _fleets.Count; i++)
			{
				if (_fleets[i].TravelNodes == null)
				{
					List<Fleet> fleetsToRemove = new List<Fleet>();
					for (int j = i + 1; j < _fleets.Count; j++)
					{
						if (_fleets[j].TravelNodes == null && _fleets[j].AdjacentSystem == _fleets[i].AdjacentSystem)
						{
							//Merge only fleets of the same type (i.e. ships with ships, transports with transports
							if (_fleets[j].Ships.Count > 0 && _fleets[i].Ships.Count > 0)
							{
								foreach (KeyValuePair<Ship, int> ship in _fleets[j].Ships)
								{
									_fleets[i].AddShips(ship.Key, ship.Value);
								}
								fleetsToRemove.Add(_fleets[j]);
							}
							else if (_fleets[j].TransportShips.Count > 0 && _fleets[i].TransportShips.Count > 0)
							{
								foreach (TransportShip ship in _fleets[j].TransportShips)
								{
									_fleets[i].AddTransport(ship.raceOnShip, ship.amount);
								}
								fleetsToRemove.Add(_fleets[j]);
							}
						}
					}
					foreach (Fleet fleet in fleetsToRemove)
					{
						_fleets.Remove(fleet);
					}
				}
			}
		}
		public void ClearEmptyFleets()
		{
			//Clear out any empty fleets left after colonizing, space combat, etc
			List<Fleet> fleetsToRemove = new List<Fleet>();
			for (int i = 0; i < _fleets.Count; i++)
			{
				if (_fleets[i].Ships.Count == 0 && _fleets[i].TransportShips.Count == 0)
				{
					fleetsToRemove.Add(_fleets[i]);
				}
			}
			foreach (var fleet in fleetsToRemove)
			{
				_fleets.Remove(fleet);
			}
		}

		public float GetExpenses()
		{
			float amount = 0;
			foreach (Fleet fleet in _fleets)
			{
				amount += fleet.GetExpenses();
			}
			return amount;
		}

		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("CurrentShipDesigns");
			foreach (var ship in CurrentDesigns)
			{
				ship.Save(writer);
			}
			writer.WriteEndElement();
			/*writer.WriteStartElement("ObsoleteShipDesigns");
			foreach (var ship in ObsoleteDesigns)
			{
				ship.Save(writer);
			}
			writer.WriteEndElement();*/
			writer.WriteStartElement("Fleets");
			foreach (var fleet in _fleets)
			{
				fleet.Save(writer);
			}
			writer.WriteEndElement();
		}

		public void Load(XElement empireDoc, Empire empire, GameMain gameMain)
		{
			var currentDesigns = empireDoc.Element("CurrentShipDesigns");
			foreach (var currentDesign in currentDesigns.Elements())
			{
				var currentShip = new Ship();
				currentShip.Load(currentDesign, gameMain);
				currentShip.Owner = empire;
				CurrentDesigns.Add(currentShip);
			}
			/*var obsoleteDesigns = empireDoc.Element("ObsoleteShipDesigns");
			foreach (var obsoleteDesign in obsoleteDesigns.Elements())
			{
				var obsoleteShip = new Ship();
				obsoleteShip.Load(obsoleteDesign, gameMain);
				obsoleteShip.Owner = empire;
				ObsoleteDesigns.Add(obsoleteShip);
			}*/
			var fleets = empireDoc.Element("Fleets");
			foreach (var fleet in fleets.Elements())
			{
				var newFleet = new Fleet();
				newFleet.Load(fleet, this, empire, gameMain);
				_fleets.Add(newFleet);
			}
		}
	}
}
