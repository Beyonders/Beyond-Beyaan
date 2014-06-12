using System;
using System.Drawing;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class FleetListScreen : WindowInterface
	{
		public Action CloseWindow;

		private BBStretchButton[] _shipNames; //This includes both background image and label, as well as centering functionality
		private BBLabel[] _statusLabels;
		private BBLabel[] _planetNames;
		private BBStretchableImage[] _planetBackgrounds;
		private BBScrollBar _scrollBar;

		private BBStretchButton[][] _shipAmountLabels;

		private int _maxVisible;
		private int _x;
		private int _y;
		private FleetManager _fleetManager;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;

			_x = (gameMain.ScreenWidth / 2) - 500;
			_y = (gameMain.ScreenHeight / 2) - 300;
			if (!Initialize((gameMain.ScreenWidth / 2) - 520, (gameMain.ScreenHeight / 2) - 320, 1040, 640, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_shipNames = new BBStretchButton[6];
			for (int i = 0; i < _shipNames.Length; i++)
			{
				_shipNames[i] = new BBStretchButton();
				if (!_shipNames[i].Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _x + 80 + (150 * i), _y, 150, 40, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			_planetBackgrounds = new BBStretchableImage[10];
			_planetNames = new BBLabel[10];
			_statusLabels = new BBLabel[11];
			_scrollBar = new BBScrollBar();

			_statusLabels[0] = new BBLabel();
			if (!_statusLabels[0].Initialize(_x, _y + 5, "Status", Color.White, out reason))
			{
				return false;
			}

			for (int i = 0; i < 10; i++)
			{
				_planetBackgrounds[i] = new BBStretchableImage();
				if (!_planetBackgrounds[i].Initialize(_x, _y + 40 + (50 * i), 980, 50, StretchableImageType.ThinBorderBG, _gameMain.Random, out reason))
				{
					return false;
				}
				_planetNames[i] = new BBLabel();
				if (!_planetNames[i].Initialize(_x + 5, _y + 65 + (50 * i), string.Empty, Color.GreenYellow, out reason))
				{
					return false;
				}
				_statusLabels[i + 1] = new BBLabel();
				if (!_statusLabels[i + 1].Initialize(_x + 5, _y + 45 + (50 * i), string.Empty, Color.Orange, out reason))
				{
					return false;
				}
			}

			_shipAmountLabels = new BBStretchButton[10][];
			for (int i = 0; i < _shipAmountLabels.Length; i++)
			{
				_shipAmountLabels[i] = new BBStretchButton[6];
				for (int j = 0; j < 6; j++)
				{
					_shipAmountLabels[i][j] = new BBStretchButton();
					if (!_shipAmountLabels[i][j].Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, _x + 80 + (150 * j), _y + 43 + (i * 50), 150, 25, _gameMain.Random, out reason))
					{
						return false;
					}
				}
			}

			if (!_scrollBar.Initialize(_x + 980, _y + 40, 500, 10, 10, false, false, _gameMain.Random, out reason))
			{
				return false;
			}

			//Set up for planet sprites
			_x += 5;
			_y += 45;

			reason = null;
			return true;
		}

		public override void Draw()
		{
			base.Draw();

			int i;

			for (i = 0; i < 6; i++)
			{
				_shipNames[i].Draw();
			}
			_statusLabels[0].Draw();
			
			for (i = 0; i < _maxVisible; i++)
			{
				_planetBackgrounds[i].Draw();
				_planetNames[i].Draw();
				_statusLabels[i + 1].Draw();
				for (int j = 0; j < 6; j++)
				{
					if (_shipAmountLabels[i][j].Enabled)
					{
						_shipAmountLabels[i][j].Draw();
					}
				}
			}
			for (; i < 10; i++)
			{
				_planetBackgrounds[i].Draw(Color.Tan, 255);
			}
			_scrollBar.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			hoveringFleet = null;
			for (int i = 0; i < maxVisible; i++)
			{
				if (fleetButtons[i].MouseHover(x, y, frameDeltaTime))
				{
					hoveringFleet = whichFleets[i + fleetIndex];
				}
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					shipButtons[i].MouseHover(x, y, frameDeltaTime);
				}
			}
			scrapFleet.MouseHover(x, y, frameDeltaTime);
			scrapShip.MouseHover(x, y, frameDeltaTime);
			showOtherFleets.MouseHover(x, y, frameDeltaTime);
			showOurFleets.MouseHover(x, y, frameDeltaTime);*/
			return false;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = base.MouseDown(x, y);
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i].MouseDown(x, y);
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					shipButtons[i].MouseDown(x, y);
				}
			}
			scrapFleet.MouseDown(x, y);
			scrapShip.MouseDown(x, y);
			showOtherFleets.MouseDown(x, y);
			showOurFleets.MouseDown(x, y);*/
			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = base.MouseUp(x, y);
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				if (fleetButtons[i].MouseUp(x, y))
				{
					foreach (Button button in fleetButtons)
					{
						button.Selected = false;
					}
					selectedFleet = i + fleetIndex;
					fleetSelected = whichFleets[i + fleetIndex];
					selectedShip = -1;
					UpdateLabels();
					fleetButtons[i].Selected = true;
					return;
				}
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					if (shipButtons[i].MouseUp(x, y))
					{
						foreach (Button button in shipButtons)
						{
							button.Selected = false;
						}
						selectedShip = i + shipIndex;
						UpdateShipSpecs();
						shipButtons[i].Selected = true;
						return;
					}
				}
			}
			if (scrapFleet.MouseUp(x, y))
			{
			}
			if (scrapShip.MouseUp(x, y))
			{
			}
			if (showOtherFleets.MouseUp(x, y))
			{
				showOtherFleets.Selected = !showOtherFleets.Selected;
				UpdateList();
				UpdateLabels();
			}
			if (showOurFleets.MouseUp(x, y))
			{
				showOurFleets.Selected = !showOurFleets.Selected;
				UpdateList();
				UpdateLabels();
			}*/
			if (!result)
			{
				if (CloseWindow != null)
				{
					CloseWindow();
				}
			}
			return result;
		}

		public void LoadScreen()
		{
			_fleetManager = _gameMain.EmpireManager.CurrentEmpire.FleetManager;
			var fleets = _fleetManager.GetFleets();

			int i;
			for (i = 0; i < _fleetManager.CurrentDesigns.Count; i++)
			{
				_shipNames[i].SetText(_fleetManager.CurrentDesigns[i].Name);
				_shipNames[i].Enabled = true;
			}
			for (; i < 6; i++)
			{
				_shipNames[i].SetText(string.Empty);
				_shipNames[i].Enabled = false;
			}

			_scrollBar.TopIndex = 0;
			if (fleets.Count > 10)
			{
				_maxVisible = 10;
				_scrollBar.SetEnabledState(true);
				_scrollBar.SetAmountOfItems(fleets.Count);
			}
			else
			{
				_maxVisible = fleets.Count;
				_scrollBar.SetEnabledState(false);
				_scrollBar.SetAmountOfItems(10);
			}

			Refresh();
		}

		private void Refresh()
		{
			var fleets = _fleetManager.GetFleets();
			int i;
			for (i = 0; i < _maxVisible; i++)
			{
				var fleet = fleets[i + _scrollBar.TopIndex];
				if (fleet.TravelNodes != null && fleet.TravelNodes.Count > 0)
				{
					//It's going somewhere
					if (fleet.TravelNodes[0].StarSystem.IsThisSystemExploredByEmpire(fleet.Empire))
					{
						_planetNames[i].SetText(fleet.TravelNodes[0].StarSystem.Name);
					}
					else
					{
						_planetNames[i].SetText("Unexplored System");
					}
					_statusLabels[i + 1].SetText("Enroute");
					_statusLabels[i + 1].SetColor(Color.Yellow, Color.Empty);
				}
				else
				{
					_planetNames[i].SetText(fleet.AdjacentSystem.Name);
					_statusLabels[i + 1].SetText("Orbiting");
					_statusLabels[i + 1].SetColor(Color.Orange, Color.Empty);
				}
				for (int j = 0; j < 6; j++)
				{
					_shipAmountLabels[i][j].Enabled = false;
					_shipAmountLabels[i][j].SetText(string.Empty);
				}
				foreach (var ship in fleet.Ships)
				{
					int index = _fleetManager.CurrentDesigns.IndexOf(ship.Key);
					_shipAmountLabels[i][index].Enabled = true;
					_shipAmountLabels[i][index].SetText(ship.Value.ToString());
				}
			}
			for (; i < 10; i++)
			{
				//Disable the remaining slots
				_planetNames[i].SetText(string.Empty);
				_statusLabels[i + 1].SetText(string.Empty);
				for (int j = 0; j < 6; j++)
				{
					_shipAmountLabels[i][j].Enabled = false;
					_shipAmountLabels[i][j].SetText(string.Empty);
				}
			}
		}

		/*public void UpdateLabels()
		{
			int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i].SetText(whichFleets[i + fleetIndex].Empire.EmpireName);
			}
			if (selectedFleet >= 0)
			{
				foreach (Button button in fleetButtons)
				{
					button.Selected = false;
				}
				fleetButtons[selectedFleet - fleetIndex].Selected = true;
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				int i = 0;
				int j = 0;
				foreach (KeyValuePair<Ship, int> ship in whichFleets[selectedFleet + fleetIndex].Ships)
				{
					if (i >= shipIndex)
					{
						if (i >= (shipIndex + maxVisible))
						{
							break;
						}
						shipButtons[j].SetText(ship.Key.Name + " x " + ship.Value);
						j++;
					}
					i++;
				}
			}
		}

		public void UpdateList()
		{
			fleetSelected = null;
			selectedFleet = -1;
			selectedShip = -1;
			fleetIndex = 0;
			shipIndex = 0;
			foreach (Button button in shipButtons)
			{
				button.Selected = false;
			}
			foreach (Button button in fleetButtons)
			{
				button.Selected = false;
			}

			if (showOurFleets.Selected && showOtherFleets.Selected)
			{
				whichFleets = allFleets;
			}
			else if (!showOtherFleets.Selected && showOurFleets.Selected)
			{
				whichFleets = ownedFleets;
			}
			else if (showOtherFleets.Selected && !showOurFleets.Selected)
			{
				whichFleets = otherFleets;
			}
			else
			{
				whichFleets = new List<Fleet>();
			}
		}

		public void UpdateShipSpecs()
		{
			if (selectedFleet != -1)
			{
				int i = 0;
				foreach (KeyValuePair<Ship, int> ship in whichFleets[selectedFleet - fleetIndex].Ships)
				{
					if (i == (selectedShip + shipIndex))
					{
						shipSelected = ship.Key;
						nameText.SetText(shipSelected.Name);
						sizeText.SetText(Utility.ShipSizeToString(shipSelected.Size));
						engineText.SetText(shipSelected.Engine.TechName);
						computerText.SetText(shipSelected.Computer.TechName);
						armorText.SetText(shipSelected.Armor.TechName);
						shieldText.SetText(shipSelected.Shield.TechName);
						weaponIndex = 0;
						UpdateWeaponSpecs();
						LoadShipSprite(whichFleets[selectedFleet - fleetIndex].Empire, ship.Key);
						break;
					}
				}
			}
		}

		public void UpdateWeaponSpecs()
		{
			int maxVisible = shipSelected.weapons.Count > weaponTexts.Length ? weaponTexts.Length : shipSelected.weapons.Count;

			for (int i = 0; i < maxVisible; i++)
			{
				weaponTexts[i].SetText(shipSelected.weapons[i + weaponIndex].GetName());
				mountsTexts[i].SetText(shipSelected.weapons[i + weaponIndex].Mounts.ToString());
			}
		}

		private void DrawGalaxyPreview(DrawingManagement drawingManagement)
		{
			List<StarSystem> systems = _gameMain.Galaxy.GetAllStars();

			foreach (StarSystem system in systems)
			{
				int x = (_gameMain.ScreenWidth / 2) + (int)(386.0f * (system.X / (float)_gameMain.Galaxy.GalaxySize));
				int y = ((_gameMain.ScreenHeight / 2) - 300) + (int)(386.0f * (system.Y / (float)_gameMain.Galaxy.GalaxySize));

				GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
				_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				system.Sprite.Draw(x, y, 0.4f, 0.4f);
				//drawingManagement.DrawSprite(SpriteName.Star, x, y, 255, 6 * system.Size, 6 * system.Size, System.Drawing.Color.White);
				GorgonLibrary.Gorgon.CurrentShader = null;
			}

			foreach (Fleet fleet in whichFleets)
			{
				int x = (_gameMain.ScreenWidth / 2) + (int)(386.0f * (fleet.GalaxyX / (float)_gameMain.Galaxy.GalaxySize));
				int y = ((_gameMain.ScreenHeight / 2) - 300) + (int)(386.0f * (fleet.GalaxyY / (float)_gameMain.Galaxy.GalaxySize));

				if (fleet == fleetSelected || fleet == hoveringFleet)
				{
					drawingManagement.DrawSprite(SpriteName.Fleet, x, y, 255, 32, 32, fleet.Empire.EmpireColor);
				}
				else
				{
					drawingManagement.DrawSprite(SpriteName.Fleet, x, y, 255, 16, 16, fleet.Empire.EmpireColor);
				}
			}
		}

		private void LoadShipSprite(Empire empire, Ship ship)
		{
			shipSprite = empire.EmpireRace.GetShip(ship.Size, ship.WhichStyle);
		}*/
	}
}
