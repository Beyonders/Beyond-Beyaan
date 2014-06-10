using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Screens;

namespace Beyond_Beyaan
{
	public enum Screen { MainMenu, NewGame, Galaxy, InGameMenu, Diplomacy, FleetList, Design, Planets, Research, ProcessTurn, Battle };

	public class GameMain
	{
		#region Constants

		public const int SIMPLE = 0;
		public const int EASY = 1;
		public const int MEDIUM = 2;
		public const int HARD = 3;
		public const int IMPOSSIBLE = 4;
		#endregion

		#region Screens
		private ScreenInterface _screenInterface;
		private MainGameMenu _mainGameMenu;
		private NewGame _newGame;
		private GalaxyScreen _galaxyScreen;
		private DiplomacyScreen _diplomacyScreen;
		private ProcessingTurnScreen _processingTurnScreen;
		//private SpaceCombat _spaceCombat;
		
		private SituationReport _situationReport;
		private Screen _currentScreen;
		#endregion

		private Form _parentForm;

		#region Properties
		internal Random Random { get; private set; }
		internal Galaxy Galaxy { get; private set; }
		internal EmpireManager EmpireManager { get; private set; }
		internal RaceManager RaceManager { get; private set; }
		internal AIManager AIManager { get; private set; }
		internal MasterTechnologyManager MasterTechnologyManager { get; private set; }
		internal int ScreenWidth { get; private set; }
		internal int ScreenHeight { get; private set; }
		internal GorgonLibrary.Graphics.FXShader ShipShader { get; private set; }
		internal GorgonLibrary.Graphics.FXShader StarShader { get; private set; }
		internal DirectoryInfo GameDataSet { get; private set; }
		internal int DifficultyLevel { get; set; }
		#endregion

		#region Mouse Stuff
		internal Point MousePos;
		private BBSprite Cursor;
		#endregion

