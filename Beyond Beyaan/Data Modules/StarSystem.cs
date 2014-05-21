using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	//public enum StarColor { RED, ORANGE, GREEN, PURPLE, BLUE, BROWN, WHITE, YELLOW }
	//public enum StarType { NORMAL, BLACK_HOLE }

	public class StarSystem
	{
		#region Member Variables
		//StarType type;
		private int x;
		private int y;
		private int size;
		private string name;

		private float[] _starColor;
		private Color _color;
		private List<Empire> exploredBy;
		public string ExploredByIDs { get; set; } //Used for saving/loading, otherwise not used.

		List<Planet> planets;
		#endregion

		#region Properties
		public int X 
		{ 
			get { return x; } 
			set { x = value; } 
		}
		public int Y
		{
			get { return y; }
			set { y = value; }
		}
		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				foreach (var planet in planets)
				{
					planet.Name = value;
				}
				StarName.SetText(name);
			}
		}
		public BBSprite Sprite
		{
			get;
			private set;
		}
		public float[] StarColor
		{
			get { return _starColor; }
		}
		public Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
				_starColor = new[]
				{
					_color.R / 255.0f,
					_color.G / 255.0f,
					_color.B / 255.0f,
					_color.A / 255.0f
				};
				if (value == Color.Yellow)
				{
					Description = "Yellow stars offer the best chance of discovering planets with friendly environment";
				}
				else if (value == Color.Red)
				{
					Description = "Red stars are old, dull stars that commonly have poor planets";
				}
				else if (value == Color.Green)
				{
					Description = "Green stars are moderately bright and have a wide range of planetary types";
				}
				else if (value == Color.Blue)
				{
					Description = "Blue stars are relatively young stars with mineral rich lifeless planets";
				}
				else if (value == Color.White)
				{
					Description = "White stars burn incredibly hot and generally have hostile planets";
				}
				else
				{
					Description = "Neutron stars are rare and offer the greatest chance of finding rich planets";
				}
			}
		}
		public int Size
		{
			get { return size; }
		}
		public List<Planet> Planets
		{
			get { return planets; }
		}
		public List<Empire> ExploredBy
		{
			get { return exploredBy; }
		}
		public int ID { get; private set; }
		public string Description { get; private set; }
		/*public StarType Type
		{
			get { return type; }
		}*/
		public BBLabel StarName { get; set; }
		public Empire DominantEmpire { get; private set; }

		public List<Empire> EmpiresWithFleetAdjacentLastTurn { get; set; }
		public List<Empire> EmpiresWithFleetAdjacentThisTurn { get; set; }
		public List<Empire> EmpiresWithPlanetsInThisSystem { get; private set; }

		public Dictionary<Empire, float> OwnerPercentage;
		#endregion

		#region Constructor
		public StarSystem(string name, int id, int x, int y, Color color, string description, Random r)
		{
			this.Sprite = SpriteManager.GetSprite("Star", r);
			this.name = name;
			this.x = x;
			this.y = y;
			this.size = 1;
			ID = id;

			Color = color;

			exploredBy = new List<Empire>();
			planets = new List<Planet>();

			//type = StarType.NORMAL;

			string reason;
			StarName = new BBLabel();
			StarName.Initialize(0, 0, name, Color.White, out reason);
			EmpiresWithFleetAdjacentLastTurn = new List<Empire>();
			EmpiresWithFleetAdjacentThisTurn = new List<Empire>();
			EmpiresWithPlanetsInThisSystem = new List<Empire>();
			OwnerPercentage = new Dictionary<Empire, float>();
		}
		public StarSystem(string name, int id, int x, int y, Color color, string description, int minPlanets, int maxPlanets, Random r) : this (name, id, x, y, color, description, r)
		{
			int amountOfPlanets = r.Next(maxPlanets - minPlanets) + minPlanets;
			for (int i = 0; i < amountOfPlanets; i++)
			{
				/*StringBuilder sb = new StringBuilder();
				sb.Append(this.name);
				sb.Append(" ");
				sb.Append(Utility.ConvertNumberToRomanNumberical(i + 1));
				planets.Add(new Planet(sb.ToString(), r, this));*/
				planets.Add(new Planet(this.name, r, this));
			}
		}
		#endregion

		#region Public Functions
		public void SetHomeworld(Empire empire, out Planet homePlanet, Random r)
		{
			if (planets.Count == 0)
			{
				planets.Add(new Planet(name, r, this)); //planets.Add(new Planet(name + " I", r, this));
				planets[0].SetHomeworld(empire, r);
				homePlanet = planets[0];
			}
			else
			{
				int whichPlanet = r.Next(planets.Count);
				planets[whichPlanet].SetHomeworld(empire, r);
				homePlanet = planets[whichPlanet];
			}
			Color = Color.Yellow; //Homeworlds always have yellow stars
			exploredBy.Add(empire);
			UpdateOwners();
		}

		private void UpdateDominantEmpire()
		{
			Dictionary<Empire, int> count = new Dictionary<Empire, int>();
			foreach (Planet planet in planets)
			{
				if (planet.Owner != null)
				{
					if (count.ContainsKey(planet.Owner))
					{
						count[planet.Owner] = count[planet.Owner] + 1;
					}
					else
					{
						count.Add(planet.Owner, 1);
					}
				}
			}
			int biggestCount = 0;
			Empire biggestOwner = null;
			foreach (KeyValuePair<Empire, int> empire in count)
			{
				if (biggestCount < empire.Value)
				{
					biggestCount = empire.Value;
					biggestOwner = empire.Key;
				}
			}
			DominantEmpire = biggestOwner;
		}

		public bool IsThisSystemExploredByEmpire(Empire empire)
		{
			return exploredBy.Contains(empire);
		}
		public bool SystemHavePlanetOwnedByEmpire(Empire empire)
		{
			return EmpiresWithPlanetsInThisSystem.Contains(empire);
		}
		public void AddEmpireExplored(Empire empire)
		{
			exploredBy.Add(empire);
		}
		public void UpdateOwners()
		{
			EmpiresWithPlanetsInThisSystem = new List<Empire>();
			int amountOfPlanetsOwned = 0;
			Dictionary<Empire, int> planetsOwned = new Dictionary<Empire,int>();
			OwnerPercentage = new Dictionary<Empire, float>();
			foreach (Planet planet in planets)
			{
				if (planet.Owner != null)
				{
					if (!EmpiresWithPlanetsInThisSystem.Contains(planet.Owner))
					{
						EmpiresWithPlanetsInThisSystem.Add(planet.Owner);
						planetsOwned.Add(planet.Owner, 1);
					}
					else
					{
						planetsOwned[planet.Owner] += 1;
					}
					amountOfPlanetsOwned++;
				}
			}

			//Update the color percentage here
			foreach (KeyValuePair<Empire, int> keyValuePair in planetsOwned)
			{
				OwnerPercentage[keyValuePair.Key] = ((float)keyValuePair.Value / (float)amountOfPlanetsOwned);
			}

			UpdateDominantEmpire();
		}
		#endregion
	}
}
