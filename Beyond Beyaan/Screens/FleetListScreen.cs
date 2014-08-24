using System;
using System.Drawing;

namespace Beyond_Beyaan.Screens
{
	public class FleetListScreen : WindowInterface
	{
		public Action CloseWindow;
		public Action<Fleet> SelectFleet;

		private BBStretchButton[] _shipNames; //This includes both background image and label, as well as centering functionality
		private BBLabel[] _statusLabels;
		private BBLabel[] _planetNames;
		private BBStretchButton[] _planetBackgrounds;
		private BBScrollBar _scrollBar;

		private BBStretchButton[][] _shipAmountLabels;

		private BBStretchableImage _maintenanceCostBackground;
		private BBLabel _maintenanceLabel;
		private BBLabel _maintenanceAmountLabel;
		private BBButton[] _scrapButtons;
		private BBStretchButton _viewSpecsButton;

		private int _maxVisible;
		private int _x;
		private int _y;
		private FleetManager _fleetManager;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;

			_x = (gameMain.ScreenWidth / 2) - 500;
			_y = (gameMain.ScreenHeight / 2) - 305;
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

			_planetBackgrounds = new BBStretchButton[10];
			_planetNames = new BBLabel[10];
			_statusLabels = new BBLabel[11];
			_scrollBar = new BBScrollBar();

			_statusLabels[0] = new BBLabel();
			if (!_statusLabels[0].Initialize(_x, _y + 10, "Status", Color.White, out reason))
			{
				return false;
			}

			for (int i = 0; i < 10; i++)
			{
				_planetBackgrounds[i] = new BBStretchButton();
				if (!_planetBackgrounds[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _x, _y + 40 + (50 * i), 980, 50, _gameMain.Random, out reason))
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

			_maintenanceCostBackground = new BBStretchableImage();
			_maintenanceLabel = new BBLabel();
			_maintenanceAmountLabel = new BBLabel();
			_scrapButtons = new BBButton[6];
			for (int i = 0; i < _scrapButtons.Length; i++)
			{
				_scrapButtons[i] = new BBButton();
				if (!_scrapButtons[i].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, _x + 112 + (150 * i), _y + 540, 75, 35, gameMain.Random, out reason))
				{
					return false;
				}
			}

			if (!_maintenanceCostBackground.Initialize(_x + 220, _y + 577, 280, 35, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_maintenanceLabel.Initialize(_x + 225, _y + 585, "Maintenance Cost:", Color.Orange, out reason))
			{
				return false;
			}
			if (!_maintenanceAmountLabel.Initialize(_x + 495, _y + 585, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_maintenanceAmountLabel.SetAlignment(true);

			_viewSpecsButton = new BBStretchButton();
			if (!_viewSpecsButton.Initialize("View Ship Specifications", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _x + 500, _y + 577, 280, 35, gameMain.Random, out reason))
			{
				return false;
			}

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
				_planetBackgrounds[i].Draw();
			}
			_scrollBar.Draw();
			for (i = 0; i < 6; i++)
			{
				_scrapButtons[i].Draw();
			}
			_maintenanceCostBackground.Draw();
			_maintenanceLabel.Draw();
			_maintenanceAmountLabel.Draw();
			_viewSpecsButton.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				_planetBackgrounds[i].MouseHover(x, y, frameDeltaTime);
			}
			return false;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = false;

			for (int i = 0; i < _maxVisible; i++)
			{
				result = _planetBackgrounds[i].MouseDown(x, y) || result;
			}

			result = base.MouseDown(x, y) || result;

			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				if (_planetBackgrounds[i].MouseUp(x, y))
				{
					SelectFleet(_fleetManager.GetFleets()[i + _scrollBar.TopIndex]);
					return true;
				}
			}

			if (!base.MouseUp(x,y))
			{
				if (CloseWindow != null)
				{
					CloseWindow();
				}
				return true;
			}
			return false;
		}

		public void LoadScreen()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			_fleetManager = currentEmpire.FleetManager;
			var fleets = _fleetManager.GetFleets();

			int i;
			for (i = 0; i < _fleetManager.CurrentDesigns.Count; i++)
			{
				_shipNames[i].SetText(_fleetManager.CurrentDesigns[i].Name);
				_shipNames[i].Enabled = true;
				_scrapButtons[i].Active = true;
			}
			for (; i < 6; i++)
			{
				_shipNames[i].SetText(string.Empty);
				_shipNames[i].Enabled = false;
				_scrapButtons[i].Active = false;
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

			_maintenanceAmountLabel.SetText(string.Format("{0:0.0} BC", currentEmpire.ShipMaintenance));

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
				_planetBackgrounds[i].Enabled = true;
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
				_planetBackgrounds[i].Enabled = false;
			}
		}
	}
}