		public bool Initalize(int screenWidth, int screenHeight, DirectoryInfo dataSet, bool showTutorial, Form parentForm, out string reason)
		{
			_parentForm = parentForm;

			Random = new Random();

			MousePos = new Point();

			ScreenWidth = screenWidth;
			ScreenHeight = screenHeight;
			GameDataSet = dataSet;

			Galaxy = new Galaxy();
			EmpireManager = new EmpireManager(this);

			ShipShader = GorgonLibrary.Graphics.FXShader.FromFile("ColorShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
			StarShader = GorgonLibrary.Graphics.FXShader.FromFile("StarShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);

			if (!SpriteManager.Initialize(GameDataSet, out reason))
			{
				return false;
			}
			if (!FontManager.Initialize(GameDataSet, out reason))
			{
				return false;
			}
			RaceManager = new RaceManager();
			if (!RaceManager.Initialize(GameDataSet, Random, out reason))
			{
				return false;
			}
			AIManager = new AIManager();
			if (!AIManager.Initialize(GameDataSet, out reason))
			{
				return false;
			}
			MasterTechnologyManager = new MasterTechnologyManager();
			if (!MasterTechnologyManager.Initialize(this, out reason))
			{
				return false;
			}
			_mainGameMenu = new MainGameMenu();
			if (!_mainGameMenu.Initialize(this, out reason))
			{
				return false;
			}

			_screenInterface = _mainGameMenu;
			_currentScreen = Screen.MainMenu;

			_situationReport = new SituationReport(this);

			Cursor = SpriteManager.GetSprite("Cursor", Random);
			if (Cursor == null)
			{
				reason = "Cursor is not defined in sprites.xml";
				return false;
			}

			reason = string.Empty;
			return true;
		}

		public void ClearAll()
		{
			//Used when exiting out of current game (new game for example)
			EmpireManager = new EmpireManager(this);
			_situationReport.Clear();
			_newGame.Clear();
		}

		public void ProcessGame(float frameDeltaTime)
		{
			bool skipUpdate = false;
			bool handleTaskBar = false;
			if (_currentScreen != Screen.MainMenu && _currentScreen != Screen.NewGame && _currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
			}
			if (handleTaskBar)
			{
				if (_situationReport.Update(MousePos.X, MousePos.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
			}
			if (!skipUpdate)
			{
				_screenInterface.Update(MousePos.X, MousePos.Y, frameDeltaTime);
			}
			_screenInterface.DrawScreen();
			if (handleTaskBar)
			{
				_situationReport.DrawSitRep();
			}
			Cursor.Draw(MousePos.X, MousePos.Y);
			Cursor.Update(frameDeltaTime, Random);
		}

		public void MouseDown(MouseEventArgs e)
		{
			int whichButton = 0;
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					{
						whichButton = 1;
					} break;
				case System.Windows.Forms.MouseButtons.Right:
					{
						whichButton = 2;
					} break;
				case System.Windows.Forms.MouseButtons.Middle:
					{
						whichButton = 3;
					} break;
			}
			bool handleTaskBar = false;
			if (_currentScreen != Screen.MainMenu && _currentScreen != Screen.NewGame && _currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
			}
			if (handleTaskBar)
			{
				if (_situationReport.MouseDown(e.X, e.Y))
				{
					return;
				}
			}
			_screenInterface.MouseDown(e.X, e.Y, whichButton);
		}

		public void MouseUp(MouseEventArgs e)
		{
			int whichButton = 0;
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					{
						whichButton = 1;
					} break;
				case System.Windows.Forms.MouseButtons.Right:
					{
						whichButton = 2;
					} break;
				case System.Windows.Forms.MouseButtons.Middle:
					{
						whichButton = 3;
					} break;
			}
			bool handleTaskBar = false;
			if (_currentScreen != Screen.MainMenu && _currentScreen != Screen.NewGame && _currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
			}
			if (handleTaskBar)
			{
				if (_situationReport.MouseUp(e.X, e.Y))
				{
					return;
				}
			}
			_screenInterface.MouseUp(e.X, e.Y, whichButton);
		}

		public void MouseScroll(int delta)
		{
			_screenInterface.MouseScroll(delta, MousePos.X, MousePos.Y);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			_screenInterface.KeyDown(e);
		}

		public void ToggleSitRep()
		{
			_situationReport.ToggleVisibility();
		}

		public void ChangeToScreen(Screen whichScreen)
		{
			switch (whichScreen)
			{
				case Screen.MainMenu:
					if (_mainGameMenu == null)
					{
						_mainGameMenu = new MainGameMenu();
						string reason;
						if (!_mainGameMenu.Initialize(this, out reason))
						{
							MessageBox.Show("Exception in loading Main Menu. Reason: " + reason);
							_parentForm.Close();
						}
					}
					_screenInterface = _mainGameMenu;
					break;
				case Screen.NewGame:
					if (_newGame == null)
					{
						_newGame = new NewGame();
						string reason;
						if (!_newGame.Initialize(this, out reason))
						{
							MessageBox.Show("Exception in loading New Game Menu. Reason: " + reason);
							_parentForm.Close();
						}
					}
					_screenInterface = _newGame;
					break;
				case Screen.Galaxy:
					if (_galaxyScreen == null)
					{
						_galaxyScreen = new GalaxyScreen();
						string reason;
						if (!_galaxyScreen.Initialize(this, out reason))
						{
							MessageBox.Show("Exception in loading Galaxy Screen. Reason: " + reason);
							_parentForm.Close();
						}
					}
					_galaxyScreen.CenterScreen();
					_screenInterface = _galaxyScreen;
					break;
				case Screen.Diplomacy:
					if (_diplomacyScreen == null)
					{
						_diplomacyScreen = new DiplomacyScreen();
						string reason;
						if (!_diplomacyScreen.Initialize(this, out reason))
						{
							MessageBox.Show("Exception in loading Diplomacy Screen. Reason: " + reason);
							_parentForm.Close();
						}
					}
					_diplomacyScreen.SetupScreen();
					_screenInterface = _diplomacyScreen;
					break;
				case Screen.ProcessTurn:
					EmpireManager.CurrentEmpire.ClearTurnData();
					if (_processingTurnScreen == null)
					{
						_processingTurnScreen = new ProcessingTurnScreen();
						string reason;
						if (!_processingTurnScreen.Initialize(this, out reason))
						{
							MessageBox.Show("Exception in loading Processing Turn Screen. Reason: " + reason);
							_parentForm.Close();
						}
					}
					if (!EmpireManager.ProcessNextEmpire())
					{
						_situationReport.Refresh();
						ChangeToScreen(Screen.Galaxy);
						break;
					}
					_screenInterface = _processingTurnScreen;
					break;
				case Screen.Battle:
					/*if (_spaceCombat == null)
					{
						_spaceCombat = new SpaceCombat();
						_spaceCombat.Initialize(this);
					}
					_spaceCombat.SetupScreen();
					_screenInterface = _spaceCombat;*/
					break;
			}
			_currentScreen = whichScreen;
		}

		public void DrawGalaxyBackground()
		{
			_galaxyScreen.DrawGalaxy();
		}

		public void RefreshSitRep()
		{
			_situationReport.Refresh();
		}
		public void HideSitRep()
		{
			_situationReport.Hide();
		}

		public void ExitGame()
		{
			//dispose of any resources in use
			_parentForm.Close();
		}

		public void SaveGame(string filename)
		{
			XDocument saveGame = new XDocument();
			using (XmlWriter writer = saveGame.CreateWriter())
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("SaveGameData");
				Galaxy.Save(writer);
				EmpireManager.Save(writer);
				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
			try
			{
				string path = Path.Combine(GameDataSet.FullName, "Saves");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				path = Path.Combine(path, filename + ".BB");
				saveGame.Save(path);
			}
			catch (Exception e)
			{
				MessageBox.Show("Failed to save file, reason: " + e.Message);
			}
		}

		public bool LoadGame(string filename)
		{
			string path = Path.Combine(GameDataSet.FullName, "Saves");
			if (!Directory.Exists(path))
			{
				//No folder exists, impossible to load anything
				return false;
			}
			try
			{
				XDocument doc = XDocument.Load(Path.Combine(path, filename));
				XElement root = doc.Root;
				if (!Galaxy.Load(root, this))
				{
					return false;
				}
				if (!EmpireManager.Load(root))
				{
					return false;
				}
				Galaxy.Reconcilate(EmpireManager);
				if (_galaxyScreen != null)
				{
					_galaxyScreen.ResetCamera();
				}
				if (_processingTurnScreen != null)
				{
					_processingTurnScreen.ResetCamera();
				}
				ChangeToScreen(Screen.Galaxy);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
