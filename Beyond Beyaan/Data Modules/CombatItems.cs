using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Data_Modules
{
	/*class CombatShip : Ship
	{
		private int amount;
		public Label AmountLabel { get; private set; }
		public int Amount
		{
			get { return amount; }
			private set 
			{ 
				amount = value;
				AmountLabel.SetText(amount.ToString());
			}
		}
		public int X { get; set; }
		public int Y { get; set; }
		public float XOffset { get; set; }
		public float YOffset { get; set; }
		public float AngleFacing
		{
			get { return angleFacing; }
			set
			{
				angleFacing = value;
				while (angleFacing < 0)
				{
					angleFacing += (float)(Math.PI * 2);
				}
				while (angleFacing > (float)(Math.PI * 2))
				{
					angleFacing -= (float)(Math.PI * 2);
				}
			}
		}
		public int CenterOffset { get; private set; }

		public bool DoneThisTurn { get; set; }
		public bool Retreating { get; set; }
		public bool DoneRetreatWait { get; set; }

		public float CurrentHitPoints { get; set; }

		public List<Point> path;

		private float angleFacing;

		public CombatShip(Ship ship, int amount)
		{
			engine = ship.engine;
			armor = ship.armor;
			shield = ship.shield;
			computer = ship.computer;
			weapons = ship.weapons;
			Size = ship.Size;
			WhichStyle = ship.WhichStyle;
			Name = ship.Name;
			CenterOffset = (Size * 16) / 2;
			path = null;
			AmountLabel = new Label(0, 0);
			Amount = amount;
			CurrentHitPoints = ship.armor.GetHP(ship.TotalSpace);
		}

		public void Update(float frameDeltaTime)
		{
			if (path != null && path.Count > 0)
			{
				//MoveTo to next point
				if (X > path[0].X)
				{
					XOffset -= 16 * frameDeltaTime;
					if ((XOffset / 16) + X <= path[0].X)
					{
						X--;
						XOffset = 0;
					}
				}
				else if (X < path[0].X)
				{
					XOffset += 16 * frameDeltaTime;
					if ((XOffset / 16) + X >= path[0].X)
					{
						X++;
						XOffset = 0;
					}
				}
				if (Y > path[0].Y)
				{
					YOffset -= 16 * frameDeltaTime;
					if ((YOffset / 16) + Y <= path[0].Y)
					{
						Y--;
						YOffset = 0;
					}
				}
				else if (Y < path[0].Y)
				{
					YOffset += 16 * frameDeltaTime;
					if ((YOffset / 16) + Y >= path[0].Y)
					{
						Y++;
						YOffset = 0;
					}
				}

				if (X == path[0].X && Y == path[0].Y)
				{
					path.Remove(path[0]);
				}
			}
		}
	}

	class CombatFleet : Fleet
	{
		public Label EmpireNameLabel;
		public static double TwoSquared = Math.Sqrt(2);

		public List<CombatShip> combatShips;
		public int Length { get; private set; }

		public CombatFleet(Fleet fleet)
		{
			this.Empire = fleet.Empire;
			List<CombatShip> tempCombatShips = new List<CombatShip>();
			combatShips = new List<CombatShip>();

			List<Ship> orderedShips = new List<Ship>();
			foreach (Ship ship in fleet.OrderedShips)
			{
				CombatShip combatShip = new CombatShip(ship, fleet.Ships[ship]);
				tempCombatShips.Add(combatShip);
			}
			tempCombatShips.Sort((ship1, ship2) => { return ship1.Size.CompareTo(ship2.Size); });

			for (int i = 0; i < tempCombatShips.Count; i += 2)
			{
				combatShips.Add(tempCombatShips[i]);
			}
			for (int i = tempCombatShips.Count - (1 + tempCombatShips.Count % 2); i > 0; i -= 2)
			{
				combatShips.Add(tempCombatShips[i]);
			}

			Length = 0;

			for (int i = 0; i < combatShips.Count; i++)
			{
				Length += (int)((combatShips[i].Size * TwoSquared) + 4);
			}

			EmpireNameLabel = new Label(this.Empire.EmpireName, 0, 0, this.Empire.EmpireColor);
		}

		public void SetStartingPosition(float radius, float angle)
		{
			int initialSize = Length / 2;
			bool first = true;
			foreach (CombatShip ship in combatShips)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					initialSize += (int)((ship.Size * TwoSquared) / 2) + 3;
				}
				float x;
				float y;
				if (angle < Math.PI / 2)
				{
					//Bottom right quadrant
					x = (float)((radius - ((ship.Size * TwoSquared) / 2)) * Math.Cos(angle)) + radius;
					y = (float)((radius - ((ship.Size * TwoSquared) / 2)) * Math.Sin(angle)) + radius;
				}
				else if (angle < Math.PI)
				{
					x = (float)((radius + ((ship.Size * TwoSquared) / 2)) * Math.Cos(angle)) + radius;
					y = (float)((radius - ((ship.Size * TwoSquared) / 2)) * Math.Sin(angle)) + radius;
				}
				else if (angle < Math.PI * 1.5)
				{
					//Top right quadrant
					x = (float)((radius + ((ship.Size * TwoSquared) / 2)) * Math.Cos(angle)) + radius;
					y = (float)((radius + ((ship.Size * TwoSquared) / 2)) * Math.Sin(angle)) + radius;
				}
				else
				{
					x = (float)((radius - ((ship.Size * TwoSquared) / 2)) * Math.Cos(angle)) + radius;
					y = (float)((radius + ((ship.Size * TwoSquared) / 2)) * Math.Sin(angle)) + radius;
				}

				ship.X = (int)(x + ((initialSize - Length) * Math.Cos(angle + (Math.PI / 2))));
				ship.Y = (int)(y + ((initialSize - Length) * Math.Sin(angle + (Math.PI / 2))));
				ship.AngleFacing = (float)(angle + (Math.PI * 1.5));
				initialSize += (int)((ship.Size * TwoSquared) / 2) + 1;
			}
		}
	}

	class CombatToProcess
	{
		public List<Fleet> fleetsInCombat;
		public int x;
		public int y;

		public CombatToProcess(int x, int y, List<Fleet> fleetsInCombat)
		{
			this.x = x;
			this.y = y;
			this.fleetsInCombat = fleetsInCombat;
		}
	}*/
}
