using System;
using System.Collections.Generic;

namespace Beyond_Beyaan
{
	public class Technology
	{
		#region Constants
		public const int DEEP_SPACE_SCANNER = 1;
		public const int IMPROVED_SPACE_SCANNER = 2;
		public const int ADVANCED_SPACE_SCANNER = 3;

		public const int TITANIUM_ARMOR = 1;
		public const int DURALLOY_ARMOR = 2;
		public const int ZORTRIUM_ARMOR = 3;
		public const int ANDRIUM_ARMOR = 4;
		public const int TRITANIUM_ARMOR = 5;
		public const int ADAMANTIUM_ARMOR = 6;
		public const int NEUTRONIUM_ARMOR = 7;

		public const int BATTLE_SUITS = 1;
		public const int ARMORED_EXOSKELETON = 2;
		public const int POWERED_ARMOR = 3;

		public const int PERSONAL_DEFLECTOR = 1;
		public const int PERSONAL_ABSORPTION = 2;
		public const int PERSONAL_BARRIER = 3;

		public const int AUTOMATED_REPAIR = 1;
		public const int ADVANCED_REPAIR = 2;

		public const int PLANETARY_V_SHIELD = 5;
		public const int PLANETARY_X_SHIELD = 10;
		public const int PLANETARY_XV_SHIELD = 15;
		public const int PLANETARY_XX_SHIELD = 20;

		public const int ZYRO_SHIELD = 1;
		public const int LIGHTNING_SHIELD = 2;

		public const int STANDARD_COLONY = 1;
		public const int BARREN_COLONY = 2;
		public const int TUNDRA_COLONY = 3;
		public const int DEAD_COLONY = 4;
		public const int VOLCANIC_COLONY = 5;
		public const int TOXIC_COLONY = 6;
		public const int RADIATED_COLONY = 7;

		public const int DEATH_SPORES = 1;
		public const int DOOM_VIRUS = 2;
		public const int BIO_TERMINATOR = 3;

		public const int BIO_TOXIN_ANTIDOTE = 1;
		public const int UNIVERSAL_ANTIDOTE = 2;

		public const int SOIL_ENRICHMENT = 1;
		public const int ADV_SOIL_ENRICHMENT = 2;
		public const int ATMOSPHERIC_TERRAFORMING = 3;

		public const int BEAM_WEAPON = 1;
		public const int BOMB_WEAPON = 2;
		public const int BIOLOGICAL_WEAPON = 3;
		public const int MISSILE_WEAPON = 4;
		public const int TORPEDO_WEAPON = 5;
		#endregion

		public int TechLevel { get; private set; }
		public TechField TechField { get; private set; }
		public string TechName { get; private set; }
		public string TechSecondaryName { get; private set; } //Used for technologies that are either armor or weapons with heavy weapon variant
		public string TechDescription { get; private set; }
		public int ResearchPoints
		{
			get { return TechLevel * TechLevel; }
		}

		public int RoboticControl { get; private set; }
		public int BattleComputer { get; private set; }
		public int ECM { get; private set; }
		public int SpaceScanner { get; private set; }
		public int Armor { get; private set; }
		public int IndustrialTech { get; private set; }
		public int IndustrialWaste { get; private set; }
		public int GroundArmor { get; private set; }
		public int Repair { get; private set; }
		public int Shield { get; private set; }
		public int PersonalShield { get; private set; }
		public int PlanetaryShield { get; private set; }
		public int MissileShield { get; private set; }
		public int EcoCleanup { get; private set; }
		public int Terraforming { get; private set; }
		public int TerraformCost { get; private set; }
		public int Colony { get; private set; }
		public int Cloning { get; private set; }
		public int BioWeapon { get; private set; }
		public int BioAntidote { get; private set; }
		public int Enrichment { get; private set; }
		public int Speed { get; private set; }
		public int ManeuverSpeed { get; private set; }
		public int FuelRange { get; private set; }

