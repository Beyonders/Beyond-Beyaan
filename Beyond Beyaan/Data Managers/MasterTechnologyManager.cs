using System;
using System.Collections.Generic;

namespace Beyond_Beyaan.Data_Managers
{
	public class MasterTechnologyManager
	{
		private GameMain _gameMain;

		public List<Technology> ComputerTechs { get; private set; }
		public List<Technology> ConstructionTechs { get; private set; }
		public List<Technology> ForceFieldTechs { get; private set; }
		public List<Technology> PlanetologyTechs { get; private set; }
		public List<Technology> PropulsionTechs { get; private set; }
		public List<Technology> WeaponTechs { get; private set; }

		#region Initialization functions
		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;

			//Later we'll replace with data files, but for now, all techs are hard-coded
			ComputerTechs = new List<Technology>();
			ConstructionTechs = new List<Technology>();
			ForceFieldTechs = new List<Technology>();
			PlanetologyTechs = new List<Technology>();
			PropulsionTechs = new List<Technology>();
			WeaponTechs = new List<Technology>();

			LoadComputerTechs();
			LoadConstructionTechs();
			LoadForceFieldTechs();
			LoadPlanetologyTechs();
			LoadPropulsionTechs();
			LoadWeaponTechs();

			reason = null;
			return true;
		}

		public Technology GetTechnologyWithName(string name) //In the future, we'll have only one list of technology, with each technology being more detailed
		{
			foreach (var tech in ComputerTechs)
			{
				if (string.Compare(name, tech.TechName, StringComparison.CurrentCulture) == 0)
				{
					return tech;
				}
			}
			foreach (var tech in ConstructionTechs)
			{
				if (string.Compare(name, tech.TechName, StringComparison.CurrentCulture) == 0)
				{
					return tech;
				}
			}
			foreach (var tech in ForceFieldTechs)
			{
				if (string.Compare(name, tech.TechName, StringComparison.CurrentCulture) == 0)
				{
					return tech;
				}
			}
			foreach (var tech in PlanetologyTechs)
			{
				if (string.Compare(name, tech.TechName, StringComparison.CurrentCulture) == 0)
				{
					return tech;
				}
			}
			foreach (var tech in PropulsionTechs)
			{
				if (string.Compare(name, tech.TechName, StringComparison.CurrentCulture) == 0)
				{
					return tech;
				}
			}
			foreach (var tech in WeaponTechs)
			{
				if (string.Compare(name, tech.TechName, StringComparison.CurrentCulture) == 0)
				{
					return tech;
				}
			}
			return null;
		}

