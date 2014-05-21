using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using GorgonLibrary.Graphics;

namespace Beyond_Beyaan.Data_Managers
{
	public static class FontManager
	{
		private static bool _initalized;
		private static Dictionary<string, Font> _fonts;
		private static string _defaultFont;

		public static bool Initialize(DirectoryInfo dataSet, out string reason)
		{
			// TODO: Add font attributes at load such as bold, etc in font.xml file
			if (_initalized)
			{
				reason = null;
				return true;
			}
			_fonts = new Dictionary<string, Font>();

			try
			{
				XDocument file = XDocument.Load(Path.Combine(dataSet.FullName, "fonts.xml"));
				XElement root = file.Element("Fonts");

				DirectoryInfo fontDirectory = new DirectoryInfo(Path.Combine(dataSet.FullName, "Fonts"));

				foreach (var element in root.Elements())
				{
					string name = string.Empty;
					int size = 12;
					string fileName = string.Empty;

					foreach (var attribute in element.Attributes())
					{
						switch (attribute.Name.LocalName.ToLower())
						{
							case "name":
								{
									name = attribute.Value;
								} break;
							case "size":
								{
									size = int.Parse(attribute.Value);
								} break;
							case "file":
								{
									fileName = attribute.Value;
								} break;
						}
					}
					if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(fileName))
					{
						reason = "Font name or file is missing in fonts.xml.";
						return false;
					}
					var files = fontDirectory.GetFiles(fileName + ".ttf");
					if (files != null && files.Length > 0)
					{
						Font font = Font.FromFile(files[0].FullName, size, true);
						_fonts.Add(name, font);
					}
					else
					{
						Font font = new Font(name, fileName, size, true);
						_fonts.Add(name, font);
					}
					if (string.IsNullOrEmpty(_defaultFont))
					{
						_defaultFont = name;
					}
				}

				_initalized = true;
				reason = null;
				return true;
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
		}

		public static Font GetFont(string name)
		{
			if (_fonts.ContainsKey(name))
			{
				return _fonts[name];
			}
			return null;
		}

		public static Font GetDefaultFont()
		{
			if (_fonts == null || _fonts.Count == 0)
			{
				return null;
			}
			return _fonts[_defaultFont];
		}
	}
}
