using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	public static class NameGenerator
	{
		static Random rnd = null;

		static public List<StringGetter[]> Generator = null;

		private static List<string> StarNames = new List<string>
		{
			"Govik",
			"Smeggit",
			"Starker",
			"Sharpe",
			"Drakknott",
			"Hypatia",
			"Tyson",
			"Jerobynn",
			"Drused",
			"Xin",
			"Moby",
			"Zandalar",
			"Solifugus",
			"Boundless Butterfly Galaxy of Chase",
			"Robynseye",
			"Arouca",
			"Aki Ohana",
			"Predestination",
			"Star's End",
			"Kar Fimbul",
			"Firefly",
			"Boordin",
			"Whichammer",
			"Dyson Alpha",
			"Feona",
			"Oatams",
			"Ersilia",
			"Sarah's Sunflower",
			"Bylon",
			"Rangulus",
			"Ptolemae",
		};

		public delegate string StringGetter();

		static List<string> NonEndingConsonantChunks = new List<string>(new string[]
        {
            "b", "br", "bl", "c", "ch", "cr", "cl", "d",
            "dr", "f", "fl", "fr", "g", "gr", "gl", "gh", "h", "j", "k",
            "kr", "kl", "l", "m", "n", "p", "pl", "pr", "qu", "r", "s",
            "st", "str", "sh", "sl", "sp", "sk", "sc", "sm", "sn",
            "t", "tr", "v", "w", "x", "y", "z"
        });

		static List<string> EndingConsonantChunks = new List<string>(new string[]
        {
            "b", 
            "c", "c", "c", "c", "c", "c", "c", "c", "c",
            "ld",
            "d",
            "f",
            "g",
            "gh",
            "h",
            "k",
            "l",
            "lm",
            "ln",
            "m",
            "n", "n","n", "n", "n", "n", "n", "n", "n", "n", "n", "n",
            "nd",
            "p",
            "r",
            "rd",
            "rn",
            "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s",
            "t", "t", "t", "t", "t",
            "v",
            "w",
            "x",
            "y", "y", "y", "y", "y",
            "z"
        });

		static List<string> Vowelies = new List<string>(new string[] {
            "a",
            "e",
            "i",
            "o",
            "u",
            "ou",
            "ea",
            "ie",
            "ei"
        });

		public static string NEC()
		{
			return NonEndingConsonantChunks[rnd.Next(NonEndingConsonantChunks.Count - 1)];
		}
		public static string V()
		{
			return Vowelies[rnd.Next(Vowelies.Count - 1)];
		}
		public static string EC()
		{
			return EndingConsonantChunks[rnd.Next(EndingConsonantChunks.Count - 1)];
		}
		public static string S()
		{
			return " ";
		}

		static StringGetter[] Seq(params StringGetter[] arr)
		{
			return arr;
		}

		static NameGenerator()
		{
			Generator = new List<StringGetter[]>();

			//Seed the random generator
			rnd = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

			Generator.AddRange(new StringGetter[][] {
                Seq(NEC, V, NEC, V, EC),
                Seq(NEC, V, NEC, V, NEC, V, EC),
                Seq(NEC, V, NEC, EC),
                Seq(NEC, V, EC),
                Seq(NEC, V, S, NEC, V, NEC, V, EC),
                Seq(NEC, V, S, NEC, V, NEC, V, NEC, V, EC)}
				);
		}

		public static string GetStarName(Random r)
		{
			if (StarNames.Count > 0)
			{
				int index = r.Next(StarNames.Count);
				string name = StarNames[index];
				StarNames.RemoveAt(index);
				return name;
			}
			return GetName();
		}

		public static void ResetStarNames()
		{
			//When starting a new game, need to reset star names
			StarNames = new List<string>
			{
				"Govik",
				"Smeggit",
				"Starker",
				"Sharpe",
				"Drakknott",
				"Hypatia",
				"Tyson",
				"Jerobynn",
				"Drused",
				"Xin",
				"Moby",
				"Zandalar",
				"Solifugus",
				"Boundless Butterfly Galaxy of Chase",
				"Robynseye",
				"Arouca",
				"Aki Ohana",
				"Predestination",
				"Star's End",
				"Kar Fimbul",
				"Firefly",
				"Boordin",
				"Whichammer",
				"Dyson Alpha",
				"Feona",
				"Oatams",
				"Ersilia",
				"Sarah's Sunflower",
				"Bylon",
				"Rangulus",
				"Ptolemae",
			};
		}

		//Function to call to get a random name
		public static string GetName()
		{
			StringBuilder sb = new StringBuilder();
			StringGetter[] Getters = Generator[rnd.Next(Generator.Count - 1)];

			bool first = true;
			foreach (StringGetter g in Getters)
			{
				string s = g();

				if (first)
				{
					s = string.Format("{0}{1}", Char.ToUpper(s[0]), s.Substring(1));
					first = false;
				}

				sb.Append(s);

				if (s == " ")
					first = true;
			}

			return sb.ToString();
		}
	}
}

