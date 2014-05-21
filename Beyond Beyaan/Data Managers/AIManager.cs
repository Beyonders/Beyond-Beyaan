using System;
using System.Collections.Generic;
using System.IO;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	class AIManager
	{
		public List<AI> AIs { get; private set; }

		public AIManager()
		{
			AIs = new List<AI>();
		}

		public bool Initialize(DirectoryInfo directory, out string reason)
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo(Path.Combine(directory.FullName, "ai"));
				if (!di.Exists)
				{
					//If it don't exist, create one so users can add races
					di.Create();
				}
				foreach (FileInfo fi in di.GetFiles("*.txt"))
				{
					AI ai = new AI();
					if (ai.Initialize(fi))
					{
						AIs.Add(ai);
					}
				}
				reason = null;
				return true;
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
		}
	}
}
