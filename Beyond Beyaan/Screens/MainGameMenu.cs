using System.Collections.Generic;
using System.IO;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class MainGameMenu : ScreenInterface
	{
		private BBSprite _background;
		private BBSprite _planet;
		private BBSprite _title;
		private GameMain _gameMain;
		private BBButton[] _buttons;
		private BBLabel _versionLabel;
		private int _x;
		private int _y;

		#region Loading Game UI
		private List<FileInfo> _files;
		private BBStretchableImage _loadBackground;
		private BBInvisibleStretchButton[] _saveGameButtons;
		private BBScrollBar _scrollBar;
		private bool _showingLoadMenu;
		private int _maxVisible;
		#endregion

		public bool Initialize(GameMain gameMain, out string reason)
		{
			this._gameMain = gameMain;

			_buttons = new BBButton[4];

			_x = (gameMain.ScreenWidth / 2) - 130;
			_y = (gameMain.ScreenHeight / 2);

			for (int i = 0; i < _buttons.Length; i++)
			{
				_buttons[i] = new BBButton();
			}

			if (!_buttons[0].Initialize("MainButtonBG", "MainButtonFG", "Continue", "LargeComputerFont", ButtonTextAlignment.CENTER, _x, _y, 260, 40, gameMain.Random, out reason, 20, -1))
			{
				return false;
			}
			if (!_buttons[1].Initialize("MainButtonBG", "MainButtonFG", "New Game", "LargeComputerFont", ButtonTextAlignment.CENTER, _x, _y + 50, 260, 40, gameMain.Random, out reason, 20, -1))
			{
				return false;
			}
			if (!_buttons[2].Initialize("MainButtonBG", "MainButtonFG", "Load Game", "LargeComputerFont", ButtonTextAlignment.CENTER, _x, _y + 100, 260, 40, gameMain.Random, out reason, 20, -1))
			{
				return false;
			}
			if (!_buttons[3].Initialize("MainButtonBG", "MainButtonFG", "Exit", "LargeComputerFont", ButtonTextAlignment.CENTER, _x, _y + 150, 260, 40, gameMain.Random, out reason, 20, -1))
			{
				return false;
			}
			for (int i = 0; i < _buttons.Length; i++)
			{
				_buttons[i].SetTextColor(System.Drawing.Color.Gold, System.Drawing.Color.Black);
			}

			_versionLabel = new BBLabel();
			if (!_versionLabel.Initialize(10, _gameMain.ScreenHeight - 30, "Version 0.59", System.Drawing.Color.White, out reason))
			{
				return false;
			}

			_background = SpriteManager.GetSprite("MainBackground", gameMain.Random);
			_planet = SpriteManager.GetSprite("MainPlanetBackground", gameMain.Random);
			_title = SpriteManager.GetSprite("Title", gameMain.Random);

			_x = (gameMain.ScreenWidth / 2) - 512;
			_y = (gameMain.ScreenHeight / 2) - 300;

			_files = Utility.GetSaveGames(gameMain.GameDataSet.FullName);
			if (_files.Count == 0)
			{
				_buttons[0].Active = false; //Disabled Continue and Load buttons since there's no games to load
				_buttons[2].Active = false;
			}

			_loadBackground = new BBStretchableImage();
			if (!_loadBackground.Initialize((gameMain.ScreenWidth / 2) - 225, (gameMain.ScreenHeight / 2) - 175, 450, 350, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			_saveGameButtons = new BBInvisibleStretchButton[10];
			for (int i = 0; i < _saveGameButtons.Length; i++)
			{
				_saveGameButtons[i] = new BBInvisibleStretchButton();
				if (!_saveGameButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, (gameMain.ScreenWidth / 2) - 220, (gameMain.ScreenHeight / 2) - 160 + (i * 32), 420, 32, gameMain.Random, out reason))
				{
					return false;
				}
			}
			_scrollBar = new BBScrollBar();
			if (!_scrollBar.Initialize((gameMain.ScreenWidth / 2) + 200, (gameMain.ScreenHeight / 2) - 160, 320, 10, 10, false, false, gameMain.Random, out reason))
			{
				return false;
			}

			_maxVisible = _files.Count > _saveGameButtons.Length ? _saveGameButtons.Length : _files.Count;
			if (_maxVisible < _saveGameButtons.Length)
			{
				//Disable the scrollbar
				_scrollBar.SetEnabledState(false);
			}
			else
			{
				_scrollBar.SetEnabledState(true);
				_scrollBar.SetAmountOfItems(_files.Count);
			}

			RefreshSaves();

			_showingLoadMenu = false;

			reason = null;
			return true;
		}

		public void DrawScreen()
		{
			_background.Draw(0, 0, _gameMain.ScreenWidth / _background.Width, _gameMain.ScreenHeight / _background.Height);
			_planet.Draw(_x, _y);
			_title.Draw(_x, _y);
			foreach (BBButton button in _buttons)
			{
				button.Draw();
			}
			if (_showingLoadMenu)
			{
				_loadBackground.Draw();
				foreach (var button in _saveGameButtons)
				{
					button.Draw();
				}
				_scrollBar.Draw();
			}
			_versionLabel.Draw();
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			if (_showingLoadMenu)
			{
				for (int i = 0; i < _maxVisible; i++)
				{
					_saveGameButtons[i].MouseHover(x, y, frameDeltaTime);
				}
				if (_scrollBar.MouseHover(x, y, frameDeltaTime))
				{
					RefreshSaves();
				}
				return;
			}
			foreach (BBButton button in _buttons)
			{
				button.MouseHover(x, y, frameDeltaTime);
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (_showingLoadMenu)
			{
				for (int i = 0; i < _maxVisible; i++)
				{
					_saveGameButtons[i].MouseDown(x, y);
				}
				_scrollBar.MouseDown(x, y);
				return;
			}
			foreach (BBButton button in _buttons)
			{
				button.MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (_showingLoadMenu)
			{
				for (int i = 0; i < _maxVisible; i++)
				{
					if (_saveGameButtons[i].MouseUp(x, y))
					{
						_gameMain.LoadGame(_files[i + _scrollBar.TopIndex].Name);
						_gameMain.ChangeToScreen(Screen.Galaxy);
						_showingLoadMenu = false;
						return;
					}
				}
				if (_scrollBar.MouseUp(x, y))
				{
					RefreshSaves();
					return;
				}
				//Since the player didn't click on one of the buttons, it's considered as cancel.
				_showingLoadMenu = false;
				return;
			}
			for (int i = 0; i < _buttons.Length; i++)
			{
				if (_buttons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 0: //Continue button
							FileInfo mostRecentGame = null;
							foreach (var file in _files)
							{
								if (mostRecentGame == null)
								{
									mostRecentGame = file;
								}
								else if (mostRecentGame.LastWriteTime.CompareTo(file.LastWriteTime) < 0)
								{
									mostRecentGame = file;
								}
							}
							if (mostRecentGame != null)
							{
								_gameMain.LoadGame(mostRecentGame.Name);
								_gameMain.ChangeToScreen(Screen.Galaxy);
							}
							break;
						case 1: //New Game button
							_gameMain.ChangeToScreen(Screen.NewGame);
							break;
						case 2:
							_showingLoadMenu = true;
							break;
						case 3:
							_gameMain.ExitGame();
							break;
					}
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				_gameMain.ExitGame();
			}
		}

		private void RefreshSaves()
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				var file = _files[i + _scrollBar.TopIndex];
				_saveGameButtons[i].SetText(file.Name.Substring(0, file.Name.Length - file.Extension.Length) + " - (" + file.LastWriteTime.ToString() + ")");
			}
		}
	}
}
