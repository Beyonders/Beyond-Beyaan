using System;

namespace Beyond_Beyaan
{
	public class TaskBar
	{
		private GameMain _gameMain;
		private BBStretchButton[] _taskButtons;

		public Action ShowGameMenu;
		public Action ShowResearchScreen;
		public Action ShowShipDesignScreen;
		public Action ShowFleetOverviewScreen;
		public Action ShowPlanetsScreen;
		public Action EndTurn;

		private int _top;
		private bool _hide;

		public bool Hide
		{
			get { return _hide; }
			set { _hide = value; }
		}

		#region Constructors
		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;
			_taskButtons = new BBStretchButton[7];

			int width = gameMain.ScreenWidth / 7;
			int offset = gameMain.ScreenWidth - (width * 7); //account for integer rounding
			_top = gameMain.ScreenHeight - 50;
			int x = 0;
			for (int i = 0; i < _taskButtons.Length; i++)
			{
				_taskButtons[i] = new BBStretchButton();
			}
			if (!_taskButtons[0].Initialize("Game Menu", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, _top, width, 50, gameMain.Random, out reason))
			{
				return false;
			}
			x += width;
			if (!_taskButtons[1].Initialize("Design Ships", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, _top, width, 50, gameMain.Random, out reason))
			{
				return false;
			}
			x += width;
			if (!_taskButtons[2].Initialize("Fleets Overview", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, _top, width, 50, gameMain.Random, out reason))
			{
				return false;
			}
			x += width;
			if (!_taskButtons[3].Initialize("Diplomacy", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, _top, width, 50, gameMain.Random, out reason))
			{
				return false;
			}
			x += width;
			if (!_taskButtons[4].Initialize("Planets Overview", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, _top, width, 50, gameMain.Random, out reason))
			{
				return false;
			}
			x += width;
			if (!_taskButtons[5].Initialize("Manage Research", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, _top, width, 50, gameMain.Random, out reason))
			{
				return false;
			}
			x += width;
			if (!_taskButtons[6].Initialize("End Turn", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, _top, width + offset, 50, gameMain.Random, out reason))
			{
				return false;
			}

			_hide = false;
			reason = null;
			return true;
		}
		#endregion

		public void Draw()
		{
			if (_hide)
			{
				return;
			}
			foreach (var button in _taskButtons)
			{
				button.Draw();
			}
		}

		public bool MouseHover(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (_hide)
			{
				return false;
			}
			foreach (var button in _taskButtons)
			{
				button.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			if (mouseY > _top)
			{
				return true;
			}
			return false;
		}

		public bool MouseDown(int mouseX, int mouseY, int whichButton)
		{
			if (_hide)
			{
				return false;
			}
			if (whichButton != 1)
			{
				return false;
			}
			foreach (var button in _taskButtons)
			{
				if (button.MouseDown(mouseX, mouseY))
				{
					return true;
				}
			}
			return false;
		}

		public bool MouseUp(int mouseX, int mouseY, int whichButton)
		{
			if (_hide)
			{
				return false;
			}
			if (whichButton != 1)
			{
				return false;
			}
			for (int i = 0; i < _taskButtons.Length; i++)
			{
				if (_taskButtons[i].MouseUp(mouseX, mouseY))
				{
					switch (i)
					{
						case 0:
						{
							if (ShowGameMenu != null)
							{
								ShowGameMenu();
								SetToScreen(Screen.InGameMenu);
							}
							break;
						}
						/*case 1: _gameMain.ChangeToScreen(Screen.Galaxy);
							break;
						case 2: _gameMain.ChangeToScreen(Screen.Diplomacy);
							break;
						case 3: _gameMain.ChangeToScreen(Screen.FleetList);
							break;*/
						case 1:
						{
							if (ShowShipDesignScreen != null)
							{
								ShowShipDesignScreen();
								SetToScreen(Screen.Design);
							}
							break;
						}
						case 2:
						{
							if (ShowFleetOverviewScreen != null)
							{
								ShowFleetOverviewScreen();
								SetToScreen(Screen.FleetList);
							}
						}
							break;
						case 4: 
							if (ShowPlanetsScreen != null)
							{
								ShowPlanetsScreen();
								SetToScreen(Screen.Planets);
							}
							break;
						case 5:
						{
							if (ShowResearchScreen != null)
							{
								ShowResearchScreen();
								SetToScreen(Screen.Research);
							}
							break;
						}
						case 6:
						{
							Clear();
							if (EndTurn != null)
							{
								EndTurn();
							}
							_gameMain.ChangeToScreen(Screen.ProcessTurn);
							_gameMain.HideSitRep();
							break;
						}
					}
					return true;
				}
			}
			return false;
		}

		public void SetToScreen(Screen whichScreen)
		{
			Clear();
			switch (whichScreen)
			{
				case Screen.InGameMenu:
					_taskButtons[0].Selected = true;
					break;
				case Screen.Design:
					_taskButtons[1].Selected = true;
					break;
				case Screen.FleetList:
					_taskButtons[2].Selected = true;
					break;
				case Screen.Diplomacy:
					_taskButtons[3].Selected = true;
					break;
				case Screen.Planets:
					_taskButtons[4].Selected = true;
					break;
				case Screen.Research:
					_taskButtons[5].Selected = true;
					break;
				case Screen.ProcessTurn:
					_taskButtons[6].Selected = true;
					break;
			}
		}

		public void Clear()
		{
			foreach (var button in _taskButtons)
			{
				button.Selected = false;
			}
		}
	}
}