		public bool ReserveFuelTanks { get; private set; }
		public bool BattleScanner { get; private set; }
		public bool HyperSpaceCommunicator { get; private set; }
		public bool OracleInterface { get; private set; }
		public bool TechnologyNullifier { get; private set; }
		public bool RepulsorBeam { get; private set; }
		public bool CloakingDevice { get; private set; }
		public bool StatisField { get; private set; }
		public bool BlackHoleGenerator { get; private set; }
		public bool InertialStabilizer { get; private set; }
		public bool EnergyPulsar { get; private set; }
		public bool WarpDissipator { get; private set; }
		public bool HighEnergyFocus { get; private set; }
		public bool Stargate { get; private set; }
		public bool SubspaceTeleporter { get; private set; }
		public bool IonicPulsar { get; private set; }
		public bool SubspaceInterdictor { get; private set; }
		public bool CombatTransporters { get; private set; }
		public bool InertialNullifier { get; private set; }
		public bool DisplacementDevice { get; private set; }
		public bool AntiMissileRockets { get; private set; }
		public bool IonStreamProjector { get; private set; }
		public bool NeutronStreamProjector { get; private set; }

		//Space/Cost/Power requirements for ship.  Generic means ship's size don't matter
		public float SmallSize { get; private set; }
		public float SmallCost { get; private set; }
		public float SmallPower { get; private set; }
		public float SmallHP { get; private set; } //Armor points for ship

		public float SmallSecondarySize { get; private set; } //Used for double hulled armor
		public float SmallSecondaryCost { get; private set; }
		public float SmallSecondaryPower { get; private set; }
		public float SmallSecondaryHP { get; private set; }

		public float MediumSize { get; private set; }
		public float MediumCost { get; private set; }
		public float MediumPower { get; private set; }
		public float MediumHP { get; private set; }

		public float MediumSecondarySize { get; private set; }
		public float MediumSecondaryCost { get; private set; }
		public float MediumSecondaryPower { get; private set; }
		public float MediumSecondaryHP { get; private set; }

		public float LargeSize { get; private set; }
		public float LargeCost { get; private set; }
		public float LargePower { get; private set; }
		public float LargeHP { get; private set; }

		public float LargeSecondarySize { get; private set; }
		public float LargeSecondaryCost { get; private set; }
		public float LargeSecondaryPower { get; private set; }
		public float LargeSecondaryHP { get; private set; }

		public float HugeSize { get; private set; }
		public float HugeCost { get; private set; }
		public float HugePower { get; private set; }
		public float HugeHP { get; private set; }

		public float HugeSecondarySize { get; private set; }
		public float HugeSecondaryCost { get; private set; }
		public float HugeSecondaryPower { get; private set; }
		public float HugeSecondaryHP { get; private set; }

		public float GenericSize { get; private set; }
		public float GenericCost { get; private set; }
		public float GenericPower { get; private set; }

		public float GenericSecondarySize { get; private set; } //Used for heavy weapons
		public float GenericSecondaryCost { get; private set; }
		public float GenericSecondaryPower { get; private set; }

		public int WeaponType { get; private set; }
		public int MinimumWeaponDamage { get; private set; }
		public int MinimumSecondaryWeaponDamage { get; private set; }
		public int MaximumWeaponDamage { get; private set; }
		public int MaximumSecondaryWeaponDamage { get; private set; }
		public bool ShieldPiercing { get; private set; }
		public float WeaponRange { get; private set; }
		public int SecondaryWeaponRange { get; private set; }
		public int NumberOfShots { get; private set; }
		public bool Streaming { get; private set; }
		public int TargetingBonus { get; private set; }
		public bool Enveloping { get; private set; }
		public bool Dissipating { get; private set; } //Used only for plasma torpedoes
		public float MissileSpeed { get; private set; }

