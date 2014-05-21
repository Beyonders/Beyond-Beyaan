using System.Collections.Generic;
using GorgonLibrary.Graphics;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class ProcessingTurnScreen : ScreenInterface
	{
		private GameMain _gameMain;
		private Camera _camera;
		private int _updateStep;

		private SystemInfoWindow _systemInfoWindow; //Notifications for such as explored system
		private ColonizeScreen _colonizeScreen;
		private SystemView _systemView; //Display the system in question
		private ResearchPrompt _researchPrompt;
		
		private RenderTarget _oldTarget;
		private RenderImage _starName;

		private Empire _whichEmpireFocusedOn;
		private Dictionary<Empire, List<StarSystem>> _exploredSystemsThisTurn;
		private Dictionary<Empire, List<Fleet>> _colonizableFleetsThisTurn;
		private Dictionary<Empire, List<TechField>> _newResearchTopicsNeeded; 

		private bool StarNamesVisible
		{
			get 
			{ 
				return	_exploredSystemsThisTurn.Count > 0 ||
						_colonizableFleetsThisTurn.Count > 0; 
			}
		}

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;
			_camera = new Camera(gameMain.Galaxy.GalaxySize * 60, gameMain.Galaxy.GalaxySize * 60, gameMain.ScreenWidth, gameMain.ScreenHeight);
			_camera.CenterCamera(_camera.Width / 2, _camera.Height / 2, _camera.MaxZoom);

			_updateStep = 0;

			_exploredSystemsThisTurn = new Dictionary<Empire, List<StarSystem>>();
			_colonizableFleetsThisTurn = new Dictionary<Empire, List<Fleet>>();
			_newResearchTopicsNeeded = new Dictionary<Empire, List<TechField>>();

			_systemInfoWindow = new SystemInfoWindow();
			if (!_systemInfoWindow.Initialize(gameMain, out reason))
			{
				return false;
			}

			_systemView = new SystemView();
			if (!_systemView.Initialize(gameMain, "ProcessingScreen", out reason))
			{
				return false;
			}

			_colonizeScreen = new ColonizeScreen();
			if (!_colonizeScreen.Initialize(gameMain, out reason))
			{
				return false;
			}

			_researchPrompt = new ResearchPrompt();
			if (!_researchPrompt.Initialize(gameMain, out reason))
			{
				return false;
			}

			_starName = new RenderImage("starNameProcessingTurnRendered", 1, 1, ImageBufferFormats.BufferRGB888A8);
			_starName.BlendingMode = BlendingModes.Modulated;

			reason = null;
			return true;
		}

		public void DrawScreen()
		{
			List<StarSystem> systems = _gameMain.Galaxy.GetStarsInArea(_camera.CameraX, _camera.CameraY, _gameMain.ScreenWidth / _camera.ZoomDistance, _gameMain.ScreenHeight / _camera.ZoomDistance);
			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
				_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				system.Sprite.Draw((int)((system.X - _camera.CameraX) * _camera.ZoomDistance), (int)((system.Y - _camera.CameraY) * _camera.ZoomDistance), _camera.ZoomDistance, _camera.ZoomDistance);
				GorgonLibrary.Gorgon.CurrentShader = null;
				if (StarNamesVisible && (_whichEmpireFocusedOn.ContactManager.IsContacted(system.DominantEmpire) || system.IsThisSystemExploredByEmpire(_whichEmpireFocusedOn)))
				{
					float x = (system.X - _camera.CameraX) * _camera.ZoomDistance;
					x -= (system.StarName.GetWidth() / 2);
					float y = ((system.Y + (system.Size * 16)) - _camera.CameraY) * _camera.ZoomDistance;
					system.StarName.MoveTo((int)x, (int)y);
					if (system.DominantEmpire != null)
					{
						// TODO: Optimize this by moving the text sprite and color shader to StarSystem, where it's updated when ownership changes
						float percentage = 1.0f;
						_oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
						_starName.Width = (int)system.StarName.GetWidth();
						_starName.Height = (int)system.StarName.GetHeight();
						GorgonLibrary.Gorgon.CurrentRenderTarget = _starName;
						system.StarName.MoveTo(0, 0);
						system.StarName.Draw();
						GorgonLibrary.Gorgon.CurrentRenderTarget = _oldTarget;
						//GorgonLibrary.Gorgon.CurrentShader = _gameMain.NameShader;
						foreach (Empire empire in system.EmpiresWithPlanetsInThisSystem)
						{
							/*_gameMain.NameShader.Parameters["EmpireColor"].SetValue(empire.ConvertedColor);
							_gameMain.NameShader.Parameters["startPos"].SetValue(percentage);
							_gameMain.NameShader.Parameters["endPos"].SetValue(percentage + system.OwnerPercentage[empire]);*/
							_starName.Blit(x, y, _starName.Width * percentage, _starName.Height, empire.EmpireColor, BlitterSizeMode.Crop);
							percentage -= system.OwnerPercentage[empire];
						}
						//GorgonLibrary.Gorgon.CurrentShader = null;
					}
					else
					{
						system.StarName.Draw();
					}
				}
			}
			foreach (Fleet fleet in _gameMain.EmpireManager.GetFleetsWithinArea(_camera.CameraX, _camera.CameraY, _gameMain.ScreenWidth / _camera.ZoomDistance, _gameMain.ScreenHeight / _camera.ZoomDistance))
			{
				var fleetIcon = fleet.Ships.Count > 0 ? fleet.Empire.EmpireRace.FleetIcon : fleet.Empire.EmpireRace.TransportIcon;
				if (fleet.AdjacentSystem != null)
				{
					if (fleet.TravelNodes != null && fleet.TravelNodes.Count > 0)
					{
						//Adjacent to a system, but is heading to another system
						fleetIcon.Draw((int)(((fleet.GalaxyX - 32) - _camera.CameraX) * _camera.ZoomDistance), (int)((fleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance), 1, 1, fleet.Empire.EmpireColor);
					}
					else
					{
						//Adjacent to a system, just chilling
						fleetIcon.Draw((int)(((fleet.GalaxyX + 32) - _camera.CameraX) * _camera.ZoomDistance), (int)((fleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance), 1, 1, fleet.Empire.EmpireColor);
					}
				}
				else
				{
					fleetIcon.Draw((int)((fleet.GalaxyX - _camera.CameraX) * _camera.ZoomDistance), (int)((fleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance), 1, 1, fleet.Empire.EmpireColor);
				}
			}
			if (_exploredSystemsThisTurn.Count > 0)
			{
				_systemView.Draw();
				_systemInfoWindow.Draw();
			}
			if (_colonizableFleetsThisTurn.Count > 0)
			{
				_colonizeScreen.Draw();
			}
			if (_newResearchTopicsNeeded.Count > 0)
			{
				_researchPrompt.Draw();
			}
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			if (_colonizableFleetsThisTurn.Count > 0)
			{
				_colonizeScreen.MouseHover(x, y, frameDeltaTime);
			}
			if (_newResearchTopicsNeeded.Count > 0)
			{
				_researchPrompt.MouseHover(x, y, frameDeltaTime);
			}
			switch (_updateStep)
			{
				case 0:
					//Process AI's Turn here
					//  TODO: AI checks for peace treaties, and plans accordingly
					//  TODO: AI plan movement on basis of need to expand, to press attack, or to defend
					//  TODO: AI designs any new ships, done randomly every 6-15 turns
					//  TODO: AI plans production strategy and set their ratio bars
					_updateStep++;
					break;
				case 1:
					_gameMain.EmpireManager.LaunchTransports();
					//  TODO: Deduct cost for transports
					//  TODO: Production is executed
					_gameMain.EmpireManager.AccureIncome();
					_gameMain.EmpireManager.UpdatePopulationGrowth();
					_gameMain.EmpireManager.AccureResearch();
					//  TODO: Trade growth occurs
					//  TODO: New spies are added
					_gameMain.EmpireManager.UpdateEmpires();
					_updateStep++;
					break;
				case 2:
					//Diplomacy update:
					//  TODO: Love nub move towards natural state if no treaty or war
					//  TODO: Points added if there's trade, non-agg, or alliance
					//  TODO: Points subtracted for military buildup, or owning more than 1/4 of map
					//  TODO: Temp modifiers are adjusted toward 0 by 10 points each
					_updateStep++;
					break;
				case 3:
					//  TODO: New missile bases/shields added here
					_gameMain.EmpireManager.UpdateMilitary();
					_updateStep++;
					break;
				case 4:
					//  TODO: Economic adjustments
					//  TODO: Collect Tax and surplus industry spending and put into reserves
					//  TODO: New factories are built
					_updateStep++;
					break;
				case 5:
					if (!_gameMain.EmpireManager.UpdateFleetMovement(frameDeltaTime))
					{
						//Finished moving, merge fleets then move to next step
						_gameMain.EmpireManager.MergeIdleFleets();
						_updateStep++;
					}
					break;
				case 6:
					// TODO: Space combat
					_updateStep++;
					break;
				case 7:
					// TODO: Spies do stuff if not hiding
					_updateStep++;
					break;
				case 8:
					if (_newResearchTopicsNeeded.Count == 0)
					{
						_newResearchTopicsNeeded = _gameMain.EmpireManager.RollForDiscoveries(_gameMain.Random);
						if (_newResearchTopicsNeeded.Count > 0)
						{
							foreach (var keyPair in _newResearchTopicsNeeded)
							{
								//How else to get the first key?
								_whichEmpireFocusedOn = keyPair.Key;
								break;
							}

							_researchPrompt.LoadEmpire(_whichEmpireFocusedOn, _newResearchTopicsNeeded[_whichEmpireFocusedOn]);
							_researchPrompt.Completed += OnResearchPromptComplete;
						}
					}
					if (_newResearchTopicsNeeded.Count == 0)
					{
						_updateStep++;
					}
					// TODO: If computer player gets a new research item, it reevaluates its need to spy on other races here
					break;
				case 9:
					if (_exploredSystemsThisTurn.Count == 0)
					{
						_exploredSystemsThisTurn = _gameMain.EmpireManager.CheckExploredSystems(_gameMain.Galaxy);
						if (_exploredSystemsThisTurn.Count > 0)
						{
							foreach (var keyPair in _exploredSystemsThisTurn)
							{
								//How else to get the first key?
								_whichEmpireFocusedOn = keyPair.Key;
								break;
							}
							_systemInfoWindow.LoadExploredSystem(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0]);
							_systemView.LoadSystem(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0], _whichEmpireFocusedOn);
							_camera.CenterCamera(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0].X, _exploredSystemsThisTurn[_whichEmpireFocusedOn][0].Y, 1);
						}
					}
					// Results of planetary exploration are shown
					if (_exploredSystemsThisTurn.Count == 0)
					{
						//Either no explored systems or finished displaying all explored systems
						_updateStep++;
					}
					break;
				case 10:
					if (_colonizableFleetsThisTurn.Count == 0)
					{
						_colonizableFleetsThisTurn = _gameMain.EmpireManager.CheckColonizableSystems(_gameMain.Galaxy);
						if (_colonizableFleetsThisTurn.Count > 0)
						{
							foreach (var keyPair in _colonizableFleetsThisTurn)
							{
								_whichEmpireFocusedOn = keyPair.Key;
								break;
							}
							_colonizeScreen.LoadFleetAndSystem(_colonizableFleetsThisTurn[_whichEmpireFocusedOn][0]);
							_colonizeScreen.Completed += OnColonizeComplete;
							_camera.CenterCamera(_colonizableFleetsThisTurn[_whichEmpireFocusedOn][0].AdjacentSystem.X, _colonizableFleetsThisTurn[_whichEmpireFocusedOn][0].AdjacentSystem.Y, 1);
						}
					}
					if (_colonizableFleetsThisTurn.Count == 0)
					{
						_updateStep++;
					}
					break;
				case 11:
					// TODO: resolve combat
					_gameMain.EmpireManager.LandTransports();
					// TODO: Orbital bombardments
					// TODO: Transports land and ground combat is resolved
					_updateStep++;
					break;
				case 12:
					// TODO: BNN checks for genocide, and if only one player remains, it is hailed as winner of game
					_updateStep++;
					break;
				case 13:
					// TODO: Random events
					// Add 2% to cumulative probability chance, multipled by game difficulty modifier, rolls d100 die to see if an event occurs
					// If event occurs, randomly select from those that haven't occured yet, and reset probability to 0.  Target player determined and event takes effect
					_updateStep++;
					break;
				case 14:
					// TODO: First contacts, if either race is not in contact, but overlaps one of two's colonies with their fuel range, establish contact
					_updateStep++;
					break;
				case 15:
					//  TODO: All computer-initiated diplomacy initiated
					//  TODO: High council convences if criterias are met.
					//  TODO: Messages from computer players are delivered to human player
					//  TODO: If contact is broken, notification occurs
					_updateStep++;
					break;
				case 16:
					//  TODO: Planetary production/completion messages are displayed (i.e. shield built).  Slider bars can be adjusted
					//  TODO: Production numbers for next turn are calculated
					_updateStep++;
					break;
				case 17:
					//  TODO: Any star systems within advanced space scanner range of colonies/ships is updated
					//  TODO: Turn advances by 1
					//  TODO: Recalculate players' current technology levels
					//  TODO: Autosave game
					_updateStep = 0;
					_gameMain.EmpireManager.ClearEmptyFleets();
					_gameMain.EmpireManager.SetInitialEmpireTurn();
					_gameMain.ChangeToScreen(Screen.Galaxy);
					break;
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (whichButton != 1)
			{
				return;
			}
			if (_colonizableFleetsThisTurn.Count > 0)
			{
				_colonizeScreen.MouseDown(x, y);
			}
			if (_newResearchTopicsNeeded.Count > 0)
			{
				_researchPrompt.MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (whichButton != 1)
			{
				return;
			}
			if (_exploredSystemsThisTurn.Count > 0)
			{
				_exploredSystemsThisTurn[_whichEmpireFocusedOn].RemoveAt(0);
				if (_exploredSystemsThisTurn[_whichEmpireFocusedOn].Count > 0)
				{
					_systemInfoWindow.LoadExploredSystem(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0]);
					_systemView.LoadSystem(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0], _whichEmpireFocusedOn);
					_camera.CenterCamera(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0].X, _exploredSystemsThisTurn[_whichEmpireFocusedOn][0].Y, 1);
				}
				else
				{
					_exploredSystemsThisTurn.Remove(_whichEmpireFocusedOn);
					if (_exploredSystemsThisTurn.Count > 0) //Hot seat humans
					{
						foreach (var keyPair in _exploredSystemsThisTurn)
						{
							_whichEmpireFocusedOn = keyPair.Key;
							break;
						}
						_systemInfoWindow.LoadExploredSystem(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0]);
						_systemView.LoadSystem(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0], _whichEmpireFocusedOn);
						_camera.CenterCamera(_exploredSystemsThisTurn[_whichEmpireFocusedOn][0].X, _exploredSystemsThisTurn[_whichEmpireFocusedOn][0].Y, 1);
					}
					else
					{
						_camera.CenterCamera(_camera.Width / 2, _camera.Height / 2, _camera.MaxZoom);
					}
				}
			}
			if (_colonizableFleetsThisTurn.Count > 0)
			{
				_colonizeScreen.MouseUp(x, y);
			}
			if (_newResearchTopicsNeeded.Count > 0)
			{
				_researchPrompt.MouseUp(x, y);
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (_colonizableFleetsThisTurn.Count > 0)
			{
				_colonizeScreen.KeyDown(e);
			}
		}

		private void OnColonizeComplete()
		{
			//Discard the current one, regardless of it colonized or not
			_colonizableFleetsThisTurn[_whichEmpireFocusedOn].RemoveAt(0);
			if (_colonizableFleetsThisTurn[_whichEmpireFocusedOn].Count > 0)
			{
				_colonizeScreen.LoadFleetAndSystem(_colonizableFleetsThisTurn[_whichEmpireFocusedOn][0]);
				_camera.CenterCamera(_colonizableFleetsThisTurn[_whichEmpireFocusedOn][0].AdjacentSystem.X, _colonizableFleetsThisTurn[_whichEmpireFocusedOn][0].AdjacentSystem.Y, 1);
			}
			else
			{
				_colonizableFleetsThisTurn.Remove(_whichEmpireFocusedOn);
				if (_colonizableFleetsThisTurn.Count > 0) //Hot seat humans
				{
					foreach (var keyPair in _colonizableFleetsThisTurn)
					{
						_whichEmpireFocusedOn = keyPair.Key;
						break;
					}

					_colonizeScreen.LoadFleetAndSystem(_colonizableFleetsThisTurn[_whichEmpireFocusedOn][0]);
					_camera.CenterCamera(_colonizableFleetsThisTurn[_whichEmpireFocusedOn][0].AdjacentSystem.X, _colonizableFleetsThisTurn[_whichEmpireFocusedOn][0].AdjacentSystem.Y, 1);
				}
				else
				{
					_camera.CenterCamera(_camera.Width / 2, _camera.Height / 2, _camera.MaxZoom);
					_colonizeScreen.Completed = null;
					_updateStep++;
				}
			}
		}

		private void OnResearchPromptComplete()
		{
			_newResearchTopicsNeeded.Remove(_whichEmpireFocusedOn);
			if (_newResearchTopicsNeeded.Count > 0)
			{
				foreach (var keyPair in _newResearchTopicsNeeded)
				{
					_whichEmpireFocusedOn = keyPair.Key;
					break;
				}

				_researchPrompt.LoadEmpire(_whichEmpireFocusedOn, _newResearchTopicsNeeded[_whichEmpireFocusedOn]);
			}
			else
			{
				_researchPrompt.Completed = null;
				_updateStep++;
			}
		}

		public void ResetCamera()
		{
			_camera = new Camera(_gameMain.Galaxy.GalaxySize * 60, _gameMain.Galaxy.GalaxySize * 60, _gameMain.ScreenWidth, _gameMain.ScreenHeight);
			_camera.CenterCamera(_camera.Width / 2, _camera.Height / 2, _camera.MaxZoom);
		}
	}
}