		private void LoadComputerTechs()
		{
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Robotic Controls 2", "Controls 2 Buildings per population", 1, roboticControl: 2));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark I", "Increases weapon accuracy to level 1.", 1, battleComputer: 1, smallCost: 4, smallPower: 3, smallSize: 3, mediumCost: 20, mediumPower: 5, mediumSize: 5, largeCost: 100, largePower: 20, largeSize: 20, hugeCost: 500, hugePower: 100, hugeSize: 100));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Scanner", "Reveals technical specifications of enemy spacecraft in combat.", 1, battleScanner: true, genericCost: 30, genericSize: 50, genericPower: 50));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark 1", "Adds 1 level to defense against enemy missile attacks.", 2, ECM: 1, smallCost: 2.5f, smallPower: 10, smallSize: 10, mediumCost: 15, mediumPower: 20, mediumSize: 20, largeCost: 100, largePower: 40, largeSize: 40, hugeCost: 625, hugePower: 170, hugeSize: 170));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Deep Space Scanner", "Detects enemy ships up to 5 parsecs away from your colonies and 1 parsec away from your ships.", 4, spaceScanner: Technology.DEEP_SPACE_SCANNER));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark II", "Increases weapon accuracy to level 2.", 5, battleComputer: 2, smallCost: 5, smallPower: 5, smallSize: 5, mediumCost: 24, mediumPower: 10, mediumSize: 10, largeCost: 120, largePower: 30, largeSize: 30, hugeCost: 600, hugePower: 150, hugeSize: 150));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark II", "Adds 2 levels to defense against enemy missile attacks.", 7, ECM: 2, smallCost: 2.7f, smallPower: 15, smallSize: 15, mediumCost: 16.5f, mediumPower: 30, mediumSize: 30, largeCost: 110, largePower: 60, largeSize: 60, hugeCost: 687.5f, hugePower: 250, hugeSize: 250));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Improved Robotic Controls III", "Allows up to three factories to be operated per population. The refit cost to upgrade to Robotic Controls III is half the standard cost of each factory.", 8, roboticControl: 3));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark III", "Increases weapon accuracy to level 3.", 10, battleComputer: 3, smallCost: 6, smallPower: 7, smallSize: 7, mediumCost: 28, mediumPower: 15, mediumSize: 15, largeCost: 140, largePower: 40, largeSize: 40, hugeCost: 700, hugePower: 200, hugeSize: 200));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark III", "Adds 3 levels to defense against enemy missile attacks.", 12, ECM: 3, smallCost: 3, smallPower: 20, smallSize: 20, mediumCost: 18, mediumPower: 40, mediumSize: 40, largeCost: 120, largePower: 80, largeSize: 80, hugeCost: 750, hugePower: 330, hugeSize: 330));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Improved Space Scanner", "Detects enemy ships up to 7 parsecs away from your colonies and 2 parsecs away from your ships. Enemy destinations and ETA can also be accurately determined.", 13, spaceScanner: Technology.IMPROVED_SPACE_SCANNER));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark IV", "Increases weapon accuracy to level 4.", 15, battleComputer: 4, smallCost: 7, smallPower: 10, smallSize: 10, mediumCost: 32, mediumPower: 20, mediumSize: 20, largeCost: 160, largePower: 50, largeSize: 50, hugeCost: 800, hugePower: 250, hugeSize: 250));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark IV", "Adds 4 levels to defense against enemy missile attacks.", 17, ECM: 4, smallCost: 3.2f, smallPower: 25, smallSize: 25, mediumCost: 19.5f, mediumPower: 50, mediumSize: 50, largeCost: 130, largePower: 100, largeSize: 100, hugeCost: 812.5f, hugePower: 410, hugeSize: 410));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Improved Robotic Controls IV", "Allows up to four factories to be operated per population. The refit cost to upgrade to Robotic Controls IV is the standard cost of each factory.", 18, roboticControl: 4));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark V", "Increases weapon accuracy to level 5.", 20, battleComputer: 5, smallCost: 8, smallPower: 12, smallSize: 12, mediumCost: 36, mediumPower: 25, mediumSize: 25, largeCost: 180, largePower: 60, largeSize: 60, hugeCost: 900, hugePower: 300, hugeSize: 300));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark V", "Adds 5 levels to defense against enemy missile attacks.", 22, ECM: 5, smallCost: 3.5f, smallPower: 30, smallSize: 30, mediumCost: 21, mediumPower: 60, mediumSize: 60, largeCost: 140, largePower: 120, largeSize: 120, hugeCost: 875, hugePower: 490, hugeSize: 490));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Advanced Space Scanner", "Allows exploration of planets from colony bases up to 9 parsecs away and detects enemy ships up to 3 parsecs away from your ships.", 23, spaceScanner: Technology.ADVANCED_SPACE_SCANNER));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark VI", "Increases weapon accuracy to level 6.", 25, battleComputer: 6, smallCost: 9, smallPower: 15, smallSize: 15, mediumCost: 40, mediumPower: 30, mediumSize: 30, largeCost: 200, largePower: 70, largeSize: 70, hugeCost: 1000, hugePower: 350, hugeSize: 350));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark VI", "Adds 6 levels to defense against enemy missile attacks.", 27, ECM: 6, smallCost: 3.7f, smallPower: 35, smallSize: 35, mediumCost: 22.5f, mediumPower: 70, mediumSize: 70, largeCost: 150, largePower: 140, largeSize: 140, hugeCost: 937.5f, hugePower: 570, hugeSize: 570));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Improved Robotic Controls V", "Allows up to five factories to be operated per population. The refit cost to upgrade to Robotic Controls V is one and a half times the standard cost of each factory.", 28, roboticControl: 5));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark VII", "Increases weapon accuracy to level 7.", 30, battleComputer: 7, smallCost: 10, smallPower: 17, smallSize: 17, mediumCost: 44, mediumPower: 35, mediumSize: 35, largeCost: 220, largePower: 80, largeSize: 80, hugeCost: 1100, hugePower: 400, hugeSize: 400));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark VII", "Adds 7 levels to defense against enemy missile attacks.", 32, ECM: 7, smallCost: 4, smallPower: 40, smallSize: 40, mediumCost: 24, mediumPower: 80, mediumSize: 80, largeCost: 160, largePower: 160, largeSize: 160, hugeCost: 1000, hugePower: 650, hugeSize: 650));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Hyperspace Communications", "Allows you to communicate with ships and transports in hyperspace, and change their destinations while in route.", 34, hyperSpaceCommunicator: true));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark VIII", "Increases weapon accuracy to level 8.", 35, battleComputer: 8, smallCost: 11, smallPower: 20, smallSize: 20, mediumCost: 48, mediumPower: 40, mediumSize: 40, largeCost: 240, largePower: 90, largeSize: 90, hugeCost: 1200, hugePower: 450, hugeSize: 450));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark VIII", "Adds 8 levels to defense against enemy missile attacks.", 37, ECM: 8, smallCost: 4.2f, smallPower: 45, smallSize: 45, mediumCost: 25.5f, mediumPower: 90, mediumSize: 90, largeCost: 170, largePower: 180, largeSize: 180, hugeCost: 1062.5f, hugePower: 730, hugeSize: 730));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Improved Robotic Controls VI", "Allows up to six factories to be operated per population. The refit cost to upgrade to Robotic Controls VI is twice the standard cost of each factory.", 38, roboticControl: 6));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark IX", "Increases weapon accuracy to level 9.", 40, battleComputer: 9, smallCost: 12, smallPower: 22, smallSize: 22, mediumCost: 52, mediumPower: 45, mediumSize: 45, largeCost: 260, largePower: 100, largeSize: 100, hugeCost: 1300, hugePower: 500, hugeSize: 500));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark IX", "Adds 9 levels to defense against enemy missile attacks.", 42, ECM: 9, smallCost: 4.5f, smallPower: 50, smallSize: 50, mediumCost: 27, mediumPower: 100, mediumSize: 100, largeCost: 180, largePower: 200, largeSize: 200, hugeCost: 1125, hugePower: 810, hugeSize: 810));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark X", "Increases weapon accuracy to level 10.", 45, battleComputer: 10, smallCost: 13, smallPower: 25, smallSize: 25, mediumCost: 56, mediumPower: 50, mediumSize: 50, largeCost: 280, largePower: 110, largeSize: 110, hugeCost: 1400, hugePower: 550, hugeSize: 550));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Oracle Interface", "Coordinates all beam weapon attacks into one simultaneous burst of concentrated fire, halving the enemy's shield strength.", 46, oracleInterface: true, smallCost: 3, smallSize: 8, smallPower: 12, mediumCost: 15, mediumSize: 40, mediumPower: 60, largeCost: 60, largeSize: 200, largePower: 300, hugeCost: 275, hugeSize: 1000, hugePower: 1500));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "ECM Jammer Mark X", "Adds 10 levels to defense against enemy missile attacks.", 47, ECM: 10, smallCost: 5, smallPower: 55, smallSize: 55, mediumCost: 28.5f, mediumPower: 110, mediumSize: 110, largeCost: 190, largePower: 220, largeSize: 220, hugeCost: 1187.5f, hugePower: 900, hugeSize: 900));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Improved Robotic Controls VII", "Allows up to seven factories to be operated per population. The refit cost to upgrade to Robotic Controls VII is 2.5 times the standard cost of each factory.", 48, roboticControl: 7));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Technology Nullifier", "Scrambles enemy battle computers, reducing the level of the computers up to 2-6 levels each time the nullifier is fired. The weapon has a 4 space range.", 49, technologyNullifier: true, genericCost: 300, genericSize: 750, genericPower: 1000));
			ComputerTechs.Add(new Technology(TechField.COMPUTER, "Battle Computer Mark XI", "Increases weapon accuracy to level 11.", 50, battleComputer: 11, smallCost: 14, smallPower: 27, smallSize: 27, mediumCost: 60, mediumPower: 55, mediumSize: 55, largeCost: 300, largePower: 120, largeSize: 120, hugeCost: 1500, hugePower: 600, hugeSize: 600));
		}

		private void LoadConstructionTechs()
		{
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Reserve Fuel Tanks", "Extends the range of a ship by an additional 3 parsecs.", 1, reserveFuelTanks: true, smallCost: 2, smallSize: 20, smallPower: 0, mediumCost: 10, mediumSize: 100, mediumPower: 0, largeCost: 50, largeSize: 500, largePower: 0, hugeCost: 250, hugeSize: 2500, hugePower: 0));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Titanium Armor", "Standard armor for ships and missile bases. Gives small ships 3 hit points, medium ships 18 hit points, large ships 100 hit points, and huge ships 600 hit points. Gives missile bases 50 hit points.", 1, secondaryName: "Titanium II Armor", armor: Technology.TITANIUM_ARMOR, smallCost: 0, smallSize: 0, smallHP: 3, mediumCost: 0, mediumSize: 0, mediumHP: 18, largeCost: 0, largeSize: 0, largeHP: 100, hugeCost: 0, hugeSize: 0, hugeHP: 600, smallSecondaryCost: 2, smallSecondarySize: 14, smallSecondaryHP: 4, mediumSecondaryCost: 10, mediumSecondarySize: 80, mediumSecondaryHP: 27, largeSecondaryCost: 50, largeSecondarySize: 400, largeSecondaryHP: 150, hugeSecondaryCost: 250, hugeSecondarySize: 2000, hugeSecondaryHP: 900));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Tech 9", "Reduces factory construction costs to 9 BC each.", 3, industrialTech: 9));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Reduced Industrial Waste 80%", "Decreases factory pollution levels to 80% of the normal rate.", 5, industrialWaste: 80));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Tech 8", "Reduces factory construction costs to 8 BC each.", 8, industrialTech: 8));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Duralloy Armor", "Increases the hit points of ships and transports by 50%. Personal combat armor is also enhanced, adding 5 to all ground attacks.", 10, secondaryName: "Duralloy II Armor", armor: Technology.DURALLOY_ARMOR, smallCost: 2, smallSize: 2, smallHP: 4, mediumCost: 10, mediumSize: 10, mediumHP: 27, largeCost: 60, largeSize: 60, largeHP: 150, hugeCost: 300, hugeSize: 300, hugeHP: 900, smallSecondaryCost: 3, smallSecondarySize: 17, smallSecondaryHP: 6, mediumSecondaryCost: 15, mediumSecondarySize: 85, mediumSecondaryHP: 40, largeSecondaryCost: 90, largeSecondarySize: 425, largeSecondaryHP: 225, hugeSecondaryCost: 450, hugeSecondarySize: 2100, hugeSecondaryHP: 1350));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Battle Suits", "Armor that not only protects but also boosts strength. Adds 10 to all ground combat rolls.", 11, groundArmor: Technology.BATTLE_SUITS));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Tech 7", "Reduces factory construction costs to 7 BC each.", 13, industrialTech: 7));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Automated Repair Unit", "Undestroyed ships can repair up to 15% of their total hit points at the end of each turn.", 14, repair: Technology.AUTOMATED_REPAIR, smallCost: 0.2f, smallSize: 3, smallPower: 3, mediumCost: 0.8f, mediumSize: 15, mediumPower: 10, largeCost: 5, largeSize: 100, largePower: 50, hugeCost: 30, hugeSize: 600, hugePower: 300));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Reduced Industrial Waste 60%", "Decreases factory pollution levels to 60% of the normal rate.", 15, industrialWaste: 60));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Zortrium Armor", "Increases the hit points of ships and transports by 100%. Personal combat armor is also enhanced, adding 10 to all ground attacks.", 17, secondaryName: "Zortrium II Armor", armor: Technology.ZORTRIUM_ARMOR, smallCost: 4, smallSize: 4, smallHP: 6, mediumCost: 20, mediumSize: 20, mediumHP: 36, largeCost: 100, largeSize: 100, largeHP: 200, hugeCost: 500, hugeSize: 500, hugeHP: 1200, smallSecondaryCost: 6, smallSecondarySize: 20, smallSecondaryHP: 9, mediumSecondaryCost: 30, mediumSecondarySize: 100, mediumSecondaryHP: 54, largeSecondaryCost: 150, largeSecondarySize: 500, largeSecondaryHP: 300, hugeSecondaryCost: 750, hugeSecondarySize: 2500, hugeSecondaryHP: 1800));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Tech 6", "Reduces factory construction costs to 6 BC each.", 18, industrialTech: 6));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Tech 5", "Reduces factory construction costs to 5 BC each.", 23, industrialTech: 5));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Armored Exoskeleton", "Advanced mobile suits that not only boost power and increase defenses but also offer limited flight to ground troops. Adds 20 to all ground combat rolls.", 24, groundArmor: Technology.ARMORED_EXOSKELETON));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Reduced Industrial Waste 40%", "Decreases factory pollution levels to 40% of the normal rate.", 25, industrialWaste: 40));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Andrium Armor", "Increases the hit points of ships and transports by 150%. Personal combat armor is also enhanced, adding 15 to all ground attacks.", 26, secondaryName: "Andrium II Armor", armor: Technology.ANDRIUM_ARMOR, smallCost: 6, smallSize: 6, smallHP: 7, mediumCost: 30, mediumSize: 30, mediumHP: 45, largeCost: 150, largeSize: 150, largeHP: 250, hugeCost: 750, hugeSize: 750, hugeHP: 1500, smallSecondaryCost: 9, smallSecondarySize: 23, smallSecondaryHP: 11, mediumSecondaryCost: 45, mediumSecondarySize: 115, mediumSecondaryHP: 67, largeSecondaryCost: 225, largeSecondarySize: 575, largeSecondaryHP: 375, hugeSecondaryCost: 1125, hugeSecondarySize: 2875, hugeSecondaryHP: 2250));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Tech 4", "Reduces factory construction costs to 4 BC each.", 28, industrialTech: 4));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Tech 3", "Reduces factory construction costs to 3 BC each.", 33, industrialTech: 3));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Tritanium Armor", "Increases the hit points of ships and transports by 200%. Personal combat armor is also enhanced, adding 20 to all ground attacks.", 34, secondaryName: "Tritanium II Armor", armor: Technology.TRITANIUM_ARMOR, smallCost: 8, smallSize: 8, smallHP: 9, mediumCost: 40, mediumSize: 40, mediumHP: 54, largeCost: 200, largeSize: 200, largeHP: 300, hugeCost: 1000, hugeSize: 1000, hugeHP: 1800, smallSecondaryCost: 12, smallSecondarySize: 26, smallSecondaryHP: 13, mediumSecondaryCost: 60, mediumSecondarySize: 130, mediumSecondaryHP: 81, largeSecondaryCost: 300, largeSecondarySize: 650, largeSecondaryHP: 450, hugeSecondaryCost: 1500, hugeSecondarySize: 3250, hugeSecondaryHP: 2700));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Reduced Industrial Waste 20%", "Decreases factory pollution levels to 20% of the normal rate.", 35, industrialWaste: 20));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Advanced Damage Control Unit", "Undestroyed ships can repair up to 30% of their total hit points at the end of each turn.", 36, repair: Technology.ADVANCED_REPAIR, smallCost: 4, smallSize: 9, smallPower: 9, mediumCost: 20, mediumSize: 45, mediumPower: 30, largeCost: 100, largeSize: 300, largePower: 150, hugeCost: 500, hugeSize: 1800, hugePower: 450));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Tech 2", "Reduces factory construction costs to 2 BC each.", 38, industrialTech: 2));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Powered Armor", "Combines high mobility, anti-grav flight, and heavy armored plating to form the most advanced armor available for ground troops. Adds 30 to all ground combat rolls.", 40, groundArmor: Technology.POWERED_ARMOR));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Adamantium Armor", "Increases the hit points of ships and transports by 250%. Personal combat armor is also enhanced, adding 25 to all ground attacks.", 42, secondaryName: "Adamantium II Armor", armor: Technology.ADAMANTIUM_ARMOR, smallCost: 10, smallSize: 10, smallHP: 10, mediumCost: 50, mediumSize: 50, mediumHP: 63, largeCost: 250, largeSize: 250, largeHP: 350, hugeCost: 1250, hugeSize: 1250, hugeHP: 2100, smallSecondaryCost: 15, smallSecondarySize: 30, smallSecondaryHP: 15, mediumSecondaryCost: 75, mediumSecondarySize: 150, mediumSecondaryHP: 94, largeSecondaryCost: 375, largeSecondarySize: 750, largeSecondaryHP: 525, hugeSecondaryCost: 1875, hugeSecondarySize: 3750, hugeSecondaryHP: 3150));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Industrial Waste Elimination", "Factories cease to pollute.", 45, industrialWaste: 0));
			ConstructionTechs.Add(new Technology(TechField.CONSTRUCTION, "Neutronium Armor", "Provides the best internal protection of any armor and increases the hit points of a ship by 300-. Personal combat armor is also enhanced, adding 30 to all ground attacks.", 50, secondaryName: "Neutronium II Armor", armor: Technology.NEUTRONIUM_ARMOR, smallCost: 12, smallSize: 12, smallHP: 12, mediumCost: 60, mediumSize: 60, mediumHP: 72, largeCost: 300, largeSize: 300, largeHP: 400, hugeCost: 1500, hugeSize: 1500, hugeHP: 2400, smallSecondaryCost: 18, smallSecondarySize: 35, smallSecondaryHP: 18, mediumSecondaryCost: 90, mediumSecondarySize: 175, mediumSecondaryHP: 108, largeSecondaryCost: 450, largeSecondarySize: 875, largeSecondaryHP: 600, hugeSecondaryCost: 2500, hugeSecondarySize: 4375, hugeSecondaryHP: 3600));
		}

		private void LoadForceFieldTechs()
		{
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class I Deflector Shield", "Absorbs 1 point of damage from all attacks.", 1, shield: 1, smallCost: 3, smallPower: 5, smallSize: 5, mediumCost: 19, mediumPower: 20, mediumSize: 20, largeCost: 120, largePower: 60, largeSize: 60, hugeCost: 750, hugePower: 250, hugeSize: 250));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class II Deflector Shield", "Absorbs 2 points of damage from all attacks.", 4, shield: 2, smallCost: 3.5f, smallPower: 10, smallSize: 10, mediumCost: 22, mediumPower: 35, mediumSize: 35, largeCost: 140, largePower: 90, largeSize: 90, hugeCost: 875, hugePower: 375, hugeSize: 375));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Personal Deflector Shield", "Protects individual ground troops with a directional force field. Adds 10 to all ground combat battles.", 8, personalShield: Technology.PERSONAL_DEFLECTOR));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class III Deflector Shield", "Absorbs 3 points of damage from all attacks.", 10, shield: 3, smallCost: 4, smallPower: 15, smallSize: 15, mediumCost: 25, mediumPower: 50, mediumSize: 50, largeCost: 160, largePower: 120, largeSize: 120, hugeCost: 1000, hugePower: 500, hugeSize: 500));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class V Planetary Shield", "Absorbs 5 points of damage from attacks against planet surfaces, and is cumulative with missile base deflector shields.", 12, planetaryShield: Technology.PLANETARY_V_SHIELD));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class IV Deflector Shield", "Absorbs 4 points of damage from all attacks.", 14, shield: 4, smallCost: 4.5f, smallPower: 20, smallSize: 20, mediumCost: 28, mediumPower: 65, mediumSize: 65, largeCost: 180, largePower: 150, largeSize: 150, hugeCost: 1125, hugePower: 625, hugeSize: 625));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Repulsor Beam", "Repels enemy ships back one space away from the attacking ship. The special weapon has a 1 space range.", 16, repulsorBeam: true, genericCost: 55, genericSize: 100, genericPower: 200));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class V Deflector Shield", "Absorbs 5 points of damage from all attacks.", 20, shield: 5, smallCost: 5, smallPower: 25, smallSize: 25, mediumCost: 31, mediumPower: 80, mediumSize: 80, largeCost: 200, largePower: 180, largeSize: 180, hugeCost: 1250, hugePower: 750, hugeSize: 750));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Personal Absorption Shield", "Absorbs damage from all hand weapons. Adds 20 to all ground combat battles.", 21, personalShield: Technology.PERSONAL_ABSORPTION));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class X Planetary Shield", "Absorbs 10 points of damage from all attacks against planet surfaces, and is cumulative with missile base deflector shields.", 22, planetaryShield: Technology.PLANETARY_X_SHIELD));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class VI Deflector Shield", "Absorbs 6 points of damage from all attacks.", 24, shield: 6, smallCost: 5.5f, smallPower: 30, smallSize: 30, mediumCost: 34, mediumPower: 95, mediumSize: 95, largeCost: 220, largePower: 210, largeSize: 210, hugeCost: 1375, hugePower: 875, hugeSize: 875));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Cloaking Device", "Renders ships nearly invisible until they attack. While cloaked ships receive a +5 to their missile and beam defenses.", 27, cloakingDevice: true, smallCost: 3, smallSize: 5, smallPower: 10, mediumCost: 15, mediumSize: 25, mediumPower: 50, largeCost: 75, largeSize: 120, largePower: 250, hugeCost: 375, hugeSize: 600, hugePower: 1250));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class VII Deflector Shield", "Absorbs 7 points of damage from all attacks.", 30, shield: 7, smallCost: 6, smallPower: 35, smallSize: 35, mediumCost: 37, mediumPower: 110, mediumSize: 110, largeCost: 240, largePower: 240, largeSize: 240, hugeCost: 1500, hugePower: 1000, hugeSize: 1000));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Zyro Shield", "An energy field that destroys incoming missiles and torpedoes 75% of the time, -1% per technology level of the missile.", 31, missileShield: Technology.ZYRO_SHIELD, smallCost: 5, smallSize: 4, smallPower: 12, mediumCost: 10, mediumSize: 20, mediumPower: 60, largeCost: 20, largeSize: 100, largePower: 300, hugeCost: 30, hugeSize: 500, hugePower: 1500));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class XV Planetary Shield", "Absorbs 15 points of damage from all attacks against planet surfaces, and is cumulative with missile base deflector shields.", 32, planetaryShield: Technology.PLANETARY_XV_SHIELD));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class IX Deflector Shield", "Absorbs 9 points of damage from all attacks.", 34, shield: 9, smallCost: 6.5f, smallPower: 40, smallSize: 40, mediumCost: 40, mediumPower: 125, mediumSize: 125, largeCost: 260, largePower: 270, largeSize: 270, hugeCost: 1625, hugePower: 1125, hugeSize: 1125));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Stasis Field", "Freezes one group of enemy ships up to one space away, for one turn. Frozen ships cannot attack or be attacked.", 37, statisField: true, genericCost: 250, genericSize: 200, genericPower: 275));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Personal Barrier Shield", "Completely encases the soldier in an nearly impenetrable force field. Adds 30 to all ground combat rolls.", 38, personalShield: Technology.PERSONAL_BARRIER));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class XI Deflector Shield", "Absorbs 11 points of damage from all attacks.", 40, shield: 11, smallCost: 7, smallPower: 45, smallSize: 45, mediumCost: 43, mediumPower: 140, mediumSize: 140, largeCost: 280, largePower: 300, largeSize: 300, hugeCost: 1750, hugePower: 1250, hugeSize: 1250));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class XX Planetary Shield", "Absorbs 20 points of damage from all attacks against planet surfaces, and is cumulative with missile base deflector shields.", 42, planetaryShield: Technology.PLANETARY_XX_SHIELD));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Black Hole Generator", "Creates a sub-space field that warps normal space creating an instantaneous black hole, destroying 25%-100% of enemy ships, -2% per shield class.", 43, blackHoleGenerator: true, genericCost: 275, genericSize: 750, genericPower: 750));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class XIII Deflector Shield", "Absorbs 13 points of damage from all attacks.", 44, shield: 13, smallCost: 8, smallPower: 50, smallSize: 50, mediumCost: 46, mediumPower: 155, mediumSize: 155, largeCost: 300, largePower: 330, largeSize: 330, hugeCost: 1875, hugePower: 1375, hugeSize: 1375));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Lightning Shield", "An energy field that destroys incoming enemy missiles and torpedoes 100% of the time, -1% per technology level of the missile.", 46, missileShield: Technology.LIGHTNING_SHIELD, smallCost: 20, smallSize: 6, smallPower: 15, mediumCost: 30, mediumSize: 30, mediumPower: 70, largeCost: 40, largeSize: 150, largePower: 350, hugeCost: 50, hugeSize: 750, hugePower: 1750));
			ForceFieldTechs.Add(new Technology(TechField.FORCE_FIELD, "Class XV Deflector Shield", "Absorbs 15 points of damage from all attacks.", 50, shield: 15, smallCost: 9, smallPower: 55, smallSize: 55, mediumCost: 49, mediumPower: 160, mediumSize: 160, largeCost: 320, largePower: 360, largeSize: 360, hugeCost: 2000, hugePower: 1500, hugeSize: 1500));
		}

		private void LoadPlanetologyTechs()
		{
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Standard Colony Base", "Permits the colonization of standard planets.", 1, colony: Technology.STANDARD_COLONY, genericCost: 350, genericSize: 700));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Ecological Restoration", "Eliminates 2 units of industrial waste for a cost of 1 BC.", 1, ecoCleanup: 2));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Terraforming +10", "Increases the population capacity of planets by 10M for a cost of 5 BC per million.", 2, terraforming: 10, terraformCost: 5));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Controlled Barren Environment", "Permits the colonization of barren planets.", 3, colony: Technology.BARREN_COLONY, genericCost: 375, genericSize: 700));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Improved Ecological Restoration", "Eliminates 3 units of industrial waste for a cost of 1 BC.", 5, ecoCleanup: 3));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Controlled Tundra Environment", "Permits the colonization of tundra planets.", 6, colony: Technology.TUNDRA_COLONY, genericCost: 400, genericSize: 700));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Terraforming +20", "Increases the population capacity of planets by 20M for a cost of 5 BC per million.", 8, terraforming: 20, terraformCost: 5));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Controlled Dead Environment", "Permits the colonization of dead planets.", 9, colony: Technology.DEAD_COLONY, genericCost: 425, genericSize: 700));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Death Spores", "Horrible biological weapons capable of reducing the maximum planetary populations by 1 million per attack.", 10, bioWeapon: Technology.DEATH_SPORES));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Controlled Inferno Environment", "Permits the colonization of inferno planets.", 12, colony: Technology.VOLCANIC_COLONY, genericCost: 450, genericSize: 700));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Enhanced Ecological Restoration", "Eliminates 5 units of industrial waste for a cost of 1 BC.", 13, ecoCleanup: 5));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Terraforming +30", "Increases the population capacity of planets by 30M for a cost of 4 BC per million.", 14, terraforming: 30, terraformCost: 4));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Controlled Toxic Environment", "Permits the colonization of toxic planets.", 15, colony: Technology.TOXIC_COLONY, genericCost: 475, genericSize: 700));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Soil Enrichment", "Converts standard planets to fertile enviroments, increasing population growth by 50% and raising the base planetary size by 25% for a one time cost of 150 BC.", 16, enrichment: Technology.SOIL_ENRICHMENT));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Bio Toxin Antidote", "Reduces casualties taken from biological weapons by 1 million per attack.", 17, bioAntidote: Technology.BIO_TOXIN_ANTIDOTE));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Controlled Radiated Environment", "Permits the colonization of radiated planets.", 18, colony: Technology.RADIATED_COLONY, genericCost: 500, genericSize: 700));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Terraforming +40", "Increases the population capacity of planets by 40M for a cost of 4 BC per million.", 20, terraforming: 40, terraformCost: 4));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Cloning", "Allows bio engineered colonists to be grown at a rate of 1M per 10 BC.", 21, cloning: 10));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Atmospheric Terraforming", "Converts hostile planets to standard minimal environments, normalizing population growth for a one time cost of 200 BC.", 22, enrichment: Technology.ATMOSPHERIC_TERRAFORMING));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Advanced Ecological Restoration", "Eliminates 10 units of industrial waste for a cost of 1 BC.", 24, ecoCleanup: 10));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Terraforming +50", "Increases the population capacity of planets by 50M for a cost of 3 BC per million.", 26, terraforming: 50, terraformCost: 3));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Doom Virus", "Dreadful biological weapons capable of reducing planetary populations by 2 million per attack.", 27, bioWeapon: Technology.DOOM_VIRUS));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Advanced Soil Enrichment", "Converts standard and fertile planets to gaias, doubling the population growth and increasing the planet's base size by 50% for the one time cost of 300 BC.", 30, enrichment: Technology.ADV_SOIL_ENRICHMENT));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Terraforming +60", "Increases the population capacity of planets by 60M for a cost of 3 BC per million.", 32, terraforming: 60, terraformCost: 3));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Complete Ecological Restoration", "Eliminates 20 units of industrial waste for a cost of 1 BC.", 34, ecoCleanup: 20));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Universal Antidote", "Reduces casualties taken from biological weapons by 2 million per attack.", 36, bioAntidote: Technology.UNIVERSAL_ANTIDOTE));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Terraforming +80", "Increases the population capacity of planets by 80M for a cost of 2 BC per million.", 38, terraforming: 80, terraformCost: 2));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Bio Terminator", "Abominable biological weapons capable of reducing planetary populations by 3 million per attack.", 40, bioWeapon: Technology.BIO_TERMINATOR));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Advanced Cloning", "Allows bio engineered colonists to be grown at a rate of 1M per 5 BC.", 42, cloning: 5));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Terraforming +100", "Increases the population capacity of planets by 100M for a cost of 2 BC per million.", 44, terraforming: 100, terraformCost: 2));
			PlanetologyTechs.Add(new Technology(TechField.PLANETOLOGY, "Complete Terraforming", "Increases the population capacity of planets by 120M for a cost of 2 BC per million.", 50, terraforming: 120, terraformCost: 2));
		}

		private void LoadPropulsionTechs()
		{
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Retro Engines", "Moves ships at warp one (1 parsecs per turn), and allows a maximum maneuverability of class I in combat.", 1, speed: 1, maneuverSpeed: 1, genericCost: 2, genericSize: 10));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Hydrogen Fuel Cells", "Fuel reserves allow ships to move up to 4 parsecs away from colony planets.", 3, fuelRange: 4));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Deutrium Fuel Cells", "Fuel reserves allow ships to move up to 5 parsecs away from colony planets.", 5, fuelRange: 5));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Nuclear Engines", "Moves ships at warp two (2 parsecs per turn), and allows a maximum maneuverability of class II in combat.", 6, speed: 2, maneuverSpeed: 1, genericCost: 4, genericSize: 18));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Irridium Fuel Cells", "Fuel reserves allow ships to move up to 6 parsecs away from colony planets.", 9, fuelRange: 6));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Inertial Stabilizer", "Generates a field that reduces the inertia of ships, and adds 2 classes of maneuverability in combat (+2 defense and +1 combat speed).", 10, inertialstabilizer: true, smallCost: 2, smallSize: 4, smallPower: 8, mediumCost: 7.5f, mediumSize: 20, mediumPower: 40, largeCost: 50, largeSize: 100, largePower: 200, hugeCost: 270, hugeSize: 500, hugePower: 1000));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Sub Light Drives", "Moves ships at warp three (3 parsecs per turn), and allows a maximum maneuverability of class III in combat.", 12, speed: 3, maneuverSpeed: 1, genericCost: 6, genericSize: 26));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Dotomite Crystals", "Fuel reserves allow ships to move up to 7 parsecs away from colony planets.", 14, fuelRange: 7));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Energy Pulsar", "A potent engine modification which generates a sudden spherical burst of energy striking all adjacent ships for up to 5 points of damage plus 1 per two ships.", 16, energypulsar: true, genericCost: 75, genericSize: 150, genericPower: 250));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Fusion Drives", "Moves ships at warp four (4 parsecs per turn), and allows a maximum maneuverability of class IV in combat.", 18, speed: 4, maneuverSpeed: 2, genericCost: 8, genericSize: 33));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Uridium Fuel Cells", "Fuel reserves allow ships to move up to 8 parsecs away from colony planets.", 19, fuelRange: 8));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Warp Dissipator", "Specialized weapon that disrupts the warp fields surrounding enemy ships, reducing their speed by 0-1 each turn the weapon is fired.", 20, warpDissipator: true, genericCost: 65, genericSize: 100, genericPower: 300));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Reajax II Fuel Cells", "Fuel reserves allow ships to move up to 9 parsecs away from colony planets.", 23, fuelRange: 9));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Impulse Drives", "Moves ships at warp five (5 parsecs per turn), and allows a maximum maneuverability of class V in combat.", 24, speed: 5, maneuverSpeed: 2, genericCost: 10, genericSize: 36));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Intergalactic Star Gates", "Allows your ships to move between any two planets equipped with star gates in only one turn. Costs 3000 BC to build.", 27, stargate: true));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Trilithium Crystals", "Fuel reserves allow ships to move up to 10 parsecs away from colony planets.", 29, fuelRange: 10));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Ion Drives", "Moves ships at warp six (6 parsecs per turn), and allows a maximum maneuverability of class VI in combat.", 30, speed: 6, maneuverSpeed: 3, genericCost: 12, genericSize: 40));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "High Energy Focus", "Increases the firing range of all energy weapons by three.", 34, highEnergyFocus: true, smallCost: 3, smallSize: 35, smallPower: 65, mediumCost: 13.5f, mediumSize: 100, mediumPower: 200, largeCost: 62.5f, largeSize: 150, largePower: 350, hugeCost: 350, hugeSize: 500, hugePower: 1000));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Anti-Matter Drives", "Moves ships at warp seven (7 parsecs per turn), and allows a maximum maneuverability of class VII in combat.", 36, speed: 7, maneuverSpeed: 3, genericCost: 14, genericSize: 44));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Sub Space Teleporter", "Teleports ships to any space on the combat map and gives first initiative to the teleporting ship.", 38, subSpaceTeleporter: true, smallCost: 2.5f, smallSize: 4, smallPower: 16, mediumCost: 10, mediumSize: 20, mediumPower: 80, largeCost: 45, largeSize: 100, largePower: 400, hugeCost: 225, hugeSize: 500, hugePower: 2000));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Ionic Pulsar", "A powerful engine modification capable of generating a spherical burst of phased energy striking all adjacent ships for up to 10 points of damage plus one per ship.", 40, ionicPulsar: true, genericCost: 150, genericSize: 400, genericPower: 750));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Thorium Cells", "Self-replenishing fuel that allows ships to move any distance from colony planets.", 41, fuelRange: int.MaxValue));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Inter-phased Drives", "Moves ships at warp eight (8 parsecs per turn), and allows a maximum maneuverability of class VIII in combat.", 42, speed: 8, maneuverSpeed: 4, genericCost: 16, genericSize: 47));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Sub Space Interdictor", "Surrounds colony planets with an intense gravity well, rendering sub space teleporters useless and halving combat transporter effectiveness. The device is a placed in all missile bases.", 43, subspaceInterdictor: true));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Combat Transporters", "Transports equipped with these devices have a 50% chance of beaming down onto enemy surfaces before the transports can be attacked by enemy ships and bases.", 45, combatTransporters: true));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Inertial Nullifier", "Generates a field that negates the inertia of ships and adds 2 classes of maneuverability in combat (+4 defense and +2 combat speed).", 46, inertialNullifier: true, smallCost: 6, smallSize: 6, smallPower: 12, mediumCost: 20, mediumSize: 30, mediumPower: 60, largeCost: 150, largeSize: 150, largePower: 300, hugeCost: 500, hugeSize: 750, hugePower: 1500));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Hyper Drives", "Moves ships at warp nine (9 parsecs per turn), and allows a maximum maneuverability of class IX in combat.", 48, speed: 9, maneuverSpeed: 4, genericCost: 18, genericSize: 50));
			PropulsionTechs.Add(new Technology(TechField.PROPULSION, "Displacement Device", "Randomly shifts the equipped ship in and out of normal space, allowing the ship to avoid one third of all non-area attacks.", 50, displacementDevice: true, smallCost: 3, smallSize: 15, smallPower: 5, mediumCost: 15, mediumSize: 75, mediumPower: 25, largeCost: 30, largeSize: 375, largePower: 125, hugeCost: 275, hugeSize: 1875, hugePower: 625));
		}

		private void LoadWeaponTechs()
		{
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Laser", "Direct-fire beam weapon that inflicts 1-4 points of damage. Heavy lasers have a two space range and do 1-7 points of damage.", 1, secondaryName: "Heavy Laser", minimumWeaponDamage: 1, maximumWeaponDamage: 4, weaponRange: 1, genericCost: 3, genericSize: 10, genericPower: 25, minimumSecondaryWeaponDamage: 1, maximumSecondaryWeaponDamage: 7, genericSecondaryCost: 9, genericSecondarySize: 30, genericSecondaryPower: 75, secondaryWeaponRange: 2, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Nuclear Missile", "Missles tipped with nuclear warheads that explode for 4 points of damage and move at a speed of 2.", 1, maximumWeaponDamage: 4, weaponRange: 2, genericCost: 5, genericSize: 50, genericPower: 20, missileSpeed: 2, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Nuclear Bomb", "Bombs that explode for 3-12 points of damage on planetary targets only.", 1, minimumWeaponDamage: 3, maximumWeaponDamage: 12, weaponRange: 1, genericCost: 3, genericSize: 40, genericPower: 10, numberOfShots: 10, weaponType: Technology.BOMB_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Hand Lasers", "Personal lasers that add 5 to your ground combat rolls.", 2));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Hyper V Rockets", "Swift missiles that explode for 6 points of damage and move at a speed of 2.5", 4, maximumWeaponDamage: 6, weaponRange: 2.5f, genericCost: 7, genericSize: 70, genericPower: 20, missileSpeed: 2.5f, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Gatling Lasers", "An advanced laser that fires up to four times per turn for 1-4 points of damage with each hit.", 5, minimumWeaponDamage: 1, maximumWeaponDamage: 4, weaponRange: 1, genericCost: 9, genericSize: 20, genericPower: 70, numberOfShots: 4, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Anti-missile Rockets", "Trans-light rockets capable of destroying incoming enemy missiles 40% of the time, -1% per technology level of the missile.", 6, smallCost: 10, smallSize: 2, smallPower: 8, mediumCost: 10, mediumSize: 10, mediumPower: 40, largeCost: 10, largeSize: 50, largePower: 200, hugeCost: 10, hugeSize: 250, hugePower: 1000, antiMissileRockets: true));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Neutron Pellet Gun", "Heavy particle stream weapon that halves the effectiveness of enemy deflector shields and inflicts 2-5 points of damage.", 7, minimumWeaponDamage: 2, maximumWeaponDamage: 5, weaponRange: 1, genericCost: 3, genericSize: 15, genericPower: 25, shieldPiercing: true, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Hyper X Rockets", "Missiles equipped with high energy warheads that explode for 8 points of damage, move at a speed of 2.5, and are controlled by a +1 level targeting computer.", 8, maximumWeaponDamage: 8, weaponRange: 2.5f, genericCost: 12, genericSize: 100, genericPower: 20, targetingBonus: 1, missileSpeed: 2.5f, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Fusion Bomb", "Bombs that explode for 5-20 points of damage on planetary targets only.", 9, minimumWeaponDamage: 5, maximumWeaponDamage: 20, weaponRange: 1, genericCost: 4, genericSize: 50, genericPower: 10, numberOfShots: 10, weaponType: Technology.BOMB_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Ion Cannon", "High intensity beam weapons capable of inflicting 3-8 points of damage. Heavy ion cannons strike for 3-15 and have a 2 space range.", 10, secondaryName: "Heavy Ion Cannon", minimumWeaponDamage: 3, maximumWeaponDamage: 8, weaponRange: 1, genericCost: 4, genericSize: 15, genericPower: 35, minimumSecondaryWeaponDamage: 3, maximumSecondaryWeaponDamage: 15, genericSecondaryCost: 11, genericSecondarySize: 45, genericSecondaryPower: 105, secondaryWeaponRange: 2, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Scatter Pack V Missiles", "Mirv versions of Hyper-V Rockets, splitting into five separate warheads that each explode for 6 points of damage and move at a speed of 2.5", 11, maximumWeaponDamage: 6, weaponRange: 5, genericCost: 18, genericSize: 115, genericPower: 50, numberOfShots: 5, missileSpeed: 2.5f, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Ion Rifle", "Personal beam weapons that add 10 to your ground attacks.", 12));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Mass Driver", "A linear accelerator that halves the effectiveness of enemy deflector shields and inflicts 5-8 points of damage.", 13, minimumWeaponDamage: 5, maximumWeaponDamage: 8, weaponRange: 1, genericCost: 9, genericSize: 55, genericPower: 50, shieldPiercing: true, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Merculite Missiles", "Hard-hitting, swift missiles that explode for 10 points of damage, move at a speed of 3, and are controlled by a +2 level targeting computer.", 14, maximumWeaponDamage: 10, weaponRange: 3, genericCost: 13, genericSize: 105, genericPower: 20, targetingBonus: 2, missileSpeed: 3, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Neutron Blaster", "High powered beam weapons capable of inflicting 3-12 points of damage. Heavy neutron blasters strike for 3-24 points and have a 2 space range.", 15, secondaryName: "Heavy Blast Cannon", minimumWeaponDamage: 3, maximumWeaponDamage: 12, weaponRange: 1, genericCost: 6, genericSize: 20, genericPower: 60, minimumSecondaryWeaponDamage: 3, maximumSecondaryWeaponDamage: 24, genericSecondaryCost: 18, genericSecondarySize: 60, genericSecondaryPower: 180, secondaryWeaponRange: 2, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Anti-matter Bomb", "Bombs that explode for 10-40 points of damage on planetary targets only.", 16, minimumWeaponDamage: 10, maximumWeaponDamage: 40, weaponRange: 1, genericCost: 5, genericSize: 75, genericPower: 10, numberOfShots: 10, weaponType: Technology.BOMB_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Graviton Beam", "Tractor-repulsor beam capable of rending ships to pieces. The beam strikes for 1-15, and the continuous streaming effect of the ray allows damage to carry over from one ship to another.", 17, minimumWeaponDamage: 1, maximumWeaponDamage: 15, weaponRange: 1, genericCost: 6, genericSize: 30, genericPower: 60, streaming: true, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Stinger Missiles", "Slow, hyper-accurate missiles that do 15 points of damage, move at a speed of 3.5, and are controlled by a sophisticated +3 level targeting computer.", 18, maximumWeaponDamage: 15, weaponRange: 3.5f, genericCost: 15, genericSize: 155, genericPower: 30, targetingBonus: 3, missileSpeed: 3.5f, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Hard Beam", "An energy-to-matter beam weapon that halves the effectiveness of enemy deflector shields, and inflicts 8-12 points of damage.", 19, minimumWeaponDamage: 8, maximumWeaponDamage: 12, weaponRange: 1, genericCost: 12, genericSize: 50, genericPower: 100, shieldPiercing: true, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Fusion Beam", "High intensity beam weapon capable of doing 4-16 points of damage. Heavy fusion beams strike for 4-30 points and have a 2 space range.", 20, secondaryName: "Heavy Fusion Beam", minimumWeaponDamage: 4, maximumWeaponDamage: 16, weaponRange: 1, genericCost: 7, genericSize: 20, genericPower: 75, minimumSecondaryWeaponDamage: 4, maximumSecondaryWeaponDamage: 30, genericSecondaryCost: 21, genericSecondarySize: 60, genericSecondaryPower: 225, secondaryWeaponRange: 2, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Ion Stream Projector", "Fires an intense ionic blast reducing an opponents armor by 20% plus 1% per two firing ships. The projector has a range of 2 spaces.", 21, genericCost: 100, genericSize: 250, genericPower: 500, ionStreamProjector: true));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Omega-V Bomb", "High yield bombs that explode for 20-50 points of damage on planetary targets only.", 22, minimumWeaponDamage: 20, maximumWeaponDamage: 50, weaponRange: 1, genericCost: 8, genericSize: 140, genericPower: 10, numberOfShots: 10, weaponType: Technology.BOMB_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Anti-matter Torpedoes", "High energy tracking torpedoes that deliver 30 points of damage but may only be fired every other turn. Each torpedo is equipped with a +4 level targeting computer.", 23, maximumWeaponDamage: 30, weaponRange: 8, genericCost: 30, genericSize: 75, genericPower: 300, targetingBonus: 4, missileSpeed: 4, weaponType: Technology.TORPEDO_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Fusion Rifle", "Inaccurate but incredibly powerful beam weapons that add 20 to your ground combat rolls.", 24));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Megabolt Cannon", "Releases multiple bolts of pure energy in a wide field. It has a +30% bonus chance to hit and strikes for 2-20 points of damage.", 25, minimumWeaponDamage: 2, maximumWeaponDamage: 20, weaponRange: 1, genericCost: 8, genericSize: 30, genericPower: 65, targetingBonus: 3, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Phasor", "Phased energy beams capable of inflicting 5-20 points of damage. Heavy phasors strike for 5-40 points of damage and have a 2 space range.", 26, secondaryName: "Heavy Phasor", minimumWeaponDamage: 5, maximumWeaponDamage: 20, weaponRange: 1, genericCost: 9, genericSize: 20, genericPower: 90, minimumSecondaryWeaponDamage: 5, maximumSecondaryWeaponDamage: 40, genericSecondaryCost: 26, genericSecondarySize: 60, genericSecondaryPower: 270, secondaryWeaponRange: 2, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Scatter Pack VII Missiles", "Mirv versions of Hyper-X Rockets, splitting into seven separate warheads that each explode for 10 points of damage, move at a speed of 3, and are guided by a +2 level targeting computer.", 27, maximumWeaponDamage: 10, weaponRange: 3, genericCost: 28, genericSize: 170, genericPower: 50, numberOfShots: 7, targetingBonus: 2, missileSpeed: 3, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Auto Blaster", "An advanced neutron blaster that fires up to three times per turn for 4-16 points of damage with each hit.", 28, minimumWeaponDamage: 4, maximumWeaponDamage: 16, weaponRange: 1, genericCost: 14, genericSize: 30, genericPower: 90, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Pulson Missiles", "Powerful missiles equipped with anti-matter warheads that explode for 20 points of damage, move at speed 4, and are controlled by a +4 level targeting computer.", 29, maximumWeaponDamage: 20, weaponRange: 4, genericCost: 20, genericSize: 160, genericPower: 40, targetingBonus: 4, missileSpeed: 4, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Tachyon Beam", "Fires an intense stream of tachyon particles that strike enemy ships for 1-25 hits. The continous streaming effect of the ray allows it to carry damage over from one ship to another.", 30, minimumWeaponDamage: 1, maximumWeaponDamage: 25, weaponRange: 1, genericCost: 9, genericSize: 30, genericPower: 80, streaming: true, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Fusion Rifle", "Potent hand held energy weapons capable of reducing an opponent to his component atoms. Adds 25 to your ground combat rolls.", 31));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Gauss Autocannon", "An advanced linear accelerator capable of firing four explosive rounds per turn that inflict 7-10 points of damage each. The projectile rounds half the effectiveness of enemy shields.", 32, minimumWeaponDamage: 7, maximumWeaponDamage: 10, weaponRange: 1, genericCost: 28, genericSize: 105, genericPower: 105, numberOfShots: 4, shieldPiercing: true, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Particle Beam", "High intensity particle accelerators capable of striking enemy ships for 10-20 points of damage and halving the effectiveness of deflector shields.", 33, minimumWeaponDamage: 10, maximumWeaponDamage: 20, weaponRange: 1, genericCost: 15, genericSize: 90, genericPower: 75, shieldPiercing: true, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Hercular Missiles", "Highly advanced missile that explodes for 25 points of damage. The hercular missile moves at speed 4.5 and is controlled by a +5 level targeting computer.", 34, maximumWeaponDamage: 25, weaponRange: 4.5f, genericCost: 26, genericSize: 220, genericPower: 40, targetingBonus: 5, missileSpeed: 4.5f, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Plasma Cannon", "Fires intense bolts of energy that inflict 6-30 points of damage.", 35, minimumWeaponDamage: 6, maximumWeaponDamage: 30, weaponRange: 1, genericCost: 12, genericSize: 30, genericPower: 110, weaponType: Technology.BEAM_WEAPON));
			//WeaponTechs.Add(new Technology("Death Ray", "An ancient weapon of unbelievably destructive power that inflicts 200 to 1000 points of damage and has a one-space range.", 36, minimumWeaponDamage: 200, maximumWeaponDamage: 1000, weaponRange: 1, genericCost: 12, genericSize: 30, genericPower: 110, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Disruptor", "Unleashes tremendous bolts of pure energy that can strike enemy targets up to 2 spaces away for 10-40 points of damage.", 37, minimumWeaponDamage: 10, maximumWeaponDamage: 40, weaponRange: 2, genericCost: 21, genericSize: 70, genericPower: 160, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Pulse Phasor", "An advanced phasor capable of firing three bursts per turn for 5-20 points of damage with each hit.", 38, minimumWeaponDamage: 5, maximumWeaponDamage: 20, weaponRange: 1, genericCost: 25, genericSize: 40, genericPower: 120, numberOfShots: 3, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Neutronium Bomb", "Devastating bombs that explode for 40-70 points of damage on planets only.", 39, minimumWeaponDamage: 40, maximumWeaponDamage: 70, weaponRange: 1, genericCost: 10, genericSize: 200, genericPower: 10, numberOfShots: 10, weaponType: Technology.BOMB_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Hellfire Torpedoes", "Enveloping energy torpedoes that simultaneously strike all shields, delivering damage equivalent to four 25 point attacks. They may only be fired once every other turn.", 40, maximumWeaponDamage: 25, weaponRange: 10, genericCost: 50, genericSize: 150, genericPower: 350, targetingBonus: 6, missileSpeed: 5, enveloping: true, weaponType: Technology.TORPEDO_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Zeon Missiles", "Most advanced missile available. Capable of striking enemy ships for 30 points of damage and moving at a speed of 5. The zeon missile is guided by a +6 level targeting computer.", 41, maximumWeaponDamage: 30, weaponRange: 5, genericCost: 30, genericSize: 250, genericPower: 50, targetingBonus: 6, missileSpeed: 5, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Plasma Rifle", "The most devastating hand held weapon available. Adds 30 to your ground attacks.", 42));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Proton Torpedoes", "High yield energy torpedoes that deliver 75 points of damage but may only be fired every other turn. Each torpedo is equipped with a +6 level targeting computer.", 43, maximumWeaponDamage: 60, weaponRange: 10, genericCost: 50, genericSize: 100, genericPower: 400, targetingBonus: 6, missileSpeed: 8, weaponType: Technology.TORPEDO_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Scatter Pack X Missiles", "Mirv versions of Stinger Missiles, splitting into ten separate warheads that each explode for 15 points of damage, move at speed 3.5, and are guided by a +3 level targeting computer.", 44, maximumWeaponDamage: 15, weaponRange: 3.5f, genericCost: 30, genericSize: 250, genericPower: 50, numberOfShots: 10, targetingBonus: 3, missileSpeed: 3.5f, weaponType: Technology.MISSILE_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Tri Focus Plasma Cannon", "Fires a triad of high intensity plasma beams capable of inflicting 20-50 points of damage.", 45, minimumWeaponDamage: 20, maximumWeaponDamage: 50, weaponRange: 1, genericCost: 25, genericSize: 70, genericPower: 180, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Stellar Converter", "Surrounds the target with an extremely powerful matter-energy conversion field, inflicting four 10-35 point attacks.  It has a range of 3 spaces.", 46, minimumWeaponDamage: 10, maximumWeaponDamage: 35, weaponRange: 3, genericCost: 50, genericSize: 200, genericPower: 300, enveloping: true, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Neutron Stream Projector", "Fires a blast of concentrated neutrino rays reducing an opponents armor by 40% plus 1% per firing ship. The projector has a range of 2 spaces.", 47, genericCost: 200, genericSize: 500, genericPower: 1250, neutronStreamProjector: true));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Mauler Device", "Unleashes enormous amounts of focused energy at enemy targets, inflicting 20-100 points of damage.", 48, minimumWeaponDamage: 20, maximumWeaponDamage: 100, weaponRange: 1, genericCost: 55, genericSize: 150, genericPower: 300, weaponType: Technology.BEAM_WEAPON));
			WeaponTechs.Add(new Technology(TechField.WEAPON, "Plasma Torpedoes", "Pure energy torpedoes that deliver 150 points of damage, but lose 15 strength per space traveled. The launcher fires every other turn and has a +7 level guidance computer.", 50, maximumWeaponDamage: 150, weaponRange: 10, genericCost: 150, genericSize: 150, genericPower: 450, targetingBonus: 7, missileSpeed: 6, dissipating: true, weaponType: Technology.TORPEDO_WEAPON));
		}
		#endregion

		public List<Technology> GetRandomizedComputerTechs()
		{
			while (true)
			{
				//Must include at least one robotic factory tech, and at least one tech from each tier
				List<Technology> randomList = new List<Technology>();
				for (int i = 0; i < 10; i++)
				{
					bool hasAtLeastOneTierTech = false;
					List<Technology> randomTierList = new List<Technology>();
					while (!hasAtLeastOneTierTech)
					{
						randomTierList = new List<Technology>();
						foreach (var tech in ComputerTechs)
						{
							if (tech.TechLevel == 1 && i == 0)
							{
								//Include starting levels if on first tier
								randomTierList.Add(tech);
							}
							else if (tech.TechLevel > (i * 5) && tech.TechLevel <= (i + 1) * 5 && _gameMain.Random.Next(100) < 50)
							{
								randomTierList.Add(tech);
								hasAtLeastOneTierTech = true;
							}
						}
					}
					randomList.AddRange(randomTierList);
				}
				foreach (var tech in randomList)
				{
					if (tech.RoboticControl > 2)
					{
						return randomList;
					}
				}
			}
		}
		public List<Technology> GetRandomizedConstructionTechs()
		{
			//Must include at least one tech from each tier
			List<Technology> randomList = new List<Technology>();
			for (int i = 0; i < 10; i++)
			{
				bool hasAtLeastOneTierTech = false;
				List<Technology> randomTierList = new List<Technology>();
				while (!hasAtLeastOneTierTech)
				{
					randomTierList = new List<Technology>();
					foreach (var tech in ConstructionTechs)
					{
						if (tech.TechLevel == 1 && i == 0)
						{
							//Include starting levels if on first tier
							randomTierList.Add(tech);
						}
						else if (tech.TechLevel > (i * 5) && tech.TechLevel <= (i + 1) * 5 && _gameMain.Random.Next(100) < 50)
						{
							randomTierList.Add(tech);
							hasAtLeastOneTierTech = true;
						}
					}
				}
				randomList.AddRange(randomTierList);
			}
			return randomList;
		}
		public List<Technology> GetRandomizedForceFieldTechs()
		{
			while (true)
			{
				//Must include at least one planetary shield tech, and at least one tech from each tier
				List<Technology> randomList = new List<Technology>();
				for (int i = 0; i < 10; i++)
				{
					bool hasAtLeastOneTierTech = false;
					List<Technology> randomTierList = new List<Technology>();
					while (!hasAtLeastOneTierTech)
					{
						randomTierList = new List<Technology>();
						foreach (var tech in ForceFieldTechs)
						{
							if (tech.TechLevel == 1 && i == 0)
							{
								//Include starting levels if on first tier
								randomTierList.Add(tech);
							}
							else if (tech.TechLevel > (i * 5) && tech.TechLevel <= (i + 1) * 5 && _gameMain.Random.Next(100) < 50)
							{
								randomTierList.Add(tech);
								hasAtLeastOneTierTech = true;
							}
						}
					}
					randomList.AddRange(randomTierList);
				}
				foreach (var tech in randomList)
				{
					if (tech.PlanetaryShield > 0)
					{
						return randomList;
					}
				}
			}
		}
		public List<Technology> GetRandomizedPlanetologyTechs()
		{
			//Must include at least one tech from each tier
			List<Technology> randomList = new List<Technology>();
			for (int i = 0; i < 10; i++)
			{
				bool hasAtLeastOneTierTech = false;
				List<Technology> randomTierList = new List<Technology>();
				while (!hasAtLeastOneTierTech)
				{
					randomTierList = new List<Technology>();
					foreach (var tech in PlanetologyTechs)
					{
						if (tech.TechLevel == 1 && i == 0)
						{
							//Include starting levels if on first tier
							randomTierList.Add(tech);
						}
						else if (tech.TechLevel > (i * 5) && tech.TechLevel <= (i + 1) * 5 && _gameMain.Random.Next(100) < 50)
						{
							randomTierList.Add(tech);
							hasAtLeastOneTierTech = true;
						}
					}
				}
				randomList.AddRange(randomTierList);
			}
			return randomList;
		}
		public List<Technology> GetRandomizedPropulsionTechs()
		{
			while (true)
			{
				//Must include at least one of 4 or 5 fuel range, and at least one tech from each tier
				List<Technology> randomList = new List<Technology>();
				for (int i = 0; i < 10; i++)
				{
					bool hasAtLeastOneTierTech = false;
					List<Technology> randomTierList = new List<Technology>();
					while (!hasAtLeastOneTierTech)
					{
						randomTierList = new List<Technology>();
						foreach (var tech in PropulsionTechs)
						{
							if (tech.TechLevel == 1 && i == 0)
							{
								//Include starting levels if on first tier
								randomTierList.Add(tech);
							}
							else if (tech.TechLevel > (i * 5) && tech.TechLevel <= (i + 1) * 5 && _gameMain.Random.Next(100) < 50)
							{
								randomTierList.Add(tech);
								hasAtLeastOneTierTech = true;
							}
						}
					}
					randomList.AddRange(randomTierList);
				}
				foreach (var tech in randomList)
				{
					if (tech.FuelRange == 4 || tech.FuelRange == 5)
					{
						return randomList;
					}
				}
			}
		}
		public List<Technology> GetRandomizedWeaponTechs()
		{
			while (true)
			{
				//Must include at least one missile or torpedo weapon, and at least one tech from each tier
				List<Technology> randomList = new List<Technology>();
				for (int i = 0; i < 10; i++)
				{
					bool hasAtLeastOneTierTech = false;
					List<Technology> randomTierList = new List<Technology>();
					while (!hasAtLeastOneTierTech)
					{
						randomTierList = new List<Technology>();
						foreach (var tech in WeaponTechs)
						{
							if (tech.TechLevel == 1 && i == 0)
							{
								//Include starting levels if on first tier
								randomTierList.Add(tech);
							}
							else if (tech.TechLevel > (i * 5) && tech.TechLevel <= (i + 1) * 5 && _gameMain.Random.Next(100) < 50)
							{
								randomTierList.Add(tech);
								hasAtLeastOneTierTech = true;
							}
						}
					}
					randomList.AddRange(randomTierList);
				}
				foreach (var tech in randomList)
				{
					if (tech.WeaponType == Technology.MISSILE_WEAPON || tech.WeaponType == Technology.TORPEDO_WEAPON)
					{
						return randomList;
					}
				}
			}
		}
	}
}
