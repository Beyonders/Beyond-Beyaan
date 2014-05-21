using System.Collections.Generic;

namespace Beyond_Beyaan
{
	public class FleetGroup
	{
		#region Properties

		public Fleet SelectedFleet { get; private set; }

		public Fleet FleetToSplit { get; private set; }

		public List<Fleet> Fleets { get; private set; }

		#endregion

		#region Constructors
		public FleetGroup(List<Fleet> fleets)
		{
			Fleets = fleets;
			if (fleets != null && fleets.Count > 0)
			{
				SelectFleet(0);
			}
		}
		#endregion

		#region Functions
		public void SelectFleet(int whichFleet)
		{
			if (whichFleet < Fleets.Count)
			{
				SelectedFleet = Fleets[whichFleet];

				FleetToSplit = new Fleet();
				FleetToSplit.Empire = SelectedFleet.Empire;
				FleetToSplit.TravelNodes = SelectedFleet.TravelNodes;
				FleetToSplit.TentativeNodes = SelectedFleet.TentativeNodes;
				FleetToSplit.AdjacentSystem = SelectedFleet.AdjacentSystem;
				FleetToSplit.GalaxyX = SelectedFleet.GalaxyX;
				FleetToSplit.GalaxyY = SelectedFleet.GalaxyY;
				foreach (KeyValuePair<Ship, int> ship in SelectedFleet.Ships)
				{
					FleetToSplit.AddShips(ship.Key, ship.Value);
				}
				foreach (var transport in SelectedFleet.TransportShips)
				{
					FleetToSplit.AddTransport(transport.raceOnShip, transport.amount);
				}
			}
		}

		public void SplitFleet(Empire empire)
		{
			Fleet fleet = new Fleet();
			fleet.Empire = FleetToSplit.Empire;
			fleet.GalaxyX = FleetToSplit.GalaxyX;
			fleet.GalaxyY = FleetToSplit.GalaxyY;
			fleet.AdjacentSystem = FleetToSplit.AdjacentSystem;

			foreach (KeyValuePair<Ship, int> ship in FleetToSplit.Ships)
			{
				if (ship.Value > 0)
				{
					SelectedFleet.SubtractShips(ship.Key, ship.Value);
					fleet.AddShips(ship.Key, ship.Value);
				}
			}
			foreach (var transport in FleetToSplit.TransportShips)
			{
				if (transport.amount > 0)
				{
					SelectedFleet.SubtractTransport(transport.raceOnShip, transport.amount);
					fleet.AddTransport(transport.raceOnShip, transport.amount);
				}
			}
			SelectedFleet.ClearEmptyShips();
			fleet.ClearEmptyShips();
			fleet.TravelNodes = FleetToSplit.TravelNodes;
			if (SelectedFleet.Ships.Count == 0 && SelectedFleet.TransportShips.Count == 0)
			{
				Fleets.Remove(SelectedFleet);
				empire.FleetManager.RemoveFleet(SelectedFleet);
			}
			if (fleet.Ships.Count > 0 || fleet.TransportShips.Count > 0)
			{
				Fleets.Add(fleet);
				empire.FleetManager.AddFleet(fleet);
			}
			SelectedFleet = fleet;
		}
		#endregion
	}
}
