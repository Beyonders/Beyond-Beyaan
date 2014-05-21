using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class Ship
	{
		public const int SMALL = 0;
		public const int MEDIUM = 1;
		public const int LARGE = 2;
		public const int HUGE = 3;

		#region Properties
		public string Name { get; set; }
		public int DesignID { get; set; }
		public Empire Owner { get; set; }
		public int Size { get; set; }
		public int WhichStyle { get; set; }
		public KeyValuePair<Equipment, float> Engine;
		public int ManeuverSpeed = 1;
		public Equipment Shield;
		public Equipment Armor;
		public Equipment Computer;
		public Equipment ECM;
		public KeyValuePair<Equipment, int>[] Weapons = new KeyValuePair<Equipment, int>[4];
		public Equipment[] Specials = new Equipment[3];
		public float Maintenance { get { return Cost * 0.02f; } }
		public int TotalSpace 
		{ 
			get 
			{
				int space = 40;
				switch (Size)
				{
					case MEDIUM: 
						space = 200;
						break;
					case LARGE:
						space = 1000;
						break;
					case HUGE:
						space = 5000;
						break;
				}
				space = (int)(space * (1.0 + (0.02 * Owner.TechnologyManager.ConstructionLevel)));
				return space;
			} 
		}
		public float Cost
		{
			get
			{
				float cost = 6;
				switch (Size)
				{
					case MEDIUM:
						cost = 36;
						break;
					case LARGE:
						cost = 200;
						break;
					case HUGE:
						cost = 1200;
						break;
				}
				var fieldLevels = Owner.TechnologyManager.GetFieldLevels();
				cost += Engine.Key.GetCost(fieldLevels, Size) * Engine.Value;
				cost += Armor.GetCost(fieldLevels, Size);
				if (Shield != null)
				{
					cost += Shield.GetCost(fieldLevels, Size);
				}
				if (Computer != null)
				{
					cost += Computer.GetCost(fieldLevels, Size);
				}
				if (ECM != null)
				{
					cost += ECM.GetCost(fieldLevels, Size);
				}
				foreach (var weapon in Weapons)
				{
					if (weapon.Key != null)
					{
						//Weapon times amount of mounts
						cost += weapon.Key.GetCost(fieldLevels, Size) * weapon.Value;
					}
				}
				foreach (var special in Specials)
				{
					if (special != null)
					{
						cost += special.GetCost(fieldLevels, Size);
					}
				}
				return cost;
			}
		}
		public float SpaceUsed
		{
			get
			{
				float sizeUsed = 0;
				var fieldLevels = Owner.TechnologyManager.GetFieldLevels();
				sizeUsed += Engine.Key.GetSize(fieldLevels, Size) * Engine.Value;
				sizeUsed += Armor.GetSize(fieldLevels, Size);
				if (Shield != null)
				{
					sizeUsed += Shield.GetSize(fieldLevels, Size);
				}
				if (Computer != null)
				{
					sizeUsed += Computer.GetSize(fieldLevels, Size);
				}
				if (ECM != null)
				{
					sizeUsed += ECM.GetSize(fieldLevels, Size);
				}
				foreach (var weapon in Weapons)
				{
					if (weapon.Key != null)
					{
						//Weapon times amount of mounts
						sizeUsed += weapon.Key.GetSize(fieldLevels, Size) * weapon.Value;
					}
				}
				foreach (var special in Specials)
				{
					if (special != null)
					{
						sizeUsed += special.GetSize(fieldLevels, Size);
					}
				}
				return sizeUsed;
			}
		}
		public float PowerUsed
		{
			get
			{
				float powerUsed = 0;
				//First, get the maneuver power requirement
				switch (Size)
				{
					case SMALL:
						powerUsed += ManeuverSpeed * 2;
						break;
					case MEDIUM:
						powerUsed += ManeuverSpeed * 15;
						break;
					case LARGE:
						powerUsed += ManeuverSpeed * 100;
						break;
					case HUGE:
						powerUsed += ManeuverSpeed * 700;
						break;
				}
				//since engines provide power, do NOT include engines in this total
				//Armor don't use up power, but perhaps in a mod?  For now, don't include armor as well
				if (Computer != null)
				{
					powerUsed += Computer.GetPower(Size);
				}
				if (ECM != null)
				{
					powerUsed += ECM.GetPower(Size);
				}
				if (Shield != null)
				{
					powerUsed += Shield.GetPower(Size);
				}
				foreach (var weapon in Weapons)
				{
					if (weapon.Key != null)
					{
						powerUsed += weapon.Key.GetPower(Size) * weapon.Value;
					}
				}
				foreach (var special in Specials)
				{
					if (special != null)
					{
						powerUsed += special.GetPower(Size);
					}
				}
				return powerUsed;
			}
		}
		public int GalaxySpeed
		{
			get { return Engine.Key.Technology.Speed; }
		}
		public int MaxHitPoints
		{
			get
			{
				switch (Size)
				{
					case SMALL:
						return (int)(Armor.UseSecondary ? Armor.Technology.SmallSecondaryHP : Armor.Technology.SmallHP);
					case MEDIUM:
						return (int)(Armor.UseSecondary ? Armor.Technology.MediumSecondaryHP : Armor.Technology.MediumHP);
					case LARGE:
						return (int)(Armor.UseSecondary ? Armor.Technology.LargeSecondaryHP : Armor.Technology.LargeHP);
					case HUGE:
						return (int)(Armor.UseSecondary ? Armor.Technology.HugeSecondaryHP : Armor.Technology.HugeHP);
				}
				return 0;
			}
		}
		public int BeamDefense
		{
			get
			{
				return (2 - Size) + ManeuverSpeed; // TODO: Add other equipment that adds to defense, such as cloaking
			}
		}
		public int MissileDefense
		{
			get
			{
				return  ((2 - Size) + ManeuverSpeed + (ECM == null ? 0 : ECM.Technology.ECM));
			}
		}
		public int AttackLevel
		{
			get
			{
				return (Computer == null ? 0 : Computer.Technology.BattleComputer); //Todo: add battle scanner's +1 if ship has it
			}
		}
		#endregion

		#region Constructors
		public Ship()
		{
		}
		public Ship(Ship shipToCopy)
		{
			Name = shipToCopy.Name;
			DesignID = shipToCopy.DesignID;
			Owner = shipToCopy.Owner;
			Size = shipToCopy.Size;
			WhichStyle = shipToCopy.WhichStyle;
			Engine = new KeyValuePair<Equipment, float>(shipToCopy.Engine.Key, shipToCopy.Engine.Value);
			Shield = shipToCopy.Shield;
			Armor = shipToCopy.Armor;
			Computer = shipToCopy.Computer;
			ECM = shipToCopy.ECM;
			Array.Copy(shipToCopy.Weapons, Weapons, Weapons.Length);
			Array.Copy(shipToCopy.Specials, Specials, Specials.Length);
		}
		#endregion

		#region Save/Load
		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("ShipDesign");
			writer.WriteAttributeString("Name", Name);
			writer.WriteAttributeString("DesignID", DesignID.ToString());
			writer.WriteAttributeString("Size", Size.ToString());
			writer.WriteAttributeString("WhichStyle", WhichStyle.ToString());
			writer.WriteAttributeString("Engine", Engine.Key.EquipmentName);
			writer.WriteAttributeString("NumOfEngines", Engine.Value.ToString());
			writer.WriteAttributeString("Armor", Armor.EquipmentName);
			writer.WriteAttributeString("Shield", Shield == null ? "" : Shield.EquipmentName);
			writer.WriteAttributeString("Computer", Computer == null ? "" : Computer.EquipmentName);
			writer.WriteAttributeString("ECM", ECM == null ? "" : ECM.EquipmentName);
			foreach (var weapon in Weapons)
			{
				writer.WriteStartElement("Weapon");
				if (weapon.Key == null)
				{
					writer.WriteAttributeString("Name", "null");
				}
				else
				{
					writer.WriteAttributeString("Name", weapon.Key.EquipmentName);
					writer.WriteAttributeString("NumOfMounts", weapon.Value.ToString());
					writer.WriteAttributeString("IsSecondary", weapon.Key.UseSecondary.ToString());
				}
				writer.WriteEndElement();
			}
			foreach (var special in Specials)
			{
				writer.WriteStartElement("Special");
				if (special == null)
				{
					writer.WriteAttributeString("Name", "null");
				}
				else
				{
					writer.WriteAttributeString("Name", special.EquipmentName);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		public void Load(XElement shipDesign, GameMain gameMain)
		{
			Name = shipDesign.Attribute("Name").Value;
			DesignID = int.Parse(shipDesign.Attribute("DesignID").Value);
			Size = int.Parse(shipDesign.Attribute("Size").Value);
			WhichStyle = int.Parse(shipDesign.Attribute("WhichStyle").Value);
			Engine = new KeyValuePair<Equipment, float>(LoadEquipment(shipDesign.Attribute("Engine").Value, gameMain), float.Parse(shipDesign.Attribute("NumOfEngines").Value));
			Armor = LoadEquipment(shipDesign.Attribute("Armor").Value, gameMain);
			Shield = LoadEquipment(shipDesign.Attribute("Shield").Value, gameMain);
			Computer = LoadEquipment(shipDesign.Attribute("Computer").Value, gameMain);
			ECM = LoadEquipment(shipDesign.Attribute("ECM").Value, gameMain);
			int iter = 0;
			foreach (var weapon in shipDesign.Elements("Weapon"))
			{
				if (weapon.Attribute("Name").Value == "null")
				{
					Weapons[iter] = new KeyValuePair<Equipment, int>();
				}
				else
				{
					var weaponTech = LoadEquipment(weapon.Attribute("Name").Value, gameMain);
					weaponTech.UseSecondary = bool.Parse(weapon.Attribute("IsSecondary").Value);
					Weapons[iter] = new KeyValuePair<Equipment, int>(weaponTech, int.Parse(weapon.Attribute("NumOfMounts").Value));

				}
				iter++;
			}
			iter = 0;
			foreach (var special in shipDesign.Elements("Special"))
			{
				if (special.Attribute("Name").Value == "null")
				{
					Specials[iter] = null;
				}
				else
				{
					var specialTech = LoadEquipment(special.Attribute("Name").Value, gameMain);
					Specials[iter] = specialTech;
				}
				iter++;
			}
		}
		private static Equipment LoadEquipment(string equipmentName, GameMain gameMain)
		{
			bool useSecondary = equipmentName.EndsWith("|Sec");
			string actualTechName = useSecondary ? equipmentName.Substring(0, equipmentName.Length - 4) : equipmentName;
			return new Equipment(gameMain.MasterTechnologyManager.GetTechnologyWithName(actualTechName), useSecondary);
		}
		#endregion

		public void UpdateEngineNumber()
		{
			float amountRequired = PowerUsed / (Engine.Key.Technology.Speed * 10);
			Engine = new KeyValuePair<Equipment, float>(Engine.Key, amountRequired);
		}

		public void Clear(List<Technology> availableArmorTechs, List<Technology> availableEngineTechs)
		{
			//Used by ship design screen to clear out everything
			for (int i = 0; i < Weapons.Length; i++)
			{
				Weapons[i] = new KeyValuePair<Equipment, int>();
			}
			for (int i = 0; i < Specials.Length; i++)
			{
				Specials[i] = null;
			}
			ECM = null;
			Computer = null;
			Shield = null;
			ManeuverSpeed = 1;
			Armor = new Equipment(availableArmorTechs[0], false);
			Engine = new KeyValuePair<Equipment, float>(new Equipment(availableEngineTechs[0], false), 0);
			UpdateEngineNumber();
		}
	}

	public class TransportShip
	{
		public Race raceOnShip;
		public int amount;
	}
}