		public Technology(TechField techField, string name, string desc, int level,
						string secondaryName = "",
						//Optional arguments goes here
						int roboticControl = 0,
						int battleComputer = 0,
						bool battleScanner = false,
						int ECM = 0,
						int spaceScanner = 0,
						bool hyperSpaceCommunicator = false,
						bool oracleInterface = false,
						bool technologyNullifier = false,
						int armor = 0,
						bool reserveFuelTanks = false,
						int industrialTech = 10,
						int industrialWaste = 100,
						int groundArmor = 0,
						int repair = 0,
						int shield = 0,
						int personalShield = 0,
						int planetaryShield = 0,
						bool repulsorBeam = false,
						bool cloakingDevice = false,
						int missileShield = 0,
						bool statisField = false,
						bool blackHoleGenerator = false,
						int ecoCleanup = 0,
						int terraforming = 0,
						int terraformCost = 6,
						int colony = 0,
						int cloning = 20,
						int bioWeapon = 0,
						int bioAntidote = 0,
						int enrichment = 0,
						int speed = 0,
						int maneuverSpeed = 0,
						int fuelRange = 0,
						bool inertialstabilizer = false,
						bool energypulsar = false,
						bool warpDissipator = false,
						bool highEnergyFocus = false,
						bool stargate = false,
						bool subSpaceTeleporter = false,
						bool ionicPulsar = false,
						bool subspaceInterdictor = false,
						bool combatTransporters = false,
						bool inertialNullifier = false,
						bool displacementDevice = false,
						bool antiMissileRockets = false,
						bool ionStreamProjector = false,
						bool neutronStreamProjector = false,
						float smallSize = 0,
						float smallCost = 0,
						float smallPower = 0,
						float smallHP = 0,
						float mediumSize = 0,
						float mediumCost = 0,
						float mediumPower = 0,
						float mediumHP = 0,
						float largeSize = 0,
						float largeCost = 0,
						float largePower = 0,
						float largeHP = 0,
						float hugeSize = 0,
						float hugeCost = 0,
						float hugePower = 0,
						float hugeHP = 0,
						float genericSize = 0,
						float genericCost = 0,
						float genericPower = 0,
						float smallSecondarySize = 0,
						float smallSecondaryCost = 0,
						float smallSecondaryPower = 0,
						float smallSecondaryHP = 0,
						float mediumSecondarySize = 0,
						float mediumSecondaryCost = 0,
						float mediumSecondaryPower = 0,
						float mediumSecondaryHP = 0,
						float largeSecondarySize = 0,
						float largeSecondaryCost = 0,
						float largeSecondaryPower = 0,
						float largeSecondaryHP = 0,
						float hugeSecondarySize = 0,
						float hugeSecondaryCost = 0,
						float hugeSecondaryPower = 0,
						float hugeSecondaryHP = 0,
						float genericSecondarySize = 0,
						float genericSecondaryCost = 0,
						float genericSecondaryPower = 0,
						int weaponType = 0,
						int minimumWeaponDamage = 0,
						int minimumSecondaryWeaponDamage = 0,
						int maximumWeaponDamage = 0,
						int maximumSecondaryWeaponDamage = 0,
						bool shieldPiercing = false,
						float weaponRange = 0,
						int secondaryWeaponRange = 0,
						int numberOfShots = 0,
						bool streaming = false,
						int targetingBonus = 0,
						bool enveloping = false,
						bool dissipating = false,
						float missileSpeed = 0
						)
		{
			TechLevel = level;
			TechField = techField;
			TechName = name;
			TechDescription = desc;
			TechSecondaryName = secondaryName;
			RoboticControl = roboticControl;
			BattleComputer = battleComputer;
			BattleScanner = battleScanner;
			this.ECM = ECM;
			SpaceScanner = spaceScanner;
			HyperSpaceCommunicator = hyperSpaceCommunicator;
			OracleInterface = oracleInterface;
			TechnologyNullifier = technologyNullifier;
			Armor = armor;
			ReserveFuelTanks = reserveFuelTanks;
			IndustrialTech = industrialTech;
			IndustrialWaste = industrialWaste;
			GroundArmor = groundArmor;
			Repair = repair;
			Shield = shield;
			PersonalShield = personalShield;
			PlanetaryShield = planetaryShield;
			RepulsorBeam = repulsorBeam;
			CloakingDevice = cloakingDevice;
			MissileShield = missileShield;
			StatisField = statisField;
			BlackHoleGenerator = blackHoleGenerator;
			EcoCleanup = ecoCleanup;
			Terraforming = terraforming;
			TerraformCost = terraformCost;
			Colony = colony;
			Cloning = cloning;
			BioWeapon = bioWeapon;
			BioAntidote = bioAntidote;
			Enrichment = enrichment;
			Speed = speed;
			ManeuverSpeed = maneuverSpeed;
			FuelRange = fuelRange;
			InertialStabilizer = inertialstabilizer;
			EnergyPulsar = energypulsar;
			WarpDissipator = warpDissipator;
			HighEnergyFocus = highEnergyFocus;
			SubspaceTeleporter = subSpaceTeleporter;
			IonicPulsar = ionicPulsar;
			SubspaceInterdictor = subspaceInterdictor;
			CombatTransporters = combatTransporters;
			InertialNullifier = inertialNullifier;
			DisplacementDevice = displacementDevice;
			AntiMissileRockets = antiMissileRockets;
			IonStreamProjector = ionStreamProjector;
			NeutronStreamProjector = neutronStreamProjector;
			Stargate = stargate;

			//Ship component info
			SmallSize = smallSize;
			SmallCost = smallCost;
			SmallPower = smallPower;
			SmallHP = smallHP;
			MediumSize = mediumSize;
			MediumCost = mediumCost;
			MediumPower = mediumPower;
			MediumHP = mediumHP;
			LargeSize = largeSize;
			LargeCost = largeCost;
			LargePower = largePower;
			LargeHP = largeHP;
			HugeSize = hugeSize;
			HugeCost = hugeCost;
			HugePower = hugePower;
			HugeHP = hugeHP;
			GenericSize = genericSize;
			GenericCost = genericCost;
			GenericPower = genericPower;
			SmallSecondarySize = smallSecondarySize;
			SmallSecondaryCost = smallSecondaryCost;
			SmallSecondaryPower = smallSecondaryPower;
			SmallSecondaryHP = smallSecondaryHP;
			MediumSecondarySize = mediumSecondarySize;
			MediumSecondaryCost = mediumSecondaryCost;
			MediumSecondaryPower = mediumSecondaryPower;
			MediumSecondaryHP = mediumSecondaryHP;
			LargeSecondarySize = largeSecondarySize;
			LargeSecondaryCost = largeSecondaryCost;
			LargeSecondaryPower = largeSecondaryPower;
			LargeSecondaryHP = largeSecondaryHP;
			HugeSecondarySize = hugeSecondarySize;
			HugeSecondaryCost = hugeSecondaryCost;
			HugeSecondaryPower = hugeSecondaryPower;
			HugeSecondaryHP = hugeSecondaryHP;
			GenericSecondarySize = genericSecondarySize;
			GenericSecondaryCost = genericSecondaryCost;
			GenericSecondaryPower = genericSecondaryPower;

			WeaponType = weaponType;
			MinimumWeaponDamage = minimumWeaponDamage;
			MinimumSecondaryWeaponDamage = minimumSecondaryWeaponDamage;
			MaximumWeaponDamage = maximumWeaponDamage;
			MaximumSecondaryWeaponDamage = maximumSecondaryWeaponDamage;
			ShieldPiercing = shieldPiercing;
			WeaponRange = weaponRange;
			SecondaryWeaponRange = secondaryWeaponRange;
			NumberOfShots = numberOfShots;
			Streaming = streaming;
			TargetingBonus = targetingBonus;
			Enveloping = enveloping;
			Dissipating = dissipating;
			MissileSpeed = missileSpeed;
		}
	}

