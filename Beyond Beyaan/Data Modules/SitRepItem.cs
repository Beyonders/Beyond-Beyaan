using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Modules
{
	public class SitRepItem
	{
		public Screen ScreenEventIsIn { get; private set; }
		public StarSystem SystemEventOccuredAt { get; private set; }
		public Planet PlanetEventOccuredAt { get; private set; }
		public Point PointEventOccuredAt { get; private set; }
		public string EventMessage { get; private set; }

		public SitRepItem(Screen screen, StarSystem system, Planet planet, Point point, string message)
		{
			ScreenEventIsIn = screen;
			SystemEventOccuredAt = system;
			PlanetEventOccuredAt = planet;
			PointEventOccuredAt = point;
			EventMessage = message;
		}
	}
}
