using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class TravelNode
	{
		public StarSystem StarSystem { get; set; }

		//For drawing
		public float Length { get; set; }
		public float Angle { get; set; }
		public bool IsValid { get; set; }
	}

	public class Fleet
	{
		#region Member Variables
		private float _galaxyX;
		private float _galaxyY;

		private Empire _empire;
		private List<TravelNode> _travelNodes;
		private List<TravelNode> _tentativeNodes;
		private Dictionary<Ship, int> ships;
		private List<Ship> _orderedShips; //For reference uses
		private List<TransportShip> _transportShips;
		private StarSystem _adjacentSystem;

		private int _maxSpeed;
		private float _remainingMoves;
		#endregion

		#region Properties
		public float GalaxyX
		{
			get { return _galaxyX; }
			set { _galaxyX = value; }
		}

		public float GalaxyY
		{
			get { return _galaxyY; }
			set { _galaxyY = value; }
		}

		public Empire Empire
		{
			get { return _empire; }
			set { _empire = value; }
		}

		public List<TravelNode> TravelNodes
		{
			get { return _travelNodes; }
			set
			{
				_travelNodes = value;
				if (_travelNodes != null && _travelNodes.Count > 0)
				{
					float totalDistance = 0;
					foreach (var node in _travelNodes)
					{
						totalDistance += node.Length;
					}
					TravelETA = (int)(totalDistance / (_maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS));
					if (totalDistance - (TravelETA * Galaxy.PARSEC_SIZE_IN_PIXELS) > 0)
					{
						TravelETA++;
					}
					TravelToFirstNodeETA = (int)(_travelNodes[0].Length / (_maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS));
					if (_travelNodes[0].Length - (TravelETA * Galaxy.PARSEC_SIZE_IN_PIXELS) > 0)
					{
						TravelToFirstNodeETA++;
					}
				}
				else
				{
					TravelETA = 0;
				}
			}
		}

		public List<TravelNode> TentativeNodes
		{
			get { return _tentativeNodes; }
			set
			{
				_tentativeNodes = value;
				if (_tentativeNodes != null)
				{
					float totalDistance = 0;
					foreach (var node in _tentativeNodes)
					{
						totalDistance += node.Length;
					}
					TentativeETA = (int)(totalDistance / (_maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS));
					if (totalDistance - (TentativeETA * Galaxy.PARSEC_SIZE_IN_PIXELS) > 0)
					{
						TentativeETA++;
					}
				}
				else
				{
					TentativeETA = 0;
				}
			}
		}

		public int TravelETA { get; private set; }
		public int TravelToFirstNodeETA { get; private set; } //So we don't display the actual ETA of an enemy's fleet
		public int TentativeETA { get; private set; }

		public StarSystem AdjacentSystem
		{
			get { return _adjacentSystem; }
			set { _adjacentSystem = value; }
		}

		public bool HasReserveTanks
		{
			get
			{
				if (HasTransports)
				{
					return false;
				}
				foreach (var ship in ships)
				{
					if (ship.Value == 0)
					{
						//Skip ships that won't be split off with this fleet
						continue;
					}
					bool hasReserve = false;
					foreach (var special in ship.Key.Specials)
					{
						if (special != null && special.Technology.ReserveFuelTanks)
						{
							hasReserve = true;
							break;
						}
					}
					if (!hasReserve)
					{
						return false;
					}
				}
				return true;
			}
		}

		public Dictionary<Ship, int> Ships
		{
			get
			{
				return ships;
			}
		}
		public List<Ship> OrderedShips
		{
			get
			{
				return _orderedShips;
			}
		}
		public int ShipCount
		{
			get
			{
				int amount = 0;
				foreach (var ship in ships)
				{
					amount += ship.Value;
				}
				return amount;
			}
		}
		public List<TransportShip> TransportShips
		{
			get { return _transportShips; }
		}
		public bool HasTransports
		{
			get { return _transportShips.Count > 0; }
		}

		public List<Empire> VisibleToWhichEmpires { get; private set; }
		#endregion

		#region Constructors
		public Fleet()
		{
			ships = new Dictionary<Ship, int>();
			_orderedShips = new List<Ship>();
			_transportShips = new List<TransportShip>();
			_remainingMoves = _maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS;
			VisibleToWhichEmpires = new List<Empire>();
		}
		#endregion

		#region Functions
		private void UpdateSpeed()
		{
			_maxSpeed = int.MaxValue;
			if (_orderedShips.Count > 0)
			{
				foreach (Ship ship in _orderedShips)
				{
					if (ship.GalaxySpeed < _maxSpeed)
					{
						_maxSpeed = ship.GalaxySpeed;
					}
				}
			}
			else
			{
				//Placeholder for now til I add technology check for best engine speed
				_maxSpeed = 1;
			}
			_remainingMoves = _maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS;
		}

		public void SetTentativePath(StarSystem destination, bool hasExtendedFuelTanks, Galaxy galaxy)
		{
			if (destination == null || destination == _adjacentSystem)
			{
				//if destination is same as origin, or nowhere, clear the tentative path
				TentativeNodes = null;
				return;
			}
			if (_travelNodes != null && _travelNodes[_travelNodes.Count - 1].StarSystem == destination)
			{
				//Same path as current path
				TentativeNodes = null;
				return;
			}
			if (TentativeNodes != null && TentativeNodes[_tentativeNodes.Count - 1].StarSystem == destination)
			{
				// Existing tentative path
				return;
			}

			StarSystem currentDestination = null;
			if (_adjacentSystem == null) //Has left a system
			{
				currentDestination = _travelNodes[0].StarSystem;
			}
			List<TravelNode> path = galaxy.GetPath(_galaxyX, _galaxyY, currentDestination, destination, hasExtendedFuelTanks, _empire);
			if (path == null)
			{
				TentativeNodes = null;
				return;
			}

			TentativeNodes = path;
			if (TentativeNodes.Count == 0)
			{
				TentativeNodes = null;
			}
		}

		public bool ConfirmPath()
		{
			if (_tentativeNodes != null)
			{
				foreach (var node in _tentativeNodes)
				{
					if (!node.IsValid)
					{
						return false;
					}
				}
				TravelNode[] nodes = new TravelNode[_tentativeNodes.Count];
				_tentativeNodes.CopyTo(nodes);
				_tentativeNodes = null;

				_travelNodes = new List<TravelNode>(nodes);
			}
			else
			{
				//Null because target is either invalid or the system the fleet is currently adjacent
				_travelNodes = null;
				_tentativeNodes = null;
			}
			return true;
		}

		/*private float CalculatePathCost(List<TravelNode> nodes)
		{
			float amount = 0;
			for (int i = 0; i < nodes.Count; i++)
			{
				float x;
				float y;
				if (i == 0)
				{
					x = (_galaxyX - nodes[i].StarSystem.X);
					y = (_galaxyY - nodes[i].StarSystem.Y);
				}
				else
				{
					x = nodes[i - 1].StarSystem.X - nodes[i].StarSystem.X;
					y = nodes[i - 1].StarSystem.Y - nodes[i].StarSystem.Y;
				}
				amount += (float)Math.Sqrt(x * x + y * y);
			}
			return amount;
		}*/

		public void AddShips(Ship ship, int amount)
		{
			if (ships.ContainsKey(ship))
			{
				ships[ship] += amount;
			}
			else
			{
				ships.Add(ship, amount);
				_orderedShips.Add(ship);
				UpdateSpeed();
			}
		}

		public void SubtractShips(Ship ship, int amount)
		{
			if (ships.ContainsKey(ship))
			{
				if (amount == -1)
				{
					//Remove this ship totally
					ships.Remove(ship);
					_orderedShips.Remove(ship);
					UpdateSpeed();
				}
				else
				{
					ships[ship] -= amount;
				}
			}
		}

		public void AddTransport(Race race, int amount)
		{
			bool added = false;
			foreach (TransportShip transport in _transportShips)
			{
				if (transport.raceOnShip == race)
				{
					transport.amount += amount;
					added = true;
					break;
				}
			}
			if (!added)
			{
				TransportShip transport = new TransportShip();
				transport.raceOnShip = race;
				transport.amount = amount;
				_transportShips.Add(transport);
			}
			_maxSpeed = 1;
		}

		public void SubtractTransport(Race race, int amount)
		{
			TransportShip transportShipToRemove = null;
			foreach (TransportShip transport in _transportShips)
			{
				if (transport.raceOnShip == race)
				{
					transport.amount -= amount;
					if (transport.amount <= 0)
					{
						transportShipToRemove = transport;
					}
				}
			}
			if (transportShipToRemove != null)
			{
				TransportShips.Remove(transportShipToRemove);
			}
		}

		public void ClearEmptyShips()
		{
			List<Ship> shipsToRemove = new List<Ship>();
			foreach (KeyValuePair<Ship, int> ship in ships)
			{
				if (ship.Value <= 0)
				{
					shipsToRemove.Add(ship.Key);
				}
			}
			foreach (Ship ship in shipsToRemove)
			{
				ships.Remove(ship);
				_orderedShips.Remove(ship);
			}
			List<TransportShip> transportsToRemove = new List<TransportShip>();
			foreach (var transport in _transportShips)
			{
				if (transport.amount <= 0)
				{
					transportsToRemove.Add(transport);
				}
			}
			foreach (var transport in transportsToRemove)
			{
				_transportShips.Remove(transport);
			}
			UpdateSpeed();
		}

		public void ResetMove()
		{
			UpdateSpeed();
			_remainingMoves = _maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS;
		}

		public bool Move(float frameDeltaTime)
		{
			if (_travelNodes == null)
			{
				return false;
			}
			if (_remainingMoves > 0)
			{
				_adjacentSystem = null; //Left the system
				float amountToMove = frameDeltaTime * _maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS;
				if (amountToMove > _remainingMoves)
				{
					amountToMove = _remainingMoves;
				}
				_remainingMoves -= amountToMove;

				float xMov = (float)(Math.Cos(_travelNodes[0].Angle * (Math.PI / 180)) * amountToMove);
				float yMov = (float)(Math.Sin(_travelNodes[0].Angle * (Math.PI / 180)) * amountToMove);

				bool isLeftOfNode = _galaxyX <= _travelNodes[0].StarSystem.X;
				bool isTopOfNode = _galaxyY <= _travelNodes[0].StarSystem.Y;

				_galaxyX += xMov;
				_galaxyY += yMov;

				if ((_galaxyX > _travelNodes[0].StarSystem.X && isLeftOfNode) ||
					(_galaxyX <= _travelNodes[0].StarSystem.X && !isLeftOfNode) ||
					(_galaxyY > _travelNodes[0].StarSystem.Y && isTopOfNode) ||
					(_galaxyY <= _travelNodes[0].StarSystem.Y && !isTopOfNode))
				{
					//TODO: Carry over excess movement to next node

					//It has arrived at destination
					_galaxyX = _travelNodes[0].StarSystem.X;
					_galaxyY = _travelNodes[0].StarSystem.Y;
					_adjacentSystem = _travelNodes[0].StarSystem;
					_travelNodes.RemoveAt(0);
					if (_travelNodes.Count == 0)
					{
						_travelNodes = null;
						_remainingMoves = 0;
					}
				}
				else
				{
					float x = _travelNodes[0].StarSystem.X - _galaxyX;
					float y = _travelNodes[0].StarSystem.Y - _galaxyY;
					_travelNodes[0].Length = (float)Math.Sqrt((x * x) + (y * y));
					_travelNodes[0].Angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
				}
			}
			return _remainingMoves > 0;
		}

		public float GetExpenses()
		{
			float amount = 0;
			foreach (KeyValuePair<Ship, int> ship in ships)
			{
				amount += ship.Key.Maintenance * ship.Value;
			}
			return amount;
		}

		public void ColonizePlanet(Ship whichShip)
		{
			//This assumes that whichShip is not null, adjacentSystem is not null, and everything has already been validated (i.e. ship has correct colony pod)
			ships[whichShip]--;
			if (ships[whichShip] == 0)
			{
				//only one ship, so remove the entry for it
				ships.Remove(whichShip);
				_orderedShips.Remove(whichShip);
			}
			_adjacentSystem.Planets[0].Colonize(_empire);
		}

		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("Fleet");
			writer.WriteAttributeString("X", _galaxyX.ToString());
			writer.WriteAttributeString("Y", _galaxyY.ToString());
			writer.WriteAttributeString("AdjacentSystem", _adjacentSystem == null ? "-1" : _adjacentSystem.ID.ToString());
			if (_travelNodes != null)
			{
				writer.WriteStartElement("TravelNodes");
				foreach (var travelNode in _travelNodes)
				{
					writer.WriteStartElement("TravelNode");
					writer.WriteAttributeString("Destination", travelNode.StarSystem.ID.ToString());
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			foreach (var ship in ships)
			{
				writer.WriteStartElement("Ship");
				writer.WriteAttributeString("ShipDesign", ship.Key.DesignID.ToString());
				writer.WriteAttributeString("NumberOfShips", ship.Value.ToString());
				writer.WriteEndElement();
			}
			foreach (var transport in _transportShips)
			{
				writer.WriteStartElement("Transport");
				writer.WriteAttributeString("Race", transport.raceOnShip.RaceName);
				writer.WriteAttributeString("Count", transport.amount.ToString());
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		public void Load(XElement fleet, FleetManager fleetManager, Empire empire, GameMain gameMain)
		{
			_empire = empire;
			_galaxyX = float.Parse(fleet.Attribute("X").Value);
			_galaxyY = float.Parse(fleet.Attribute("Y").Value);
			_adjacentSystem = gameMain.Galaxy.GetStarWithID(int.Parse(fleet.Attribute("AdjacentSystem").Value));
			var travelNodes = fleet.Element("TravelNodes");
			if (travelNodes != null)
			{
				_travelNodes = new List<TravelNode>();
				StarSystem startingPlace = null;
				foreach (var travelNode in travelNodes.Elements())
				{
					var destination = gameMain.Galaxy.GetStarWithID(int.Parse(travelNode.Attribute("Destination").Value));
					if (startingPlace == null)
					{
						_travelNodes.Add(gameMain.Galaxy.GenerateTravelNode(_galaxyX, _galaxyY, destination));
					}
					else
					{
						_travelNodes.Add(gameMain.Galaxy.GenerateTravelNode(startingPlace, destination));
					}
					startingPlace = destination;
				}
			}
			foreach (var ship in fleet.Elements("Ship"))
			{
				AddShips(fleetManager.GetShipWithDesignID(int.Parse(ship.Attribute("ShipDesign").Value)), int.Parse(ship.Attribute("NumberOfShips").Value));
			}
			foreach (var transport in fleet.Elements("Transport"))
			{
				AddTransport(gameMain.RaceManager.GetRace(transport.Attribute("Race").Value), int.Parse(transport.Attribute("Count").Value));
			}
		}
		#endregion
	}
}
