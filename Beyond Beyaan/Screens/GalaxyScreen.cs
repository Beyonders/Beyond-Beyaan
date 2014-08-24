using System;
using System.Collections.Generic;
using System.Drawing;
using GorgonLibrary.Graphics;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class GalaxyScreen : ScreenInterface
	{
		private GameMain _gameMain;
		private Camera _camera;

		private RenderTarget _oldTarget;
		private RenderImage _starName;
		private RenderImage _backBuffer;
		private SystemView _systemView;
		private FleetView _fleetView;

		private TaskBar _taskBar;
		private InGameMenu _inGameMenu;
		private ResearchScreen _researchScreen;
		private ShipDesignScreen _shipDesignScreen;
		private PlanetsView _planetsView;
		private FleetListScreen _fleetListScreen;

		private WindowInterface _windowShowing;

		//private int maxVisible;
		private BBSprite _pathSprite;
		private BBSprite _fuelCircle;
		private BBSprite[] _selectionSprites;
		private BBLabel _travelETA;
		private BBLabel _tentativeETA;

		private bool _showingFuelRange;
		private bool _showingRadarRange;
		private bool _showingOwners;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;
			_pathSprite = SpriteManager.GetSprite("Path", _gameMain.Random);
			_fuelCircle = SpriteManager.GetSprite("FuelCircle", _gameMain.Random);
			_selectionSprites = new BBSprite[4];
			_selectionSprites[0] = SpriteManager.GetSprite("SelectionTL", _gameMain.Random);
			_selectionSprites[1] = SpriteManager.GetSprite("SelectionTR", _gameMain.Random);
			_selectionSprites[2] = SpriteManager.GetSprite("SelectionBL", _gameMain.Random);
			_selectionSprites[3] = SpriteManager.GetSprite("SelectionBR", _gameMain.Random);
			_showingFuelRange = false;
			_showingRadarRange = false;
			_showingOwners = false;

			_camera = new Camera(_gameMain.Galaxy.GalaxySize * 60, _gameMain.Galaxy.GalaxySize * 60, _gameMain.ScreenWidth, _gameMain.ScreenHeight);

			_starName = new RenderImage("starNameRendered", 1, 1, ImageBufferFormats.BufferRGB888A8);
			_starName.BlendingMode = BlendingModes.Modulated;

			_backBuffer = new RenderImage("galaxyBackBuffer", _gameMain.ScreenWidth, _gameMain.ScreenHeight, ImageBufferFormats.BufferRGB888A8);
			_backBuffer.BlendingMode = BlendingModes.Modulated;

			_systemView = new SystemView();
			if (!_systemView.Initialize(_gameMain, "GalaxyScreen", out reason))
			{
				return false;
			}
			_fleetView = new FleetView();
			if (!_fleetView.Initialize(_gameMain, out reason))
			{
				return false;
			}

			_taskBar = new TaskBar();
			if (!_taskBar.Initialize(_gameMain, out reason))
			{
				return false;
			}
			_inGameMenu = new InGameMenu();
			_researchScreen = new ResearchScreen();
			_shipDesignScreen = new ShipDesignScreen();
			_planetsView = new PlanetsView();
			_fleetListScreen = new FleetListScreen();
			if (!_inGameMenu.Initialize(_gameMain, out reason))
			{
				return false;
			}
			if (!_researchScreen.Initialize(_gameMain, out reason))
			{
				return false;
			}
			if (!_shipDesignScreen.Initialize(_gameMain, out reason))
			{
				return false;
			}
			if (!_planetsView.Initialize(_gameMain, out reason))
			{
				return false;
			}
			if (!_fleetListScreen.Initialize(_gameMain, out reason))
			{
				return false;
			}
			_inGameMenu.CloseWindow = CloseWindow;
			_researchScreen.CloseWindow = CloseWindow;
			_shipDesignScreen.CloseWindow = CloseWindow;
			_planetsView.CloseWindow = CloseWindow;
			_planetsView.CenterToSystem = CenterToSystem;
			_fleetListScreen.SelectFleet = SelectFleet;

			_taskBar.ShowGameMenu = ShowInGameMenu;
			_taskBar.ShowResearchScreen = ShowResearchScreen;
			_taskBar.ShowShipDesignScreen = ShowShipDesignScreen;
			_taskBar.ShowPlanetsScreen = ShowPlanetsView;
			_taskBar.ShowFleetOverviewScreen = ShowFleetListScreen;
			_taskBar.EndTurn = CloseWindow;

			_travelETA = new BBLabel();
			_tentativeETA = new BBLabel();

			if (!_travelETA.Initialize(0, 0, "ETA", Color.White, out reason))
			{
				return false;
			}
			if (!_tentativeETA.Initialize(0, 0, "ETA", Color.White, out reason))
			{
				return false;
			}

			reason = null;
			return true;
		}

		private void CenterToSystem(StarSystem starSystem)
		{
			_gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem = starSystem;
			CloseWindow();
		}

		private void SelectFleet(Fleet fleet)
		{
			CloseWindow();
			CenterScreen(fleet);
		}

		public void CenterScreen()
		{
			if (_gameMain.EmpireManager.CurrentEmpire != null && _gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem != null)
			{
				_gameMain.EmpireManager.CurrentEmpire.SelectedSystem = _gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem;
				_camera.CenterCamera(_gameMain.EmpireManager.CurrentEmpire.SelectedSystem.X, _gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Y, _camera.ZoomDistance);
				_systemView.LoadSystem();
			}
		}

		public void CenterScreen(Fleet fleet)
		{
			_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = new FleetGroup(new List<Fleet> { fleet });
			_gameMain.EmpireManager.CurrentEmpire.SelectedSystem = null;
			_camera.CenterCamera((int)fleet.GalaxyX, (int)fleet.GalaxyY, _camera.ZoomDistance);
			_fleetView.LoadFleetGroup(_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup);
		}

		private void DrawETA(TravelNode node, bool isTentative)
		{
			if (isTentative)
			{
				_tentativeETA.MoveTo((int)((node.StarSystem.X - _camera.CameraX) * _camera.ZoomDistance), (int)((node.StarSystem.Y - _camera.CameraY - 32) * _camera.ZoomDistance));
				if (node.Angle > 180)
				{
					_tentativeETA.SetAlignment(true);
				}
				else
				{
					_tentativeETA.SetAlignment(false);
				}
				_tentativeETA.Draw();
			}
			else
			{
				_travelETA.MoveTo((int)((node.StarSystem.X - _camera.CameraX) * _camera.ZoomDistance), (int)((node.StarSystem.Y - _camera.CameraY - 32) * _camera.ZoomDistance));
				if (node.Angle > 180)
				{
					_travelETA.SetAlignment(true);
				}
				else
				{
					_travelETA.SetAlignment(false);
				}
				_travelETA.Draw();
			}
		}

		//Used when other non-combat screens are open, to fill in the blank areas
		public void DrawGalaxy()
		{
			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			FleetGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;
			List<StarSystem> systems = _gameMain.Galaxy.GetAllStars();
			bool displayName = _camera.ZoomDistance > 0.8f;

			if (_showingFuelRange)
			{
				// TODO: Optimize this by going through an empire's owned systems, instead of all stars
				float scale = (currentEmpire.TechnologyManager.FuelRange / 3.0f) * _camera.ZoomDistance;
				float extendedScale = ((currentEmpire.TechnologyManager.FuelRange + 3) / 3.0f) * _camera.ZoomDistance;
				_backBuffer.Clear(Color.Black);
				_oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
				GorgonLibrary.Gorgon.CurrentRenderTarget = _backBuffer;
				List<StarSystem> ownedSystems = new List<StarSystem>();
				foreach (StarSystem system in systems)
				{
					if (system.Planets[0].Owner == currentEmpire)
					{
						_fuelCircle.Draw((system.X - _camera.CameraX) * _camera.ZoomDistance, (system.Y - _camera.CameraY) * _camera.ZoomDistance, extendedScale, extendedScale, Color.LimeGreen);
						ownedSystems.Add(system);
					}
				}
				GorgonLibrary.Gorgon.CurrentRenderTarget = _oldTarget;
				_backBuffer.Blit(0, 0, _gameMain.ScreenWidth, _gameMain.ScreenHeight, Color.FromArgb(75, Color.White), BlitterSizeMode.Crop);
				_backBuffer.Clear(Color.Black);
				GorgonLibrary.Gorgon.CurrentRenderTarget = _backBuffer;
				foreach (StarSystem system in ownedSystems)
				{
					_fuelCircle.Draw((system.X - _camera.CameraX) * _camera.ZoomDistance, (system.Y - _camera.CameraY) * _camera.ZoomDistance, scale, scale, Color.LimeGreen);
				}
				GorgonLibrary.Gorgon.CurrentRenderTarget = _oldTarget;
				_backBuffer.Blit(0, 0, _gameMain.ScreenWidth, _gameMain.ScreenHeight, Color.FromArgb(75, Color.White), BlitterSizeMode.Crop);
			}
			else if (_showingRadarRange)
			{
				// TODO: Optimize this by going through an empire's owned systems, instead of all stars
				float planetScale = (currentEmpire.TechnologyManager.PlanetRadarRange / 3.0f) * _camera.ZoomDistance;
				float fleetScale = (currentEmpire.TechnologyManager.FleetRadarRange / 3.0f) * _camera.ZoomDistance;
				_backBuffer.Clear(Color.Black);
				_oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
				GorgonLibrary.Gorgon.CurrentRenderTarget = _backBuffer;
				foreach (StarSystem system in systems)
				{
					if (system.Planets[0].Owner == currentEmpire)
					{
						_fuelCircle.Draw((system.X - _camera.CameraX) * _camera.ZoomDistance, (system.Y - _camera.CameraY) * _camera.ZoomDistance, planetScale, planetScale, Color.Tomato);
					}
				}
				if (currentEmpire.TechnologyManager.FleetRadarRange > 0)
				{
					foreach (var fleet in currentEmpire.FleetManager.GetFleets())
					{
						_fuelCircle.Draw((fleet.GalaxyX - _camera.CameraX) * _camera.ZoomDistance, (fleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance, fleetScale, fleetScale, Color.Tomato);
					}
				}
				GorgonLibrary.Gorgon.CurrentRenderTarget = _oldTarget;
				_backBuffer.Blit(0, 0, _gameMain.ScreenWidth, _gameMain.ScreenHeight, Color.FromArgb(75, Color.White), BlitterSizeMode.Crop);
			}

			if (selectedSystem != null)
			{
				if (selectedSystem.Planets[0].TransferSystem.Key.StarSystem != selectedSystem)
				{
					_pathSprite.Draw((selectedSystem.X - _camera.CameraX) * _camera.ZoomDistance, (selectedSystem.Y - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (selectedSystem.Planets[0].TransferSystem.Key.Length / _pathSprite.Width), _camera.ZoomDistance, Color.Green, selectedSystem.Planets[0].TransferSystem.Key.Angle);
				}
				if (selectedSystem.Planets[0].RelocateToSystem != null)
				{
					_pathSprite.Draw((selectedSystem.X - _camera.CameraX) * _camera.ZoomDistance, (selectedSystem.Y - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (selectedSystem.Planets[0].RelocateToSystem.Length / _pathSprite.Width), _camera.ZoomDistance, Color.Blue, selectedSystem.Planets[0].RelocateToSystem.Angle);
				}
				if (_systemView.IsTransferring && _systemView.TransferSystem != null)
				{
					_pathSprite.Draw((selectedSystem.X - _camera.CameraX) * _camera.ZoomDistance, (selectedSystem.Y - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (_systemView.TransferSystem.Length / _pathSprite.Width), _camera.ZoomDistance, _systemView.TransferSystem.IsValid ? Color.LightGreen : Color.Red, _systemView.TransferSystem.Angle);
				}
				if (_systemView.IsRelocating && _systemView.RelocateSystem != null)
				{
					_pathSprite.Draw((selectedSystem.X - _camera.CameraX) * _camera.ZoomDistance, (selectedSystem.Y - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (_systemView.RelocateSystem.Length / _pathSprite.Width), _camera.ZoomDistance, _systemView.RelocateSystem.IsValid ? Color.LightSkyBlue : Color.Red, _systemView.RelocateSystem.Angle);
				}
			}

			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
				if (_showingOwners)
				{
					var darkGray = new [] {0.25f, 0.25f, 0.25f, 1};
					if (system.IsThisSystemExploredByEmpire(currentEmpire))
					{
						_gameMain.StarShader.Parameters["StarColor"].SetValue(system.Planets[0].Owner == null
							                                                      ? darkGray
							                                                      : system.Planets[0].Owner.ConvertedColor);
					}
					else
					{
						_gameMain.StarShader.Parameters["StarColor"].SetValue(darkGray);
					}
				}
				else
				{
					_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				}
				system.Sprite.Draw((int)((system.X - _camera.CameraX) * _camera.ZoomDistance), (int)((system.Y - _camera.CameraY) * _camera.ZoomDistance), _camera.ZoomDistance, _camera.ZoomDistance);
				GorgonLibrary.Gorgon.CurrentShader = null;

				if (system == currentEmpire.SelectedSystem)
				{
					_selectionSprites[0].Draw(((system.X - 16) - _camera.CameraX) * _camera.ZoomDistance, ((system.Y - 16) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
					_selectionSprites[1].Draw(((system.X) - _camera.CameraX) * _camera.ZoomDistance, ((system.Y - 16) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
					_selectionSprites[2].Draw(((system.X - 16) - _camera.CameraX) * _camera.ZoomDistance, ((system.Y) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
					_selectionSprites[3].Draw(((system.X) - _camera.CameraX) * _camera.ZoomDistance, ((system.Y) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
				}

				if (displayName && (_gameMain.EmpireManager.CurrentEmpire.ContactManager.IsContacted(system.DominantEmpire) || system.IsThisSystemExploredByEmpire(_gameMain.EmpireManager.CurrentEmpire)))
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
							_starName.Blit(x, y, _starName.Width * percentage, _starName.Height, empire.EmpireColor, GorgonLibrary.Graphics.BlitterSizeMode.Crop);
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

			if (selectedFleetGroup != null && selectedFleetGroup.SelectedFleet.TravelNodes != null)
			{
				if (selectedFleetGroup.SelectedFleet.Empire == currentEmpire)
				{
					for (int i = 0; i < selectedFleetGroup.SelectedFleet.TravelNodes.Count; i++)
					{
						if (i == 0)
						{
							var travelNode = selectedFleetGroup.FleetToSplit.TravelNodes[0];
							if (selectedFleetGroup.SelectedFleet.AdjacentSystem != null)
							{
								//Haven't left yet, so calculate custom path from left side of star
								float x = travelNode.StarSystem.X - (selectedFleetGroup.FleetToSplit.GalaxyX - 32);
								float y = travelNode.StarSystem.Y - selectedFleetGroup.FleetToSplit.GalaxyY;
								float length = (float)Math.Sqrt((x * x) + (y * y));
								float angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
								_pathSprite.Draw((selectedFleetGroup.SelectedFleet.GalaxyX - _camera.CameraX - 32) * _camera.ZoomDistance, (selectedFleetGroup.SelectedFleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (length / _pathSprite.Width), _camera.ZoomDistance, Color.Green, angle);
							}
							else
							{
								_pathSprite.Draw((selectedFleetGroup.SelectedFleet.GalaxyX - _camera.CameraX) * _camera.ZoomDistance, (selectedFleetGroup.SelectedFleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (travelNode.Length / _pathSprite.Width), _camera.ZoomDistance, Color.Green, travelNode.Angle);
							}
							if (selectedFleetGroup.SelectedFleet.TravelNodes.Count == 1)
							{
								//Only one node, so draw ETA at this star
								DrawETA(travelNode, false);
							}
						}
						else
						{
							_pathSprite.Draw((selectedFleetGroup.SelectedFleet.TravelNodes[i - 1].StarSystem.X - _camera.CameraX) * _camera.ZoomDistance, (selectedFleetGroup.SelectedFleet.TravelNodes[i - 1].StarSystem.Y - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (selectedFleetGroup.SelectedFleet.TravelNodes[i].Length / _pathSprite.Width), _camera.ZoomDistance, Color.Green, selectedFleetGroup.SelectedFleet.TravelNodes[i].Angle);
							if (i == selectedFleetGroup.SelectedFleet.TravelNodes.Count - 1)
							{
								//Last node, so draw ETA here
								DrawETA(selectedFleetGroup.FleetToSplit.TravelNodes[i], false);
							}
						}
					}
				}
				else if (currentEmpire.TechnologyManager.ShowEnemyETA && selectedFleetGroup.SelectedFleet.TravelNodes.Count > 0)
				{
					_pathSprite.Draw((selectedFleetGroup.SelectedFleet.GalaxyX - _camera.CameraX) * _camera.ZoomDistance, (selectedFleetGroup.SelectedFleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (selectedFleetGroup.SelectedFleet.TravelNodes[0].Length / _pathSprite.Width), _camera.ZoomDistance, Color.Red, selectedFleetGroup.SelectedFleet.TravelNodes[0].Angle);
					//Player do not know if there's other stops, so draw ETA for first one
					DrawETA(selectedFleetGroup.FleetToSplit.TravelNodes[0], false);
				}
			}

			if (selectedFleetGroup != null && selectedFleetGroup.FleetToSplit.TentativeNodes != null)
			{
				for (int i = 0; i < selectedFleetGroup.FleetToSplit.TentativeNodes.Count; i++)
				{
					if (i == 0)
					{
						var travelNode = selectedFleetGroup.FleetToSplit.TentativeNodes[0];
						if (selectedFleetGroup.FleetToSplit.AdjacentSystem != null && selectedFleetGroup.FleetToSplit.TravelNodes != null)
						{
							//Haven't left yet, so calculate custom path from left side of star
							float x = travelNode.StarSystem.X - (selectedFleetGroup.FleetToSplit.GalaxyX - 32);
							float y = travelNode.StarSystem.Y - selectedFleetGroup.FleetToSplit.GalaxyY;
							float length = (float)Math.Sqrt((x * x) + (y * y));
							float angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
							_pathSprite.Draw((selectedFleetGroup.FleetToSplit.GalaxyX - _camera.CameraX - 32) * _camera.ZoomDistance, (selectedFleetGroup.FleetToSplit.GalaxyY - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (length / _pathSprite.Width), _camera.ZoomDistance, travelNode.IsValid ? Color.LightGreen : Color.Red, angle);
						}
						else if (selectedFleetGroup.FleetToSplit.AdjacentSystem != null)
						{
							//Haven't left, and not on enroute already, so calculate path from right side of star
							float x = travelNode.StarSystem.X - (selectedFleetGroup.FleetToSplit.GalaxyX + 32);
							float y = travelNode.StarSystem.Y - selectedFleetGroup.FleetToSplit.GalaxyY;
							float length = (float)Math.Sqrt((x * x) + (y * y));
							float angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
							_pathSprite.Draw((selectedFleetGroup.FleetToSplit.GalaxyX - _camera.CameraX + 32) * _camera.ZoomDistance, (selectedFleetGroup.FleetToSplit.GalaxyY - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (length / _pathSprite.Width), _camera.ZoomDistance, travelNode.IsValid ? Color.LightGreen : Color.Red, angle);
						}
						else
						{
							_pathSprite.Draw((selectedFleetGroup.FleetToSplit.GalaxyX - _camera.CameraX) * _camera.ZoomDistance, (selectedFleetGroup.FleetToSplit.GalaxyY - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (selectedFleetGroup.FleetToSplit.TentativeNodes[0].Length / _pathSprite.Width), _camera.ZoomDistance, Color.LightGreen, selectedFleetGroup.FleetToSplit.TentativeNodes[0].Angle);
						}
						if (selectedFleetGroup.FleetToSplit.TentativeNodes.Count == 1)
						{
							//Only one node, so draw ETA at this star
							DrawETA(travelNode, true);
						}
					}
					else
					{
						_pathSprite.Draw((selectedFleetGroup.FleetToSplit.TentativeNodes[i - 1].StarSystem.X - _camera.CameraX) * _camera.ZoomDistance, (selectedFleetGroup.FleetToSplit.TentativeNodes[i - 1].StarSystem.Y - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance * (selectedFleetGroup.FleetToSplit.TentativeNodes[i].Length / _pathSprite.Width), _camera.ZoomDistance, selectedFleetGroup.FleetToSplit.TentativeNodes[i].IsValid ? Color.LightGreen : Color.Red, selectedFleetGroup.FleetToSplit.TentativeNodes[i].Angle);
						if (i == selectedFleetGroup.FleetToSplit.TentativeNodes.Count - 1)
						{
							//Last node, so draw ETA here
							DrawETA(selectedFleetGroup.FleetToSplit.TentativeNodes[i], true);
						}
					}
				}
			}

			foreach (Fleet fleet in _gameMain.EmpireManager.GetFleetsWithinArea(_camera.CameraX, _camera.CameraY, _gameMain.ScreenWidth / _camera.ZoomDistance, _gameMain.ScreenHeight / _camera.ZoomDistance))
			{
				BBSprite fleetIcon = fleet.Ships.Count > 0 ? fleet.Empire.EmpireRace.FleetIcon : fleet.Empire.EmpireRace.TransportIcon;
				if (fleet.AdjacentSystem != null)
				{
					if (fleet.TravelNodes != null && fleet.TravelNodes.Count > 0)
					{
						//Adjacent to a system, but is heading to another system
						if (selectedFleetGroup != null && selectedFleetGroup.Fleets.Contains(fleet))
						{
							_selectionSprites[0].Draw(((fleet.GalaxyX - 48) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY - 16) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
							_selectionSprites[1].Draw(((fleet.GalaxyX - 32) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY - 16) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
							_selectionSprites[2].Draw(((fleet.GalaxyX - 48) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
							_selectionSprites[3].Draw(((fleet.GalaxyX - 32) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
						}
						fleetIcon.Draw((int)(((fleet.GalaxyX - 32) - _camera.CameraX) * _camera.ZoomDistance), (int)((fleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance), _camera.ZoomDistance, _camera.ZoomDistance, fleet.Empire.EmpireColor);
					}
					else
					{
						//Adjacent to a system, just chilling
						if (selectedFleetGroup != null && selectedFleetGroup.Fleets.Contains(fleet))
						{
							_selectionSprites[0].Draw(((fleet.GalaxyX + 16) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY - 16) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
							_selectionSprites[1].Draw(((fleet.GalaxyX + 32) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY - 16) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
							_selectionSprites[2].Draw(((fleet.GalaxyX + 16) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
							_selectionSprites[3].Draw(((fleet.GalaxyX + 32) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
						}
						fleetIcon.Draw((int)(((fleet.GalaxyX + 32) - _camera.CameraX) * _camera.ZoomDistance), (int)((fleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance), _camera.ZoomDistance, _camera.ZoomDistance, fleet.Empire.EmpireColor);
					}
				}
				else
				{
					//Fleet is enroute, no moving of icon needed
					if (selectedFleetGroup != null && selectedFleetGroup.Fleets.Contains(fleet))
					{
						_selectionSprites[0].Draw(((fleet.GalaxyX - 16) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY - 16) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
						_selectionSprites[1].Draw(((fleet.GalaxyX) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY - 16) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
						_selectionSprites[2].Draw(((fleet.GalaxyX - 16) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
						_selectionSprites[3].Draw(((fleet.GalaxyX) - _camera.CameraX) * _camera.ZoomDistance, ((fleet.GalaxyY) - _camera.CameraY) * _camera.ZoomDistance, _camera.ZoomDistance, _camera.ZoomDistance, Color.Green);
					}
					fleetIcon.Draw((int)((fleet.GalaxyX - _camera.CameraX) * _camera.ZoomDistance), (int)((fleet.GalaxyY - _camera.CameraY) * _camera.ZoomDistance), _camera.ZoomDistance, _camera.ZoomDistance, fleet.Empire.EmpireColor);
				}
			}
		}

		public void DrawScreen()
		{
			DrawGalaxy();

			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			FleetGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;

			_taskBar.Draw();

			if (selectedFleetGroup != null)
			{
				_fleetView.Draw();
			}
			if (selectedSystem != null)
			{
				_systemView.Draw();
			}
			if (_windowShowing != null)
			{
				_windowShowing.Draw();
			}
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			_pathSprite.Update(frameDeltaTime, _gameMain.Random);
			_gameMain.Galaxy.Update(frameDeltaTime, _gameMain.Random);

			if (_taskBar.MouseHover(x, y, frameDeltaTime))
			{
				return;
			}
			if (_windowShowing != null)
			{
				_windowShowing.MouseHover(x, y, frameDeltaTime);
				return;
			}

			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			if (currentEmpire.SelectedSystem != null)
			{
				if (_systemView.MouseHover(x, y, frameDeltaTime))
				{
					return;
				}
			}
			if (currentEmpire.SelectedFleetGroup != null)
			{
				if (_fleetView.MouseHover(x, y, frameDeltaTime))
				{
					return;
				}
				if (currentEmpire.SelectedFleetGroup.SelectedFleet.Empire == currentEmpire)
				{
					Point hoveringPoint = new Point();

					hoveringPoint.X = (int)((x / _camera.ZoomDistance) + _camera.CameraX);
					hoveringPoint.Y = (int)((y / _camera.ZoomDistance) + _camera.CameraY);

					StarSystem selectedSystem = _gameMain.Galaxy.GetStarAtPoint(hoveringPoint);
					currentEmpire.SelectedFleetGroup.FleetToSplit.SetTentativePath(selectedSystem, currentEmpire.SelectedFleetGroup.FleetToSplit.HasReserveTanks, _gameMain.Galaxy);
					RefreshETAText();
				}
			}
			
			_camera.HandleUpdate(frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (whichButton == 1)
			{
				if (_taskBar.MouseDown(x, y, whichButton))
				{
					return;
				}
				if (_windowShowing != null)
				{
					_windowShowing.MouseDown(x, y);
					return;
				}
				if (_gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null)
				{
					if (_systemView.MouseDown(x, y))
					{
						return;
					}
				}
				else if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					if (_fleetView.MouseDown(x, y))
					{
						return;
					}
				}
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (whichButton == 1)
			{
				var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
				if (_taskBar.MouseUp(x, y, whichButton))
				{
					//clear the fleet/planet window
					currentEmpire.SelectedSystem = null;
					currentEmpire.SelectedFleetGroup = null;
					return;
				}
				if (_windowShowing != null)
				{
					_windowShowing.MouseUp(x, y);
					return;
				}
				if (currentEmpire.SelectedSystem != null)
				{
					if (_systemView.MouseUp(x, y))
					{
						return;
					}
					if (_systemView.IsTransferring)
					{
						Point point = new Point();

						point.X = (int)((x / _camera.ZoomDistance) + _camera.CameraX);
						point.Y = (int)((y / _camera.ZoomDistance) + _camera.CameraY);

						StarSystem system = _gameMain.Galaxy.GetStarAtPoint(point);
						if (system != null)
						{
							var path = _gameMain.Galaxy.GetPath(currentEmpire.SelectedSystem.X, currentEmpire.SelectedSystem.Y, null, system, false, currentEmpire);
							if (path.Count > 0)
							{
								if (!path[0].StarSystem.IsThisSystemExploredByEmpire(currentEmpire) || path[0].StarSystem.Planets[0].Owner == null)
								{
									path[0].IsValid = false;
								}
								_systemView.TransferSystem = path[0];
							}
						}
						else
						{
							//Clicked to clear the option
							_systemView.IsTransferring = false;
							_systemView.TransferSystem = null;
						}
						return;
					}
					if (_systemView.IsRelocating)
					{
						Point point = new Point();

						point.X = (int)((x / _camera.ZoomDistance) + _camera.CameraX);
						point.Y = (int)((y / _camera.ZoomDistance) + _camera.CameraY);

						StarSystem system = _gameMain.Galaxy.GetStarAtPoint(point);
						if (system != null)
						{
							var path = _gameMain.Galaxy.GetPath(currentEmpire.SelectedSystem.X, currentEmpire.SelectedSystem.Y, null, system, false, currentEmpire);
							if (path.Count > 0)
							{
								if (path[0].StarSystem.Planets[0].Owner != currentEmpire)
								{
									path[0].IsValid = false;
								}
								_systemView.RelocateSystem = path[0];
							}
						}
						else
						{
							//Clicked to clear the option
							_systemView.IsRelocating = false;
							_systemView.RelocateSystem = null;
						}
						return;
					}
				}
				if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					if (_fleetView.MouseUp(x, y))
					{
						RefreshETAText();
						return;
					}
				}
				//If a window is open, but the player didn't click on another system or fleet, the action is to close the current window
				bool clearingUI = _gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null || _gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null;
				Point pointClicked = new Point();

				pointClicked.X = (int)((x / _camera.ZoomDistance) + _camera.CameraX);
				pointClicked.Y = (int)((y / _camera.ZoomDistance) + _camera.CameraY);

				StarSystem selectedSystem = _gameMain.Galaxy.GetStarAtPoint(pointClicked);
				if (selectedSystem != null && selectedSystem == _gameMain.EmpireManager.CurrentEmpire.SelectedSystem)
				{
					return;
				}
				_gameMain.EmpireManager.CurrentEmpire.SelectedSystem = selectedSystem;

				if (selectedSystem != null)
				{
					_gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem = selectedSystem;
					_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = null;
					_systemView.LoadSystem();
					return;
				}

				FleetGroup selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint(pointClicked.X, pointClicked.Y);
				_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = selectedFleetGroup;

				if (selectedFleetGroup != null)
				{
					_fleetView.LoadFleetGroup(selectedFleetGroup);
					RefreshETAText();
					return;
				}
				if (!clearingUI)
				{
					_camera.ScrollToPosition(pointClicked.X, pointClicked.Y);
				}
			}
			else if (whichButton == 2)
			{
				if (_windowShowing != null)
				{
					return;
				}
				var selectedFleetGroup = _gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup;
				if (selectedFleetGroup != null && selectedFleetGroup.FleetToSplit.Empire == _gameMain.EmpireManager.CurrentEmpire)
				{
					bool isIdling = selectedFleetGroup.SelectedFleet.AdjacentSystem != null;
					bool hasDestination = selectedFleetGroup.SelectedFleet.TravelNodes != null;
					if (selectedFleetGroup.FleetToSplit.ConfirmPath())
					{
						_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.SplitFleet(_gameMain.EmpireManager.CurrentEmpire);
						_gameMain.EmpireManager.CurrentEmpire.FleetManager.MergeIdleFleets();
						if (isIdling && !hasDestination) //Select the remaining idling ships on right of star
						{
							Point point = new Point((int)selectedFleetGroup.SelectedFleet.GalaxyX + 32, (int)selectedFleetGroup.SelectedFleet.GalaxyY);
							selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint(point.X, point.Y);
							if (selectedFleetGroup == null) // No ships left, select the fleet on left of star
							{
								selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint(point.X - 64, point.Y);
							}
						}
						else if (isIdling)
						{
							if (selectedFleetGroup.SelectedFleet.TravelNodes == null) //cleared out movement order
							{
								selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint((int)selectedFleetGroup.SelectedFleet.GalaxyX + 32, (int)selectedFleetGroup.SelectedFleet.GalaxyY);
							}
							else
							{
								selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint((int)selectedFleetGroup.SelectedFleet.GalaxyX - 32, (int)selectedFleetGroup.SelectedFleet.GalaxyY);
							}
						}
						else
						{
							selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint((int)selectedFleetGroup.SelectedFleet.GalaxyX, (int)selectedFleetGroup.SelectedFleet.GalaxyY);
						}
						_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = selectedFleetGroup;
						_fleetView.LoadFleetGroup(selectedFleetGroup);
					}
				}
			}
		}

		private void RefreshETAText()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;

			//Update the ETAs
			if (currentEmpire.SelectedFleetGroup.SelectedFleet.Empire == currentEmpire)
			{
				if (currentEmpire.SelectedFleetGroup.FleetToSplit.TentativeNodes != null)
				{
					if (
						currentEmpire.SelectedFleetGroup.FleetToSplit.TentativeNodes[
							currentEmpire.SelectedFleetGroup.FleetToSplit.TentativeNodes.Count - 1].IsValid)
					{
						_tentativeETA.SetText("ETA: " + currentEmpire.SelectedFleetGroup.FleetToSplit.TentativeETA);
					}
					else
					{
						_tentativeETA.SetText("Out of range");
					}
				}

				if (currentEmpire.SelectedFleetGroup.SelectedFleet.Empire == currentEmpire)
				{
					_travelETA.SetText("ETA: " + currentEmpire.SelectedFleetGroup.SelectedFleet.TravelETA);
				}
			}
			else
			{
				_travelETA.SetText("ETA: " + currentEmpire.SelectedFleetGroup.SelectedFleet.TravelToFirstNodeETA);
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
			if (_windowShowing != null)
			{
				return;
			}
			_camera.MouseWheel(direction, x, y);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (_windowShowing != null)
			{
				if (!_windowShowing.KeyDown(e))
				{
					if (e.Key == KeyboardKeys.Escape)
					{
						//Parent window didn't handle escape, so close window
						//Close the current window
						CloseWindow();
						return;
					}
				}
				return;
			}
			if (e.Key == KeyboardKeys.Escape)
			{
				Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
				StarSystem selectedSystem = currentEmpire.SelectedSystem;
				FleetGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;
				if (selectedFleetGroup != null)
				{
					currentEmpire.SelectedFleetGroup = null;
				}
				if (selectedSystem != null)
				{
					currentEmpire.SelectedSystem = null;
				}
				return;
			}
			if (_gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null)
			{
				if (_systemView.KeyDown(e))
				{
					return;
				}
			}
			if (e.Key == KeyboardKeys.F)
			{
				_showingRadarRange = false;
				_showingFuelRange = !_showingFuelRange;
			}
			if (e.Key == KeyboardKeys.R)
			{
				_showingFuelRange = false;
				_showingRadarRange = !_showingRadarRange;
			}
			if (e.Key == KeyboardKeys.B)
			{
				_showingOwners = !_showingOwners; //This does not affect the other two toggable views since it only changes the stars' color
			}
			if (e.Key == KeyboardKeys.Escape)
			{
				_taskBar.SetToScreen(Screen.InGameMenu);
				ShowInGameMenu();
			}
			if (e.Key == KeyboardKeys.Space)
			{
				_gameMain.ToggleSitRep();
			}
		}

		private void CloseWindow()
		{
			_windowShowing = null;
			CenterScreen();
			_taskBar.Clear();
		}

		private void ShowInGameMenu()
		{
			_windowShowing = _inGameMenu;
			_inGameMenu.GetSaveList();
		}

		private void ShowResearchScreen()
		{
			_windowShowing = _researchScreen;
			_researchScreen.Load();
		}

		private void ShowShipDesignScreen()
		{
			_windowShowing = _shipDesignScreen;
			_shipDesignScreen.Load();
		}

		private void ShowPlanetsView()
		{
			_windowShowing = _planetsView;
			_planetsView.Load();
		}

		private void ShowFleetListScreen()
		{
			_windowShowing = _fleetListScreen;
			_fleetListScreen.LoadScreen();
		}

		public void ResetCamera()
		{
			_camera = new Camera(_gameMain.Galaxy.GalaxySize * 60, _gameMain.Galaxy.GalaxySize * 60, _gameMain.ScreenWidth, _gameMain.ScreenHeight);
		}
	}
}
