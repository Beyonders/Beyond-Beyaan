using System;
using System.Collections.Generic;
using System.Drawing;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum PLANET_TYPE { TERRAN = 0, JUNGLE, OCEAN, BADLAND, STEPPE, DESERT, ARCTIC, BARREN, TUNDRA, DEAD, VOLCANIC, TOXIC, RADIATED, NONE }
	public enum OUTPUT_TYPE { RESEARCH, DEFENSE, INFRASTRUCTURE, ENVIRONMENT, CONSTRUCTION }
	public enum PLANET_CONSTRUCTION_BONUS { ULTRAPOOR, POOR, AVERAGE, RICH, ULTRARICH }
	public enum PLANET_ENVIRONMENT_BONUS { HOSTILE, AVERAGE, FERTILE, GAIA }
	public enum PLANET_RESEARCH_BONUS { AVERAGE, ARTIFACTS, BEYAAN }
	public enum PLANET_ASTEROID_DENSITY { NONE, LOW, HIGH }
	public class Planet
	{
		#region Constants

		private const string NONE = "None";
		private const string RADIATED = "Radiated";
		private const string TOXIC = "Toxic";
		private const string VOLCANIC = "Volcanic";
		private const string DEAD = "Dead";
		private const string TUNDRA = "Tundra";
		private const string BARREN = "Barren";
		private const string ARCTIC = "Arctic";
		private const string DESERT = "Desert";
		private const string STEPPE = "Steppe";
		private const string BADLANDS = "Badlands";
		private const string OCEANIC = "Oceanic";
		private const string JUNGLE = "Jungle";
		private const string TERRAN = "Terran";
		#endregion

		#region Member Variables
		private StarSystem _whichSystem;
		private PLANET_TYPE _planetType;
		private string _planetTypeString;
		private Empire _owner;
		string _name;
		private float _factoryInvestments; //Amount of BCs spent to determine if we need to refit or not
		private float _baseInvestments; //Amount of BCs spent to determine if we need to upgrade or not
		private float _shieldProjectRevenues;
		private float _terraformProjectRevenues;
		private float _terraformPop;
		private int _populationMax;
		private Dictionary<Race, float> _racePopulations;
		private List<Race> _races;
		private Ship _shipBeingBuilt;
		#endregion

		#region Properties
		public StarSystem System
		{
			get { return _whichSystem; }
		}
		public PLANET_TYPE PlanetType
		{
			get { return _planetType; }
		}
		public int ColonyRequirement
		{
			get
			{
				switch (_planetType)
				{
					case PLANET_TYPE.TERRAN:
					case PLANET_TYPE.OCEAN:
					case PLANET_TYPE.JUNGLE:
					case PLANET_TYPE.DESERT:
					case PLANET_TYPE.BADLAND:
					case PLANET_TYPE.STEPPE:
					case PLANET_TYPE.ARCTIC:
						return 1;
					case PLANET_TYPE.BARREN:
						return 2;
					case PLANET_TYPE.TUNDRA:
						return 3;
					case PLANET_TYPE.DEAD:
						return 4;
					case PLANET_TYPE.VOLCANIC:
						return 5;
					case PLANET_TYPE.TOXIC:
						return 6;
					case PLANET_TYPE.RADIATED:
						return 7;
					default:
						return int.MaxValue;
				}
			}
		}
		public string PlanetTypeString
		{
			get { return _planetTypeString; }
		}
		public BBSprite SmallSprite { get; private set; }
		public BBSprite GroundSprite { get; private set; }
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		public Empire Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}
		public int OwnerID { get; set; } //Useful for saving/loading purposes, otherwise unused
		public float TotalPopulation
		{
			get 
			{
				float totalPopulation = 0.0f;
				foreach (Race race in _races)
				{
					totalPopulation += _racePopulations[race];
				}
				return totalPopulation;
			}
		}
		public float TotalMaxPopulation
		{
			get
			{
				return _populationMax + _terraformPop;
			}
		}

		public float Waste { get; private set; }

		public float Factories { get; set; }
		public int Bases { get; set; }
		public float NextBaseInvestment { get; private set; }
		public int ShieldLevel { get; set; }
		public float Reserves { get; set; }

		#region Production
		public float TotalProduction
		{
			get 
			{
				float planetProduction = 0;
				planetProduction += TotalPopulation * (((Owner.TechnologyManager.PlanetologyLevel * 3) + 50) / 100.0f);
				if (TotalPopulation >= Factories / Owner.TechnologyManager.RoboticControls)
				{
					planetProduction += Factories;
				}
				else
				{
					planetProduction += TotalPopulation * Owner.TechnologyManager.RoboticControls;
				}
				return planetProduction;
			}
		}
		public float TotalProductionWithTrade
		{
			get
			{
				return TotalProduction + ProductionFromTrade;
			}
		}
		
		public float ProductionFromTrade { get; set; } //This is set externally from EmpireManager class
		public float ProductionLostFromExpenses { get; set; } //This is set externally from EmpireManager class

		public float ActualProduction
		{
			get
			{
				float actualProd = TotalProductionWithTrade - ProductionLostFromExpenses;
				if (Reserves > 0)
				{
					if (Reserves > actualProd) //Cap at double the production
					{
						actualProd *= 2;
					}
					else
					{
						actualProd += Reserves;
					}
				}
				return actualProd;
			}
		}
		#endregion

		public List<Race> Races
		{
			get	{ return _races;	}
		}
		public float PopulationMax
		{
			get { return _populationMax; }
		}
		public Ship ShipBeingBuilt
		{
			get { return _shipBeingBuilt; }
			set { _shipBeingBuilt = value; }
		}
		public int ShipBeingBuiltID { get; set; } //Useful for saving/loading purposes, otherwise unused
		public TravelNode RelocateToSystem { get; set; }
		public int RelocateToSystemID { get; set; } //Useful for saving/loading purposes, otherwise unused
		public KeyValuePair<TravelNode, int> TransferSystem { get; set; }
		public KeyValuePair<int, int> TransferSystemID { get; set; } //Useful for saving/loading purposes, otherwise unused
		public float ShipConstructionLength
		{
			get
			{
				if (ConstructionAmount <= 0)
				{
					return -1;
				}
				float remaining = (_shipBeingBuilt.Cost - ShipConstructionAmount);
				float totalConstruction = ConstructionAmount * 0.01f * ActualProduction;
				if (remaining > totalConstruction)
				{
					return remaining / totalConstruction;
				}
				int amount = 1;
				totalConstruction -= remaining;
				if (totalConstruction > 0)
				{
					amount += (int)(totalConstruction / _shipBeingBuilt.Cost);
				}
				return 1.0f / amount;
			}
		}

		public float ShipConstructionAmount { get; set; }
		public float AmountLostToRefitThisTurn { get; set; }
		public float AmountOfBuildingsThisTurn { get; set; }
		public float AmountOfBCGeneratedThisTurn { get; set; }
		public float AmountOfRPGeneratedThisTurn { get; set; }
		public float AmountOfWasteCleanupNeeded { get; set; }
		public float TerraformProjectInvestment { get; set; }
		public float ExtraPopulationCloned { get; set; }
		public float AmountLostToUpgradeThisTurn { get; set; }
		public float AmountOfBaseInvestmentThisTurn { get; set; }
		public float AmountToInvestInShield { get; set; }

		public PLANET_CONSTRUCTION_BONUS ConstructionBonus { get; private set; }
		public PLANET_ENVIRONMENT_BONUS EnvironmentBonus { get; private set; }
		public PLANET_RESEARCH_BONUS ResearchBonus { get; private set; }
		public PLANET_ASTEROID_DENSITY AsteroidDensity { get; private set; }

		public string InfrastructureStringOutput
		{
			get
			{
				int maxFactories = (int)(TotalMaxPopulation * Owner.TechnologyManager.RoboticControls);
				if (InfrastructureAmount > 0)
				{
					if (AmountOfBuildingsThisTurn > 0)
					{
						if (AmountOfBCGeneratedThisTurn == 0)
						{
							return string.Format("{0:0.0}/{1} (+{2:0.0}) Factories", Factories, maxFactories, AmountOfBuildingsThisTurn);
						}
						return string.Format("{0:0.0}/{1} (+{2:0.0}) Factories (+{3:0.0} BC)", Factories, maxFactories, AmountOfBuildingsThisTurn, AmountOfBCGeneratedThisTurn);
					}
					if (AmountLostToRefitThisTurn > 0)
					{
						return "Refitting Factories";
					}
					return string.Format("{0:0.0}/{1} Factories (+{2:0.0} BC)", Factories, maxFactories, AmountOfBCGeneratedThisTurn);
				}
				return string.Format("{0:0.0}/{1} Factories", Factories, maxFactories);
			}
		}
		public string ResearchStringOutput
		{
			get
			{
				if (ResearchAmount > 0)
				{
					return string.Format("{0:0.0} Research Points", AmountOfRPGeneratedThisTurn);
				}
				return "Not Researching";
			}
		}
		public string DefenseStringOutput
		{
			get
			{
				if (AmountToInvestInShield > 0)
				{
					return "Building Shield";
				}
				if (AmountLostToUpgradeThisTurn > 0 && AmountOfBaseInvestmentThisTurn == 0)
				{
					return "Upgrading Bases";
				}
				string baseString;
				if (AmountOfBaseInvestmentThisTurn > 0)
				{
					float amountInvested = NextBaseInvestment + AmountOfBaseInvestmentThisTurn;
					//TODO: Factor in Nebula
					if (amountInvested > _owner.TechnologyManager.MissileBaseCost)
					{
						baseString = string.Format("{0} (+{1}) Bases", Bases, (int) (amountInvested / _owner.TechnologyManager.MissileBaseCost));
					}
					else
					{
						float turns = (_owner.TechnologyManager.MissileBaseCost - NextBaseInvestment) / AmountOfBaseInvestmentThisTurn;
						if (turns - (int)turns > 0)
						{
							turns += 1;
						}
						baseString = string.Format("{0} (+1 in {1} years) Bases", Bases, (int)turns);
					}
				}
				else
				{
					baseString = string.Format("{0} Bases", Bases);
				}
				switch (ShieldLevel)
				{
					case 5:
						baseString += " (V)";
						break;
					case 10:
						baseString += " (X)";
						break;
					case 15:
						baseString += " (XV)";
						break;
					case 20:
						baseString += " (XX)";
						break;
				}
				return baseString;
			}
		}
		public string EnvironmentStringOutput
		{
			get
			{
				if (AmountOfWasteCleanupNeeded > 0)
				{
					return "Polluting";
				}
				if (TerraformProjectInvestment > 0)
				{
					if (EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.HOSTILE)
					{
						return "Atmospheric Terraforming";
					}
					if (EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.AVERAGE && _owner.TechnologyManager.HasSoilEnrichment)
					{
						return "Soil Enriching";
					}
					if (EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.GAIA && _owner.TechnologyManager.HasAdvancedSoilEnrichment)
					{
						return "Gaia Terraforming";
					}
					return "Terraforming";
				}
				if (ExtraPopulationCloned >= 0.1)
				{
					return string.Format("+{0:0.0} Population", ExtraPopulationCloned);
				}
				return "Clean";
			}
		}
		public string ConstructionStringOutput
		{
			get
			{
				float length = ShipConstructionLength;
				if (length < 0)
				{
					return ShipBeingBuilt.Name + " - No activity";
				}
				if (length <= 1)
				{
					int amount = (int)(1.0f / length);
					if (amount == 1)
					{
						return ShipBeingBuilt.Name + " in 1 year";
					}
					return ShipBeingBuilt.Name + " x " + amount + " in 1 year";
				}
				int turns = (int)length;
				if (length - turns > 0)
				{
					turns++;
				}
				return ShipBeingBuilt.Name + " in " + turns + " years";
			}
		}

		public int ResearchAmount { get; set; } //Usually those should be modified by only this class, but for saving/loading purposes, they can be set directly from other places.
		public int DefenseAmount { get; set; }
		public int InfrastructureAmount { get; set; }
		public int EnvironmentAmount { get; set; }
		public int ConstructionAmount { get; set; }

		public bool InfrastructureLocked { get; set; }
		public bool EnvironmentLocked { get; set; }
		public bool ResearchLocked { get; set; }
		public bool DefenseLocked { get; set; }
		public bool ConstructionLocked { get; set; }
		#endregion

		#region Constructor
		public Planet(string name, string type, int maxPop, Empire owner, StarSystem system, Random r)
		{
			SetValues(name, type, maxPop, system, owner, r);
		}
		public Planet(string name, Random r, StarSystem system)
		{
			_populationMax = r.Next(0, 150) - 25;

			ConstructionBonus = PLANET_CONSTRUCTION_BONUS.AVERAGE;
			EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.AVERAGE;
			ResearchBonus = PLANET_RESEARCH_BONUS.AVERAGE;

			int randomNum = r.Next(100);
			var color = system.Color;

			string type = string.Empty;
			if (color == Color.Red)
			{
				//TODO: Handle nebula generation
				if (randomNum < 5)
				{
					type = NONE;
				}
				else if (randomNum < 10)
				{
					type = RADIATED;
				}
				else if (randomNum < 15)
				{
					type = TOXIC;
				}
				else if (randomNum < 20)
				{
					type = VOLCANIC;
				}
				else if (randomNum < 25)
				{
					type = DEAD;
				}
				else if (randomNum < 30)
				{
					type = TUNDRA;
				}
				else if (randomNum < 35)
				{
					type = BARREN;
				}
				else if (randomNum < 40)
				{
					type = ARCTIC;
				}
				else if (randomNum < 50)
				{
					type = DESERT;
				}
				else if (randomNum < 60)
				{
					type = STEPPE;
				}
				else if (randomNum < 75)
				{
					type = BADLANDS;
				}
				else if (randomNum < 85)
				{
					type = OCEANIC;
				}
				else if (randomNum < 95)
				{
					type = JUNGLE;
				}
				else
				{
					type = TERRAN;
				}
			}
			else if (color == Color.Green)
			{
				//TODO: Handle nebula generation
				if (randomNum < 5)
				{
					type = NONE;
				}
				else if (randomNum < 10)
				{
					type = RADIATED;
				}
				else if (randomNum < 15)
				{
					type = TOXIC;
				}
				else if (randomNum < 20)
				{
					type = VOLCANIC;
				}
				else if (randomNum < 25)
				{
					type = DEAD;
				}
				else if (randomNum < 30)
				{
					type = TUNDRA;
				}
				else if (randomNum < 35)
				{
					type = BARREN;
				}
				else if (randomNum < 40)
				{
					type = ARCTIC;
				}
				else if (randomNum < 45)
				{
					type = DESERT;
				}
				else if (randomNum < 55)
				{
					type = STEPPE;
				}
				else if (randomNum < 65)
				{
					type = BADLANDS;
				}
				else if (randomNum < 75)
				{
					type = OCEANIC;
				}
				else if (randomNum < 85)
				{
					type = JUNGLE;
				}
				else
				{
					type = TERRAN;
				}
			}
			else if (color == Color.Yellow)
			{
				//TODO: Handle nebula generation
				if (randomNum < 5)
				{
					type = VOLCANIC;
				}
				else if (randomNum < 10)
				{
					type = TUNDRA;
				}
				else if (randomNum < 15)
				{
					type = BARREN;
				}
				else if (randomNum < 20)
				{
					type = ARCTIC;
				}
				else if (randomNum < 25)
				{
					type = DESERT;
				}
				else if (randomNum < 30)
				{
					type = STEPPE;
				}
				else if (randomNum < 40)
				{
					type = BADLANDS;
				}
				else if (randomNum < 50)
				{
					type = OCEANIC;
				}
				else if (randomNum < 60)
				{
					type = JUNGLE;
				}
				else
				{
					type = TERRAN;
				}
			}
			else if (color == Color.Blue)
			{
				//TODO: Handle nebula generation
				if (randomNum < 15)
				{
					type = NONE;
				}
				else if (randomNum < 25)
				{
					type = RADIATED;
				}
				else if (randomNum < 35)
				{
					type = TOXIC;
				}
				else if (randomNum < 45)
				{
					type = VOLCANIC;
				}
				else if (randomNum < 55)
				{
					type = DEAD;
				}
				else if (randomNum < 65)
				{
					type = TUNDRA;
				}
				else if (randomNum < 75)
				{
					type = BARREN;
				}
				else if (randomNum < 85)
				{
					type = ARCTIC;
				}
				else if (randomNum < 90)
				{
					type = DESERT;
				}
				else if (randomNum < 95)
				{
					type = STEPPE;
				}
				else
				{
					type = BADLANDS;
				}
			}
			else if (color == Color.White)
			{
				//TODO: Handle nebula generation
				if (randomNum < 10)
				{
					type = NONE;
				}
				else if (randomNum < 20)
				{
					type = RADIATED;
				}
				else if (randomNum < 30)
				{
					type = TOXIC;
				}
				else if (randomNum < 40)
				{
					type = VOLCANIC;
				}
				else if (randomNum < 50)
				{
					type = DEAD;
				}
				else if (randomNum < 60)
				{
					type = TUNDRA;
				}
				else if (randomNum < 70)
				{
					type = BARREN;
				}
				else if (randomNum < 80)
				{
					type = ARCTIC;
				}
				else if (randomNum < 85)
				{
					type = DESERT;
				}
				else if (randomNum < 90)
				{
					type = STEPPE;
				}
				else if (randomNum < 95)
				{
					type = BADLANDS;
				}
				else
				{
					type = OCEANIC;
				}
			}
			else
			{
				//TODO: Handle nebula generation
				if (randomNum < 20)
				{
					type = NONE;
				}
				else if (randomNum < 45)
				{
					type = RADIATED;
				}
				else if (randomNum < 60)
				{
					type = TOXIC;
				}
				else if (randomNum < 75)
				{
					type = VOLCANIC;
				}
				else if (randomNum < 85)
				{
					type = DEAD;
				}
				else if (randomNum < 90)
				{
					type = TUNDRA;
				}
				else if (randomNum < 95)
				{
					type = BARREN;
				}
				else
				{
					type = ARCTIC;
				}
			}

			switch (type)
			{
				case NONE:
					_populationMax = 0;
					break;
				case RADIATED:
				case TOXIC:
				case VOLCANIC:
					_populationMax = r.Next(2, 9) * 5;
					break;
				case DEAD:
				case TUNDRA:
					_populationMax = r.Next(4, 11) * 5;
					break;
				case BARREN:
				case ARCTIC:
					_populationMax = r.Next(6, 11) * 5;
					break;
				case DESERT:
					_populationMax = r.Next(7, 11) * 5;
					break;
				case STEPPE:
					_populationMax = r.Next(9, 13) * 5;
					break;
				case BADLANDS:
					_populationMax = r.Next(11, 15) * 5;
					break;
				case OCEANIC:
					_populationMax = r.Next(13, 17) * 5;
					break;
				case JUNGLE:
					_populationMax = r.Next(15, 19) * 5;
					break;
				case TERRAN:
					_populationMax = r.Next(17, 21) * 5;
					break;
			}

			//Add some variety to the population size
			while (r.Next(100) < 20)
			{
				if (r.Next(100) < 50)
				{
					//decrease the population by 20
					_populationMax -= 20;
					if (_populationMax < 10)
					{
						_populationMax = 10;
					}
				}
				else
				{
					//increase the population by 20
					_populationMax += 20;
					if (_populationMax > 140)
					{
						_populationMax = 140;
					}
				}
			}

			//Now roll for poor/ultra poor if the planet is steppe, badlands, oceanic, jungle, or terran
			if (type == STEPPE || type == BADLANDS || type == OCEANIC || type == JUNGLE || type == TERRAN)
			{
				randomNum = r.Next(1000);
				if (system.Color == Color.Blue || system.Color == Color.White || system.Color == Color.Yellow)
				{
					if (randomNum < 25) //2.5% chance of ultrapoor
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRAPOOR;
					}
					else if (randomNum < 100) //7.5% chance of poor
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.POOR;
					}
				}
				else if (system.Color == Color.Red)
				{
					if (randomNum < 60) //6% chance of ultrapoor
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRAPOOR;
					}
					else if (randomNum < 200) //14% chance of poor
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.POOR;
					}
				}
				else if (system.Color == Color.Green)
				{
					if (randomNum < 135) //13.5% chance of ultrapoor
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRAPOOR;
					}
					else if (randomNum < 300) //16.5% chance of poor
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.POOR;
					}
				}
			}

			//roll for rich/ultrarich only if the planet is not poor already
			if (ConstructionBonus == PLANET_CONSTRUCTION_BONUS.AVERAGE)
			{
				//todo: handle nebula generation
				randomNum = r.Next(10000);
				if (type == RADIATED)
				{
					if (system.Color == Color.Red || system.Color == Color.Yellow || system.Color == Color.Green || system.Color == Color.White)
					{
						if (randomNum < 875)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 3500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else if (system.Color == Color.Blue)
					{
						if (randomNum < 1575)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 4500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else //neutron star
					{
						if (randomNum < 3000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 6000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
				}
				else if (type == TOXIC)
				{
					if (system.Color == Color.Red || system.Color == Color.Yellow || system.Color == Color.Green || system.Color == Color.White)
					{
						if (randomNum < 750)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 3000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else if (system.Color == Color.Blue)
					{
						if (randomNum < 1400)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 4000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else //neutron star
					{
						if (randomNum < 2750)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 5500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
				}
				else if (type == VOLCANIC)
				{
					if (system.Color == Color.Red || system.Color == Color.Yellow || system.Color == Color.Green || system.Color == Color.White)
					{
						if (randomNum < 625)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 2500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else if (system.Color == Color.Blue)
					{
						if (randomNum < 1225)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 3500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else //neutron star
					{
						if (randomNum < 2500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 5000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
				}
				else if (type == DEAD)
				{
					if (system.Color == Color.Red || system.Color == Color.Yellow || system.Color == Color.Green || system.Color == Color.White)
					{
						if (randomNum < 500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 2000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else if (system.Color == Color.Blue)
					{
						if (randomNum < 1050)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 3000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else //neutron star
					{
						if (randomNum < 2250)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 4500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
				}
				else if (type == TUNDRA)
				{
					if (system.Color == Color.Red || system.Color == Color.Yellow || system.Color == Color.Green || system.Color == Color.White)
					{
						if (randomNum < 375)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 1500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else if (system.Color == Color.Blue)
					{
						if (randomNum < 875)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 2500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else //neutron star
					{
						if (randomNum < 2000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 4000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
				}
				else if (type == BARREN)
				{
					if (system.Color == Color.Red || system.Color == Color.Yellow || system.Color == Color.Green || system.Color == Color.White)
					{
						if (randomNum < 250)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 1000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else if (system.Color == Color.Blue)
					{
						if (randomNum < 700)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 2000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else //neutron star
					{
						if (randomNum < 1750)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 3500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
				}
				else if (type == ARCTIC)
				{
					if (system.Color == Color.Red || system.Color == Color.Yellow || system.Color == Color.Green || system.Color == Color.White)
					{
						if (randomNum < 125)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else if (system.Color == Color.Blue)
					{
						if (randomNum < 525)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 1500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
					else //neutron star
					{
						if (randomNum < 1500)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
						}
						else if (randomNum < 3000)
						{
							ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
						}
					}
				}
				else if (type == DESERT && system.Color == Color.Blue)
				{
					if (randomNum < 350)
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
					}
					else if (randomNum < 1000)
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
					}
				}
				else if (type == STEPPE && system.Color == Color.Blue)
				{
					if (randomNum < 175)
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.ULTRARICH;
					}
					else if (randomNum < 500)
					{
						ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
					}
				}
			}

			//Now determine asteroid density
			randomNum = r.Next(100);
			if (randomNum < 25) //Always 25% chance of low density, no matter what type/bonus the planet has
			{
				AsteroidDensity = PLANET_ASTEROID_DENSITY.LOW;
			}
			else
			{
				int extraValue = 0;
				switch (type)
				{
					case RADIATED:
						extraValue = 12;
						break;
					case TOXIC:
						extraValue = 11;
						break;
					case VOLCANIC:
						extraValue = 10;
						break;
					case DEAD:
						extraValue = 9;
						break;
					case TUNDRA:
						extraValue = 8;
						break;
					case BARREN:
						extraValue = 7;
						break;
					case ARCTIC:
						extraValue = 6;
						break;
					case DESERT:
						extraValue = 5;
						break;
					case STEPPE:
						extraValue = 4;
						break;
					case BADLANDS:
						extraValue = 3;
						break;
					case OCEANIC:
						extraValue = 2;
						break;
					case JUNGLE:
						extraValue = 1;
						break;
				}
				if (ConstructionBonus == PLANET_CONSTRUCTION_BONUS.ULTRARICH)
				{
					if (randomNum < 62 + extraValue)
					{
						AsteroidDensity = PLANET_ASTEROID_DENSITY.HIGH;
					}
					else
					{
						AsteroidDensity = PLANET_ASTEROID_DENSITY.NONE;
					}
				}
				else if (ConstructionBonus == PLANET_CONSTRUCTION_BONUS.RICH)
				{
					if (randomNum < 52 + extraValue)
					{
						AsteroidDensity = PLANET_ASTEROID_DENSITY.HIGH;
					}
					else
					{
						AsteroidDensity = PLANET_ASTEROID_DENSITY.NONE;
					}
				}
				else
				{
					if (randomNum < 42 + extraValue)
					{
						AsteroidDensity = PLANET_ASTEROID_DENSITY.HIGH;
					}
					else
					{
						AsteroidDensity = PLANET_ASTEROID_DENSITY.NONE;
					}
				}
			}
			
			//Now roll for fertility
			if (type == DESERT || type == STEPPE || type == BADLANDS || type == OCEANIC || type == JUNGLE || type == TERRAN)
			{
				randomNum = r.Next(100);
				if (randomNum < 9) //original is 1 / 12th, so this is close enough
				{
					EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.FERTILE;
					float value = _populationMax / 5;
					value *= 1.25f;
					_populationMax = ((int) value) * 5;
					if (_populationMax > 140)
					{
						_populationMax = 140;
					}
				}
			}
			else if (type == RADIATED || type == TOXIC || type == VOLCANIC || type == DEAD || type == TUNDRA || type == BARREN)
			{
				EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.HOSTILE;
			}

			//Finally, roll for artifacts
			if (ConstructionBonus == PLANET_CONSTRUCTION_BONUS.AVERAGE)
			{
				randomNum = r.Next(100);
				if (randomNum < 10) //10% chance of having artifacts
				{
					ResearchBonus = PLANET_RESEARCH_BONUS.ARTIFACTS;
				}
			}

			SetValues(name, type, _populationMax, system, null, r);
		}
		private void SetValues(string name, string type, int maxPop, StarSystem system, Empire empire, Random r)
		{
			_whichSystem = system;
			this._name = name;
			_races = new List<Race>();
			_racePopulations = new Dictionary<Race, float>();
			TransferSystem = new KeyValuePair<TravelNode, int>(new TravelNode(), 0);
			TransferSystemID = new KeyValuePair<int, int>();
			RelocateToSystem = null;
			Owner = empire;
			_populationMax = maxPop;

			switch (type)
			{
				case ARCTIC:
					{
						_planetType = PLANET_TYPE.ARCTIC;
						SmallSprite = SpriteManager.GetSprite("ArcticPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("ArcticGround", r);
					} break;
				case BADLANDS:
					{
						_planetType = PLANET_TYPE.BADLAND;
						SmallSprite = SpriteManager.GetSprite("BadlandsPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("BadlandsGround", r);
					} break;
				case BARREN:
					{
						_planetType = PLANET_TYPE.BARREN;
						SmallSprite = SpriteManager.GetSprite("BarrenPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("BarrenGround", r);
					} break;
				case DEAD:
					{
						_planetType = PLANET_TYPE.DEAD;
						SmallSprite = SpriteManager.GetSprite("DeadPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("DeadGround", r);
					} break;
				case DESERT:
					{
						_planetType = PLANET_TYPE.DESERT;
						SmallSprite = SpriteManager.GetSprite("DesertPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("DesertGround", r);
					} break;
				case JUNGLE:
					{
						_planetType = PLANET_TYPE.JUNGLE;
						SmallSprite = SpriteManager.GetSprite("JunglePlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("JungleGround", r);
					} break;
				case NONE:
					{
						_planetType = PLANET_TYPE.NONE;
						SmallSprite = SpriteManager.GetSprite("AsteroidsPlanetSmall", r);
					} break;
				case OCEANIC:
					{
						_planetType = PLANET_TYPE.OCEAN;
						SmallSprite = SpriteManager.GetSprite("OceanicPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("OceanicGround", r);
					} break;
				case RADIATED:
					{
						_planetType = PLANET_TYPE.RADIATED;
						SmallSprite = SpriteManager.GetSprite("RadiatedPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("RadiatedGround", r);
					} break;
				case STEPPE:
					{
						_planetType = PLANET_TYPE.STEPPE;
						SmallSprite = SpriteManager.GetSprite("SteppePlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("SteppeGround", r);
					} break;
				case TERRAN:
					{
						_planetType = PLANET_TYPE.TERRAN;
						SmallSprite = SpriteManager.GetSprite("TerranPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("TerranGround", r);
					} break;
				case TOXIC:
					{
						_planetType = PLANET_TYPE.TOXIC;
						SmallSprite = SpriteManager.GetSprite("ToxicPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("ToxicGround", r);
					} break;
				case TUNDRA:
					{
						_planetType = PLANET_TYPE.TUNDRA;
						SmallSprite = SpriteManager.GetSprite("TundraPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("TundraGround", r);
					} break;
				case VOLCANIC:
					{
						_planetType = PLANET_TYPE.VOLCANIC;
						SmallSprite = SpriteManager.GetSprite("VolcanicPlanetSmall", r);
						GroundSprite = SpriteManager.GetSprite("VolcanicGround", r);
					} break;
			}

			_planetTypeString = Utility.PlanetTypeToString(_planetType);
		}
		#endregion

		public void SetHomeworld(Empire owner, Random r) //Set this planet as homeworld
		{
			_owner = owner;
			_planetType = PLANET_TYPE.TERRAN;
			_planetTypeString = Utility.PlanetTypeToString(_planetType);
			SmallSprite = SpriteManager.GetSprite("TerranPlanetSmall", r);
			_populationMax = 100;
			_races.Add(owner.EmpireRace);
			_racePopulations.Add(owner.EmpireRace, 40);
			Factories = 30;
			_factoryInvestments = 300; //Start with 300 BCs invested in factories
			SetOutputAmount(OUTPUT_TYPE.INFRASTRUCTURE, 100, true);
			ResearchBonus = PLANET_RESEARCH_BONUS.AVERAGE;
			ConstructionBonus = PLANET_CONSTRUCTION_BONUS.AVERAGE;
			EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.AVERAGE;
		}

		public void SetOutputAmount(OUTPUT_TYPE outputType, int amount, bool forceChange)
		{
			if (forceChange)
			{
				//First, try and change it without forcing it
				SetOutputAmount(outputType, amount, false);
				switch (outputType)
				{
					case OUTPUT_TYPE.CONSTRUCTION:
						{
							if (ConstructionAmount == amount)
							{
								//Success
								return;
							}
						} break;
					case OUTPUT_TYPE.DEFENSE:
						{
							if (DefenseAmount == amount)
							{
								//Success
								return;
							}
						}
						break;
					case OUTPUT_TYPE.ENVIRONMENT:
						{
							if (EnvironmentAmount == amount)
							{
								//Success
								return;
							}
						}
						break;
					case OUTPUT_TYPE.INFRASTRUCTURE:
						{
							if (InfrastructureAmount == amount)
							{
								//Success
								return;
							}
						}
						break;
					case OUTPUT_TYPE.RESEARCH:
						{
							if (ResearchAmount == amount)
							{
								//Success
								return;
							}
						}
						break;
				}
			}
			
			int remainingPercentile = 100;
			if (InfrastructureLocked && !forceChange)
			{
				remainingPercentile -= InfrastructureAmount;
			}
			if (EnvironmentLocked && !forceChange)
			{
				remainingPercentile -= EnvironmentAmount;
			}
			if (DefenseLocked && !forceChange)
			{
				remainingPercentile -= DefenseAmount;
			}
			if (ConstructionLocked && !forceChange)
			{
				remainingPercentile -= ConstructionAmount;
			}
			if (ResearchLocked && !forceChange)
			{
				remainingPercentile -= ResearchAmount;
			}

			//if the player set the slider to or beyond the allowed percentile, all other sliders are set to 0
			if (amount >= remainingPercentile)
			{
				//set all sliders to 0, and change amount to remainingPercentile
				if (!InfrastructureLocked)
				{
					InfrastructureAmount = 0;
				}
				if (!EnvironmentLocked)
				{
					EnvironmentAmount = 0;
				}
				if (!DefenseLocked)
				{
					DefenseAmount = 0;
				}
				if (!ResearchLocked)
				{
					ResearchAmount = 0;
				}
				if (!ConstructionLocked)
				{
					ConstructionAmount = 0;
				}
				amount = remainingPercentile;
			}

			//Now scale
			int totalPointsExcludingSelectedType = GetPointsExcludingSelectedTypeAndLockedTypes(outputType);
			switch (outputType)
			{
				case OUTPUT_TYPE.INFRASTRUCTURE:
					{
						InfrastructureAmount = amount;
						remainingPercentile -= InfrastructureAmount;
					} break;
				case OUTPUT_TYPE.ENVIRONMENT:
					{
						EnvironmentAmount = amount;
						remainingPercentile -= EnvironmentAmount;
					} break;
				case OUTPUT_TYPE.DEFENSE:
					{
						DefenseAmount = amount;
						remainingPercentile -= DefenseAmount;
					} break;
				case OUTPUT_TYPE.CONSTRUCTION:
					{
						ConstructionAmount = amount;
						remainingPercentile -= ConstructionAmount;
					} break;
				case OUTPUT_TYPE.RESEARCH:
					{
						ResearchAmount = amount;
						remainingPercentile -= ResearchAmount;
					} break;
			}
			if (remainingPercentile < totalPointsExcludingSelectedType)
			{
				int amountToDeduct = totalPointsExcludingSelectedType - remainingPercentile;
				int prevValue;
				if ((!ResearchLocked || forceChange) && outputType != OUTPUT_TYPE.RESEARCH)
				{
					prevValue = ResearchAmount;
					ResearchAmount -= (ResearchAmount >= amountToDeduct ? amountToDeduct : ResearchAmount);
					amountToDeduct -= (prevValue - ResearchAmount);
				}
				if (amountToDeduct > 0)
				{
					if ((!ConstructionLocked || forceChange) && outputType != OUTPUT_TYPE.CONSTRUCTION)
					{
						prevValue = ConstructionAmount;
						ConstructionAmount -= (ConstructionAmount >= amountToDeduct ? amountToDeduct : ConstructionAmount);
						amountToDeduct -= (prevValue - ConstructionAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if ((!DefenseLocked || forceChange) && outputType != OUTPUT_TYPE.DEFENSE)
					{
						prevValue = DefenseAmount;
						DefenseAmount -= (DefenseAmount >= amountToDeduct ? amountToDeduct : DefenseAmount);
						amountToDeduct -= (prevValue - DefenseAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if ((!InfrastructureLocked || forceChange) && outputType != OUTPUT_TYPE.INFRASTRUCTURE)
					{
						prevValue = InfrastructureAmount;
						InfrastructureAmount -= (InfrastructureAmount >= amountToDeduct ? amountToDeduct : InfrastructureAmount);
						amountToDeduct -= (prevValue - InfrastructureAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if ((!EnvironmentLocked || forceChange) && outputType != OUTPUT_TYPE.ENVIRONMENT)
					{
						prevValue = EnvironmentAmount;
						EnvironmentAmount -= (EnvironmentAmount >= amountToDeduct ? amountToDeduct : EnvironmentAmount);
						amountToDeduct -= (prevValue - EnvironmentAmount);
					}
				}
			}
			if (remainingPercentile > totalPointsExcludingSelectedType) //excess points needed to allocate
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedType;
				if (!InfrastructureLocked && outputType != OUTPUT_TYPE.INFRASTRUCTURE)
				{
					InfrastructureAmount += amountToAdd;
					amountToAdd = 0;
				}
				if (amountToAdd > 0)
				{
					if (!EnvironmentLocked && outputType != OUTPUT_TYPE.ENVIRONMENT)
					{
						EnvironmentAmount += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!DefenseLocked && outputType != OUTPUT_TYPE.DEFENSE)
					{
						DefenseAmount += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ResearchLocked && outputType != OUTPUT_TYPE.RESEARCH)
					{
						ResearchAmount += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ConstructionLocked && outputType != OUTPUT_TYPE.CONSTRUCTION)
					{
						ConstructionAmount += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0) //All other sliders has been locked, allocate the remaining points back to this output
				{
					switch (outputType)
					{
						case OUTPUT_TYPE.INFRASTRUCTURE:
								{
									InfrastructureAmount += amountToAdd;
									break;
								}
						case OUTPUT_TYPE.RESEARCH:
								{
									ResearchAmount += amountToAdd;
									break;
								}
						case OUTPUT_TYPE.ENVIRONMENT:
								{
									EnvironmentAmount += amountToAdd;
									break;
								}
						case OUTPUT_TYPE.DEFENSE:
								{
									DefenseAmount += amountToAdd;
									break;
								}
						case OUTPUT_TYPE.CONSTRUCTION:
								{
									ConstructionAmount += amountToAdd;
									break;
								}
					}
				}
			}

			UpdateOutputs();
		}

		private int GetPointsExcludingSelectedTypeAndLockedTypes(OUTPUT_TYPE type)
		{
			int points = 0;
			if (type != OUTPUT_TYPE.ENVIRONMENT && !EnvironmentLocked)
			{
				points += EnvironmentAmount;
			}
			if (type != OUTPUT_TYPE.RESEARCH && !ResearchLocked)
			{
				points += ResearchAmount;
			}
			if (type != OUTPUT_TYPE.CONSTRUCTION && !ConstructionLocked)
			{
				points += ConstructionAmount;
			}
			if (type != OUTPUT_TYPE.DEFENSE && !DefenseLocked)
			{
				points += DefenseAmount;
			}
			if (type != OUTPUT_TYPE.INFRASTRUCTURE && !InfrastructureLocked)
			{
				points += InfrastructureAmount;
			}
			return points;
		}

		public void SetCleanup()
		{
			float cleanupNeeded = Waste; //Start with any leftover waste from last turn
			float factoriesOperated = TotalPopulation > (Factories / Owner.TechnologyManager.RoboticControls)
										? Factories
										: (TotalPopulation * Owner.TechnologyManager.RoboticControls);
			cleanupNeeded += factoriesOperated * _owner.TechnologyManager.IndustryWasteRate;
			if (cleanupNeeded > _populationMax - 10)
			{
				cleanupNeeded = _populationMax - 10;
			}
			float amountOfProductionUsed = (cleanupNeeded / (ActualProduction * _owner.TechnologyManager.IndustryCleanupPerBC)) * 100;
			int percentage = (int)amountOfProductionUsed + ((amountOfProductionUsed - (int)amountOfProductionUsed) > 0 ? 1 : 0);
			
			//Check if we need to set the minimum environment cleanup
			if (EnvironmentAmount < percentage || 
				(TotalPopulation >= TotalMaxPopulation && 
				((EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.HOSTILE && !_owner.TechnologyManager.HasAtmosphericTerraform) ||
				(EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.AVERAGE && !_owner.TechnologyManager.HasSoilEnrichment) ||
				(EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.GAIA && EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.HOSTILE && !_owner.TechnologyManager.HasAdvancedSoilEnrichment))))
			{
				//Only set it if it's below the desired amount
				SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, percentage, true);
			}
		}

		//Used for end of turn processing
		public void UpdatePlanet()
		{
			bool setInfrastructureToZero = false;
			bool setDefenseToZero = false;
			bool setEnvironmentToZero = false;
			bool setConstructionToZero = false; //Only used when Stargates are built
			//Update ship construction
			if (ConstructionAmount > 0)
			{
				ShipConstructionAmount += ConstructionAmount * 0.01f * ActualProduction;
			}

			Factories += AmountOfBuildingsThisTurn;
			_factoryInvestments += AmountLostToRefitThisTurn;
			_factoryInvestments += AmountOfBuildingsThisTurn * _owner.TechnologyManager.FactoryCost;

			if (Factories >= TotalMaxPopulation * _owner.TechnologyManager.RoboticControls)
			{
				setInfrastructureToZero = true;
				//TODO: Notify player
			}

			Waste = AmountOfWasteCleanupNeeded;
			if (Waste > PopulationMax - 10)
			{
				Waste = PopulationMax - 10;
			}

			if (AmountLostToUpgradeThisTurn > 0)
			{
				_baseInvestments += AmountLostToUpgradeThisTurn;
				AmountLostToUpgradeThisTurn = 0;
			}
			if (AmountToInvestInShield > 0)
			{
				_shieldProjectRevenues += AmountToInvestInShield;
				if (_shieldProjectRevenues >= 500)
				{
					//Upgrade the shield to next level
					ShieldLevel += 5;
					_shieldProjectRevenues -= 500;
					if (ShieldLevel == _owner.TechnologyManager.HighestPlanetaryShield)
					{
						//TODO: Notify player that shield has been built, and invest the remaining BCs into bases, then set military spending to 0
						float amountOfBCs = _shieldProjectRevenues;
						setDefenseToZero = true;
						if (Bases * _owner.TechnologyManager.MissileBaseCost < _baseInvestments)
						{
							float amountNeeded = (Bases * _owner.TechnologyManager.MissileBaseCost) - _baseInvestments;
							if (amountOfBCs < amountNeeded)
							{
								_baseInvestments += amountOfBCs;
							}
							else
							{
								_baseInvestments += amountNeeded;
								AmountOfBaseInvestmentThisTurn += amountOfBCs - amountNeeded;
							}
						}
						else
						{
							//Free to build new bases
							AmountOfBaseInvestmentThisTurn += amountOfBCs;
						}
					}
				}
			}
			if (AmountOfBaseInvestmentThisTurn > 0)
			{
				//Add to investment, and see if we built some bases
				NextBaseInvestment += AmountOfBaseInvestmentThisTurn;
				_baseInvestments += AmountOfBaseInvestmentThisTurn;
				//TODO: Factor in Nebula
				while (NextBaseInvestment >= _owner.TechnologyManager.MissileBaseCost)
				{
					Bases++;
					NextBaseInvestment -= _owner.TechnologyManager.MissileBaseCost;
				}
			}
			
			if (TerraformProjectInvestment > 0)
			{
				if (_owner.TechnologyManager.HasAtmosphericTerraform && (_planetType == PLANET_TYPE.RADIATED ||
					_planetType == PLANET_TYPE.TOXIC || _planetType == PLANET_TYPE.VOLCANIC || _planetType == PLANET_TYPE.DEAD ||
					_planetType == PLANET_TYPE.TUNDRA || _planetType == PLANET_TYPE.BARREN))
				{
					//Invest into changing this to Arctic planet
					_terraformProjectRevenues += TerraformProjectInvestment;
					TerraformProjectInvestment = 0;
					if (_terraformProjectRevenues >= 200) //Converted to Arctic
					{
						_terraformProjectRevenues -= 200;
						//Barren planets get no population bonus.
						if (_planetType == PLANET_TYPE.TUNDRA || _planetType == PLANET_TYPE.DEAD)
						{
							_populationMax += 10;
						}
						else if (_planetType != PLANET_TYPE.BARREN)
						{
							_populationMax += 20;
						}
						_planetType = PLANET_TYPE.ARCTIC;
						EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.AVERAGE;
						TerraformProjectInvestment = _terraformProjectRevenues; //Roll over into next terraforming project
						_terraformProjectRevenues = 0;
						//TODO: Notify player
					}
				}
				if (TerraformProjectInvestment > 0)
				{
					//Handle soil/advanced soil enrichment here
					if ((_owner.TechnologyManager.HasSoilEnrichment || _owner.TechnologyManager.HasAdvancedSoilEnrichment) && !(_planetType == PLANET_TYPE.RADIATED ||
						_planetType == PLANET_TYPE.TOXIC || _planetType == PLANET_TYPE.VOLCANIC || _planetType == PLANET_TYPE.DEAD ||
						_planetType == PLANET_TYPE.TUNDRA || _planetType == PLANET_TYPE.BARREN))
					{
						if (EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.AVERAGE)
						{
							_terraformProjectRevenues += TerraformProjectInvestment;
							TerraformProjectInvestment = 0;
							if (_terraformProjectRevenues >= 150)
							{
								//it is now fertile
								EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.FERTILE;
								_terraformProjectRevenues -= 150;
								_populationMax += (((_populationMax - 5) / 20) + 1) * 5;
								if (!_owner.TechnologyManager.HasAdvancedSoilEnrichment)
								{
									TerraformProjectInvestment = _terraformProjectRevenues; //Stop at fertile, not gaia, and roll over BCs into terraforming
									_terraformProjectRevenues = 0;
									//TODO: Notify player
								}
							}
						}
						if (EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.FERTILE && _owner.TechnologyManager.HasAdvancedSoilEnrichment)
						{
							//Gaia development here
							_terraformProjectRevenues += TerraformProjectInvestment;
							TerraformProjectInvestment = 0;
							if (_terraformProjectRevenues >= 150) //Already deducted 150 for Fertile process from Gaia's 300 cost
							{
								EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.GAIA;
								_terraformProjectRevenues -= 150;
								_populationMax += (((_populationMax - 20) / 25) + 1) * 5;
								TerraformProjectInvestment = _terraformProjectRevenues;
								_terraformProjectRevenues = 0; //No more projects at this point, only terraforming
								//TODO: Notify player
							}
						}
					}
				}
				if (TerraformProjectInvestment > 0)
				{
					//Handle terraforming and pop growth here
					if (_terraformPop < _owner.TechnologyManager.MaxTerraformPop)
					{
						_terraformPop += TerraformProjectInvestment / _owner.TechnologyManager.TerraformCost;
						if (_terraformPop >= _owner.TechnologyManager.MaxTerraformPop)
						{
							float excess = _terraformPop - _owner.TechnologyManager.MaxTerraformPop;
							_terraformPop = _owner.TechnologyManager.MaxTerraformPop;
							TerraformProjectInvestment = excess * _owner.TechnologyManager.TerraformCost;
							//TODO: Notify player
						}
					}
					if (TerraformProjectInvestment > 0)
					{
						//Finally add population
						ExtraPopulationCloned += TerraformProjectInvestment / _owner.TechnologyManager.CloningCost;
						if (TotalPopulation + ExtraPopulationCloned > TotalMaxPopulation)
						{
							ExtraPopulationCloned = TotalMaxPopulation - TotalPopulation;
							//Excess BC are wasted, matching MoO 1's handling
						}
					}
				}
			}
			if (ExtraPopulationCloned > 0)
			{
				float totalPop = TotalPopulation;
				//Sanity check
				if (TotalPopulation + ExtraPopulationCloned > TotalMaxPopulation)
				{
					ExtraPopulationCloned = TotalMaxPopulation - TotalPopulation;
				}
				foreach (var race in _races)
				{
					_racePopulations[race] += ExtraPopulationCloned * (_racePopulations[race] / totalPop);
				}
				if (TotalPopulation >= TotalMaxPopulation)
				{
					setEnvironmentToZero = true;
					//Extra population will die off due to growth formula, this is very minor, in decimal place.
					//TODO: Notify player
				}
			}

			foreach (Race race in _races)
			{
				_racePopulations[race] += CalculateRaceGrowth(race);
			}

			_races.Sort((a, b) => { return (_racePopulations[a].CompareTo(_racePopulations[b])); });

			if (setConstructionToZero)
			{
				SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, 0, true);
			}
			if (setEnvironmentToZero)
			{
				SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, 0, true);
			}
			if (setInfrastructureToZero)
			{
				SetOutputAmount(OUTPUT_TYPE.INFRASTRUCTURE, 0, true);
			}
			if (setDefenseToZero)
			{
				SetOutputAmount(OUTPUT_TYPE.DEFENSE, 0, true);
			}
			//Update the enviroment to at least sufficient to clean up waste
			SetCleanup();
			UpdateOutputs();

			//Deduct reserves if any exists
			if (Reserves > 0)
			{
				Reserves -= (TotalProductionWithTrade - ProductionLostFromExpenses);
				if (Reserves < 0)
				{
					Reserves = 0;
				}
			}
		}

		public Ship CheckIfShipBuilt(out int amount)
		{
			if (ShipConstructionAmount >= _shipBeingBuilt.Cost)
			{
				amount = 0;
				while (ShipConstructionAmount >= _shipBeingBuilt.Cost)
				{
					amount++;
					ShipConstructionAmount -= _shipBeingBuilt.Cost;
				}
				return _shipBeingBuilt;
			}
			amount = 0;
			return null;
		}

		public void UpdateOutputs() //After the Empire class updates the planet's production, this is called to update values
		{
			#region Infrastructure Output

			AmountLostToRefitThisTurn = 0;
			AmountOfBuildingsThisTurn = 0;
			AmountOfBCGeneratedThisTurn = 0;
			if (InfrastructureAmount > 0)
			{
				float amountOfBC = (InfrastructureAmount * 0.01f * ActualProduction);
				if (Factories * _owner.TechnologyManager.FactoryCost > _factoryInvestments)
				{
					//We need to refit
					AmountLostToRefitThisTurn = amountOfBC / _owner.TechnologyManager.FactoryDiscount; //adjust the BC spending to match factory cost, i.e. if 5 bc per factory, it is now 10 bc with 5 actual bcs
					if (_factoryInvestments + AmountLostToRefitThisTurn >= Factories * _owner.TechnologyManager.FactoryCost)
					{
						//More BC than needed to refit
						AmountLostToRefitThisTurn -= (_factoryInvestments + AmountLostToRefitThisTurn) - (Factories * _owner.TechnologyManager.FactoryCost);
					}
					amountOfBC -= (AmountLostToRefitThisTurn * _owner.TechnologyManager.FactoryDiscount);
				}
				if (amountOfBC > 0)
				{
					if (Factories >= TotalMaxPopulation * _owner.TechnologyManager.RoboticControls)
					{
						AmountOfBuildingsThisTurn = 0; //Already reached the max
						AmountOfBCGeneratedThisTurn = amountOfBC * 0.5f; //Lose half to corruption
					}
					else
					{
						float amountRemaining = (TotalMaxPopulation * _owner.TechnologyManager.RoboticControls) - Factories;
						float adjustedBC = amountOfBC / _owner.TechnologyManager.FactoryDiscount;
						AmountOfBuildingsThisTurn = adjustedBC / _owner.TechnologyManager.FactoryCost;
						if (AmountOfBuildingsThisTurn > amountRemaining) //Will put some into reserve
						{
							AmountOfBCGeneratedThisTurn = (AmountOfBuildingsThisTurn - amountRemaining) * 5;
							AmountOfBuildingsThisTurn = amountRemaining;
						}
					}
				}
			}
			else
			{
				//No output at all in this field
				AmountOfBuildingsThisTurn = 0;
				AmountOfBCGeneratedThisTurn = 0;
			}
			#endregion

			#region Defense Output

			AmountToInvestInShield = 0;
			AmountOfBaseInvestmentThisTurn = 0;
			AmountLostToUpgradeThisTurn = 0;
			if (DefenseAmount > 0)
			{
				//First check to see if there's available shield projects to invest in first
				if (ShieldLevel < _owner.TechnologyManager.HighestPlanetaryShield)
				{
					AmountToInvestInShield = DefenseAmount * 0.01f * ActualProduction;
				}
				else
				{
					//Building bases, but first, do we need to upgrade?
					//TODO: Factor in nebula
					float amountOfBCs = DefenseAmount * 0.01f * ActualProduction;
					if (Bases * _owner.TechnologyManager.MissileBaseCost > _baseInvestments)
					{
						float amountNeeded = (Bases * _owner.TechnologyManager.MissileBaseCost) - _baseInvestments;
						if (amountOfBCs < amountNeeded)
						{
							AmountLostToUpgradeThisTurn = amountOfBCs;
						}
						else
						{
							AmountLostToUpgradeThisTurn = amountNeeded;
							AmountOfBaseInvestmentThisTurn = amountOfBCs - amountNeeded;
						}
					}
					else
					{
						//Free to build new bases
						AmountOfBaseInvestmentThisTurn = amountOfBCs;
					}
				}
			}
			#endregion

			#region Environment Output

			AmountOfWasteCleanupNeeded = Waste; //Start with any leftover waste from last turn
			float factoriesOperated = TotalPopulation > (Factories / Owner.TechnologyManager.RoboticControls)
										? Factories
										: (TotalPopulation * Owner.TechnologyManager.RoboticControls);
			AmountOfWasteCleanupNeeded += factoriesOperated * _owner.TechnologyManager.IndustryWasteRate;
			if (AmountOfWasteCleanupNeeded > _populationMax - 10)
			{
				AmountOfWasteCleanupNeeded = _populationMax - 10; //cap at base pop - 10
			}
			TerraformProjectInvestment = 0;
			ExtraPopulationCloned = 0;
			if (EnvironmentAmount > 0)
			{
				float amountOfBC = EnvironmentAmount * 0.01f * ActualProduction;
				float wasteBCCleanup = AmountOfWasteCleanupNeeded / Owner.TechnologyManager.IndustryCleanupPerBC;

				if (amountOfBC < wasteBCCleanup) //Polluting a bit
				{
					AmountOfWasteCleanupNeeded -= amountOfBC * Owner.TechnologyManager.IndustryCleanupPerBC;
				}
				else
				{
					AmountOfWasteCleanupNeeded = 0;
					amountOfBC -= wasteBCCleanup; //Use remaining funds for terraforming/pop growth
					if (amountOfBC > 0)
					{
						if (_owner.TechnologyManager.HasAtmosphericTerraform && EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.HOSTILE ||
							_owner.TechnologyManager.HasSoilEnrichment && EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.AVERAGE ||
							_owner.TechnologyManager.HasAdvancedSoilEnrichment && EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.GAIA ||
							_terraformPop < _owner.TechnologyManager.MaxTerraformPop)
						{
							//Invest into improving the planet
							TerraformProjectInvestment += amountOfBC;
						}
						else
						{
							//Grow some more people
							ExtraPopulationCloned = amountOfBC / _owner.TechnologyManager.CloningCost;
							if (TotalMaxPopulation < ExtraPopulationCloned + TotalPopulation)
							{
								ExtraPopulationCloned = TotalMaxPopulation - (ExtraPopulationCloned + TotalMaxPopulation);
							}
						}
					}
				}
			}
			#endregion

			if (ResearchAmount > 0)
			{
				AmountOfRPGeneratedThisTurn = ResearchAmount * 0.01f * ActualProduction;
			}
		}

		private float CalculateTotalPopGrowth()
		{
			//Calculate normal population growth using formula (rate of growth * population) * (1 - (population / planet's capacity))
			float popGrowth = 0;
			foreach (Race race in _races)
			{
				popGrowth += CalculateRaceGrowth(race);
			}

			return popGrowth;
		}

		private float CalculateRaceGrowth(Race race)
		{
			float pop = _racePopulations[race];
			pop = pop - (pop * ((Waste / _populationMax) * 0.1f));
			pop = pop + (_racePopulations[race] * ((1 - (pop / TotalMaxPopulation)) * 0.1f));
			return (pop - _racePopulations[race]);
		}

		public float GetRacePopulation(Race whichRace)
		{
			return _racePopulations[whichRace];
		}

		public void AddRacePopulation(Race whichRace, float amount)
		{
			if (_racePopulations.ContainsKey(whichRace))
			{
				_racePopulations[whichRace] += amount;
			}
			else
			{
				//Add it
				if (!_races.Contains(whichRace))
				{
					_races.Add(whichRace);
				}
				_racePopulations[whichRace] = amount;
			}
		}

		public void RemoveRacePopulation(Race whichRace, float amount)
		{
			_racePopulations[whichRace] -= amount;
			if (_racePopulations[whichRace] <= 0)
			{
				//They died out on this planet
				RemoveRace(whichRace);
			}
		}

		public void RemoveRace(Race whichRace)
		{
			_racePopulations.Remove(whichRace);
			_races.Remove(whichRace);
		}

		public void Colonize(Empire whichEmpire)
		{
			_owner = whichEmpire;
			whichEmpire.PlanetManager.AddOwnedPlanet(this);
			_racePopulations = new Dictionary<Race, float>();
			_racePopulations.Add(whichEmpire.EmpireRace, 2);
			_races.Add(whichEmpire.EmpireRace);
			SetOutputAmount(OUTPUT_TYPE.INFRASTRUCTURE, 100, true);
			_shipBeingBuilt = whichEmpire.FleetManager.CurrentDesigns[0];
			System.UpdateOwners();
		}
	}
}