	public class Equipment
	{
		public Technology Technology { get; private set; }
		public bool UseSecondary { get; set; }
		public string DisplayName
		{
			get
			{
				if (Technology.WeaponType == Technology.MISSILE_WEAPON)
				{
					return UseSecondary ? Technology.TechName + " x 5" : Technology.TechName + " x 2";
				}
				return UseSecondary ? Technology.TechSecondaryName : Technology.TechName;
			}
		}
		public string EquipmentName
		{
			get { return UseSecondary ? Technology.TechName + "|Sec" : Technology.TechName; }
		}

		//This stores whether or not the secondary technology item is used, as well as store other useful data for use in space combat
		public Equipment(Technology whichTech, bool useSecondary)
		{
			Technology = whichTech;
			UseSecondary = useSecondary;
		}

		public float GetCost(Dictionary<TechField, int> techLevels, int shipSize)
		{
			float initialCost = 0;
			if (Technology.GenericCost == 0) //It uses the ship-specific size cost
			{
				switch (shipSize)
				{
					case 0:
						initialCost = UseSecondary ? Technology.SmallSecondaryCost : Technology.SmallCost;
						break;
					case 1:
						initialCost = UseSecondary ? Technology.MediumSecondaryCost : Technology.MediumCost;
						break;
					case 2:
						initialCost = UseSecondary ? Technology.LargeSecondaryCost : Technology.LargeCost;
						break;
					case 3:
						initialCost = UseSecondary ? Technology.HugeSecondaryCost : Technology.HugeCost;
						break;
				}
			}
			else
			{
				if (Technology.WeaponType == Technology.MISSILE_WEAPON)
				{
					initialCost = UseSecondary ? Technology.GenericCost * 1.5f : Technology.GenericCost;
				}
				else
				{
					initialCost = UseSecondary ? Technology.GenericSecondaryCost : Technology.GenericCost;
				}
			}
			int levelDifference = techLevels[Technology.TechField] - Technology.TechLevel;
			if (levelDifference < 0)
			{
				levelDifference = 0;
			}
			else if (levelDifference > 50)
			{
				levelDifference = 50; //Cap the miniaturization at 50 levels
			}
			return (float)(initialCost * (Math.Pow(0.5, (levelDifference / 10.0))));
		}

