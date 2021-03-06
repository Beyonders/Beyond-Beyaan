﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	/*class SpaceCombat : ScreenInterface
	{
		const int COMBATSIZE = 200;

		//bool loaded;
		GameMain _gameMain;
		//Camera camera;
		//StarSystem system;
		//List<Fleet> originalFleets;
		//List<CombatFleet> fleetsInCombat;
		private int combatIter;
		//private int whichEmpireTurn;
		//private int selectedShipIter;
		private Button[] actionButtons;
		private List<CombatShip> retreatingShips;
		//private bool processRetreating;
		//private float retreatProcess;
		private CombatShip selectedShip;
		private Label shipNameLabel;
		private Label engineLabel;
		private Label armorLabel;
		private Label shieldLabel;
		private Label computerLabel;
		private Label hitPointsLabel;
		private Button[] weaponButtons;
		private ScrollBar weaponScrollBar;
		private int selectedWeapon;
		private int maxVisible;

		private CombatShip SelectedShip
		{
			get { return selectedShip; }
			set
			{
				selectedShip = value;
				shipNameLabel.SetText(selectedShip.Name);
				engineLabel.SetText(selectedShip.Engine.TechName);
				armorLabel.SetText(selectedShip.Armor.TechName);
				shieldLabel.SetText(selectedShip.Shield.TechName);
				computerLabel.SetText(selectedShip.Computer.TechName);
				if (selectedShip.Weapons.Count > 6)
				{
					maxVisible = 6;
					weaponScrollBar.SetEnabledState(true);
					weaponScrollBar.SetAmountOfItems(selectedShip.Weapons.Count);
				}
				else
				{
					maxVisible = selectedShip.Weapons.Count;
					weaponScrollBar.SetEnabledState(false);
					weaponScrollBar.SetAmountOfItems(6);
				}
				for (int i = 0; i < maxVisible; i++)
				{
					string text = selectedShip.Weapons[i].TechName; //+ " x " + selectedShip.Weapons[i].Mounts;
					weaponButtons[i].SetText(text);
				}
				selectedWeapon = 0;
				foreach (Button button in weaponButtons)
				{
					button.Selected = false;
				}
				weaponButtons[selectedWeapon].Selected = true;
				weaponScrollBar.TopIndex = 0;
				//hitPointsLabel.SetText("HP: " + string.Format("{0}", selectedShip.CurrentHitPoints) + "/" + selectedShip.Armor.GetHP(selectedShip.TotalSpace));
			}
		}

		private int x;
		private int y;

		public void Initialize(GameMain _gameMain)
		{
			this._gameMain = _gameMain;
			//camera = new Camera(this._gameMain.ScreenWidth, this._gameMain.ScreenHeight);
			retreatingShips = new List<CombatShip>();

			x = (_gameMain.ScreenWidth / 2) - 140;
			y = _gameMain.ScreenHeight - 40;
			actionButtons = new Button[7];
			actionButtons[0] = new Button(SpriteName.AutoBackground, SpriteName.AutoForeground, string.Empty, x, y, 40, 40);
			actionButtons[1] = new Button(SpriteName.RetreatBackground, SpriteName.RetreatForeground, string.Empty, x + 40, y, 40, 40);
			actionButtons[2] = new Button(SpriteName.PrevShipBackground, SpriteName.PrevShipForeground, string.Empty, x + 80, y, 40, 40);
			actionButtons[3] = new Button(SpriteName.DonePrevShipBackground, SpriteName.DonePrevShipForeground, string.Empty, x + 120, y, 40, 40);
			actionButtons[4] = new Button(SpriteName.DoneNextShipBackground, SpriteName.DoneNextShipForeground, string.Empty, x + 160, y, 40, 40);
			actionButtons[5] = new Button(SpriteName.NextShipBackground, SpriteName.NextShipForeground, string.Empty, x + 200, y, 40, 40);
			actionButtons[6] = new Button(SpriteName.EOT, SpriteName.HighlightedEOT, string.Empty, x + 240, y, 40, 40);

			shipNameLabel = new Label(string.Empty, 25, _gameMain.ScreenHeight - 150);
			computerLabel = new Label(string.Empty, 25, _gameMain.ScreenHeight - 125);
			engineLabel = new Label(string.Empty, 25, _gameMain.ScreenHeight - 100);
			shieldLabel = new Label(string.Empty, 25, _gameMain.ScreenHeight - 75);
			armorLabel = new Label(string.Empty, 25, _gameMain.ScreenHeight - 50);
			hitPointsLabel = new Label(string.Empty, 25, _gameMain.ScreenHeight - 25);

			weaponButtons = new Button[6];
			for (int i = 0; i < weaponButtons.Length; i++)
			{
				weaponButtons[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, 150, _gameMain.ScreenHeight - (150 - (i * 25)), 180, 25);
			}

			weaponScrollBar = new ScrollBar(330, _gameMain.ScreenHeight - 150, 16, 118, 6, 6, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton, SpriteName.ScrollDownBackgroundButton,
				SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton, SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
		}

		public void SetupScreen()
		{
			combatIter = 0;
			//whichEmpireTurn = 0;
			SetupBattle(_gameMain.EmpireManager.CombatsToProcess[combatIter].fleetsInCombat, null);
		}

		public void ResetScreen()
		{
			//whichEmpireTurn = 0;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			/*GorgonLibrary.Graphics.Sprite shipSprite;

			foreach (CombatFleet fleet in fleetsInCombat)
			{
				GorgonLibrary.Gorgon.CurrentShader = _gameMain.ShipShader;
				_gameMain.ShipShader.Parameters["EmpireColor"].SetValue(fleet.Empire.ConvertedColor);
				foreach (CombatShip ship in fleet.combatShips)
				{
					shipSprite = fleet.Empire.EmpireRace.GetShip(ship.Size, ship.WhichStyle);
					shipSprite.Axis = new GorgonLibrary.Vector2D(shipSprite.Width / 2, shipSprite.Height / 2);
					shipSprite.Rotation = (float)(ship.AngleFacing / (2 * Math.PI)) * 360;
					shipSprite.SetPosition(((((ship.X - camera.CameraX) * 16) + (ship.XOffset - camera.XOffset) + ship.CenterOffset) * camera.Scale), ((((ship.Y - camera.CameraY) * 16) + (ship.YOffset - camera.YOffset) + ship.CenterOffset) * camera.Scale));
					if (processRetreating && ship == SelectedShip)
					{
						if (ship.Size % 2 == 0)
						{
							shipSprite.SetScale(camera.Scale * retreatProcess, camera.Scale * retreatProcess);
						}
						else
						{
							shipSprite.SetScale(((shipSprite.Width - 16) / shipSprite.Width) * camera.Scale * retreatProcess, ((shipSprite.Height - 16) / shipSprite.Height) * camera.Scale * retreatProcess);
						}
					}
					else
					{
						if (ship.Size % 2 == 0)
						{
							shipSprite.SetScale(camera.Scale, camera.Scale);
						}
						else
						{
							shipSprite.SetScale(((shipSprite.Width - 16) / shipSprite.Width) * camera.Scale, ((shipSprite.Height - 16) / shipSprite.Height) * camera.Scale);
						}
					}
					
					shipSprite.Draw();
					if (ship == SelectedShip && !processRetreating)
					{
						SpriteName name = SpriteName.ShipSelection32;
						switch (ship.Size)
						{
							case 1: 
							case 2: name = SpriteName.ShipSelection32;
								break;
							case 3: 
							case 4: name = SpriteName.ShipSelection64;
								break;
							case 5:
							case 6: name = SpriteName.ShipSelection96;
								break;
							case 7:
							case 8: name = SpriteName.ShipSelection128;
								break;
							case 9:
							case 10: name = SpriteName.ShipSelection160;
								break;
						}
						drawingManagement.SetSpriteScale(name, (ship.Size * 16) * camera.Scale, (ship.Size * 16) * camera.Scale);
						drawingManagement.DrawSprite(name, (int)((((ship.X - camera.CameraX) * 16) + (ship.XOffset - camera.XOffset)) * camera.Scale), (int)((((ship.Y - camera.CameraY) * 16) + (ship.YOffset - camera.YOffset)) * camera.Scale), 255, System.Drawing.Color.White);
					}
				}
				GorgonLibrary.Gorgon.CurrentShader = null;
			}

			if (!processRetreating)
			{
				int empireY = _gameMain.ScreenHeight - (fleetsInCombat.Count * 25);
				for (int i = 0; i < fleetsInCombat.Count; i++)
				{
					int empireX = (int)(_gameMain.ScreenWidth - (fleetsInCombat[i].EmpireNameLabel.GetWidth() + 5));
					fleetsInCombat[i].EmpireNameLabel.MoveTo(empireX, empireY);
					fleetsInCombat[i].EmpireNameLabel.Draw();
					if (i == whichEmpireTurn)
					{
						drawingManagement.DrawSprite(SpriteName.EmpireTurnArrow, empireX - 30, empireY - 4, 255, System.Drawing.Color.White);
					}
					empireY += 25;
				}

				foreach (Button button in actionButtons)
				{
					button.Draw(drawingManagement);
				}

				drawingManagement.DrawSprite(SpriteName.ControlBackground, 0, _gameMain.ScreenHeight - 160, 100, 350, 160, System.Drawing.Color.White);
				shipNameLabel.Draw();
				computerLabel.Draw();
				shieldLabel.Draw();
				hitPointsLabel.Draw();
				armorLabel.Draw();
				engineLabel.Draw();
				for (int i = 0; i < maxVisible; i++)
				{
					weaponButtons[i].Draw(drawingManagement);
				}
				weaponScrollBar.DrawScrollBar(drawingManagement);
			}*/
		/*}

		public void MouseHover(int mouseX, int mouseY, float frameDeltaTime)
		{
			/*if (processRetreating)
			{
				if (retreatProcess <= 0) //This ship has finished retreating
				{
					foreach (CombatFleet fleet in fleetsInCombat)
					{
						if (fleet.combatShips.Contains(retreatingShips[0]))
						{
							fleet.combatShips.Remove(retreatingShips[0]);
							break;
						}
					}
					retreatingShips.Remove(retreatingShips[0]);
					//Since the ships has left battlefield successfully, no need to reduce ships.
					if (retreatingShips.Count > 0)
					{
						selectedShip = retreatingShips[0];
						camera.CenterCamera(selectedShip.X, selectedShip.Y);
						retreatProcess = 1.0f;
					}
					else
					{
						processRetreating = false;
					}
				}
				retreatProcess -= frameDeltaTime;
				if (retreatProcess < 0)
				{
					retreatProcess = 0;
				}
				return;
			}

			foreach (Button button in actionButtons)
			{
				button.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			if (mouseX >= x && mouseX < x + 280 && mouseY >= y)
			{
				return;
			}
			camera.HandleUpdate(mouseX, mouseY, frameDeltaTime);*/
		/*}

		public void MouseDown(int x, int y, int whichButton)
		{
			foreach (Button button in actionButtons)
			{
				button.MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*for (int i = 0; i < actionButtons.Length; i++)
			{
				if (actionButtons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 1:
							{
								SelectedShip.Retreating = true;
								actionButtons[i].Enabled = false;
							} break;
						case 2:
							{
								if (SelectedShip != fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter])
								{
									SelectedShip = fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter];
								}
								else
								{
									int amountOfShipsToCheck = fleetsInCombat[whichEmpireTurn].combatShips.Count;
									int nextShipToCheck = selectedShipIter;
									int shipsChecked = 0;
									while (shipsChecked < amountOfShipsToCheck)
									{
										nextShipToCheck--;
										if (nextShipToCheck < 0)
										{
											nextShipToCheck = fleetsInCombat[whichEmpireTurn].combatShips.Count - 1;
										}
										if (!fleetsInCombat[whichEmpireTurn].combatShips[nextShipToCheck].DoneThisTurn)
										{
											selectedShipIter = nextShipToCheck;
											break;
										}
										shipsChecked++;
									}
									SelectedShip = fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter];
								}
								actionButtons[1].Enabled = !SelectedShip.Retreating;
								camera.CenterCamera(SelectedShip.X, SelectedShip.Y);
							} break;
						case 3:
							{
								fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter].DoneThisTurn = true;
								int amountOfShipsToCheck = fleetsInCombat[whichEmpireTurn].combatShips.Count;
								int nextShipToCheck = selectedShipIter;
								int shipsChecked = 0;
								while (shipsChecked < amountOfShipsToCheck)
								{
									nextShipToCheck--;
									if (nextShipToCheck < 0)
									{
										nextShipToCheck = fleetsInCombat[whichEmpireTurn].combatShips.Count - 1;
									}
									if (!fleetsInCombat[whichEmpireTurn].combatShips[nextShipToCheck].DoneThisTurn)
									{
										selectedShipIter = nextShipToCheck;
										break;
									}
									shipsChecked++;
								}
								SelectedShip = fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter];
								actionButtons[1].Enabled = !SelectedShip.Retreating;
								camera.CenterCamera(SelectedShip.X, SelectedShip.Y);
							} break;
						case 4:
							{
								fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter].DoneThisTurn = true;
								int amountOfShipsToCheck = fleetsInCombat[whichEmpireTurn].combatShips.Count;
								int nextShipToCheck = selectedShipIter;
								int shipsChecked = 0;
								while (shipsChecked < amountOfShipsToCheck)
								{
									nextShipToCheck++;
									if (nextShipToCheck >= amountOfShipsToCheck)
									{
										nextShipToCheck = 0;
									}
									if (!fleetsInCombat[whichEmpireTurn].combatShips[nextShipToCheck].DoneThisTurn)
									{
										selectedShipIter = nextShipToCheck;
										break;
									}
									shipsChecked++;
								}
								SelectedShip = fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter];
								actionButtons[1].Enabled = !SelectedShip.Retreating;
								camera.CenterCamera(SelectedShip.X, SelectedShip.Y);
							} break;
						case 5:
							{
								if (SelectedShip != fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter])
								{
									SelectedShip = fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter];
								}
								else
								{
									int amountOfShipsToCheck = fleetsInCombat[whichEmpireTurn].combatShips.Count;
									int nextShipToCheck = selectedShipIter;
									int shipsChecked = 0;
									while (shipsChecked < amountOfShipsToCheck)
									{
										nextShipToCheck++;
										if (nextShipToCheck >= amountOfShipsToCheck)
										{
											nextShipToCheck = 0;
										}
										if (!fleetsInCombat[whichEmpireTurn].combatShips[nextShipToCheck].DoneThisTurn)
										{
											selectedShipIter = nextShipToCheck;
											break;
										}
										shipsChecked++;
									}
									SelectedShip = fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter];
								}
								actionButtons[1].Enabled = !SelectedShip.Retreating;
								camera.CenterCamera(SelectedShip.X, SelectedShip.Y);
							} break;
						case 6:
							{
								MoveToNextEmpireTurn();
							} break;
					}
					return;
				}
			}
			IsClickOnShip(x, y);*/
		/*}

		public void MouseScroll(int direction, int x, int y)
		{
			/*if (!processRetreating)
			{
				camera.MouseWheel(direction, x, y);
			}*/
		/*}

		public void KeyDown(KeyboardInputEventArgs e)
		{
		}

		private void IsClickOnShip(int x, int y)
		{
			/*int X = (int)(((x / camera.Scale) + camera.XOffset) / 16) + camera.CameraX;
			int Y = (int)(((y / camera.Scale) + camera.YOffset) / 16) + camera.CameraY;

			foreach (CombatFleet fleet in fleetsInCombat)
			{
				for (int i = 0; i < fleet.combatShips.Count; i++)
				{
					if (fleet.combatShips[i].X <= X && fleet.combatShips[i].X + fleet.combatShips[i].Size > X && fleet.combatShips[i].Y <= Y && fleet.combatShips[i].Y + fleet.combatShips[i].Size > Y)
					{
						SelectedShip = fleet.combatShips[i];
						if (fleet == fleetsInCombat[whichEmpireTurn])
						{
							selectedShipIter = i;
							actionButtons[1].Enabled = !SelectedShip.Retreating;
						}
						else
						{
							actionButtons[1].Enabled = false;
						}
						return;
					}
				}
			}*/
		/*}

		public void SetupBattle(List<Fleet> fleets, StarSystem system)
		{
			/*fleetsInCombat = new List<CombatFleet>();
			
			int totalCircumferenceNeeded = 0;

			//To do: randomize order of empires
			foreach (Fleet fleet in fleets)
			{
				CombatFleet combatFleet = new CombatFleet(fleet);
				totalCircumferenceNeeded += combatFleet.Length;
				combatFleet.Empire = fleet.Empire;
				fleetsInCombat.Add(combatFleet);
			}
			totalCircumferenceNeeded *= 5;

			if (totalCircumferenceNeeded < 50)
			{
				totalCircumferenceNeeded = 50;
			}

			float radius = totalCircumferenceNeeded / 2.0f;
			radius = (float)(radius / Math.PI);

			float angle = 0;

			foreach (CombatFleet fleet in fleetsInCombat)
			{
				fleet.SetStartingPosition(radius, angle);
				angle += (float)((Math.PI * 2) / fleetsInCombat.Count);
			}

			camera.InitCamera((int)(radius * 3), 16);

			this.system = system;

			selectedShipIter = 0;
			SelectedShip = fleetsInCombat[0].combatShips[selectedShipIter];
			camera.CenterCamera(SelectedShip.X, SelectedShip.Y);*/
		/*}

		private void MoveToNextEmpireTurn()
		{
			/*whichEmpireTurn++;
			if (whichEmpireTurn >= fleetsInCombat.Count)
			{
				whichEmpireTurn = 0;
				//Since all empires are done, reset all ships' "Done" to false
				foreach (CombatFleet fleet in fleetsInCombat)
				{
					foreach (CombatShip ship in fleet.combatShips)
					{
						ship.DoneThisTurn = false;
						if (ship.DoneRetreatWait)
						{
							retreatingShips.Add(ship);
						}
						if (ship.Retreating)
						{
							ship.DoneRetreatWait = true;
						}
					}
				}
			}
			selectedShipIter = 0;
			SelectedShip = fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter];
			actionButtons[1].Enabled = !selectedShip.Retreating;
			camera.CenterCamera(SelectedShip.X, SelectedShip.Y);
			if (retreatingShips.Count > 0)
			{
				processRetreating = true;
				retreatProcess = 1.0f;
				selectedShip = retreatingShips[0];
				camera.CenterCamera(selectedShip.X, selectedShip.Y);
			}*/
	//	}
	//}
}
