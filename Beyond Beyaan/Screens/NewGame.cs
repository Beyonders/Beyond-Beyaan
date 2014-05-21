using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class NewGame : ScreenInterface
	{
		private GameMain _gameMain;

		private int _xPos;
		private int _yPos;

		private BBStretchableImage _busyImage;
		private BBLabel _busyText;
		private BBStretchableImage _background;
		private BBSprite _nebulaBackground;
		private BBSprite _randomRaceSprite;

		private BBStretchableImage _playerBackground;
		private BBStretchableImage _playerInfoBackground;
		private BBStretchButton _playerRaceButton;
		private BBSingleLineTextBox _playerEmperorName;
		private BBSingleLineTextBox _playerHomeworldName;
		private BBTextBox _playerRaceDescription;
		private BBLabel[] _playerLabels;

		private BBStretchableImage _AIBackground;
		private BBStretchButton[] _AIRaceButtons;

		private Race[] _playerRaces;
		private BBSprite[] _raceSprites;
		private Color[] _playerColors;

		private BBStretchableImage _galaxyBackground;
		private BBComboBox _galaxyComboBox;

		private BBLabel _difficultyLabel;
		private BBComboBox _difficultyComboBox;

		private BBLabel _numberOfAILabel;
		private BBNumericUpDown _numericUpDownAI;
		private Camera camera;
		private bool _generatingGalaxy;

		private BBStretchButton _okButton;
		private BBStretchButton _cancelButton;

		private RaceSelection _raceSelection;
		private bool _showingSelection;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			this._gameMain = gameMain;

			_showingSelection = false;
			_background = new BBStretchableImage();
			_playerBackground = new BBStretchableImage();
			_playerInfoBackground = new BBStretchableImage();
			_playerRaceButton = new BBStretchButton();
			_playerRaceDescription = new BBTextBox();
			_playerLabels = new BBLabel[3];
			_playerEmperorName = new BBSingleLineTextBox();
			_playerHomeworldName = new BBSingleLineTextBox();
			_AIBackground = new BBStretchableImage();
			_AIRaceButtons = new BBStretchButton[5];
			_raceSprites = new BBSprite[6];
			_playerRaces = new Race[6];
			_playerColors = new Color[6];
			_numberOfAILabel = new BBLabel();
			_numericUpDownAI = new BBNumericUpDown();
			_busyImage = new BBStretchableImage();
			_busyText = new BBLabel();
			_okButton = new BBStretchButton();
			_cancelButton = new BBStretchButton();

			_difficultyComboBox = new BBComboBox();
			_difficultyLabel = new BBLabel();

			_nebulaBackground = SpriteManager.GetSprite("TitleNebula", gameMain.Random);

			_playerColors[0] = Color.Blue;
			_playerColors[1] = Color.Red;
			_playerColors[2] = Color.Yellow;
			_playerColors[3] = Color.Green;
			_playerColors[4] = Color.Purple;
			_playerColors[5] = Color.Orange;

			_xPos = (gameMain.ScreenWidth / 2) - 440;
			_yPos = (gameMain.ScreenHeight / 2) - 340;
			if (_nebulaBackground == null)
			{
				reason = "TitleNebula sprite doesn't exist.";
				return false;
			}
			if (!_background.Initialize(_xPos, _yPos, 880, 680, StretchableImageType.MediumBorder, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_playerBackground.Initialize(_xPos + 30, _yPos + 30, 820, 170, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_playerInfoBackground.Initialize(_xPos + 40, _yPos + 60, 295, 130, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_playerRaceButton.Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 340, _yPos + 40, 500, 150, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_playerRaceDescription.Initialize(_xPos + 485, _yPos + 51, 345, 130, true, true, "RaceDescriptionTextBox", gameMain.Random, out reason))
			{
				return false;
			}
			_playerLabels[0] = new BBLabel();
			if (!_playerLabels[0].Initialize(_xPos + 90, _yPos + 36, "Player Race Information", Color.White, out reason))
			{
				return false;
			}
			_playerLabels[1] = new BBLabel();
			if (!_playerLabels[1].Initialize(_xPos + 45, _yPos + 70, "Emperor Name:", Color.White, out reason))
			{
				return false;
			}
			if (!_playerEmperorName.Initialize(string.Empty, _xPos + 50, _yPos + 90, 275, 35, false, gameMain.Random, out reason))
			{
				return false;
			}
			_playerLabels[2] = new BBLabel();
			if (!_playerLabels[2].Initialize(_xPos + 45, _yPos + 125, "Homeworld Name:", Color.White, out reason))
			{
				return false;
			}
			if (!_playerHomeworldName.Initialize(string.Empty, _xPos + 50, _yPos + 145, 275, 35, false, gameMain.Random, out reason))
			{
				return false;
			}
			_playerRaceDescription.SetText("A random race will be chosen.  If the Emperor and/or Homeworld name fields are left blank, default race names for those will be used.");
			_randomRaceSprite = SpriteManager.GetSprite("RandomRace", gameMain.Random);
			if (_randomRaceSprite == null)
			{
				reason = "RandomRace sprite does not exist.";
				return false;
			}

			for (int i = 0; i < _raceSprites.Length; i++)
			{
				_raceSprites[i] = _randomRaceSprite;
			}

			if (!_AIBackground.Initialize(_xPos + 30, _yPos + 205, 820, 220, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_difficultyLabel.Initialize(_xPos + 40, _yPos + 220, "Difficulty Level:", Color.White, out reason))
			{
				return false;
			}
			List<string> difficultyItems = new List<string>
											{
												"Simple",
												"Easy",
												"Medium",
												"Hard",
												"Impossible"
											};
			if (!_difficultyComboBox.Initialize(difficultyItems, _xPos + 170, _yPos + 215, 200, 35, 5, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_numberOfAILabel.Initialize(_xPos + 730, _yPos + 220, "Number of Computer Players:", Color.White, out reason))
			{
				return false;
			}
			_numberOfAILabel.SetAlignment(true);
			if (!_numericUpDownAI.Initialize(_xPos + 735, _yPos + 222, 75, 1, 5, 5, gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < _AIRaceButtons.Length; i++)
			{
				_AIRaceButtons[i] = new BBStretchButton();
				if (!_AIRaceButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 40 + (i * 155), _yPos + 260, 150, 150, gameMain.Random, out reason))
				{
					return false;
				}
			}

			_galaxyBackground = new BBStretchableImage();
			_galaxyComboBox = new BBComboBox();

			List<string> items = new List<string>();
			items.Add("Small Galaxy");
			items.Add("Medium Galaxy");
			items.Add("Large Galaxy");
			items.Add("Huge Galaxy");

			if (!_galaxyBackground.Initialize(_xPos + 30, _yPos + 430, 240, 235, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_galaxyComboBox.Initialize(items, _xPos + 30, _yPos + 430, 240, 35, 4, gameMain.Random, out reason))
			{
				return false;
			}

			_raceSelection = new RaceSelection();
			if (!_raceSelection.Initialize(gameMain, out reason))
			{
				return false;
			}
			_raceSelection.OnOkClick = OnRaceSelectionOKClick;

			_generatingGalaxy = false;
			if (!_busyImage.Initialize(gameMain.ScreenWidth / 2 - 100, gameMain.ScreenHeight / 2 - 50, 200, 100, StretchableImageType.MediumBorder, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_busyText.Initialize(gameMain.ScreenWidth / 2, gameMain.ScreenHeight / 2, "Generating Galaxy", Color.White, out reason))
			{
				return false;
			}
			if (!_okButton.Initialize("Start Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 660, _yPos + 610, 200, 50, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_cancelButton.Initialize("Main Menu", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 450, _yPos + 610, 200, 50, gameMain.Random, out reason))
			{
				return false;
			}

			LoadLastSettings();

			reason = null;
			return true;
		}

		public void Clear()
		{
			LoadLastSettings();
		}

		public void DrawScreen()
		{
			for (int i = 0; i < _gameMain.ScreenWidth; i += (int)_nebulaBackground.Width)
			{
				for (int j = 0; j < _gameMain.ScreenHeight; j += (int)_nebulaBackground.Height)
				{
					_nebulaBackground.Draw(i, j);
				}
			}
			_background.Draw();
			_playerBackground.Draw();
			_playerInfoBackground.Draw();
			for (int i = 0; i < _playerLabels.Length; i++)
			{
				_playerLabels[i].Draw();
			}
			_playerRaceButton.Draw();
			_raceSprites[0].Draw(_xPos + 350, _yPos + 51);
			_playerRaceDescription.Draw();
			_playerEmperorName.Draw();
			_playerHomeworldName.Draw();

			_AIBackground.Draw();
			_difficultyLabel.Draw();

			_numberOfAILabel.Draw();
			_numericUpDownAI.Draw();
			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				_AIRaceButtons[i].Draw();
				_raceSprites[i + 1].Draw(_xPos + 51 + (i * 155), _yPos + 271);
			}

			_galaxyBackground.Draw();
			//if (GameConfiguration.AllowGalaxyPreview)
			{
				DrawGalaxyPreview();
			}

			_okButton.Draw();
			_cancelButton.Draw();

			//Comboboxes should be drawn last, due to their "drop-down" feature
			_galaxyComboBox.Draw();
			_difficultyComboBox.Draw();

			if (_showingSelection)
			{
				_raceSelection.Draw();
			}
			if (_generatingGalaxy)
			{
				_busyImage.Draw();
				_busyText.Draw();
			}
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			if (_generatingGalaxy)
			{
				return;
			}
			if (_showingSelection)
			{
				_raceSelection.MouseHover(x, y, frameDeltaTime);
				return;
			}
			_playerEmperorName.Update(frameDeltaTime);
			_playerHomeworldName.Update(frameDeltaTime);
			_galaxyComboBox.MouseHover(x, y, frameDeltaTime);
			_difficultyComboBox.MouseHover(x, y, frameDeltaTime);
			_playerRaceButton.MouseHover(x, y, frameDeltaTime);
			
			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				_AIRaceButtons[i].MouseHover(x, y, frameDeltaTime);
			}

			_numericUpDownAI.MouseHover(x, y, frameDeltaTime);
			_playerRaceDescription.MouseHover(x, y, frameDeltaTime);
			_okButton.MouseHover(x, y, frameDeltaTime);
			_cancelButton.MouseHover(x, y, frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (whichButton != 1)
			{
				return;
			}
			if (_generatingGalaxy)
			{
				return;
			}
			if (_showingSelection)
			{
				_raceSelection.MouseDown(x, y);
				return;
			}
			if (_playerRaceDescription.MouseDown(x, y))
			{
				return;
			}
			if (_galaxyComboBox.MouseDown(x, y))
			{
				return;
			}
			if (_difficultyComboBox.MouseDown(x, y))
			{
				return;
			}
			if (_playerEmperorName.MouseDown(x, y))
			{
				return;
			}
			if (_playerHomeworldName.MouseDown(x, y))
			{
				return;
			}
			if (_playerRaceButton.MouseDown(x, y))
			{
				return;
			}
			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				if (_AIRaceButtons[i].MouseDown(x, y))
				{
					return;
				}
			}
			if (_okButton.MouseDown(x, y))
			{
				return;
			}
			if (_cancelButton.MouseDown(x, y))
			{
				return;
			}
			_numericUpDownAI.MouseDown(x, y);
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (whichButton != 1)
			{
				return;
			}
			if (_generatingGalaxy)
			{
				return;
			}
			if (_showingSelection)
			{
				_raceSelection.MouseUp(x, y);
				return;
			}
			_playerEmperorName.MouseUp(x, y);
			_playerHomeworldName.MouseUp(x, y);
			_numericUpDownAI.MouseUp(x, y);
			_difficultyComboBox.MouseUp(x, y);
			if (_playerRaceDescription.MouseUp(x, y))
			{
				return;
			}
			if (_galaxyComboBox.MouseUp(x, y))
			{
				if (!_galaxyComboBox.Dropped)
				{
					//Update galaxy here
					_generatingGalaxy = true;
					_busyText.MoveTo((int)((_gameMain.ScreenWidth / 2) - (_busyText.GetWidth() / 2)), (int)((_gameMain.ScreenHeight / 2) - (_busyText.GetHeight() / 2)));
					_gameMain.Galaxy.OnGenerateComplete += OnGalaxyGenerated;
					string reason;
					if (!_gameMain.Galaxy.GenerateGalaxy((GALAXYTYPE)_galaxyComboBox.SelectedIndex, 1, 1, _gameMain.Random, out reason))
					{
						_generatingGalaxy = false;
					}
				}
			}
			if (_playerRaceButton.MouseUp(x, y))
			{
				_showingSelection = true;
				_raceSelection.SetCurrentPlayerInfo(0, _playerRaces[0], _playerColors[0]);
			}
			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				if (_AIRaceButtons[i].MouseUp(x, y))
				{
					_showingSelection = true;
					_raceSelection.SetCurrentPlayerInfo(i + 1, _playerRaces[i + 1], _playerColors[i + 1]);
				}
			}
			if (_cancelButton.MouseUp(x, y))
			{
				_gameMain.ChangeToScreen(Screen.MainMenu);
			}
			if (_okButton.MouseUp(x, y))
			{
				//See if the galaxy is generated.  If not, generate it now, then proceed to player setup
				if (_gameMain.Galaxy.GetAllStars().Count == 0)
				{
					SetUpGalaxy();
				}
				else
				{
					SetUpEmpiresAndStart();
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (_generatingGalaxy)
			{
				return;
			}
			if (e.Key == KeyboardKeys.Escape)
			{
				if (_showingSelection)
				{
					//Close the race selection
					_showingSelection = false;
					return;
				}
				_gameMain.ChangeToScreen(Screen.MainMenu);
			}
			if (_playerEmperorName.KeyDown(e))
			{
				return;
			}
			if (_playerHomeworldName.KeyDown(e))
			{
				return;
			}
			
		}

		private void DrawGalaxyPreview()
		{
			if (_generatingGalaxy)
			{
				//Don't draw anything, the systems may get updated in middle of drawing, and cause an exception
				return;
			}
			List<StarSystem> systems = _gameMain.Galaxy.GetAllStars();

			if (systems.Count > 0 && camera != null)
			{
				foreach (StarSystem system in systems)
				{
					GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
					//_xPos + 30, _yPos + 430, 240, 235
					int x = _xPos + 60 + (int)((system.X - camera.CameraX) * camera.ZoomDistance);
					int y = _yPos + 470 + (int)((system.Y - camera.CameraY) * camera.ZoomDistance);
					_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
					system.Sprite.Draw(x, y, 0.4f, 0.4f);
					GorgonLibrary.Gorgon.CurrentShader = null;
				}
				//numOfStarsLabel.Draw();
			}
		}

		private void OnGalaxyGenerated()
		{
			_generatingGalaxy = false;
			_gameMain.Galaxy.OnGenerateComplete = null;
			camera = new Camera(_gameMain.Galaxy.GalaxySize * 60, _gameMain.Galaxy.GalaxySize * 60, 190, 190);
			camera.CenterCamera(camera.Width / 2, camera.Height / 2, camera.MaxZoom);
		}

		private void OnGalaxyGeneratedThenPlayerStart()
		{
			_gameMain.Galaxy.OnGenerateComplete = null;
			SetUpEmpiresAndStart();
		}

		private void SetUpGalaxy()
		{
			_generatingGalaxy = true;
			_busyText.MoveTo((int)((_gameMain.ScreenWidth / 2.0f) - (_busyText.GetWidth() / 2)), (int)((_gameMain.ScreenHeight / 2.0f) - (_busyText.GetHeight() / 2)));
			_gameMain.Galaxy.OnGenerateComplete += OnGalaxyGeneratedThenPlayerStart;
			string reason;
			if (!_gameMain.Galaxy.GenerateGalaxy((GALAXYTYPE)_galaxyComboBox.SelectedIndex, 1, 1, _gameMain.Random, out reason))
			{
				_generatingGalaxy = false;
			}
		}

		private void SetUpEmpiresAndStart()
		{
			SaveSettings();
			List<Race> selectedRaces = new List<Race>();
			foreach (Race race in _playerRaces)
			{
				if (race != null)
				{
					selectedRaces.Add(race);
				}
			}
			while (_playerRaces[0] == null)
			{
				int random = _gameMain.Random.Next(_gameMain.RaceManager.Races.Count);
				if (!selectedRaces.Contains(_gameMain.RaceManager.Races[random]))
				{
					_playerRaces[0] = _gameMain.RaceManager.Races[random];
					selectedRaces.Add(_playerRaces[0]);
				}
			}
			//Galaxy already generated, move on to player setup
			_gameMain.DifficultyLevel = _difficultyComboBox.SelectedIndex;
			string emperorName = string.IsNullOrEmpty(_playerEmperorName.Text) ? _playerRaces[0].GetRandomEmperorName() : _playerEmperorName.Text;
			string homeworldName = string.IsNullOrEmpty(_playerHomeworldName.Text) ? NameGenerator.GetName() : _playerHomeworldName.Text;
			Empire empire = new Empire(emperorName, 0, _playerRaces[0], PlayerType.HUMAN, _playerColors[0], _gameMain);
			Planet homePlanet;
			StarSystem homeSystem = _gameMain.Galaxy.SetHomeworld(empire, out homePlanet);
			if (homeSystem == null)
			{
				_gameMain.EmpireManager.Reset();
				//No valid systems, start again
				SetUpGalaxy();
				return;
			}
			homeSystem.Name = homeworldName;
			empire.SetHomeSystem(homeSystem, homePlanet);
			empire.UpdateProduction(); //This sets up expenses and all that
			_gameMain.EmpireManager.AddEmpire(empire);

			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				while (_playerRaces[i + 1] == null)
				{
					int random = _gameMain.Random.Next(_gameMain.RaceManager.Races.Count);
					if (!selectedRaces.Contains(_gameMain.RaceManager.Races[random]))
					{
						_playerRaces[i + 1] = _gameMain.RaceManager.Races[random];
						selectedRaces.Add(_playerRaces[i + 1]);
					}
				}
				empire = new Empire(_playerRaces[i + 1].GetRandomEmperorName(), i + 1, _playerRaces[i + 1], PlayerType.CPU, _playerColors[i + 1], _gameMain);
				//AI always have 50 initial population
				homeSystem = _gameMain.Galaxy.SetHomeworld(empire, out homePlanet);
				if (homeSystem == null)
				{
					_gameMain.EmpireManager.Reset();
					//No valid systems, start again
					SetUpGalaxy();
					return;
				}
				homeSystem.Name = NameGenerator.GetName();
				empire.SetHomeSystem(homeSystem, homePlanet);
				empire.UpdateProduction(); //This sets up expenses and all that
				_gameMain.EmpireManager.AddEmpire(empire);
			}
			_gameMain.EmpireManager.SetupContacts();
			_gameMain.EmpireManager.SetInitialEmpireTurn();
			_generatingGalaxy = false;
			_gameMain.ChangeToScreen(Screen.Galaxy);
		}

		private void OnRaceSelectionOKClick(int whichPlayer, Race whichRace)
		{
			_showingSelection = false;
			for (int i = 0; i < _playerRaces.Length; i++)
			{
				if (i != whichPlayer && _playerRaces[i] == whichRace)
				{
					_playerRaces[i] = null;
					_raceSprites[i] = _randomRaceSprite;
					if (i == 0)
					{
						_playerRaceDescription.SetText("A random race will be chosen.  If the Emperor and/or Homeworld name fields are left blank, default race names for those will be used.");
					}
					break;
				}
			}
			_playerRaces[whichPlayer] = whichRace;
			if (whichRace == null)
			{
				_raceSprites[whichPlayer] = _randomRaceSprite;
			}
			else
			{
				_raceSprites[whichPlayer] = whichRace.MiniAvatar;
			}
			if (whichPlayer == 0)
			{
				if (whichRace == null)
				{
					_playerRaceDescription.SetText("A random race will be chosen.  If the Emperor and/or Homeworld name fields are left blank, default race names for those will be used.");
				}
				else
				{
					_playerRaceDescription.SetText(whichRace.RaceDescription);
				}
			}
		}

		private void LoadLastSettings()
		{
			var path = Path.Combine(_gameMain.GameDataSet.FullName, "GameSettings.config");
			if (File.Exists(path))
			{
				using (StreamReader reader = new StreamReader(path))
				{
					_playerEmperorName.SetText(reader.ReadLine());
					_playerHomeworldName.SetText(reader.ReadLine());
					var raceName = reader.ReadLine();
					if (raceName == "Random")
					{
						_playerRaces[0] = null;
						_raceSprites[0] = _randomRaceSprite;
					}
					else
					{
						foreach (var race in _gameMain.RaceManager.Races)
						{
							if (race.RaceName == raceName)
							{
								_playerRaces[0] = race;
								_raceSprites[0] = race.MiniAvatar;
								break;
							}
						}
					}
					_galaxyComboBox.SelectedIndex = int.Parse(reader.ReadLine());
					_difficultyComboBox.SelectedIndex = int.Parse(reader.ReadLine());
					_numericUpDownAI.SetValue(int.Parse(reader.ReadLine()));
					for (int i = 0; i < _numericUpDownAI.Value; i++)
					{
						raceName = reader.ReadLine();
						if (raceName == "Random")
						{
							_playerRaces[i + 1] = null;
							_raceSprites[i + 1] = _randomRaceSprite;
						}
						else
						{
							foreach (var race in _gameMain.RaceManager.Races)
							{
								if (race.RaceName == raceName)
								{
									_playerRaces[i + 1] = race;
									_raceSprites[i + 1] = race.MiniAvatar;
									break;
								}
							}
						}
					}
				}
			}
			else
			{
				_playerRaces = new Race[6];
				_numericUpDownAI.SetValue(1);
				_playerEmperorName.SetText(string.Empty);
				_playerHomeworldName.SetText(string.Empty);
				for (int i = 0; i < _raceSprites.Length; i++)
				{
					_raceSprites[i] = _randomRaceSprite;
				}
			}
		}

		private void SaveSettings()
		{
			var path = Path.Combine(_gameMain.GameDataSet.FullName, "GameSettings.config");
			using (StreamWriter writer = new StreamWriter(path))
			{
				writer.WriteLine(_playerEmperorName.Text);
				writer.WriteLine(_playerHomeworldName.Text);
				if (_playerRaces[0] != null)
				{
					writer.WriteLine(_playerRaces[0].RaceName);
				}
				else
				{
					writer.WriteLine("Random");
				}
				writer.WriteLine(_galaxyComboBox.SelectedIndex);
				writer.WriteLine(_difficultyComboBox.SelectedIndex);
				writer.WriteLine(_numericUpDownAI.Value);
				for (int i = 0; i < _numericUpDownAI.Value; i++)
				{
					if (_playerRaces[i + 1] != null)
					{
						writer.WriteLine(_playerRaces[i + 1].RaceName);
					}
					else
					{
						writer.WriteLine("Random");
					}
				}
			}
		}
	}
}
