using System;
using System.Collections.Generic;
using System.Drawing;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class FleetView : WindowInterface
	{
		#region Member Variables
		private FleetGroup _selectedFleetGroup;
		private Fleet _selectedFleet;

		private BBLabel _empireNameLabel;
		private BBStretchableImage _empireBackground;
		private BBButton _nextFleet;
		private BBButton _previousFleet;

		private BBStretchableImage[] _shipBackground;
		private BBStretchableImage _shipPreview;
		private BBScrollBar[] _shipSliders;
		private BBLabel[] _shipLabels;
		private int _maxVisible;

		private bool _showingPreview;
		private BBSprite _shipSprite;
		private Point _shipPoint;
		private float[] _empireColor;
		private bool _isTransports;
		#endregion

		#region Constructor
		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!Initialize(gameMain.ScreenWidth - 300, 0, 300, 480, StretchableImageType.ThinBorderBG, gameMain, true, gameMain.Random, out reason))
			{
				return false;
			}

			_empireBackground = new BBStretchableImage();
			_empireNameLabel = new BBLabel();
			_nextFleet = new BBButton();
			_previousFleet = new BBButton();

			_shipBackground = new BBStretchableImage[6];
			_shipPreview = new BBStretchableImage();

			_shipSliders = new BBScrollBar[6];
			_shipLabels = new BBLabel[6];

			if (!_empireBackground.Initialize(_xPos + 10, _yPos + 10, 280, 40, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_empireNameLabel.Initialize(_xPos + 10, _yPos + 10, string.Empty, Color.White, out reason))
			{
				return false;
			}
			if (!_previousFleet.Initialize("ScrollLeftBGButton", "ScrollLeftFGButton", string.Empty, ButtonTextAlignment.LEFT, _xPos + 18, _yPos + 22, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_nextFleet.Initialize("ScrollRightBGButton", "ScrollRightFGButton", string.Empty, ButtonTextAlignment.LEFT, _xPos + 266, _yPos + 22, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_shipPreview.Initialize(0, 0, 170, 170, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			
			for (int i = 0; i < _shipBackground.Length; i++)
			{
				_shipBackground[i] = new BBStretchableImage();
				_shipLabels[i] = new BBLabel();
				_shipSliders[i] = new BBScrollBar();

				if (!_shipBackground[i].Initialize(_xPos + 10, _yPos + 55 + (i * 55), 280, 55, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_shipLabels[i].Initialize(_xPos + 15, _yPos + 65 + (i * 55), "Test", Color.White, out reason))
				{
					return false;
				}
				if (!_shipSliders[i].Initialize(_xPos + 15, _yPos + 85 + (i * 55), 270, 1, 1, true, true, gameMain.Random, out reason))
				{
					return false;
				}
			}
			return true;
		}
		#endregion

		public override void Draw()
		{
			base.Draw();
			_empireBackground.Draw();
			_empireNameLabel.Draw();
			_previousFleet.Draw();
			_nextFleet.Draw();
			for (int i = 0; i < _maxVisible; i++)
			{
				_shipBackground[i].Draw();
				_shipLabels[i].Draw();
				if (!_isTransports)
				{
					_shipSliders[i].Draw();
				}
			}
			if (_showingPreview)
			{
				_shipPreview.Draw();
				GorgonLibrary.Gorgon.CurrentShader = _gameMain.ShipShader;
				_gameMain.ShipShader.Parameters["EmpireColor"].SetValue(_empireColor);
				_shipSprite.Draw(_shipPoint.X, _shipPoint.Y);
				GorgonLibrary.Gorgon.CurrentShader = null;
			}
		}

		public override void MoveWindow()
		{
			base.MoveWindow();

			_empireBackground.MoveTo(_xPos + 10, _yPos + 10);
			_empireNameLabel.MoveTo(_xPos + 150 - (int)(_empireNameLabel.GetWidth() / 2), _yPos + 30 - (int)(_empireNameLabel.GetHeight() / 2));
			_previousFleet.MoveTo(_xPos + 18, _yPos + 22);
			_nextFleet.MoveTo(_xPos + 266, _yPos + 22);
			
			for (int i = 0; i < _shipBackground.Length; i++)
			{
				_shipBackground[i].MoveTo(_xPos + 10, _yPos + 55 + (i * 55));
				_shipLabels[i].MoveTo(_xPos + 15, _yPos + 65 + (i * 55));
				_shipSliders[i].MoveTo(_xPos + 15, _yPos + 85 + (i * 55));
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			_previousFleet.MouseHover(x, y, frameDeltaTime);
			_nextFleet.MouseHover(x, y, frameDeltaTime);
			bool withinX = (x >= _xPos + 10 && x < _xPos + 290);
			bool showingPreview = false;
			for (int i = 0; i < _maxVisible; i++)
			{
				if (_shipSliders[i].MouseHover(x, y, frameDeltaTime))
				{
					result = true;
					if (!_isTransports)
					{
						Ship ship = _selectedFleet.OrderedShips[i];
						_selectedFleetGroup.FleetToSplit.Ships[ship] = _shipSliders[i].TopIndex;
						_shipLabels[i].SetText(_shipSliders[i].TopIndex + " x " + ship.Name);
					}
				}
				int tempY = _yPos + 55 + (i * 55);
				if (!_isTransports && withinX && y >= tempY && y < tempY + 55)
				{
					var ship = _selectedFleet.OrderedShips[i];
					_shipSprite = ship.Owner.EmpireRace.GetShip(ship.Size, ship.WhichStyle);
					_empireColor = ship.Owner.ConvertedColor;
					//Show ship preview for this ship
					if (_xPos > 170)
					{
						_shipPreview.MoveTo(_xPos - 170, tempY - 62);
						_shipPoint.X = _xPos - 85;
						_shipPoint.Y = tempY + 23;
						showingPreview = true;
					}
					else
					{
						_shipPreview.MoveTo(_xPos + 300, tempY - 62);
						_shipPoint.X = _xPos + 385;
						_shipPoint.Y = tempY + 23;
						showingPreview = true;
					}
				}
			}
			_showingPreview = showingPreview;
			return base.MouseHover(x, y, frameDeltaTime) || result;
		}

		public override bool MouseDown(int x, int y)
		{
			_nextFleet.MouseDown(x, y);
			_previousFleet.MouseDown(x, y);

			if (!_isTransports)
			{
				for (int i = 0; i < _maxVisible; i++)
				{
					if (_shipSliders[i].MouseDown(x, y))
					{
						return true;
					}
				}
			}
			return base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = false;
			if (_previousFleet.MouseUp(x, y))
			{
				int index = _selectedFleetGroup.Fleets.IndexOf(_selectedFleet);
				index--;
				if (index < 0)
				{
					index = _selectedFleetGroup.Fleets.Count - 1;
				}
				_selectedFleetGroup.SelectFleet(index);
				_selectedFleet = _selectedFleetGroup.SelectedFleet;
				LoadShips();
				result = true;
			}
			if (_nextFleet.MouseUp(x, y))
			{
				int index = _selectedFleetGroup.Fleets.IndexOf(_selectedFleet);
				index++;
				if (index >= _selectedFleetGroup.Fleets.Count)
				{
					index = 0;
				}
				_selectedFleetGroup.SelectFleet(index);
				_selectedFleet = _selectedFleetGroup.SelectedFleet;
				LoadShips();
				result = true;
			}
			if (!_isTransports)
			{
				for (int i = 0; i < _maxVisible; i++)
				{
					if (_shipSliders[i].MouseUp(x, y))
					{
						Ship ship = _selectedFleet.OrderedShips[i];
						_selectedFleetGroup.FleetToSplit.Ships[ship] = _shipSliders[i].TopIndex;
						_shipLabels[i].SetText(_shipSliders[i].TopIndex + " x " + ship.Name);
						result = true;
					}
				}
			}
			return base.MouseUp(x, y) || result;
		}

		public void LoadFleetGroup(FleetGroup selectedFleetGroup)
		{
			_selectedFleetGroup = selectedFleetGroup;
			_selectedFleet = selectedFleetGroup.SelectedFleet;

			if (_selectedFleetGroup.Fleets.Count == 1)
			{
				//Disable the next/prev buttons
				_nextFleet.Active = false;
				_previousFleet.Active = false;
			}
			else
			{
				_nextFleet.Active = true;
				_previousFleet.Active = true;
			}
			
			RefreshFleet();
			LoadShips();
		}

		private void LoadShips()
		{
			if (_selectedFleet.Ships.Count > 0)
			{
				_maxVisible = _selectedFleet.Ships.Count;
				_isTransports = false;
			}
			else //Transports
			{
				_maxVisible = _selectedFleet.TransportShips.Count;
				_isTransports = true;
			}
			_windowHeight = 65 + (_maxVisible * 55);
			_backGroundImage.Resize(300, _windowHeight);

			bool isOwned = _selectedFleet.Empire == _gameMain.EmpireManager.CurrentEmpire;
			foreach (var slider in _shipSliders)
			{
				slider.SetEnabledState(isOwned);
			}
			RefreshShips();
		}

		private void RefreshShips()
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				if (_selectedFleet.OrderedShips.Count > 0)
				{
					Ship ship = _selectedFleet.OrderedShips[i];
					_shipSliders[i].SetAmountOfItems(_selectedFleet.Ships[ship] + 1);
					int amount = _selectedFleetGroup.FleetToSplit.Ships[ship];
					_shipLabels[i].SetText(amount + " x " + ship.Name);
					_shipSliders[i].TopIndex = amount;
				}
				else
				{
					_shipSliders[i].SetAmountOfItems(_selectedFleet.TransportShips[i].amount);
					int amount = _selectedFleetGroup.FleetToSplit.TransportShips[i].amount;
					_shipLabels[i].SetText(amount + " x " + _selectedFleetGroup.FleetToSplit.TransportShips[i].raceOnShip.RaceName);
					_shipSliders[i].TopIndex = amount;
				}
			}
		}

		private void RefreshFleet()
		{
			_empireNameLabel.SetText(string.Format("{0} Fleet", _selectedFleet.Empire.EmpireRace.RaceName));
			_empireNameLabel.MoveTo(_xPos + 150 - (int)(_empireNameLabel.GetWidth() / 2), _yPos + 30 - (int)(_empireNameLabel.GetHeight() / 2));
		}
	}
}