		public float GetActualCost(Dictionary<TechField, int> techLevels, int shipSize, float costPerPower)
		{
			float cost = GetCost(techLevels, shipSize);
			return (cost + GetPower(shipSize) * costPerPower);
		}

		public float GetSize(Dictionary<TechField, int> techLevels, int shipSize)
		{
			float initialSize = 0;
			if (Technology.GenericSize == 0) //It uses the ship-specific size cost
			{
				switch (shipSize)
				{
					case 0:
						initialSize = UseSecondary ? Technology.SmallSecondarySize : Technology.SmallSize;
						break;
					case 1:
						initialSize = UseSecondary ? Technology.MediumSecondarySize : Technology.MediumSize;
						break;
					case 2:
						initialSize = UseSecondary ? Technology.LargeSecondarySize : Technology.LargeSize;
						break;
					case 3:
						initialSize = UseSecondary ? Technology.HugeSecondarySize : Technology.HugeSize;
						break;
				}
			}
			else
			{
				if (Technology.WeaponType == Technology.MISSILE_WEAPON)
				{
					initialSize = UseSecondary ? Technology.GenericSize * 1.5f : Technology.GenericSize;
				}
				else
				{
					initialSize = UseSecondary ? Technology.GenericSecondarySize : Technology.GenericSize;
				}
			}
			int levelDifference = techLevels[Technology.TechField] - Technology.TechLevel;
			if (levelDifference < 0)
			{
				levelDifference = 0;
			}
			else if (levelDifference > 50)
			{
				levelDifference = 50; //Cap the miniaturization at 50 levels
			}
			if (Technology.TechField == TechField.WEAPON) //Weapons enjoy 50% miniauratization rate
			{
				return (float)(initialSize * (Math.Pow(0.5, (levelDifference / 10.0))));
			}
			return (float)(initialSize * (Math.Pow(0.75, (levelDifference / 10.0))));
		}

		public float GetActualSize(Dictionary<TechField, int> techLevels, int shipSize, float spacePerPower)
		{
			float size = GetSize(techLevels, shipSize);
			return (size + GetPower(shipSize) * spacePerPower);
		}

		public float GetPower(int shipSize)
		{
			if (Technology.GenericPower == 0) //It uses the ship-specific size cost
			{
				switch (shipSize)
				{
					case 0:
						return UseSecondary ? Technology.SmallSecondaryPower : Technology.SmallPower;
					case 1:
						return UseSecondary ? Technology.MediumSecondaryPower : Technology.MediumPower;
					case 2:
						return UseSecondary ? Technology.LargeSecondaryPower : Technology.LargePower;
					case 3:
						return UseSecondary ? Technology.HugeSecondaryPower : Technology.HugePower;
				}
			}
			if (Technology.WeaponType == Technology.MISSILE_WEAPON)
			{
				return UseSecondary ? Technology.GenericPower * 1.5f : Technology.GenericPower;
			}
			return UseSecondary ? Technology.GenericSecondaryPower : Technology.GenericPower;
		}

		public int GetMinDamage()
		{
			if (Technology.WeaponType == Technology.MISSILE_WEAPON)
			{
				return Technology.MinimumWeaponDamage;
			}
			return UseSecondary ? Technology.MinimumSecondaryWeaponDamage : Technology.MinimumWeaponDamage;
		}

		public int GetMaxDamage()
		{
			if (Technology.WeaponType == Technology.MISSILE_WEAPON)
			{
				return Technology.MaximumWeaponDamage;
			}
			return UseSecondary ? Technology.MaximumSecondaryWeaponDamage : Technology.MaximumWeaponDamage;
		}

		public int GetRange()
		{
			if (Technology.WeaponType == Technology.MISSILE_WEAPON)
			{
				return (int)(UseSecondary ? ((Technology.WeaponRange + 0.1f) * 2) : (Technology.WeaponRange + 1.1f) * 2); //.1f is to compenstate for any float rounding error
			}
			return (int)(UseSecondary ? Technology.SecondaryWeaponRange : Technology.WeaponRange);
		}
	}
}
