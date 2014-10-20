using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;


namespace Beyond_Beyaan
{
	static class Utility
	{
		/// <summary>
		/// Calculates a full circle
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="originSize">Size of the object</param>
		/// <returns></returns>
		public static bool[][] CalculateDisc(int radius, int originSize)
		{
			if (originSize < 1)
			{
				return null;
			}
			bool[][] grid = CalculateCircle(radius, originSize);

			for (int i = 0; i < grid.Length; i++)
			{
				int top = 0;
				int bottom = grid[i].Length - 1;
				bool foundTop = false;
				bool foundBottom = false;

				while (top <= bottom)
				{
					if (grid[i][top])
					{
						foundTop = true;
					}
					if (grid[i][bottom])
					{
						foundBottom = true;
					}
					if (foundTop)
					{
						grid[i][top] = true;
					}
					if (foundBottom)
					{
						grid[i][bottom] = true;
					}
					top++;
					bottom--;
				}
			}

			return grid;
		}

		public static bool[][] CalculateCircle(int radius, int originSize)
		{
			if (originSize < 1)
			{
				return null;
			}
			int size = (radius * 2) + originSize;

			bool[][] grid = new bool[size][];
			for (int i = 0; i < grid.Length; i++)
			{
				grid[i] = new bool[size];
			}

			int f = 1 - radius;
			int ddF_x = 1;
			int ddF_y = -2 * radius;
			int x = 0;
			int y = radius;

			int origin = radius;
			int modifiedOriginSize = originSize - 1;

			for (int i = 0; i < originSize; i++)
			{
				grid[origin + i][origin + radius + modifiedOriginSize] = true;
				grid[origin + i][0] = true;
				grid[origin + radius + modifiedOriginSize][origin + i] = true;
				grid[0][origin + i] = true;
			}

			while (x < y)
			{
				if (f >= 0)
				{
					y--;
					ddF_y += 2;
					f += ddF_y;
				}
				x++;
				ddF_x += 2;
				f += ddF_x;


				grid[origin + modifiedOriginSize + x][origin + modifiedOriginSize + y] = true;
				grid[origin - x][origin + modifiedOriginSize + y] = true;
				grid[origin + modifiedOriginSize + x][origin - y] = true;
				grid[origin - x][origin - y] = true;

				grid[origin + modifiedOriginSize + y][origin + modifiedOriginSize + x] = true;
				grid[origin - y][origin + modifiedOriginSize + x] = true;
				grid[origin + modifiedOriginSize + y][origin - x] = true;
				grid[origin - y][origin - x] = true;
			}

			return grid;
		}

		/// <summary>
		/// Converts from a numeric value to roman numbers
		/// </summary>
		/// <param name="value">Value to convert</param>
		/// <returns>Resulting Roman number</returns>
		public static string ConvertNumberToRomanNumberical(int value)
		{
			//This algorithm is courtesy of
			//http://www.blackwasp.co.uk/NumberToRoman.aspx

			if (value < 0 || value > 3999)
			{
				throw new ArgumentException("Value must be in the range 0 - 3,999.");
			}
			if (value == 0)
			{
				return "N";
			}
			int[] values = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
			string[] numerals = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

			StringBuilder result = new StringBuilder();

			// Loop through each of the values to diminish the number
			for (int i = 0; i < 13; i++)
			{
				// If the number being converted is less than the test value, append
				// the corresponding numeral or numeral pair to the resultant string
				while (value >= values[i])
				{
					value -= values[i];
					result.Append(numerals[i]);
				}
			}

			return result.ToString();
		}

