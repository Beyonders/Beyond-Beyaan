using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Beyond_Beyaan
{
	public class EmpireManager
	{
		#region Variables
		private List<Empire> _empires;
		//private List<CombatToProcess> combatsToProcess;
		private Empire currentEmpire;
		private int empireIter;
		private GameMain _gameMain;
		#endregion

		#region Properties
		public Empire CurrentEmpire
		{
			//Current human empire
			get { return currentEmpire; }
		}
		/*public List<CombatToProcess> CombatsToProcess
		{
			get { return combatsToProcess; }
		}
		public bool HasCombat
		{
			get { return combatsToProcess.Count > 0; }
		}*/
		#endregion

		#region Constructors
		public EmpireManager(GameMain gameMain)
		{
			_gameMain = gameMain;
			_empires = new List<Empire>();
			empireIter = -1;
			//combatsToProcess = new List<CombatToProcess>();
		}
		#endregion

		#region Functions
		public void AddEmpire(Empire empire)
		{
			_empires.Add(empire);
		}

		public void RemoveEmpire(Empire empire)
		{
			_empires.Remove(empire);
		}

		public Empire GetEmpire(int empireId)
		{
			foreach (Empire empire in _empires)
			{
				if (empire.EmpireID == empireId)
				{
					return empire;
				}
			}
			return null;
		}

		public void Reset()
		{
			_empires.Clear();
		}

		public void SetupContacts()
		{
			foreach (Empire empire in _empires)
			{
				empire.SetUpContacts(_empires);
			}
		}

		public void SetInitialEmpireTurn()
		{
			foreach (Empire empire in _empires)
			{
				//Reset fleet movement and stuff
				empire.FleetManager.ResetFleetMovements();
			}
			empireIter = 0;
			//If first player isn't human, find the next one
			for (int i = 0; i < _empires.Count; i++)
			{
				if (_empires[i].Type != PlayerType.HUMAN)
				{
					_empires[i].HandleAIEmpire();
				}
				else
				{
					currentEmpire = _empires[i];
					empireIter = i;
					break;
				}
			}
		}

		public bool ProcessNextEmpire()
		{
			if (empireIter + 1 == _empires.Count)
			{
				//It've reached the end
				return true;
			}
			//This will update each empire if they're AI.  If an empire is human-controlled, it stops and waits for the player to press end of turn
			for (int i = empireIter + 1; i < _empires.Count; i++)
			{
				empireIter = i;
				if (_empires[i].Type != PlayerType.HUMAN)
				{
					_empires[i].HandleAIEmpire();
					if (i + 1 == _empires.Count)
					{
						//reached end of list with CPU player as last player
						return true;
					}
				}
				else
				{
					currentEmpire = _empires[i];
					break;
				}
			}
			return false;
		}

		public List<Fleet> GetFleetsWithinArea(float left, float top, float width, float height)
		{
			List<Fleet> fleets = new List<Fleet>();
			foreach (Empire empire in _empires)
			{
				foreach (Fleet fleet in empire.FleetManager.GetFleets())
				{
					if (empire != _empires[0] && !fleet.VisibleToWhichEmpires.Contains(_empires[0]))
					{
						continue;
					}
					if (fleet.GalaxyX + 16 < left || fleet.GalaxyY + 16 < top || fleet.GalaxyX - 16 > left + width || fleet.GalaxyY - 16 > top + height)
					{
						continue;
					}
					fleets.Add(fleet);
				}
			}
			return fleets;
		}

		public FleetGroup GetFleetsAtPoint(int x, int y)
		{
			List<Fleet> fleets = new List<Fleet>();
			foreach (Empire empire in _empires)
			{
				foreach (Fleet fleet in empire.FleetManager.ReturnFleetAtPoint(x, y))
				{
					//Don't include fleets the human player can't see.
					if (empire == _empires[0] || fleet.VisibleToWhichEmpires.Contains(_empires[0]))
					{
						fleets.Add(fleet);
					}
				}
			}
			return (fleets.Count > 0 ? new FleetGroup(fleets) : null);
		}

		public void ResetFleetMovement()
		{
			foreach (Empire empire in _empires)
			{
				empire.FleetManager.ResetFleetMovements();
			}
		}

		public bool UpdateFleetMovement(float frameDeltaTime)
		{
			bool stillHaveMovement = false;
			foreach (Empire empire in _empires)
			{
				if (empire.FleetManager.MoveFleets(frameDeltaTime))
				{
					stillHaveMovement = true;
				}
				foreach (var fleet in empire.FleetManager.GetFleets())
				{
					fleet.VisibleToWhichEmpires.Clear();
					//Update whether or not it's visible to other empires
					foreach (Empire otherEmpire in _empires)
					{
						if (otherEmpire == empire)
						{
							continue; //no need to check owner
						}
						foreach (var planet in otherEmpire.PlanetManager.Planets)
						{
							float xDis = fleet.GalaxyX - planet.System.X;
							float yDis = fleet.GalaxyY - planet.System.Y;
							if ((xDis * xDis) + (yDis * yDis) < otherEmpire.TechnologyManager.PlanetRadarRange)
							{
								fleet.VisibleToWhichEmpires.Add(otherEmpire);
								break;
							}
						}
						if (otherEmpire.TechnologyManager.FleetRadarRange > 0 && !fleet.VisibleToWhichEmpires.Contains(otherEmpire))
						{
							//It first checks if other empire even have fleet radar, and if it's not already detected by planets, to improve performance
							foreach (var otherFleet in otherEmpire.FleetManager.GetFleets())
							{
								float xDis = fleet.GalaxyX - otherFleet.GalaxyX;
								float yDis = fleet.GalaxyY - otherFleet.GalaxyY;
								if ((xDis * xDis) + (yDis * yDis) < otherEmpire.TechnologyManager.FleetRadarRange)
								{
									fleet.VisibleToWhichEmpires.Add(otherEmpire);
									break;
								}
							}
						}
					}
				}
			}
			return stillHaveMovement;
		}

		public void MergeIdleFleets()
		{
			foreach (Empire empire in _empires)
			{
				empire.FleetManager.MergeIdleFleets();
			}
		}

		public void ClearEmptyFleets()
		{
			foreach (Empire empire in _empires)
			{
				empire.FleetManager.ClearEmptyFleets();
			}
		}

		public void UpdateEmpires()
		{
			foreach (Empire empire in _empires)
			{
				empire.SitRepManager.ClearItems();
				empire.ContactManager.UpdateContacts(empire.SitRepManager);
			}
		}

		public void UpdateMilitary()
		{
			foreach (Empire empire in _empires)
			{
				empire.CheckForBuiltShips();
			}
		}

		public void AccureIncome()
		{
			foreach (Empire empire in _empires)
			{
				empire.UpdateProduction(); //Factor in trade income, expenses, etc
				empire.AccureIncome(); //If any income (tax, industry points, etc), collect them
			}
		}

		public void AccureResearch()
		{
			foreach (Empire empire in _empires)
			{
				empire.UpdateResearchPoints();
				empire.TechnologyManager.AccureResearch(empire.ResearchPoints);
			}
		}

		public Dictionary<Empire, List<TechField>> RollForDiscoveries(Random r)
		{
			Dictionary<Empire, List<TechField>> itemsNeedingSelection = new Dictionary<Empire, List<TechField>>();
			foreach (Empire empire in _empires)
			{
				var items = empire.TechnologyManager.RollForDiscoveries(r, empire.SitRepManager);
				if (empire.Type == PlayerType.HUMAN && items.Count > 0)
				{
					itemsNeedingSelection.Add(empire, items);
				}
			}
			return itemsNeedingSelection;
		}

		public void UpdatePopulationGrowth()
		{
			foreach (Empire empire in _empires)
			{
				empire.PlanetManager.UpdatePopGrowth();
			}
		}

		public void LaunchTransports()
		{
			foreach (Empire empire in _empires)
			{
				//go through each system and see if a system is sending out transports
				empire.LaunchTransports();
			}
		}

		public void LandTransports()
		{
			//TODO: Get list of conflict transports
			foreach (Empire empire in _empires)
			{
				//go through each system and see if transports has arrived.  Return any combat conflicts (non-combats are landed automatically)
				empire.LandTransports();
			}
		}

		public Dictionary<Empire, List<StarSystem>> CheckExploredSystems(Galaxy galaxy)
		{
			Dictionary<Empire, List<StarSystem>> exploredSystems = new Dictionary<Empire, List<StarSystem>>();
			foreach (Empire empire in _empires)
			{
				List<StarSystem> temp = empire.CheckExploredSystems(galaxy);
				if (empire.Type == PlayerType.HUMAN && temp.Count > 0)
				{
					exploredSystems.Add(empire, temp);
				}
			}
			return exploredSystems;
		}

		public Dictionary<Empire, List<Fleet>> CheckColonizableSystems(Galaxy galaxy)
		{
			Dictionary<Empire, List<Fleet>> colonizableSystems = new Dictionary<Empire, List<Fleet>>();
			foreach (Empire empire in _empires)
			{
				List<Fleet> temp = empire.CheckColonizableSystems(galaxy);
				if (empire.Type == PlayerType.HUMAN && temp.Count > 0)
				{
					colonizableSystems.Add(empire, temp);
				}
			}
			return colonizableSystems;
		}

		public void LookForCombat()
		{
			/*List<Fleet> fleets = new List<Fleet>();
			foreach (Empire empire in _empires)
			{
				foreach (Fleet fleet in empire.FleetManager.GetFleets())
				{
					fleets.Add(fleet);
				}
			}
			combatsToProcess = new List<CombatToProcess>();
			CombatToProcess combatToProcess = new CombatToProcess(0, 0, fleets);
			combatsToProcess.Add(combatToProcess);*/
		}

		/*public void UpdateInfluenceMaps(Galaxy galaxy)
		{
			GridCell[][] gridCells = galaxy.GetGridCells();
			Dictionary<Empire, int>[][] cells = new Dictionary<Empire,int>[gridCells.Length][];
			for (int i = 0; i < gridCells.Length; i++)
			{
				cells[i] = new Dictionary<Empire, int>[gridCells.Length];
				for (int j = 0; j < cells[i].Length; j++)
				{
					cells[i][j] = new Dictionary<Empire, int>();
				}
			}

			foreach (StarSystem system in galaxy.GetAllStars())
			{
				if (system.DominantEmpire != null)
				{
					//Get the total influence from this system
					float totalPopulation = 0.0f;
					foreach (Planet planet in system.Planets)
					{
						if (planet.Owner == system.DominantEmpire)
						{
							totalPopulation += planet.TotalPopulation;
						}
					}

					int roundedPopulation = (int)totalPopulation;
					if (totalPopulation - roundedPopulation >= 0.5f)
					{
						//round it up
						roundedPopulation++;
					}

					for (int i = 0; i < system.Size; i++)
					{
						for (int j = 0; j < system.Size; j++)
						{
							Dictionary<Empire, int> cell = cells[system.X + i][system.Y + j];
							//Mark this system's gridcells as owned by the current empire
							if (!cell.ContainsKey(system.DominantEmpire))
							{
								cell.Add(system.DominantEmpire, -1);
							}
							else
							{
								cell[system.DominantEmpire] = -1;
							}
						}
					}

					int distance = 1;
					while (roundedPopulation > 0)
					{
						bool[][] disc = Utility.CalculateDisc(distance, system.Size);
						int x = system.X - distance;
						int y = system.Y - distance;
						for (int i = 0; i < disc.Length; i++)
						{
							for (int j = 0; j < disc[i].Length; j++)
							{
								if (disc[i][j] && x + i >= 0 && x + i < gridCells.Length && y + j >= 0 && y + j < gridCells.Length)
								{
									Dictionary<Empire, int> cell = cells[x + i][y + j];
									if (cell.ContainsKey(system.DominantEmpire))
									{
										if (cell[system.DominantEmpire] >= 0)
										{
											cell[system.DominantEmpire] += 1;
										}
									}
									else
									{
										cell.Add(system.DominantEmpire, 1);
									}
								}
							}
						}
						distance++;
						roundedPopulation -= 20;
					}
				}
			}

			//Now that the influence map is calculated, figure out the dominant and secondary influence per grid cell
			for (int i = 0; i < cells.Length; i++)
			{
				for (int j = 0; j < cells.Length; j++)
				{
					int dominantValue = 0;
					int secondaryValue = 0;
					foreach (KeyValuePair<Empire, int> influence in cells[i][j])
					{
						if ((influence.Value > dominantValue && dominantValue >= 0) || influence.Value == -1)
						{
							gridCells[i][j].secondaryEmpire = gridCells[i][j].dominantEmpire;
							gridCells[i][j].dominantEmpire = influence.Key;
							secondaryValue = dominantValue;
							dominantValue = influence.Value;
						}
						else if (influence.Value > secondaryValue)
						{
							gridCells[i][j].secondaryEmpire = influence.Key;
							secondaryValue = influence.Value;
						}
					}

					if (gridCells[i][j].secondaryEmpire == null || gridCells[i][j].dominantEmpire == null)
					{
						continue;
					}
					gridCells[i][j].dominantEmpire.ContactManager.EstablishContact(gridCells[i][j].secondaryEmpire, gridCells[i][j].dominantEmpire.SitRepManager);
					gridCells[i][j].secondaryEmpire.ContactManager.EstablishContact(gridCells[i][j].dominantEmpire, gridCells[i][j].secondaryEmpire.SitRepManager);
				}
			}
			List<Fleet> allFleets = GetFleetsWithinArea(-1, -1, galaxy.GalaxySize + 2, galaxy.GalaxySize + 2);
			foreach (Empire empire in _empires)
			{
				//empire.CreateInfluenceMapSprite(gridCells);

				List<Fleet> visibleFleets = new List<Fleet>();
				foreach (Fleet fleet in allFleets)
				{
					if (fleet.Empire == empire)
					{
						continue;
					}
					if (gridCells[fleet.GalaxyX][fleet.GalaxyY].dominantEmpire == empire || gridCells[fleet.GalaxyX][fleet.GalaxyY].secondaryEmpire == empire)
					{
						visibleFleets.Add(fleet);
					}
				}

				visibleFleets.Sort((Fleet a, Fleet b) => { return string.Compare(a.Empire.EmpireName, b.Empire.EmpireName); });
				empire.SetVisibleFleets(visibleFleets);
			}
		}*/

		/*public void UpdateMigration(Galaxy galaxy)
		{
			GridCell[][] gridCells = galaxy.GetGridCells();
			foreach (Empire empire in _empires)
			{
				empire.SystemsUnderInfluence = new List<StarSystem>();
			}
			List<StarSystem> systems = galaxy.GetAllStars();
			foreach (StarSystem system in systems)
			{
				foreach (Empire empire in system.EmpiresWithPlanetsInThisSystem)
				{
					empire.SystemsUnderInfluence.Add(system);
				}
				system.EmpiresWithFleetAdjacentLastTurn = new List<Empire>();
				foreach (Empire empire in system.EmpiresWithFleetAdjacentThisTurn)
				{
					system.EmpiresWithFleetAdjacentLastTurn.Add(empire);
				}
				system.EmpiresWithFleetAdjacentThisTurn = new List<Empire>();

				List<Fleet> fleetsAdjacentThisTurn = GetFleetsWithinArea(system.X - 1, system.Y - 1, system.Size + 2, system.Size + 2);
				foreach (Fleet fleet in fleetsAdjacentThisTurn)
				{
					if (!system.EmpiresWithFleetAdjacentThisTurn.Contains(fleet.Empire))
					{
						system.EmpiresWithFleetAdjacentThisTurn.Add(fleet.Empire);
					}
				}

				foreach (Empire empire in system.EmpiresWithFleetAdjacentThisTurn)
				{
					if (system.EmpiresWithFleetAdjacentLastTurn.Contains(empire) && !empire.SystemsUnderInfluence.Contains(system))
					{
						empire.SystemsUnderInfluence.Add(system);
					}
				}
			}
			foreach (Empire empire in _empires)
			{
				//Now that we know which systems are claimed by which empires, time to process the actual migration
				empire.UpdateMigration(galaxy);
			}
			foreach (StarSystem system in systems)
			{
				system.UpdateOwners();
			}
		}*/
		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("Empires");
			foreach (var empire in _empires)
			{
				empire.Save(writer);
			}
			writer.WriteEndElement();
		}
		public bool Load(XElement root)
		{
			_empires = new List<Empire>();
			var empires = root.Element("Empires");
			foreach (var empire in empires.Elements())
			{
				var newEmpire = new Empire();
				newEmpire.Load(empire, _gameMain);
				_empires.Add(newEmpire);
			}
			foreach (var empire in _empires)
			{
				empire.SetUpContacts(_empires);
				//TODO: Update all empires' contacts
			}
			SetInitialEmpireTurn();
			return true;
		}
		#endregion
	}
}
