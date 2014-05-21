using System;
using System.Collections.Generic;
using System.Drawing;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class ColonizeScreen : WindowInterface
	{
		private Fleet _colonizingFleet;
		private List<Ship> _colonyShips; 
		private StarSystem _starSystem;

		//For now, only 1 planet per system
		private BBStretchButton[] _shipButtons;
		private BBLabel _systemNameLabel;
		private BBLabel _instructionLabel;

		private BBButton _colonizeButton;
		private BBButton _cancelButton;
		public Action Completed;

		private int _maxShips;

		private bool _colonizing;
		private bool _showingText;
		private BBStretchableImage _groundViewBackground;
		private float _landingShipPos;
		private BBStretchableImage _nameBackground;
		private BBSingleLineTextBox _nameTextBox;
		private BBSprite _groundView;
		private BBSprite _landingShip;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!this.Initialize(gameMain.ScreenWidth / 2 - 200, gameMain.ScreenHeight / 2 - 300, 400, 250, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_shipButtons = new BBStretchButton[4];
			for (int i = 0; i < _shipButtons.Length; i++)
			{
				_shipButtons[i] = new BBStretchButton();
				if (!_shipButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 20, _yPos + 60, 200, 40, gameMain.Random, out reason))
				{
					return false;
				}
			}
			_instructionLabel = new BBLabel();
			_systemNameLabel = new BBLabel();
			_cancelButton = new BBButton();
			_colonizeButton = new BBButton();
			_groundViewBackground = new BBStretchableImage();
			_nameBackground = new BBStretchableImage();
			_nameTextBox = new BBSingleLineTextBox();

			if (!_instructionLabel.Initialize(_xPos + 20, _yPos + 25, "Select a ship to colonize this planet", Color.White, out reason))
			{
				return false;
			}
			if (!_systemNameLabel.Initialize(_xPos + 300, _yPos + 130, string.Empty, Color.White, out reason))
			{
				return false;
			}
			if (!_cancelButton.Initialize("CancelColonizeBG", "CancelColonizeFG", string.Empty, ButtonTextAlignment.LEFT, _xPos + 230, _yPos + 195, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_colonizeButton.Initialize("TransferToBG", "TransferToFG", string.Empty, ButtonTextAlignment.LEFT, _xPos + 310, _yPos + 195, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_nameBackground.Initialize(gameMain.ScreenWidth / 2 - 100, gameMain.ScreenHeight / 2 - 40, 200, 80, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_nameTextBox.Initialize(string.Empty, gameMain.ScreenWidth / 2 - 80, gameMain.ScreenHeight / 2 - 20, 160, 40, false, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_groundViewBackground.Initialize(gameMain.ScreenWidth / 2 - 440, gameMain.ScreenHeight / 2 - 340, 880, 680, StretchableImageType.ThickBorder, gameMain.Random, out reason))
			{
				return false;
			}
			_colonizing = false;

			return true;
		}

		public void LoadFleetAndSystem(Fleet fleet)
		{
			_colonizingFleet = fleet;
			_starSystem = fleet.AdjacentSystem;
			_colonyShips = new List<Ship>();
			foreach (var ship in _colonizingFleet.OrderedShips)
			{
				foreach (var special in ship.Specials)
				{
					if (special == null)
					{
						continue;
					}
					if (special.Technology.Colony >= _starSystem.Planets[0].ColonyRequirement)
					{
						_colonyShips.Add(ship);
						break;
					}
				}
			}
			//TODO: Add scrollbar to support more than 4 different colony ship designs
			_maxShips = _colonyShips.Count > 4 ? 4 : _colonyShips.Count;
			for (int i = 0; i < _maxShips; i++)
			{
				_shipButtons[i].SetText(_colonyShips[i].Name + (_colonizingFleet.Ships[_colonyShips[i]] > 1 ? " (" + _colonizingFleet.Ships[_colonyShips[i]] + ")" : string.Empty));
			}
			_shipButtons[0].Selected = true;
			_systemNameLabel.SetText(_starSystem.Name);
			_systemNameLabel.MoveTo(_xPos + 300 - (int)(_systemNameLabel.GetWidth() / 2), _yPos + 130 - (int)(_systemNameLabel.GetHeight() / 2));
		}

		public override void Draw()
		{
			if (!_colonizing)
			{
				base.Draw();
				_instructionLabel.Draw();
				for (int i = 0; i < _maxShips; i++)
				{
					_shipButtons[i].Draw();
				}
				_systemNameLabel.Draw();
				_starSystem.Planets[0].SmallSprite.Draw(_xPos + 285, _yPos + 80);
				_cancelButton.Draw();
				_colonizeButton.Draw();
			}
			else
			{
				_groundViewBackground.Draw();
				_groundView.Draw(_gameMain.ScreenWidth / 2 - 400, _gameMain.ScreenHeight / 2 - 300);
				_landingShip.Draw(_gameMain.ScreenWidth / 2 + 50, _landingShipPos);
				if (_showingText)
				{
					_nameBackground.Draw();
					_nameTextBox.Draw();
				}
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (!_colonizing)
			{
				for (int i = 0; i < _maxShips; i++)
				{
					_shipButtons[i].MouseHover(x, y, frameDeltaTime);
				}
				_cancelButton.MouseHover(x, y, frameDeltaTime);
				_colonizeButton.MouseHover(x, y, frameDeltaTime);
			}
			else
			{
				if (!_showingText)
				{
					_landingShipPos += (frameDeltaTime * 100);
					if (_landingShipPos >= _gameMain.ScreenHeight / 2 + 50)
					{
						_showingText = true;
						_landingShipPos = _gameMain.ScreenHeight / 2 + 50;
					}
				}
				else
				{
					_nameTextBox.Update(frameDeltaTime);
				}
			}
			return false;
		}

		public override bool MouseDown(int x, int y)
		{
			if (!_colonizing)
			{
				for (int i = 0; i < _maxShips; i++)
				{
					_shipButtons[i].MouseDown(x, y);
				}
				_cancelButton.MouseDown(x, y);
				_colonizeButton.MouseDown(x, y);
			}
			else if (_showingText)
			{
				_nameTextBox.MouseDown(x, y);
			}
			return false;
		}

		public override bool MouseUp(int x, int y)
		{
			if (!_colonizing)
			{
				for (int i = 0; i < _maxShips; i++)
				{
					if (_shipButtons[i].MouseUp(x, y))
					{
						foreach (var button in _shipButtons)
						{
							button.Selected = false;
						}
						_shipButtons[i].Selected = true;
					}
				}
				if (_cancelButton.MouseUp(x, y))
				{
					if (Completed != null)
					{
						Completed();
					}
				}
				if (_colonizeButton.MouseDown(x, y))
				{
					int whichShip = 0;
					for (int i = 0; i < _maxShips; i++)
					{
						if (_shipButtons[i].Selected)
						{
							whichShip = i;
							break;
						}
					}
					var ship = _colonyShips[whichShip];
					_colonizingFleet.ColonizePlanet(ship);
					_nameTextBox.SetText(_starSystem.Name);
					//Select the textbox so it'd capture the keypresses
					_nameTextBox.MouseDown(_gameMain.ScreenWidth / 2, _gameMain.ScreenHeight / 2);
					_nameTextBox.MouseUp(_gameMain.ScreenWidth / 2, _gameMain.ScreenHeight / 2);
					_landingShipPos = _gameMain.ScreenHeight / 2 - 300;
					_groundView = _starSystem.Planets[0].GroundSprite;
					_landingShip = _colonizingFleet.Empire.EmpireRace.LandingShip;
					_showingText = false;
					_colonizing = true;
				}
			}
			else
			{
				if (!_showingText)
				{
					_showingText = true;
					_landingShipPos = _gameMain.ScreenHeight / 2 + 50;
				}
				else
				{
					if (!_nameTextBox.MouseUp(x, y) && !string.IsNullOrEmpty(_nameTextBox.Text))
					{
						_starSystem.Name = _nameTextBox.Text;
						_colonizing = false;
						_showingText = false;
						//Done
						if (Completed != null)
						{
							Completed();
						}
					}
				}
			}
			return false;
		}

		public override bool KeyDown(GorgonLibrary.InputDevices.KeyboardInputEventArgs e)
		{
			if (_showingText)
			{
				if ((e.Key == GorgonLibrary.InputDevices.KeyboardKeys.Enter || e.Key == GorgonLibrary.InputDevices.KeyboardKeys.Return) && !string.IsNullOrEmpty(_nameTextBox.Text))
				{
					_starSystem.Name = _nameTextBox.Text;
					_colonizing = false;
					_showingText = false;
					//Done
					if (Completed != null)
					{
						Completed();
					}
				}
				_nameTextBox.KeyDown(e);
			}
			else if (e.Key == GorgonLibrary.InputDevices.KeyboardKeys.Enter || e.Key == GorgonLibrary.InputDevices.KeyboardKeys.Return)
			{
				_showingText = true;
				_landingShipPos = _gameMain.ScreenHeight / 2 + 50;
			}
			return true;
		}
	}
}