		public static int GetIntValue(string value, Random r)
		{
			//There are three possible types of "value"
			//The first is a basic value, like "5", it is always returned
			//The second is a range of values, like "0,5", randomly pick one inside the range
			//The third is a weighted range of values, like "0,1,0.05" which leans toward 0 with 5% chance of picking 1
			string[] parts = value.Split(new[] { ',' });
			if (parts.Length == 1)
			{
				//Just return the value
				return int.Parse(parts[0]);
			}
			if (parts.Length == 2)
			{
				//return a value in the range
				return r.Next(int.Parse(parts[0]), int.Parse(parts[1]) + 1);
			}
			if (parts.Length == 3)
			{
				//return a value in the weighted range
				double randVal;
				do
				{
					randVal = r.NextDouble();
				} while (randVal == 0); //Make sure it's not 0, otherwise it'd throw an exception in the next line of code

				double weight = NormalCDFInverse(randVal);
				int min = int.Parse(parts[0]);
				int max = int.Parse(parts[1]);
				float shift = float.Parse(parts[2], CultureInfo.InvariantCulture); //Shift moves the standard distribution left or right (does not skew it, just moves it, 0.5 is default)
				int newValue = (int)(((min + max) * shift) + (weight * (max - min)));
				if (newValue < min)
				{
					return min;
				}
				if (newValue > max)
				{
					return max;
				}
				return newValue;
			}
			throw new Exception("GetIntValue cannot parse the value of '" + value + "'");
		}

		//This function was obtained from http://www.johndcook.com/csharp_phi_inverse.html

		//Brent here: It seems like LogOnePlusX isn't actually used, perhaps it's a faster but less accurate version of Math.Log?  To-do: Investigate.

		// compute log(1+x) without losing precision for small values of x
		private static double LogOnePlusX(double x)
		{
			if (x <= -1.0)
			{
				string msg = String.Format("Invalid input argument: {0}", x);
				throw new ArgumentOutOfRangeException(msg);
			}

			if (Math.Abs(x) > 1e-4)
			{
				// x is large enough that the obvious evaluation is OK
				return Math.Log(1.0 + x);
			}

			// Use Taylor approx. 
			// log(1 + x) = x - x^2/2 with error roughly x^3/3
			// Since |x| < 10^-4, |x|^3 < 10^-12, 
			// relative error less than 10^-8

			return (-0.5 * x + 1.0) * x;
		}

		private static double RationalApproximation(double t)
		{
			// Abramowitz and Stegun formula 26.2.23.
			// The absolute value of the error should be less than 4.5 e-4.
			double[] c = { 2.515517, 0.802853, 0.010328 };
			double[] d = { 1.432788, 0.189269, 0.001308 };
			return t - ((c[2] * t + c[1]) * t + c[0]) /
						(((d[2] * t + d[1]) * t + d[0]) * t + 1.0);
		}

		private static double NormalCDFInverse(double p)
		{
			if (p <= 0.0 || p >= 1.0)
			{
				string msg = String.Format("Invalid input argument: {0}.", p);
				throw new ArgumentOutOfRangeException(msg);
			}

			// See article above for explanation of this section.
			if (p < 0.5)
			{
				// F^-1(p) = - G^-1(p)
				return -RationalApproximation(Math.Sqrt(-2.0 * Math.Log(p)));
			}
			// F^-1(p) = G^-1(1-p)
			return RationalApproximation(Math.Sqrt(-2.0 * Math.Log(1.0 - p)));
		}

		public static string PlanetTypeToString(PLANET_TYPE planetType)
		{
			switch (planetType)
			{
				case PLANET_TYPE.ARCTIC: return "Arctic";
				case PLANET_TYPE.BADLAND: return "Badlands";
				case PLANET_TYPE.BARREN: return "Barren";
				case PLANET_TYPE.DEAD: return "Dead";
				case PLANET_TYPE.DESERT: return "Desert";
				case PLANET_TYPE.JUNGLE: return "Jungle";
				case PLANET_TYPE.NONE: return "None";
				case PLANET_TYPE.OCEAN: return "Oceanic";
				case PLANET_TYPE.RADIATED: return "Radiated";
				case PLANET_TYPE.STEPPE: return "Steppe";
				case PLANET_TYPE.TERRAN: return "Terran";
				case PLANET_TYPE.TOXIC: return "Toxic";
				case PLANET_TYPE.TUNDRA: return "Tundra";
				case PLANET_TYPE.VOLCANIC: return "Volcanic";
			}
			return String.Empty;
		}

