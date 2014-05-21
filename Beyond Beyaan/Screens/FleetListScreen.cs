using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class FleetListScreen : ScreenInterface
	{
		/*GameMain _gameMain;

		Label fleetLabel;
		Label shipLabel;
		Label shipNameLabel;
		Label sizeLabel;
		Label engineLabel;
		Label computerLabel;
		Label armorLabel;
		Label shieldLabel;
		Label weaponLabel;
		Label mountsLabel;
		Label shotsLabel;
		Label specs;

		Button[] fleetButtons;
		Button[] shipButtons;
		Button scrapFleet;
		Button scrapShip;
		Button showOurFleets;
		Button showOtherFleets;

		List<Fleet> ownedFleets;
		List<Fleet> otherFleets;
		List<Fleet> allFleets;
		List<Fleet> whichFleets;

		SingleLineTextBox nameText;
		SingleLineTextBox sizeText;
		SingleLineTextBox engineText;
		SingleLineTextBox computerText;
		SingleLineTextBox armorText;
		SingleLineTextBox shieldText;
		SingleLineTextBox[] weaponTexts;
		SingleLineTextBox[] mountsTexts;
		SingleLineTextBox[] shotsTexts;

		int weaponIndex;
		int fleetIndex;
		int shipIndex;
		int selectedFleet;
		int selectedShip;

		Fleet fleetSelected;
		Fleet hoveringFleet;

		Ship shipSelected;
		private BBSprite shipSprite;*/

		public bool Initialize(GameMain gameMain, out string reason)
		{
			/*this._gameMain = _gameMain;

			int x = (_gameMain.ScreenWidth / 2) - 400;
			int y = (_gameMain.ScreenHeight / 2) - 300;

			fleetLabel = new Label("Fleets", x + 2, y + 2);
			shipLabel = new Label("Ships in this fleet", x + 202, y + 2);
			specs = new Label("Specifications", x + 200, y + 400);
			shipNameLabel = new Label("Name:", x + 200, y + 425);
			sizeLabel = new Label("Size", x + 200, y + 450);
			engineLabel = new Label("Engine:", x + 200, y + 475);
			computerLabel = new Label("Computer:", x + 200, y + 500);
			armorLabel = new Label("Armor:", x + 200, y + 525);
			shieldLabel = new Label("Shield:", x + 200, y + 550);
			weaponLabel = new Label("Weapons", x + 452, y + 402);
			mountsLabel = new Label("Mounts", x + 675, y + 402);
			shotsLabel = new Label("Shots", x + 730, y + 402);

			fleetButtons = new Button[11];
			shipButtons = new Button[11];

			for (int i = 0; i < fleetButtons.Length; i++)
			{
				fleetButtons[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 2, y + 25 + (i * 25), 182, 25);
				shipButtons[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 202, y + 25 + (i * 25), 182, 25);
			}

			scrapFleet = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Scrap this fleet", x + 2, y + 304, 182, 25);
			showOurFleets = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Show our fleets", x + 2, y + 332, 182, 25);
			scrapShip = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Scrap this ship", x + 202, y + 304, 182, 25);
			showOtherFleets = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Show other fleets", x + 202, y + 332, 182, 25);

			nameText = new SingleLineTextBox(x + 275, y + 425, 150, 23, SpriteName.MiniBackgroundButton);
			sizeText = new SingleLineTextBox(x + 275, y + 450, 150, 23, SpriteName.MiniBackgroundButton);
			engineText = new SingleLineTextBox(x + 275, y + 475, 150, 23, SpriteName.MiniBackgroundButton);
			computerText = new SingleLineTextBox(x + 275, y + 500, 150, 23, SpriteName.MiniBackgroundButton);
			armorText = new SingleLineTextBox(x + 275, y + 525, 150, 23, SpriteName.MiniBackgroundButton);
			shieldText = new SingleLineTextBox(x + 275, y + 550, 150, 23, SpriteName.MiniBackgroundButton);

			weaponTexts = new SingleLineTextBox[6];
			mountsTexts = new SingleLineTextBox[6];
			shotsTexts = new SingleLineTextBox[6];

			for (int i = 0; i < weaponTexts.Length; i++)
			{
				weaponTexts[i] = new SingleLineTextBox(x + 450, y + 425, 200, 23, SpriteName.MiniBackgroundButton);
				mountsTexts[i] = new SingleLineTextBox(x + 675, y + 425, 40, 23, SpriteName.MiniBackgroundButton);
				shotsTexts[i] = new SingleLineTextBox(x + 730, y + 425, 40, 23, SpriteName.MiniBackgroundButton);
			}*/
			reason = null;
			return true;
		}

		public void DrawScreen()
		{
			/*_gameMain.DrawGalaxyBackground();

			drawingManagement.DrawSprite(SpriteName.ControlBackground, (_gameMain.ScreenWidth / 2) - 400, (_gameMain.ScreenHeight / 2) - 300, 255, 800, 600, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.Screen, (_gameMain.ScreenWidth / 2), (_gameMain.ScreenHeight / 2) - 300, 255, 399, 399, System.Drawing.Color.White);

			DrawGalaxyPreview(drawingManagement);

			fleetLabel.Draw();
			shipLabel.Draw();

			int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i].Draw(drawingManagement);
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					shipButtons[i].Draw(drawingManagement);
				}
			}

			scrapFleet.Draw(drawingManagement);
			showOurFleets.Draw(drawingManagement);
			scrapShip.Draw(drawingManagement);
			showOtherFleets.Draw(drawingManagement);

			if (selectedFleet != -1 && selectedShip != -1)
			{
				engineLabel.Draw();
				computerLabel.Draw();
				armorLabel.Draw();
				shieldLabel.Draw();
				shipNameLabel.Draw();
				sizeLabel.Draw();
				weaponLabel.Draw();
				mountsLabel.Draw();
				shotsLabel.Draw();
				specs.Draw();

				GorgonLibrary.Gorgon.CurrentShader = _gameMain.ShipShader;
				_gameMain.ShipShader.Parameters["EmpireColor"].SetValue(whichFleets[selectedFleet].Empire.ConvertedColor);
				shipSprite.Draw((_gameMain.ScreenWidth / 2) - 398, (_gameMain.ScreenHeight / 2) + 98, (180.0f / shipSprite.Width), (180.0f / shipSprite.Height));
				GorgonLibrary.Gorgon.CurrentShader = null;
				//DrawingManagement.DrawSprite(SpriteName.Corvette, (_gameMain.ScreenWidth / 2) - 398, (_gameMain.ScreenHeight / 2) + 98, 255, 180, 180, System.Drawing.Color.White);

				nameText.Draw(drawingManagement);
				sizeText.Draw(drawingManagement);
				engineText.Draw(drawingManagement);
				computerText.Draw(drawingManagement);
				armorText.Draw(drawingManagement);
				shieldText.Draw(drawingManagement);

				maxVisible = shipSelected.Weapons.Count > weaponTexts.Length ? weaponTexts.Length : shipSelected.Weapons.Count;

				for (int i = 0; i < maxVisible; i++)
				{
					weaponTexts[i].Draw(drawingManagement);
					mountsTexts[i].Draw(drawingManagement);
					shotsTexts[i].Draw(drawingManagement);
				}
			}*/
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			hoveringFleet = null;
			for (int i = 0; i < maxVisible; i++)
			{
				if (fleetButtons[i].MouseHover(x, y, frameDeltaTime))
				{
					hoveringFleet = whichFleets[i + fleetIndex];
				}
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					shipButtons[i].MouseHover(x, y, frameDeltaTime);
				}
			}
			scrapFleet.MouseHover(x, y, frameDeltaTime);
			scrapShip.MouseHover(x, y, frameDeltaTime);
			showOtherFleets.MouseHover(x, y, frameDeltaTime);
			showOurFleets.MouseHover(x, y, frameDeltaTime);*/
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i].MouseDown(x, y);
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					shipButtons[i].MouseDown(x, y);
				}
			}
			scrapFleet.MouseDown(x, y);
			scrapShip.MouseDown(x, y);
			showOtherFleets.MouseDown(x, y);
			showOurFleets.MouseDown(x, y);*/
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				if (fleetButtons[i].MouseUp(x, y))
				{
					foreach (Button button in fleetButtons)
					{
						button.Selected = false;
					}
					selectedFleet = i + fleetIndex;
					fleetSelected = whichFleets[i + fleetIndex];
					selectedShip = -1;
					UpdateLabels();
					fleetButtons[i].Selected = true;
					return;
				}
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					if (shipButtons[i].MouseUp(x, y))
					{
						foreach (Button button in shipButtons)
						{
							button.Selected = false;
						}
						selectedShip = i + shipIndex;
						UpdateShipSpecs();
						shipButtons[i].Selected = true;
						return;
					}
				}
			}
			if (scrapFleet.MouseUp(x, y))
			{
			}
			if (scrapShip.MouseUp(x, y))
			{
			}
			if (showOtherFleets.MouseUp(x, y))
			{
				showOtherFleets.Selected = !showOtherFleets.Selected;
				UpdateList();
				UpdateLabels();
			}
			if (showOurFleets.MouseUp(x, y))
			{
				showOurFleets.Selected = !showOurFleets.Selected;
				UpdateList();
				UpdateLabels();
			}*/
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			/*if (e.Key == KeyboardKeys.Escape)
			{
				_gameMain.ChangeToScreen(Screen.Galaxy);
			}
			if (e.Key == KeyboardKeys.Space)
			{
				_gameMain.ToggleSitRep();
			}*/
		}

		public void LoadScreen()
		{
			/*Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			ownedFleets = currentEmpire.FleetManager.GetFleets();
			otherFleets = currentEmpire.VisibleFleets;
			allFleets = new List<Fleet>();
			foreach (Fleet fleet in ownedFleets)
			{
				allFleets.Add(fleet);
			}
			foreach (Fleet fleet in otherFleets)
			{
				allFleets.Add(fleet);
			}
			showOtherFleets.Selected = true;
			showOurFleets.Selected = true;

			fleetIndex = 0;
			shipIndex = 0;
			selectedFleet = -1;
			selectedShip = -1;
			fleetSelected = null;

			UpdateList();
			UpdateLabels();*/
		}

		/*public void UpdateLabels()
		{
			int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i].SetText(whichFleets[i + fleetIndex].Empire.EmpireName);
			}
			if (selectedFleet >= 0)
			{
				foreach (Button button in fleetButtons)
				{
					button.Selected = false;
				}
				fleetButtons[selectedFleet - fleetIndex].Selected = true;
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				int i = 0;
				int j = 0;
				foreach (KeyValuePair<Ship, int> ship in whichFleets[selectedFleet + fleetIndex].Ships)
				{
					if (i >= shipIndex)
					{
						if (i >= (shipIndex + maxVisible))
						{
							break;
						}
						shipButtons[j].SetText(ship.Key.Name + " x " + ship.Value);
						j++;
					}
					i++;
				}
			}
		}

		public void UpdateList()
		{
			fleetSelected = null;
			selectedFleet = -1;
			selectedShip = -1;
			fleetIndex = 0;
			shipIndex = 0;
			foreach (Button button in shipButtons)
			{
				button.Selected = false;
			}
			foreach (Button button in fleetButtons)
			{
				button.Selected = false;
			}

			if (showOurFleets.Selected && showOtherFleets.Selected)
			{
				whichFleets = allFleets;
			}
			else if (!showOtherFleets.Selected && showOurFleets.Selected)
			{
				whichFleets = ownedFleets;
			}
			else if (showOtherFleets.Selected && !showOurFleets.Selected)
			{
				whichFleets = otherFleets;
			}
			else
			{
				whichFleets = new List<Fleet>();
			}
		}

		public void UpdateShipSpecs()
		{
			if (selectedFleet != -1)
			{
				int i = 0;
				foreach (KeyValuePair<Ship, int> ship in whichFleets[selectedFleet - fleetIndex].Ships)
				{
					if (i == (selectedShip + shipIndex))
					{
						shipSelected = ship.Key;
						nameText.SetText(shipSelected.Name);
						sizeText.SetText(Utility.ShipSizeToString(shipSelected.Size));
						engineText.SetText(shipSelected.Engine.TechName);
						computerText.SetText(shipSelected.Computer.TechName);
						armorText.SetText(shipSelected.Armor.TechName);
						shieldText.SetText(shipSelected.Shield.TechName);
						weaponIndex = 0;
						UpdateWeaponSpecs();
						LoadShipSprite(whichFleets[selectedFleet - fleetIndex].Empire, ship.Key);
						break;
					}
				}
			}
		}

		public void UpdateWeaponSpecs()
		{
			int maxVisible = shipSelected.weapons.Count > weaponTexts.Length ? weaponTexts.Length : shipSelected.weapons.Count;

			for (int i = 0; i < maxVisible; i++)
			{
				weaponTexts[i].SetText(shipSelected.weapons[i + weaponIndex].GetName());
				mountsTexts[i].SetText(shipSelected.weapons[i + weaponIndex].Mounts.ToString());
			}
		}

		private void DrawGalaxyPreview(DrawingManagement drawingManagement)
		{
			List<StarSystem> systems = _gameMain.Galaxy.GetAllStars();

			foreach (StarSystem system in systems)
			{
				int x = (_gameMain.ScreenWidth / 2) + (int)(386.0f * (system.X / (float)_gameMain.Galaxy.GalaxySize));
				int y = ((_gameMain.ScreenHeight / 2) - 300) + (int)(386.0f * (system.Y / (float)_gameMain.Galaxy.GalaxySize));

				GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
				_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				system.Sprite.Draw(x, y, 0.4f, 0.4f);
				//drawingManagement.DrawSprite(SpriteName.Star, x, y, 255, 6 * system.Size, 6 * system.Size, System.Drawing.Color.White);
				GorgonLibrary.Gorgon.CurrentShader = null;
			}

			foreach (Fleet fleet in whichFleets)
			{
				int x = (_gameMain.ScreenWidth / 2) + (int)(386.0f * (fleet.GalaxyX / (float)_gameMain.Galaxy.GalaxySize));
				int y = ((_gameMain.ScreenHeight / 2) - 300) + (int)(386.0f * (fleet.GalaxyY / (float)_gameMain.Galaxy.GalaxySize));

				if (fleet == fleetSelected || fleet == hoveringFleet)
				{
					drawingManagement.DrawSprite(SpriteName.Fleet, x, y, 255, 32, 32, fleet.Empire.EmpireColor);
				}
				else
				{
					drawingManagement.DrawSprite(SpriteName.Fleet, x, y, 255, 16, 16, fleet.Empire.EmpireColor);
				}
			}
		}

		private void LoadShipSprite(Empire empire, Ship ship)
		{
			shipSprite = empire.EmpireRace.GetShip(ship.Size, ship.WhichStyle);
		}*/
	}
}
