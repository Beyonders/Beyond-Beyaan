using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum TechField { COMPUTER, CONSTRUCTION, FORCE_FIELD, PLANETOLOGY, PROPULSION, WEAPON }

	public class TechnologyManager
	{
		public const int COST_MODIFIER = 40;

		#region Properties
		public List<Technology> ResearchedComputerTechs { get; private set; }
		public List<Technology> ResearchedConstructionTechs { get; private set; }
		public List<Technology> ResearchedForceFieldTechs { get; private set; }
		public List<Technology> ResearchedPlanetologyTechs { get; private set; }
		public List<Technology> ResearchedPropulsionTechs { get; private set; }
		public List<Technology> ResearchedWeaponTechs { get; private set; }

		public List<Technology> UnresearchedComputerTechs { get; private set; }
		public List<Technology> UnresearchedConstructionTechs { get; private set; }
		public List<Technology> UnresearchedForceFieldTechs { get; private set; }
		public List<Technology> UnresearchedPlanetologyTechs { get; private set; }
		public List<Technology> UnresearchedPropulsionTechs { get; private set; }
		public List<Technology> UnresearchedWeaponTechs { get; private set; }

		public Technology WhichComputerBeingResearched { get; set; }
		public Technology WhichConstructionBeingResearched { get; set; }
		public Technology WhichForceFieldBeingResearched { get; set; }
		public Technology WhichPlanetologyBeingResearched { get; set; }
		public Technology WhichPropulsionBeingResearched { get; set; }
		public Technology WhichWeaponBeingResearched { get; set; }

		public float ComputerResearchAmount { get; set; }
		public float ConstructionResearchAmount { get; set; }
		public float ForceFieldResearchAmount { get; set; }
		public float PlanetologyResearchAmount { get; set; }
		public float PropulsionResearchAmount { get; set; }
		public float WeaponResearchAmount { get; set; }

		public int ComputerLevel 
		{ 
			get	
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedComputerTechs.Count; i++)
				{
					if (ResearchedComputerTechs[i].TechLevel > level)
					{
						level = ResearchedComputerTechs[i].TechLevel;
					}
					if (ResearchedComputerTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedComputerTechs.Count + 1 - numberOfStartingTechs;
				return Math.Min(level, 99);
			}
		}
		public int ConstructionLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedConstructionTechs.Count; i++)
				{
					if (ResearchedConstructionTechs[i].TechLevel > level)
					{
						level = ResearchedConstructionTechs[i].TechLevel;
					}
					if (ResearchedConstructionTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedConstructionTechs.Count + 1 - numberOfStartingTechs;
				return Math.Min(level, 99);
			}
		}
		public int ForceFieldLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedForceFieldTechs.Count; i++)
				{
					if (ResearchedForceFieldTechs[i].TechLevel > level)
					{
						level = ResearchedForceFieldTechs[i].TechLevel;
					}
					if (ResearchedForceFieldTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedForceFieldTechs.Count + 1 - numberOfStartingTechs;
				return Math.Min(level, 99);
			}
		}
		public int PlanetologyLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedPlanetologyTechs.Count; i++)
				{
					if (ResearchedPlanetologyTechs[i].TechLevel > level)
					{
						level = ResearchedPlanetologyTechs[i].TechLevel;
					}
					if (ResearchedPlanetologyTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedPlanetologyTechs.Count + 1 - numberOfStartingTechs;
				return Math.Min(level, 99);
			}
		}
		public int PropulsionLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedPropulsionTechs.Count; i++)
				{
					if (ResearchedPropulsionTechs[i].TechLevel > level)
					{
						level = ResearchedPropulsionTechs[i].TechLevel;
					}
					if (ResearchedPropulsionTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedPropulsionTechs.Count + 1 - numberOfStartingTechs;
				return Math.Min(level, 99);
			}
		}
		public int WeaponLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedWeaponTechs.Count; i++)
				{
					if (ResearchedWeaponTechs[i].TechLevel > level)
					{
						level = ResearchedWeaponTechs[i].TechLevel;
					}
					if (ResearchedWeaponTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedWeaponTechs.Count + 1 - numberOfStartingTechs;
				return Math.Min(level, 99);
			}
		}

		public bool ComputerLocked { get; set; }
		public bool ConstructionLocked { get; set; }
		public bool ForceFieldLocked { get; set; }
		public bool PlanetologyLocked { get; set; }
		public bool PropulsionLocked { get; set; }
		public bool WeaponLocked { get; set; }

		public int ComputerPercentage { get; private set; }
		public int ConstructionPercentage { get; private set; }
		public int ForceFieldPercentage { get; private set; }
		public int PlanetologyPercentage { get; private set; }
		public int PropulsionPercentage { get; private set; }
		public int WeaponPercentage { get; private set; }

		//1.25 for poor, 1.00 for average, .80 for good, and .60 for excellent
		public Dictionary<TechField,float> RaceModifiers { get; set; }
		#endregion

		#region Constructor
		public TechnologyManager()
		{
			//Set the initial starting percentages
			ComputerPercentage = 20;
			ConstructionPercentage = 10;
			ForceFieldPercentage = 15;
			PlanetologyPercentage = 15;
			PropulsionPercentage = 20;
			WeaponPercentage = 20;

			RaceModifiers = new Dictionary<TechField, float>();
			RaceModifiers[TechField.COMPUTER] = 1;
			RaceModifiers[TechField.CONSTRUCTION] = 1;
			RaceModifiers[TechField.FORCE_FIELD] = 1;
			RaceModifiers[TechField.PLANETOLOGY] = 1;
			RaceModifiers[TechField.PROPULSION] = 1;
			RaceModifiers[TechField.WEAPON] = 1;

			ResearchedComputerTechs = new List<Technology>();
			ResearchedConstructionTechs = new List<Technology>();
			ResearchedForceFieldTechs = new List<Technology>();
			ResearchedPlanetologyTechs = new List<Technology>();
			ResearchedPropulsionTechs = new List<Technology>();
			ResearchedWeaponTechs = new List<Technology>();

			UnresearchedComputerTechs = new List<Technology>();
			UnresearchedConstructionTechs = new List<Technology>();
			UnresearchedForceFieldTechs = new List<Technology>();
			UnresearchedPlanetologyTechs = new List<Technology>();
			UnresearchedPropulsionTechs = new List<Technology>();
			UnresearchedWeaponTechs = new List<Technology>();

			UpdateValues();
		}
		#endregion

		#region Functions
		public void SetComputerTechs(List<Technology> techs)
		{
			UnresearchedComputerTechs = new List<Technology>(techs);
			ResearchedComputerTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedComputerTechs.Add(tech);
					UnresearchedComputerTechs.Remove(tech);
				}
			}
		}
		public void SetConstructionTechs(List<Technology> techs)
		{
			UnresearchedConstructionTechs = new List<Technology>(techs);
			ResearchedConstructionTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedConstructionTechs.Add(tech);
					UnresearchedConstructionTechs.Remove(tech);
				}
			}
		}
		public void SetForceFieldTechs(List<Technology> techs)
		{
			UnresearchedForceFieldTechs = new List<Technology>(techs);
			ResearchedForceFieldTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedForceFieldTechs.Add(tech);
					UnresearchedForceFieldTechs.Remove(tech);
				}
			}
		}
		public void SetPlanetologyTechs(List<Technology> techs)
		{
			UnresearchedPlanetologyTechs = new List<Technology>(techs);
			ResearchedPlanetologyTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedPlanetologyTechs.Add(tech);
					UnresearchedPlanetologyTechs.Remove(tech);
				}
			}
		}
		public void SetPropulsionTechs(List<Technology> techs)
		{
			UnresearchedPropulsionTechs = new List<Technology>(techs);
			ResearchedPropulsionTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedPropulsionTechs.Add(tech);
					UnresearchedPropulsionTechs.Remove(tech);
				}
			}
		}
		public void SetWeaponTechs(List<Technology> techs)
		{
			UnresearchedWeaponTechs = new List<Technology>(techs);
			ResearchedWeaponTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedWeaponTechs.Add(tech);
					UnresearchedWeaponTechs.Remove(tech);
				}
			}
		}

		public Dictionary<TechField, int> GetFieldLevels()
		{
			Dictionary<TechField, int> fields = new Dictionary<TechField, int>();
			fields[TechField.COMPUTER] = ComputerLevel;
			fields[TechField.CONSTRUCTION] = ConstructionLevel;
			fields[TechField.FORCE_FIELD] = ForceFieldLevel;
			fields[TechField.PLANETOLOGY] = PlanetologyLevel;
			fields[TechField.PROPULSION] = PropulsionLevel;
			fields[TechField.WEAPON] = WeaponLevel;
			return fields;
		}

		public string GetFieldProgressString(TechField whichField, float researchPoints)
		{
			string progressString = string.Empty;
			int chance = 0;
			float amount = 0;
			float researchedAmount = 0;
			int researchCost = 0;
			switch (whichField)
			{
				case TechField.COMPUTER:
					{
						if (WhichComputerBeingResearched == null)
						{
							return "N/A";
						}
						amount = GetFieldInvestmentAmount(whichField, researchPoints);
						//Temporarily update the research amount to get accurate chance of discovery
						float oldAmount = ComputerResearchAmount;
						ComputerResearchAmount += amount;
						chance = GetChanceForDiscovery(whichField);
						ComputerResearchAmount = oldAmount;
						researchedAmount = ComputerResearchAmount;
						researchCost = (int)(WhichComputerBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.COMPUTER]);
					} break;
				case TechField.CONSTRUCTION:
					{
						if (WhichConstructionBeingResearched == null)
						{
							return "N/A";
						}
						amount = GetFieldInvestmentAmount(whichField, researchPoints);
						//Temporarily update the research amount to get accurate chance of discovery
						float oldAmount = ConstructionResearchAmount;
						ConstructionResearchAmount += amount;
						chance = GetChanceForDiscovery(whichField);
						ConstructionResearchAmount = oldAmount;
						researchedAmount = ConstructionResearchAmount;
						researchCost = (int)(WhichConstructionBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.CONSTRUCTION]);
					} break;
				case TechField.FORCE_FIELD:
					{
						if (WhichForceFieldBeingResearched == null)
						{
							return "N/A";
						}
						amount = GetFieldInvestmentAmount(whichField, researchPoints);
						//Temporarily update the research amount to get accurate chance of discovery
						float oldAmount = ForceFieldResearchAmount;
						ForceFieldResearchAmount += amount;
						chance = GetChanceForDiscovery(whichField);
						ForceFieldResearchAmount = oldAmount;
						researchedAmount = ForceFieldResearchAmount;
						researchCost = (int)(WhichForceFieldBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.FORCE_FIELD]);
					} break;
				case TechField.PLANETOLOGY:
					{
						if (WhichPlanetologyBeingResearched == null)
						{
							return "N/A";
						}
						amount = GetFieldInvestmentAmount(whichField, researchPoints);
						//Temporarily update the research amount to get accurate chance of discovery
						float oldAmount = PlanetologyResearchAmount;
						PlanetologyResearchAmount += amount;
						chance = GetChanceForDiscovery(whichField);
						PlanetologyResearchAmount = oldAmount;
						researchedAmount = PlanetologyResearchAmount;
						researchCost = (int)(WhichPlanetologyBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.PLANETOLOGY]);
					} break;
				case TechField.PROPULSION:
					{
						if (WhichPropulsionBeingResearched == null)
						{
							return "N/A";
						}
						amount = GetFieldInvestmentAmount(whichField, researchPoints);
						//Temporarily update the research amount to get accurate chance of discovery
						float oldAmount = PropulsionResearchAmount;
						PropulsionResearchAmount += amount;
						chance = GetChanceForDiscovery(whichField);
						PropulsionResearchAmount = oldAmount;
						researchedAmount = PropulsionResearchAmount;
						researchCost = (int)(WhichPropulsionBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.PROPULSION]);
					} break;
				case TechField.WEAPON:
					{
						if (WhichWeaponBeingResearched == null)
						{
							return "N/A";
						}
						amount = GetFieldInvestmentAmount(whichField, researchPoints);
						//Temporarily update the research amount to get accurate chance of discovery
						float oldAmount = WeaponResearchAmount;
						WeaponResearchAmount += amount;
						chance = GetChanceForDiscovery(whichField);
						WeaponResearchAmount = oldAmount;
						researchedAmount = WeaponResearchAmount;
						researchCost = (int)(WhichWeaponBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.WEAPON]);
					} break;
			}
			progressString = string.Format("{0:0.0} / {1:0.0}", researchedAmount, researchCost);
			progressString += " (" + (amount >= 0 ? " +" : "") + string.Format("{0:0.0}", amount) + ")";
			if (chance > 0)
			{
				progressString += " " + chance + "%";
			}
			
			return progressString;
		}

		public void AccureResearch(float researchPoints)
		{
			ComputerResearchAmount += GetFieldInvestmentAmount(TechField.COMPUTER, researchPoints);
			ConstructionResearchAmount += GetFieldInvestmentAmount(TechField.CONSTRUCTION, researchPoints);
			ForceFieldResearchAmount += GetFieldInvestmentAmount(TechField.FORCE_FIELD, researchPoints);
			PlanetologyResearchAmount += GetFieldInvestmentAmount(TechField.PLANETOLOGY, researchPoints);
			PropulsionResearchAmount += GetFieldInvestmentAmount(TechField.PROPULSION, researchPoints);
			WeaponResearchAmount += GetFieldInvestmentAmount(TechField.WEAPON, researchPoints);
		}

		public List<TechField> RollForDiscoveries(Random r, SitRepManager sitRepManager)
		{
			List<TechField> fieldsNeedingNewItems = new List<TechField>();
			//Only items being currently researched have a chance of being discovered, otherwise they degrade (handled in AccureResearch function)
			if (ComputerPercentage > 0)
			{
				if (WhichComputerBeingResearched == null && UnresearchedComputerTechs.Count > 0 && ComputerResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.COMPUTER);
				}
				else if (WhichComputerBeingResearched != null)
				{
					//See if it is discovered this turn
					int chance = GetChanceForDiscovery(TechField.COMPUTER);
					if (chance > 0 && (r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichComputerBeingResearched.TechName + " has been discovered."));
						ResearchedComputerTechs.Add(WhichComputerBeingResearched);
						UnresearchedComputerTechs.Remove(WhichComputerBeingResearched);
						ComputerResearchAmount = 0;
						fieldsNeedingNewItems.Add(TechField.COMPUTER);
					}
				}
			}

			if (ConstructionPercentage > 0)
			{
				if (WhichConstructionBeingResearched == null && UnresearchedConstructionTechs.Count > 0 && ConstructionResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.CONSTRUCTION);
				}
				else if (WhichConstructionBeingResearched != null)
				{
					//See if it is discovered this turn
					int chance = GetChanceForDiscovery(TechField.CONSTRUCTION);
					if (chance > 0 && (r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichConstructionBeingResearched.TechName + " has been discovered."));
						ResearchedConstructionTechs.Add(WhichConstructionBeingResearched);
						UnresearchedConstructionTechs.Remove(WhichConstructionBeingResearched);
						ConstructionResearchAmount = 0;
						fieldsNeedingNewItems.Add(TechField.CONSTRUCTION);
					}
				}
			}

			if (ForceFieldPercentage > 0)
			{
				if (WhichForceFieldBeingResearched == null && UnresearchedForceFieldTechs.Count > 0 && ForceFieldResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.FORCE_FIELD);
				}
				else if (WhichForceFieldBeingResearched != null)
				{
					//See if it is discovered this turn
					int chance = GetChanceForDiscovery(TechField.FORCE_FIELD);
					if (chance > 0 && (r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichForceFieldBeingResearched.TechName + " has been discovered."));
						ResearchedForceFieldTechs.Add(WhichForceFieldBeingResearched);
						UnresearchedForceFieldTechs.Remove(WhichForceFieldBeingResearched);
						ForceFieldResearchAmount = 0;
						fieldsNeedingNewItems.Add(TechField.FORCE_FIELD);
					}
				}
			}

			if (PlanetologyPercentage > 0)
			{
				if (WhichPlanetologyBeingResearched == null && UnresearchedPlanetologyTechs.Count > 0 && PlanetologyResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.PLANETOLOGY);
				}
				else if (WhichPlanetologyBeingResearched != null)
				{
					//See if it is discovered this turn
					int chance = GetChanceForDiscovery(TechField.PLANETOLOGY);
					if (chance > 0 && (r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichPlanetologyBeingResearched.TechName + " has been discovered."));
						ResearchedPlanetologyTechs.Add(WhichPlanetologyBeingResearched);
						UnresearchedPlanetologyTechs.Remove(WhichPlanetologyBeingResearched);
						PlanetologyResearchAmount = 0;
						fieldsNeedingNewItems.Add(TechField.PLANETOLOGY);
					}
				}
			}

			if (PropulsionPercentage > 0)
			{
				if (WhichPropulsionBeingResearched == null && UnresearchedPropulsionTechs.Count > 0 && PropulsionResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.PROPULSION);
				}
				else if (WhichPropulsionBeingResearched != null)
				{
					//See if it is discovered this turn
					int chance = GetChanceForDiscovery(TechField.PROPULSION);
					if (chance > 0 && (r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichPropulsionBeingResearched.TechName + " has been discovered."));
						ResearchedPropulsionTechs.Add(WhichPropulsionBeingResearched);
						UnresearchedPropulsionTechs.Remove(WhichPropulsionBeingResearched);
						PropulsionResearchAmount = 0;
						fieldsNeedingNewItems.Add(TechField.PROPULSION);
					}
				}
			}

			if (WeaponPercentage > 0)
			{
				if (WhichWeaponBeingResearched == null && UnresearchedWeaponTechs.Count > 0 && WeaponResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.WEAPON);
				}
				else if (WhichWeaponBeingResearched != null)
				{
					//See if it is discovered this turn
					int chance = GetChanceForDiscovery(TechField.WEAPON);
					if (chance > 0 && (r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichWeaponBeingResearched.TechName + " has been discovered."));
						ResearchedWeaponTechs.Add(WhichWeaponBeingResearched);
						UnresearchedWeaponTechs.Remove(WhichWeaponBeingResearched);
						WeaponResearchAmount = 0;
						fieldsNeedingNewItems.Add(TechField.WEAPON);
					}
				}
			}

			UpdateValues();

			return fieldsNeedingNewItems;
		}

		private float GetFieldInvestmentAmount(TechField whichField, float researchPoints)
		{
			switch (whichField)
			{
				case TechField.COMPUTER:
				{
					if (ComputerPercentage == 0)
					{
						if (ComputerResearchAmount > 0)
						{
							//Lose 10% of total research invested if no research is being invested
							return (ComputerResearchAmount * 0.9f) - ComputerResearchAmount;
						}
						return 0;
					}
					float interest = ComputerResearchAmount * 0.15f;
					float newPoints = (researchPoints * (ComputerPercentage * 0.01f));
					if ((newPoints * 2) < interest)
					{
						//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
						interest = newPoints * 2;
					}
					return (newPoints + interest);
				}
				case TechField.CONSTRUCTION:
				{
					if (ConstructionPercentage == 0)
					{
						if (ConstructionResearchAmount > 0)
						{
							//Lose 10% of total research invested if no research is being invested
							return (ConstructionResearchAmount * 0.9f) - ConstructionResearchAmount;
						}
						return 0;
					}
					float interest = ConstructionResearchAmount * 0.15f;
					float newPoints = (researchPoints * (ConstructionPercentage * 0.01f));
					if ((newPoints * 2) < interest)
					{
						//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
						interest = newPoints * 2;
					}
					return (newPoints + interest);
				}
				case TechField.FORCE_FIELD:
				{
					if (ForceFieldPercentage == 0)
					{
						if (ForceFieldResearchAmount > 0)
						{
							//Lose 10% of total research invested if no research is being invested
							return (ForceFieldResearchAmount * 0.9f) - ForceFieldResearchAmount;
						}
						return 0;
					}
					float interest = ForceFieldResearchAmount * 0.15f;
					float newPoints = (researchPoints * (ForceFieldPercentage * 0.01f));
					if ((newPoints * 2) < interest)
					{
						//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
						interest = newPoints * 2;
					}
					return (newPoints + interest);
				}
				case TechField.PLANETOLOGY:
				{
					if (PlanetologyPercentage == 0)
					{
						if (PlanetologyResearchAmount > 0)
						{
							//Lose 10% of total research invested if no research is being invested
							return (PlanetologyResearchAmount * 0.9f) - PlanetologyResearchAmount;
						}
						return 0;
					}
					float interest = PlanetologyResearchAmount * 0.15f;
					float newPoints = (researchPoints * (PlanetologyPercentage * 0.01f));
					if ((newPoints * 2) < interest)
					{
						//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
						interest = newPoints * 2;
					}
					return (newPoints + interest);
				}
				case TechField.PROPULSION:
				{
					if (PropulsionPercentage == 0)
					{
						if (PropulsionResearchAmount > 0)
						{
							//Lose 10% of total research invested if no research is being invested
							return (PropulsionResearchAmount * 0.9f) - PropulsionResearchAmount;
						}
						return 0;
					}
					float interest = PropulsionResearchAmount * 0.15f;
					float newPoints = (researchPoints * (PropulsionPercentage * 0.01f));
					if ((newPoints * 2) < interest)
					{
						//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
						interest = newPoints * 2;
					}
					return (newPoints + interest);
				}
				case TechField.WEAPON:
				{
					if (WeaponPercentage == 0)
					{
						if (WeaponResearchAmount > 0)
						{
							//Lose 10% of total research invested if no research is being invested
							return (WeaponResearchAmount * 0.9f) - WeaponResearchAmount;
						}
						return 0;
					}
					float interest = WeaponResearchAmount * 0.15f;
					float newPoints = (researchPoints * (WeaponPercentage * 0.01f));
					if ((newPoints * 2) < interest)
					{
						//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
						interest = newPoints * 2;
					}
					return (newPoints + interest);
				}
				default: return 0;
			}
		}

		private int GetChanceForDiscovery(TechField whichField)
		{
			//Only items being currently researched have a chance of being discovered
			switch (whichField)
			{
				case TechField.COMPUTER:
				{
					if (ComputerPercentage > 0 && WhichComputerBeingResearched != null)
					{
						int researchPointsRequired = (int)(WhichComputerBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.COMPUTER]);
						if (ComputerResearchAmount > researchPointsRequired) //We now have a chance of discovering it
						{
							return (int)(((ComputerResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
						}
					}
				} break;
				case TechField.CONSTRUCTION:
					{
						if (ConstructionPercentage > 0 && WhichConstructionBeingResearched != null)
						{
							int researchPointsRequired = (int)(WhichConstructionBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.CONSTRUCTION]);
							if (ConstructionResearchAmount > researchPointsRequired) //We now have a chance of discovering it
							{
								return (int)(((ConstructionResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
							}
						}
					} break;
				case TechField.FORCE_FIELD:
					{
						if (ForceFieldPercentage > 0 && WhichForceFieldBeingResearched != null)
						{
							int researchPointsRequired = (int)(WhichForceFieldBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.FORCE_FIELD]);
							if (ForceFieldResearchAmount > researchPointsRequired) //We now have a chance of discovering it
							{
								return (int)(((ForceFieldResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
							}
						}
					} break;
				case TechField.PLANETOLOGY:
					{
						if (PlanetologyPercentage > 0 && WhichPlanetologyBeingResearched != null)
						{
							int researchPointsRequired = (int)(WhichPlanetologyBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.PLANETOLOGY]);
							if (PlanetologyResearchAmount > researchPointsRequired) //We now have a chance of discovering it
							{
								return (int)(((PlanetologyResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
							}
						}
					} break;
				case TechField.PROPULSION:
					{
						if (PropulsionPercentage > 0 && WhichPropulsionBeingResearched != null)
						{
							int researchPointsRequired = (int)(WhichPropulsionBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.PROPULSION]);
							if (PropulsionResearchAmount > researchPointsRequired) //We now have a chance of discovering it
							{
								return (int)(((PropulsionResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
							}
						}
					} break;
				case TechField.WEAPON:
					{
						if (WeaponPercentage > 0 && WhichWeaponBeingResearched != null)
						{
							int researchPointsRequired = (int)(WhichWeaponBeingResearched.ResearchPoints * COST_MODIFIER * RaceModifiers[TechField.WEAPON]);
							if (WeaponResearchAmount > researchPointsRequired) //We now have a chance of discovering it
							{
								return (int)(((WeaponResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
							}
						}
					} break;
			}
			return 0;
		}

		public void SetPercentage(TechField whichField, int amount)
		{
			int remainingPercentile = 100;
			if (ComputerLocked)
			{
				remainingPercentile -= ComputerPercentage;
			}
			if (ConstructionLocked)
			{
				remainingPercentile -= ConstructionPercentage;
			}
			if (ForceFieldLocked)
			{
				remainingPercentile -= ForceFieldPercentage;
			}
			if (PlanetologyLocked)
			{
				remainingPercentile -= PlanetologyPercentage;
			}
			if (PropulsionLocked)
			{
				remainingPercentile -= PropulsionPercentage;
			}
			if (WeaponLocked)
			{
				remainingPercentile -= WeaponPercentage;
			}

			if (amount >= remainingPercentile)
			{
				if (!ComputerLocked)
				{
					ComputerPercentage = 0;
				}
				if (!ConstructionLocked)
				{
					ConstructionPercentage = 0;
				}
				if (!ForceFieldLocked)
				{
					ForceFieldPercentage = 0;
				}
				if (!PlanetologyLocked)
				{
					PlanetologyPercentage = 0;
				}
				if (!PropulsionLocked)
				{
					PropulsionPercentage = 0;
				}
				if (!WeaponLocked)
				{
					WeaponPercentage = 0;
				}
				amount = remainingPercentile;
			}

			//Now scale
			int totalPointsExcludingSelectedType = 0;
			switch (whichField)
			{
				case TechField.COMPUTER:
					{
						ComputerPercentage = amount;
						remainingPercentile -= ComputerPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.COMPUTER);
					} break;
				case TechField.CONSTRUCTION:
					{
						ConstructionPercentage = amount;
						remainingPercentile -= ConstructionPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.CONSTRUCTION);
					} break;
				case TechField.FORCE_FIELD:
					{
						ForceFieldPercentage = amount;
						remainingPercentile -= ForceFieldPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.FORCE_FIELD);
					} break;
				case TechField.PLANETOLOGY:
					{
						PlanetologyPercentage = amount;
						remainingPercentile -= PlanetologyPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.PLANETOLOGY);
					} break;
				case TechField.PROPULSION:
					{
						PropulsionPercentage = amount;
						remainingPercentile -= PropulsionPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.PROPULSION);
					} break;
				case TechField.WEAPON:
					{
						WeaponPercentage = amount;
						remainingPercentile -= WeaponPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.WEAPON);
					} break;
			}

			if (remainingPercentile < totalPointsExcludingSelectedType)
			{
				int amountToDeduct = totalPointsExcludingSelectedType - remainingPercentile;
				int prevValue;
				if (!ComputerLocked && whichField != TechField.COMPUTER)
				{
					prevValue = ComputerPercentage;
					ComputerPercentage -= (ComputerPercentage >= amountToDeduct ? amountToDeduct : ComputerPercentage);
					amountToDeduct -= (prevValue - ComputerPercentage);
				}
				if (amountToDeduct > 0)
				{
					if (!ConstructionLocked && whichField != TechField.CONSTRUCTION)
					{
						prevValue = ConstructionPercentage;
						ConstructionPercentage -= (ConstructionPercentage >= amountToDeduct ? amountToDeduct : ConstructionPercentage);
						amountToDeduct -= (prevValue - ConstructionPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ForceFieldLocked && whichField != TechField.FORCE_FIELD)
					{
						prevValue = ForceFieldPercentage;
						ForceFieldPercentage -= (ForceFieldPercentage >= amountToDeduct ? amountToDeduct : ForceFieldPercentage);
						amountToDeduct -= (prevValue - ForceFieldPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!PlanetologyLocked && whichField != TechField.PLANETOLOGY)
					{
						prevValue = PlanetologyPercentage;
						PlanetologyPercentage -= (PlanetologyPercentage >= amountToDeduct ? amountToDeduct : PlanetologyPercentage);
						amountToDeduct -= (prevValue - PlanetologyPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!PropulsionLocked && whichField != TechField.PROPULSION)
					{
						prevValue = PropulsionPercentage;
						PropulsionPercentage -= (PropulsionPercentage >= amountToDeduct ? amountToDeduct : PropulsionPercentage);
						amountToDeduct -= (prevValue - PropulsionPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!WeaponLocked && whichField != TechField.WEAPON)
					{
						prevValue = WeaponPercentage;
						WeaponPercentage -= (WeaponPercentage >= amountToDeduct ? amountToDeduct : WeaponPercentage);
						amountToDeduct -= (prevValue - WeaponPercentage);
					}
				}
			}

			if (remainingPercentile > totalPointsExcludingSelectedType) //excess points needed to allocate
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedType;
				if (!ComputerLocked && whichField != TechField.COMPUTER)
				{
					ComputerPercentage += amountToAdd;
					amountToAdd = 0;
				}
				if (amountToAdd > 0)
				{
					if (!ConstructionLocked && whichField != TechField.CONSTRUCTION)
					{
						ConstructionPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ForceFieldLocked && whichField != TechField.FORCE_FIELD)
					{
						ForceFieldPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!PlanetologyLocked && whichField != TechField.PLANETOLOGY)
					{
						PlanetologyPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!PropulsionLocked && whichField != TechField.PROPULSION)
					{
						PropulsionPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!WeaponLocked && whichField != TechField.WEAPON)
					{
						WeaponPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					//All fields are already checked, so have to add the remaining back to the current tech field
					switch (whichField)
					{
						case TechField.COMPUTER:
							ComputerPercentage += amountToAdd;
							break;
						case TechField.CONSTRUCTION:
							ConstructionPercentage += amountToAdd;
							break;
						case TechField.FORCE_FIELD:
							ForceFieldPercentage += amountToAdd;
							break;
						case TechField.PLANETOLOGY:
							PlanetologyPercentage += amountToAdd;
							break;
						case TechField.PROPULSION:
							PropulsionPercentage += amountToAdd;
							break;
						case TechField.WEAPON:
							WeaponPercentage += amountToAdd;
							break;
					}
				}
			}
		}

		private int GetTotalPercentageExcludingTypeAndLocked(TechField techField)
		{
			int total = 0;

			if (!ComputerLocked && techField != TechField.COMPUTER)
			{
				total += ComputerPercentage;
			}
			if (!ConstructionLocked && techField != TechField.CONSTRUCTION)
			{
				total += ConstructionPercentage;
			}
			if (!ForceFieldLocked && techField != TechField.FORCE_FIELD)
			{
				total += ForceFieldPercentage;
			}
			if (!PlanetologyLocked && techField != TechField.PLANETOLOGY)
			{
				total += PlanetologyPercentage;
			}
			if (!PropulsionLocked && techField != TechField.PROPULSION)
			{
				total += PropulsionPercentage;
			}
			if (!WeaponLocked && techField != TechField.WEAPON)
			{
				total += WeaponPercentage;
			}

			return total;
		}

		public int FuelRange { get; private set; }
		public int PlanetRadarRange { get; private set; }
		public int FleetRadarRange { get; private set; }
		public bool ShowEnemyETA { get; private set; }
		public bool RadarExplorePlanets { get; private set; }
		public int RoboticControls { get; private set; }
		public int FactoryCost { get; private set; }
		public float FactoryDiscount { get; private set; }
		public float IndustryWasteRate { get; private set; }
		public int IndustryCleanupPerBC { get; private set; }
		public bool HasAtmosphericTerraform { get; private set; }
		public bool HasSoilEnrichment { get; private set; }
		public bool HasAdvancedSoilEnrichment { get; private set; }
		public int MaxTerraformPop { get; private set; }
		public int TerraformCost { get; private set; }
		public int CloningCost { get; private set; }
		public float MissileBaseCost { get; private set; }
		public float MissileBaseCostInNebula { get; private set; } //No shields in Nebula, so factor in that (unlike MoO 1)
		public int HighestPlanetaryShield { get; private set; }

		private void UpdateValues()
		{
			//After researching or obtaining a technology, update all values
			FuelRange = 3;
			RoboticControls = 2;
			IndustryWasteRate = 1.0f;
			IndustryCleanupPerBC = 2;
			MaxTerraformPop = 0;
			TerraformCost = 6;
			CloningCost = 20;
			FactoryCost = 10;
			FactoryDiscount = 10;
			HighestPlanetaryShield = 0;
			Technology bestArmor = null;
			Technology bestBattleComputer = null;
			Technology bestMissile = null;
			Technology bestShield = null;
			Technology bestECM = null;
			int bestScanner = 0;
			foreach (var tech in ResearchedPropulsionTechs)
			{
				if (tech.FuelRange > FuelRange)
				{
					FuelRange = tech.FuelRange;
				}
			}
			foreach (var tech in ResearchedComputerTechs)
			{
				if (tech.RoboticControl > RoboticControls)
				{
					RoboticControls = tech.RoboticControl;
					FactoryCost = RoboticControls * 5;
				}
				if (tech.BattleComputer > 0 && (bestBattleComputer == null || bestBattleComputer.BattleComputer < tech.BattleComputer))
				{
					bestBattleComputer = tech;
				}
				if (tech.ECM > 0 && (bestECM == null || bestECM.ECM < tech.ECM))
				{
					bestECM = tech;
				}
				if (tech.SpaceScanner > bestScanner)
				{
					bestScanner = tech.SpaceScanner;
				}
			}
			switch (bestScanner)
			{
				case 0:
					FleetRadarRange = 0;
					PlanetRadarRange = 3;
					ShowEnemyETA = false;
					RadarExplorePlanets = false;
					break;
				case Technology.DEEP_SPACE_SCANNER:
					FleetRadarRange = 1;
					PlanetRadarRange = 5;
					ShowEnemyETA = false;
					RadarExplorePlanets = false;
					break;
				case Technology.IMPROVED_SPACE_SCANNER:
					FleetRadarRange = 2;
					PlanetRadarRange = 7;
					ShowEnemyETA = true;
					RadarExplorePlanets = false;
					break;
				case Technology.ADVANCED_SPACE_SCANNER:
					FleetRadarRange = 3;
					PlanetRadarRange = 9;
					ShowEnemyETA = true;
					RadarExplorePlanets = true;
					break;
			}
			foreach (var tech in ResearchedConstructionTechs)
			{
				if (tech.IndustrialWaste / 100.0f < IndustryWasteRate)
				{
					IndustryWasteRate = tech.IndustrialWaste / 100.0f;
				}
				if (tech.IndustrialTech < FactoryDiscount)
				{
					FactoryDiscount = tech.IndustrialTech;
				}
				if (tech.Armor > 0 && (bestArmor == null || bestArmor.Armor < tech.Armor))
				{
					bestArmor = tech;
				}
			}
			foreach (var tech in ResearchedForceFieldTechs)
			{
				if (tech.PlanetaryShield > HighestPlanetaryShield)
				{
					HighestPlanetaryShield = tech.PlanetaryShield;
				}
				if (tech.Shield > 0 && (bestShield == null || bestShield.Shield < tech.Shield))
				{
					bestShield = tech;
				}
			}
			//Convert FactoryDiscount to a decimal for less math later on
			FactoryDiscount *= 0.1f;
			foreach (var tech in ResearchedPlanetologyTechs)
			{
				if (tech.EcoCleanup > IndustryCleanupPerBC)
				{
					IndustryCleanupPerBC = tech.EcoCleanup;
				}
				if (tech.Enrichment == Technology.SOIL_ENRICHMENT)
				{
					HasSoilEnrichment = true;
				}
				if (tech.Enrichment == Technology.ADV_SOIL_ENRICHMENT)
				{
					HasAdvancedSoilEnrichment = true;
				}
				if (tech.Enrichment == Technology.ATMOSPHERIC_TERRAFORMING)
				{
					HasAtmosphericTerraform = true;
				}
				if (tech.Terraforming > MaxTerraformPop)
				{
					MaxTerraformPop = tech.Terraforming;
				}
				if (tech.TerraformCost < TerraformCost)
				{
					TerraformCost = tech.TerraformCost;
				}
				if (tech.Cloning < CloningCost)
				{
					CloningCost = tech.Cloning;
				}
			}

			foreach (var tech in ResearchedWeaponTechs)
			{
				if (tech.WeaponType == Technology.MISSILE_WEAPON && (bestMissile == null || bestMissile.MaximumWeaponDamage < tech.MaximumWeaponDamage))
				{
					bestMissile = tech;
				}
			}

			//Now add up the cost for missile base this turn
			/* Missile base cost is broken down as following:
			 * Battle Scanner and Subspace Interdictor are free
			 * Armor = 60% of Large Ship Cost, 1/2 HP of Large Ship HP
			 * Missiles = 540% of base cost, 3 missiles fired per base, +1 speed and double range
			 * Shields = 61% of Large Ship Cost
			 * Battle Computers = 61% of Large Ship Cost
			 * ECM = 62% of Large Ship Cost
			 * Base Slab (Construction Tech Level 1) = 120 BC
			 * */

			Dictionary<TechField, int> techLevels = new Dictionary<TechField, int>();
			techLevels.Add(TechField.FORCE_FIELD, ForceFieldLevel);
			techLevels.Add(TechField.WEAPON, WeaponLevel);
			techLevels.Add(TechField.PROPULSION, PropulsionLevel);
			techLevels.Add(TechField.COMPUTER, ComputerLevel);
			techLevels.Add(TechField.CONSTRUCTION, ConstructionLevel);
			techLevels.Add(TechField.PLANETOLOGY, PlanetologyLevel);

			//Factor in Slab's base price of 120, minus minizaturation
			int levelDifference = ConstructionLevel;
			if (levelDifference > 50)
			{
				levelDifference = 50; //Cap the miniaturization at 50 levels
			}
			MissileBaseCost = (float)(120 * (Math.Pow(0.5, (levelDifference / 10.0))));
			if (bestArmor != null)
			{
				var armor = new Equipment(bestArmor, false);
				MissileBaseCost += armor.GetCost(techLevels, 2) * 0.6f;
			}
			if (bestMissile != null)
			{
				var missile = new Equipment(bestMissile, false);
				MissileBaseCost += missile.GetCost(techLevels, 2) * 5.4f;
			}
			if (bestBattleComputer != null)
			{
				var battleComputer = new Equipment(bestBattleComputer, false);
				MissileBaseCost += battleComputer.GetCost(techLevels, 2) * 0.61f;
			}
			if (bestECM != null)
			{
				var ecm = new Equipment(bestECM, false);
				MissileBaseCost += ecm.GetCost(techLevels, 2) * 0.62f;
			}
			MissileBaseCostInNebula = MissileBaseCost; //Since shields don't work in nebulaes, doesn't make sense to build shields
			if (bestShield != null)
			{
				var shield = new Equipment(bestShield, false);
				MissileBaseCost += shield.GetCost(techLevels, 2) * 0.61f;
			}
		}

		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("Technologies");
			writer.WriteStartElement("Computer");
			writer.WriteAttributeString("Researching", WhichComputerBeingResearched == null ? string.Empty : WhichComputerBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", ComputerPercentage.ToString());
			writer.WriteAttributeString("Locked", ComputerLocked ? "True" : "False");
			writer.WriteAttributeString("Invested", ComputerResearchAmount.ToString());
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedComputerTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedComputerTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("Construction");
			writer.WriteAttributeString("Researching", WhichConstructionBeingResearched == null ? string.Empty : WhichConstructionBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", ConstructionPercentage.ToString());
			writer.WriteAttributeString("Locked", ConstructionLocked ? "True" : "False");
			writer.WriteAttributeString("Invested", ConstructionResearchAmount.ToString());
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedConstructionTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedConstructionTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("ForceField");
			writer.WriteAttributeString("Researching", WhichForceFieldBeingResearched == null ? string.Empty : WhichForceFieldBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", ForceFieldPercentage.ToString());
			writer.WriteAttributeString("Locked", ForceFieldLocked ? "True" : "False");
			writer.WriteAttributeString("Invested", ForceFieldResearchAmount.ToString());
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedForceFieldTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedForceFieldTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("Planetology");
			writer.WriteAttributeString("Researching", WhichPlanetologyBeingResearched == null ? string.Empty : WhichPlanetologyBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", PlanetologyPercentage.ToString());
			writer.WriteAttributeString("Locked", PlanetologyLocked ? "True" : "False");
			writer.WriteAttributeString("Invested", PlanetologyResearchAmount.ToString());
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedPlanetologyTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedPlanetologyTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("Propulsion");
			writer.WriteAttributeString("Researching", WhichPropulsionBeingResearched == null ? string.Empty : WhichPropulsionBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", PropulsionPercentage.ToString());
			writer.WriteAttributeString("Locked", PropulsionLocked ? "True" : "False");
			writer.WriteAttributeString("Invested", PropulsionResearchAmount.ToString());
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedPropulsionTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedPropulsionTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("Weapon");
			writer.WriteAttributeString("Researching", WhichWeaponBeingResearched == null ? string.Empty : WhichWeaponBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", WeaponPercentage.ToString());
			writer.WriteAttributeString("Locked", WeaponLocked ? "True" : "False");
			writer.WriteAttributeString("Invested", WeaponResearchAmount.ToString());
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedWeaponTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedWeaponTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		public void Load(XElement empire, MasterTechnologyManager MTM)
		{
			var technologies = empire.Element("Technologies");
			var compTechs = technologies.Element("Computer");
			var constTechs = technologies.Element("Construction");
			var ffTechs = technologies.Element("ForceField");
			var planetTechs = technologies.Element("Planetology");
			var propTechs = technologies.Element("Propulsion");
			var weaponTechs = technologies.Element("Weapon");

			WhichComputerBeingResearched = MTM.GetTechnologyWithName(compTechs.Attribute("Researching").Value);
			WhichConstructionBeingResearched = MTM.GetTechnologyWithName(constTechs.Attribute("Researching").Value);
			WhichForceFieldBeingResearched = MTM.GetTechnologyWithName(ffTechs.Attribute("Researching").Value);
			WhichPlanetologyBeingResearched = MTM.GetTechnologyWithName(planetTechs.Attribute("Researching").Value);
			WhichPropulsionBeingResearched = MTM.GetTechnologyWithName(propTechs.Attribute("Researching").Value);
			WhichWeaponBeingResearched = MTM.GetTechnologyWithName(weaponTechs.Attribute("Researching").Value);

			ComputerPercentage = int.Parse(compTechs.Attribute("Percentage").Value);
			ConstructionPercentage = int.Parse(constTechs.Attribute("Percentage").Value);
			ForceFieldPercentage = int.Parse(ffTechs.Attribute("Percentage").Value);
			PlanetologyPercentage = int.Parse(planetTechs.Attribute("Percentage").Value);
			PropulsionPercentage = int.Parse(propTechs.Attribute("Percentage").Value);
			WeaponPercentage = int.Parse(weaponTechs.Attribute("Percentage").Value);

			ComputerLocked = bool.Parse(compTechs.Attribute("Locked").Value);
			ConstructionLocked = bool.Parse(constTechs.Attribute("Locked").Value);
			ForceFieldLocked = bool.Parse(ffTechs.Attribute("Locked").Value);
			PlanetologyLocked = bool.Parse(planetTechs.Attribute("Locked").Value);
			PropulsionLocked = bool.Parse(propTechs.Attribute("Locked").Value);
			WeaponLocked = bool.Parse(weaponTechs.Attribute("Locked").Value);

			ComputerResearchAmount = float.Parse(compTechs.Attribute("Invested").Value);
			ConstructionResearchAmount = float.Parse(constTechs.Attribute("Invested").Value);
			ForceFieldResearchAmount = float.Parse(ffTechs.Attribute("Invested").Value);
			PlanetologyResearchAmount = float.Parse(planetTechs.Attribute("Invested").Value);
			PropulsionResearchAmount = float.Parse(propTechs.Attribute("Invested").Value);
			WeaponResearchAmount = float.Parse(weaponTechs.Attribute("Invested").Value);

			foreach (var researched in compTechs.Element("Researched").Elements())
			{
				ResearchedComputerTechs.Add(MTM.GetTechnologyWithName(researched.Value));
			}
			foreach (var unresearched in compTechs.Element("Unresearched").Elements())
			{
				UnresearchedComputerTechs.Add(MTM.GetTechnologyWithName(unresearched.Value));
			}
			foreach (var researched in constTechs.Element("Researched").Elements())
			{
				ResearchedConstructionTechs.Add(MTM.GetTechnologyWithName(researched.Value));
			}
			foreach (var unresearched in constTechs.Element("Unresearched").Elements())
			{
				UnresearchedConstructionTechs.Add(MTM.GetTechnologyWithName(unresearched.Value));
			}
			foreach (var researched in ffTechs.Element("Researched").Elements())
			{
				ResearchedForceFieldTechs.Add(MTM.GetTechnologyWithName(researched.Value));
			}
			foreach (var unresearched in ffTechs.Element("Unresearched").Elements())
			{
				UnresearchedForceFieldTechs.Add(MTM.GetTechnologyWithName(unresearched.Value));
			}
			foreach (var researched in planetTechs.Element("Researched").Elements())
			{
				ResearchedPlanetologyTechs.Add(MTM.GetTechnologyWithName(researched.Value));
			}
			foreach (var unresearched in planetTechs.Element("Unresearched").Elements())
			{
				UnresearchedPlanetologyTechs.Add(MTM.GetTechnologyWithName(unresearched.Value));
			}
			foreach (var researched in propTechs.Element("Researched").Elements())
			{
				ResearchedPropulsionTechs.Add(MTM.GetTechnologyWithName(researched.Value));
			}
			foreach (var unresearched in propTechs.Element("Unresearched").Elements())
			{
				UnresearchedPropulsionTechs.Add(MTM.GetTechnologyWithName(unresearched.Value));
			}
			foreach (var researched in weaponTechs.Element("Researched").Elements())
			{
				ResearchedWeaponTechs.Add(MTM.GetTechnologyWithName(researched.Value));
			}
			foreach (var unresearched in weaponTechs.Element("Unresearched").Elements())
			{
				UnresearchedWeaponTechs.Add(MTM.GetTechnologyWithName(unresearched.Value));
			}

			UpdateValues();
		}
		#endregion
	}
}
