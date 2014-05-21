using System.IO;
using System.Xml.Linq;

namespace Beyond_Beyaan
{
	public class GameConfiguration
	{
		public static bool AllowGalaxyPreview { get; private set; }

		public static bool Initialize(FileInfo file, out string reason)
		{
			if (!file.Exists)
			{
				reason = "configuration.xml file does not exist.";
				return false;
			}

			XDocument doc = XDocument.Load(file.FullName);
			XElement root = doc.Element("GameConfig");

			foreach (var attribute in root.Attributes())
			{
				switch (attribute.Name.LocalName)
				{
					case "AllowGalaxyPreview":
					{
						bool result;
						if (!ParseValue(attribute.Value, out result))
						{
							reason = "Failed to parse value of 'AllowGalaxyPreview'";
							return false;
						}
						AllowGalaxyPreview = result;
						break;
					}
				}
			}

			reason = null;
			return true;
		}

		private static bool ParseValue(string value, out bool result)
		{
			int temp;
			if (!int.TryParse(value, out temp))
			{
				result = false;
				return false;
			}
			result = temp != 0;
			return true;
		}
	}
}
