using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class InGameMenu : WindowInterface
	{
		public Action CloseWindow;

		private BBStretchButton[] _buttons;
		private BBStretchableImage _saveGameListBackground;
		private BBInvisibleStretchButton[] _saveGameButtons;
		private BBScrollBar _scrollBar;
		private int _maxVisible;
		private int _selectedGame;

		private BBStretchableImage _saveGameNamePromptBackground;
		private BBLabel _saveGameNamePromptInstructionLabel;
		private BBSingleLineTextBox _saveGameNameField;
		private bool _promptShowing;

		private List<FileInfo> _files;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!base.Initialize((gameMain.ScreenWidth / 2) - 250, (gameMain.ScreenHeight / 2) - 200, 500, 400, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			int x = (gameMain.ScreenWidth / 2) - 150;
			int y = (gameMain.ScreenHeight / 2) - 50;
			_saveGameNamePromptBackground = new BBStretchableImage();
			if (!_saveGameNamePromptBackground.Initialize(x, y, 300, 100, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			_saveGameNamePromptInstructionLabel = new BBLabel();
			if (!_saveGameNamePromptInstructionLabel.Initialize(x + 20, y + 10, "Please input name for the save:", Color.White, out reason))
			{
				return false;
			}
			_saveGameNameField = new BBSingleLineTextBox();
			if (!_saveGameNameField.Initialize(string.Empty, x + 20, y + 40, 250, 40, false, gameMain.Random, out reason))
			{
				return false;
			}

			_buttons = new BBStretchButton[4];

			_buttons[0] = new BBStretchButton();
			if (!_buttons[0].Initialize("New Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 30, _yPos + 350, 200, 35, gameMain.Random, out reason))
			{
				return false;
			}
			_buttons[1] = new BBStretchButton();
			if (!_buttons[1].Initialize("Save Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 270, _yPos + 300, 200, 35, gameMain.Random, out reason))
			{
				return false;
			}
			_buttons[2] = new BBStretchButton();
			if (!_buttons[2].Initialize("Load Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 30, _yPos + 300, 200, 35, gameMain.Random, out reason))
			{
				return false;
			}
			_buttons[2].Enabled = false;

			_buttons[3] = new BBStretchButton();
			if (!_buttons[3].Initialize("Exit Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 270, _yPos + 350, 200, 35, gameMain.Random, out reason))
			{
				return false;
			}

			_saveGameListBackground = new BBStretchableImage();
			if (!_saveGameListBackground.Initialize(_xPos + 20, _yPos + 20, 460, 325, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}

			_saveGameButtons = new BBInvisibleStretchButton[8];
			for (int i = 0; i < _saveGameButtons.Length; i++)
			{
				_saveGameButtons[i] = new BBInvisibleStretchButton();
				if (!_saveGameButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, _xPos + 30, _yPos + 35 + (i * 32), 420, 32, gameMain.Random, out reason))
				{
					return false;
				}
			}

			_scrollBar = new BBScrollBar();
			if (!_scrollBar.Initialize(_xPos + 455, _yPos + 37, 256, _saveGameButtons.Length, _saveGameButtons.Length, false, false, gameMain.Random, out reason))
			{
				return false;
			}

			_maxVisible = 0;
			_scrollBar.SetEnabledState(false);
			_selectedGame = -1;
			_files = new List<FileInfo>();
			_promptShowing = false;

			return true;
		}

		public override void Draw()
		{
			base.Draw();

			_saveGameListBackground.Draw();

			for (int i = 0; i < _buttons.Length; i++)
			{
				_buttons[i].Draw();
			}

			for (int i = 0; i < _maxVisible; i++)
			{
				_saveGameButtons[i].Draw();
			}

			_scrollBar.Draw();

			if (_promptShowing)
			{
				_saveGameNamePromptBackground.Draw();
				_saveGameNamePromptInstructionLabel.Draw();
				_saveGameNameField.Draw();
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (_promptShowing)
			{
				_saveGameNameField.Update(frameDeltaTime);
				return true;
			}
			bool result = false;
			for (int i = 0; i < _buttons.Length; i++)
			{
				result = _buttons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				result = _saveGameButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			if (_scrollBar.MouseHover(x, y, frameDeltaTime))
			{
				result = true;
				RefreshSaveButtons();
			}
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			if (_promptShowing)
			{
				return _saveGameNameField.MouseDown(x, y);
			}
			bool result = _scrollBar.MouseDown(x, y);
			for (int i = 0; i < _buttons.Length; i++)
			{
				result = _buttons[i].MouseDown(x, y) || result;
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				result = _saveGameButtons[i].MouseDown(x, y) || result;
			}
			return base.MouseDown(x, y) || result;
		}

		public override bool MouseUp(int x, int y)
		{
			if (_promptShowing)
			{
				if (!_saveGameNameField.MouseUp(x, y) && !string.IsNullOrEmpty(_saveGameNameField.Text))
				{
					_promptShowing = false;
					_gameMain.SaveGame(_saveGameNameField.Text);
					GetSaveList(); //Refresh the list after saving
				}
			}
			if (_scrollBar.MouseUp(x, y))
			{
				RefreshSaveButtons();
				return true;
			}
			if (_buttons[0].MouseUp(x, y))
			{
				var func = CloseWindow;
				if (func != null)
				{
					func();
				}
				_gameMain.ClearAll();
				_gameMain.ChangeToScreen(Screen.NewGame);
				return true;
			}
			if (_buttons[1].MouseUp(x, y))
			{
				_promptShowing = true;
				if (_selectedGame >= 0)
				{
					_saveGameNameField.SetText(_files[_selectedGame].Name.Substring(0, _files[_selectedGame].Name.Length - _files[_selectedGame].Extension.Length));
				}
				else
				{
					_saveGameNameField.SetText(string.Empty);
				}
				_saveGameNameField.Select();
				return true;
			}
			if (_buttons[2].MouseUp(x, y))
			{
				var func = CloseWindow;
				if (func != null)
				{
					func();
				}
				_gameMain.LoadGame(_files[_selectedGame].Name);
				return true;
			}
			if (_buttons[3].MouseUp(x, y))
			{
				//TODO: Add prompt to ensure user really want to exit
				_gameMain.ExitGame();
				return true;
			}

			for (int i = 0; i < _maxVisible; i++)
			{
				if (_saveGameButtons[i].MouseUp(x, y))
				{
					foreach (var button in _saveGameButtons)
					{
						button.Selected = false;
					}
					_saveGameButtons[i].Selected = true;
					_selectedGame = i + _scrollBar.TopIndex;
					_buttons[2].Enabled = true;
					return true;
				}
			}

			if (!base.MouseUp(x, y))
			{
				//Clicked outside the window, close this window
				if (CloseWindow != null)
				{
					CloseWindow();
				}
			}
			else
			{
				//Clicked inside window, clear save game selection, if any
				_selectedGame = -1;
				foreach (var button in _saveGameButtons)
				{
					button.Selected = false;
				}
				_buttons[2].Enabled = false;
			}
			return false;
		}

		public override bool KeyDown(KeyboardInputEventArgs e)
		{
			if (_promptShowing)
			{
				bool result = _saveGameNameField.KeyDown(e);
				if (e.Key == KeyboardKeys.Enter || e.Key == KeyboardKeys.Return)
				{
					result = true;
					_promptShowing = false;
					_gameMain.SaveGame(_saveGameNameField.Text);
					GetSaveList(); //Refresh the list after saving
				}
				else if (e.Key == KeyboardKeys.Escape)
				{
					_promptShowing = false;
					result = true;
				}
				return result;
			}
			if (e.Key == KeyboardKeys.Escape && CloseWindow != null)
			{
				CloseWindow();
				return true;
			}
			return false;
		}

		public void GetSaveList()
		{
			_files = Utility.GetSaveGames(_gameMain.GameDataSet.FullName);
			
			_maxVisible = 0;
			if (_files.Count > _saveGameButtons.Length)
			{
				_maxVisible = _saveGameButtons.Length;
				_scrollBar.SetEnabledState(true);
				_scrollBar.SetAmountOfItems(_files.Count);
			}
			else
			{
				_maxVisible = _files.Count;
				_scrollBar.SetEnabledState(false);
				_scrollBar.SetAmountOfItems(_saveGameButtons.Length);
			}
			_selectedGame = -1;
			RefreshSaveButtons();
		}

		private void RefreshSaveButtons()
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				var file = _files[i + _scrollBar.TopIndex];
				_saveGameButtons[i].SetText(file.Name.Substring(0, file.Name.Length - file.Extension.Length) + " - (" + file.LastWriteTime.ToString() + ")");
				_saveGameButtons[i].Selected = false;
				if (i + _scrollBar.TopIndex == _selectedGame)
				{
					_saveGameButtons[i].Selected = true;
				}
			}
		}
	}
}
