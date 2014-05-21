using System;
using System.Drawing;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	class RaceSelection : WindowInterface
	{
		private BBSprite _randomSprite;
		private BBStretchButton[] _raceButtons;
		private BBStretchableImage _raceBackground;
		private BBScrollBar _raceScrollBar;
		private RaceManager _raceManager;

		private BBTextBox _raceDescription;

		private BBStretchButton _okButton;

		private int _whichPlayerSelecting;
		private Race _whichRaceSelected;

		private int _maxVisible;

		public Action<int, Race> OnOkClick { get; set; }

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;

			if (!base.Initialize((gameMain.ScreenWidth / 2) - 320, (gameMain.ScreenHeight / 2) - 320, 640, 640, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}
			_randomSprite = SpriteManager.GetSprite("RandomRace", gameMain.Random);
			if (_randomSprite == null)
			{
				reason = "RandomRace sprite does not exist.";
				return false;
			}

			_raceButtons = new BBStretchButton[15];
			_raceScrollBar = new BBScrollBar();
			_raceBackground = new BBStretchableImage();
			_raceDescription = new BBTextBox();
			_okButton = new BBStretchButton();
			_raceManager = gameMain.RaceManager;

			for (int i = 0; i < _raceButtons.Length; i++)
			{
				_raceButtons[i] = new BBStretchButton();
				if (!_raceButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 10, _yPos + 10 + (i * 40), 280, 40, gameMain.Random, out reason))
				{
					return false;
				}
			}
			//Add 1 for the random race option
			int scrollValue = (_raceManager.Races.Count + 1) < _raceButtons.Length ? _raceButtons.Length : (_raceManager.Races.Count + 1);
			if (!_raceScrollBar.Initialize(_xPos + 290, _yPos + 10, 600, _raceButtons.Length, scrollValue, false, false, gameMain.Random, out reason))
			{
				return false;
			}
			_maxVisible = (_raceManager.Races.Count + 1) > _raceButtons.Length ? _raceButtons.Length : (_raceManager.Races.Count + 1);
			if (_raceManager.Races.Count < 15)
			{
				_raceScrollBar.SetEnabledState(false);
			}
			if (!_raceBackground.Initialize(_xPos + 310, _yPos + 10, 310, 550, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_raceDescription.Initialize(_xPos + 315, _yPos + 325, 300, 215, true, true, "RaceSelectionDescriptionTextBox", gameMain.Random, out reason))
			{
				return false;
			}
			if (!_okButton.Initialize("Select Race", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 310, _yPos + 570, 310, 40, gameMain.Random, out reason))
			{
				return false;
			}
			RefreshRaceLabels();
			RefreshRaceDescription();
			reason = null;
			return true;
		}

		public void SetCurrentPlayerInfo(int whichPlayer, Race race, Color color)
		{
			_whichPlayerSelecting = whichPlayer;
			_whichRaceSelected = race;
			RefreshRaceLabels(); //This also sets the correct button to selected state
			RefreshRaceDescription();
		}

		public override void Draw()
		{
			base.Draw();
			_raceBackground.Draw();
			for (int i = 0; i < _maxVisible; i++)
			{
				_raceButtons[i].Draw();
			}
			_raceScrollBar.Draw();
			if (_whichRaceSelected == null)
			{
				_randomSprite.Draw(_xPos + 315, _yPos + 15, 300 / _randomSprite.Width, 300 / _randomSprite.Height);
			}
			else
			{
				_whichRaceSelected.NeutralAvatar.Draw(_xPos + 315, _yPos + 15, 300 / _whichRaceSelected.NeutralAvatar.Width, 300 / _whichRaceSelected.NeutralAvatar.Height);
			}
			_raceDescription.Draw();
			_okButton.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			_okButton.MouseHover(x, y, frameDeltaTime);
			_raceDescription.MouseHover(x, y, frameDeltaTime);
			for (int i = 0; i < _maxVisible; i++)
			{
				_raceButtons[i].MouseHover(x, y, frameDeltaTime);
			}
			return base.MouseHover(x, y, frameDeltaTime);
		}

		public override bool MouseDown(int x, int y)
		{
			if (_okButton.MouseDown(x, y))
			{
				return true;
			}
			if (_raceDescription.MouseDown(x, y))
			{
				return true;
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				if (_raceButtons[i].MouseDown(x, y))
				{
					return true;
				}
			}
			return base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
			if (_okButton.MouseUp(x, y))
			{
				if (OnOkClick != null)
				{
					OnOkClick(_whichPlayerSelecting, _whichRaceSelected);
					return true;
				}
			}
			if (_raceDescription.MouseUp(x, y))
			{
				return true;
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				if (_raceButtons[i].MouseUp(x, y))
				{
					if (_raceButtons[i].DoubleClicked)
					{
						if (OnOkClick != null)
						{
							OnOkClick(_whichPlayerSelecting, _whichRaceSelected);
							return true;
						}
					}
					if (i == 0 && _raceScrollBar.TopIndex == 0)
					{
						_whichRaceSelected = null;
					}
					else
					{
						_whichRaceSelected = _raceManager.Races[i + _raceScrollBar.TopIndex - 1];
					}
					RefreshRaceDescription();
					RefreshRaceLabels();
					return true;
				}
			}
			return base.MouseUp(x, y);
		}

		private void RefreshRaceLabels()
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				_raceButtons[i].Selected = false;
				if (i + _raceScrollBar.TopIndex == 0)
				{
					//This is the random race selection
					_raceButtons[i].SetText("Random Race");
					if (_whichRaceSelected == null)
					{
						_raceButtons[i].Selected = true;
					}
				}
				else
				{
					_raceButtons[i].SetText(_raceManager.Races[i - 1 + _raceScrollBar.TopIndex].RaceName);
					if (_whichRaceSelected == _raceManager.Races[i - 1 + _raceScrollBar.TopIndex])
					{
						_raceButtons[i].Selected = true;
					}
				}
			}
		}

		private void RefreshRaceDescription()
		{
			if (_whichRaceSelected == null)
			{
				_raceDescription.SetText("A random race will be selected.");
			}
			else
			{
				_raceDescription.SetText(_whichRaceSelected.RaceDescription);
			}
		}
	}
}
