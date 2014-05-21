using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Beyond_Beyaan.Data_Modules
{
	public class AI
	{
		public string AIName { get; private set; }

		public bool Initialize(FileInfo file)
		{
			List<string> errors = new List<string>();
			string[] lines;

			try
			{
				lines = File.ReadAllLines(file.FullName);
			}
			catch (Exception e)
			{
				errors.Add("Exception occured: " + e.Message);
				return false;
			}

			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
				{
					continue;
				}
				string[] parts = line.Split(new[] { '|' });
				if (parts.Length == 2)
				{
					if (string.Compare(parts[0], "name", true) == 0)
					{
						AIName = parts[1];
						continue;
					}
				}
				else
				{
					errors.Add("\"" + line + "\" is not a valid line.  Must have two values, separated by |");
					break;
				}
			}

			return errors.Count == 0;
		}
	}
}
