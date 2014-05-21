using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public static class SpriteManager
	{
		public static Dictionary<string, BaseSprite> Sprites { get; private set; }
		private static bool _initalized;

		public static bool Initialize(DirectoryInfo directory, out string reason)
		{
			if (_initalized)
			{
				reason = null;
				return true;
			}
			Sprites = new Dictionary<string, BaseSprite>();
			string file = Path.Combine(directory.FullName, "sprites.xml");
			string graphicDirectory = Path.Combine(directory.FullName, "graphics");
			if (!File.Exists(file))
			{
				reason = "Sprites.xml file does not exist";
				return false;
			}

			XDocument doc = XDocument.Load(file);
			XElement root = doc.Element("Sprites");
			foreach (XElement sprite in root.Elements())
			{
				var newSprite = new BaseSprite();
				if (!newSprite.LoadSprite(sprite, graphicDirectory, out reason))
				{
					return false;
				}
				Sprites.Add(newSprite.Name, newSprite);
			}
			_initalized = true;
			reason = null;
			return true;
		}

		public static BBSprite GetSprite(string name, Random r)
		{
			if (Sprites.ContainsKey(name))
			{
				return new BBSprite(Sprites[name], r);
			}
			return null;
		}
	}
}
