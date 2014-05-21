using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class SitRepManager
	{
		public List<SitRepItem> Items { get; private set; }

		public bool HasItems { get { return Items.Count > 0; } }

		public SitRepManager()
		{
			Items = new List<SitRepItem>();
		}

		public void AddItem(SitRepItem sitRepItem)
		{
			Items.Add(sitRepItem);
		}

		public void ClearItems()
		{
			Items.Clear();
		}
	}
}