		/*public static SpriteName PlanetConstructionBonusToSprite(PLANET_CONSTRUCTION_BONUS bonus)
		{
			switch (bonus)
			{
				case PLANET_CONSTRUCTION_BONUS.ULTRAPOOR: return SpriteName.PlanetConstructionBonus1;
				case PLANET_CONSTRUCTION_BONUS.POOR: return SpriteName.PlanetConstructionBonus2;
				case PLANET_CONSTRUCTION_BONUS.ULTRARICH: return SpriteName.PlanetConstructionBonus3;
				case PLANET_CONSTRUCTION_BONUS.RICH: return SpriteName.PlanetConstructionBonus4;
			}
			//If it reaches here, something went wrong
			return SpriteName.CancelBackground;
		}
		public static SpriteName PlanetEnvironmentBonusToSprite(PLANET_ENVIRONMENT_BONUS bonus)
		{
			switch (bonus)
			{
				case PLANET_ENVIRONMENT_BONUS.HOSTILE: return SpriteName.PlanetEnvironmentBonus2;
				case PLANET_ENVIRONMENT_BONUS.FERTILE: return SpriteName.PlanetEnvironmentBonus3;
				case PLANET_ENVIRONMENT_BONUS.GAIA: return SpriteName.PlanetEnvironmentBonus4;
			}
			//If it reaches here, something went wrong
			return SpriteName.CancelBackground;
		}
		public static SpriteName PlanetEntertainmentBonusToSprite(PLANET_RESEARCH_BONUS bonus)
		{
			switch (bonus)
			{
				case PLANET_RESEARCH_BONUS.ARTIFACTS: return SpriteName.PlanetEntertainmentBonus3;
				case PLANET_RESEARCH_BONUS.BEYAAN: return SpriteName.PlanetEntertainmentBonus4;
			}
			//If it reaches here, something went wrong
			return SpriteName.CancelBackground;
		}*/

		public static string ShipSizeToString(int size)
		{
			switch (size)
			{
				case 1: return "Lancer";
				case 2: return "Corvette";
				case 3: return "Frigate";
				case 4: return "Destroyer";
				case 5: return "Cruiser";
				case 6: return "Battlecruiser";
				case 7: return "Battleship";
				case 8: return "Behemoth";
				case 9: return "Titan";
				case 10: return "Leviathian";
			}
			return string.Empty;
		}

		public static string RelationToLabel(int relation)
		{
			if (relation < 25)
			{
				return "Hate";
			}
			if (relation < 50)
			{
				return "Loathe";
			}
			if (relation < 75)
			{
				return "Despise";
			}
			if (relation < 95)
			{
				return "Dislike";
			}
			if (relation < 105)
			{
				return "Neutral";
			}
			if (relation < 125)
			{
				return "Like";
			}
			if (relation < 150)
			{
				return "Respect";
			}
			if (relation < 175)
			{
				return "Esteem";
			}
			return "Venerate";
		}

		public static List<FileInfo> GetSaveGames(string dataSetPath)
		{
			string path = Path.Combine(dataSetPath, "Saves");
			DirectoryInfo di = new DirectoryInfo(path);
			if (!di.Exists)
			{
				return new List<FileInfo>();
			}
			try
			{
				return new List<FileInfo>(di.GetFiles("*.BB"));
			}
			catch
			{
				return new List<FileInfo>();
			}
		}
	}

	public struct Point
	{
		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static bool operator ==(Point p1, Point p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(Point p1, Point p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public struct PointF
	{
		public float X;
		public float Y;

		public PointF(float x, float y)
		{
			X = x;
			Y = y;
		}

		public static bool operator ==(PointF p1, PointF p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(PointF p1, PointF p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
