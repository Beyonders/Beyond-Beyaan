namespace Beyond_Beyaan.Screens
{
	class SituationReport
	{
		//private const int AMOUNT_VISIBLE = 10;
		private GameMain gameMain;

		/*private Label title;
		private Button[] buttons;
		private ScrollBar scrollBar;
		private int topIndex;
		private bool isVisible;*/

		public SituationReport(GameMain gameMain)
		{
			this.gameMain = gameMain;

			/*int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			buttons = new Button[AMOUNT_VISIBLE];
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i] = new Button(SpriteName.NormalBackgroundButton, SpriteName.NormalForegroundButton, string.Empty, x + 5, y + 35 + (i * 40), 775, 40);
			}
			scrollBar = new ScrollBar(x + 780, y + 25, 16, 574, AMOUNT_VISIBLE, AMOUNT_VISIBLE, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
				SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton, SpriteName.ScrollVerticalForegroundButton,
				SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
			topIndex = 0;
			isVisible = false;
			title = new Label("Situation Report", x + 5, y + 5);*/
		}

		public void ResetIndex()
		{
			//topIndex = 0;
		}

		public void DrawSitRep()
		{
			/*if (!isVisible)
			{
				return;
			}
			SitRepManager sitRepManager = gameMain.EmpireManager.CurrentEmpire.SitRepManager;

			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			drawingManagement.DrawSprite(SpriteName.Screen, x, y, 255, 800, 600, System.Drawing.Color.White);
			int maxVisible = AMOUNT_VISIBLE;
			if (sitRepManager.Items.Count > AMOUNT_VISIBLE)
			{
				scrollBar.DrawScrollBar(drawingManagement);
			}
			else
			{
				maxVisible = sitRepManager.Items.Count;
			}

			title.Draw();

			for (int i = 0; i < maxVisible; i++)
			{
				buttons[i].Draw(drawingManagement);
			}*/
		}

		public bool Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			/*if (!isVisible)
			{
				return false;
			}

			SitRepManager sitRepManager = gameMain.EmpireManager.CurrentEmpire.SitRepManager;
			int maxVisible = AMOUNT_VISIBLE;

			if (sitRepManager.Items.Count < AMOUNT_VISIBLE)
			{
				maxVisible = sitRepManager.Items.Count;
			}
			else
			{
				if (scrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
				{
					topIndex = scrollBar.TopIndex;
					RefreshLabels(sitRepManager);
				}
			}
			for (int i = 0; i < maxVisible; i++)
			{
				buttons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
			}*/
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			/*if (!isVisible)
			{
				return false;
			}

			SitRepManager sitRepManager = gameMain.EmpireManager.CurrentEmpire.SitRepManager;
			int maxVisible = AMOUNT_VISIBLE;

			if (sitRepManager.Items.Count < AMOUNT_VISIBLE)
			{
				maxVisible = sitRepManager.Items.Count;
			}
			else
			{
				scrollBar.MouseDown(x, y);
			}
			for (int i = 0; i < maxVisible; i++)
			{
				buttons[i].MouseDown(x, y);
			}*/
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			/*if (!isVisible)
			{
				return false;
			}

			SitRepManager sitRepManager = gameMain.EmpireManager.CurrentEmpire.SitRepManager;
			int maxVisible = AMOUNT_VISIBLE;

			if (sitRepManager.Items.Count < AMOUNT_VISIBLE)
			{
				maxVisible = sitRepManager.Items.Count;
			}
			else
			{
				if (scrollBar.MouseUp(x, y))
				{
					topIndex = scrollBar.TopIndex;
					RefreshLabels(sitRepManager);
				}
			}
			for (int i = 0; i < maxVisible; i++)
			{
				if (buttons[i].MouseUp(x, y))
				{
					SitRepItem item = sitRepManager.Items[i + topIndex];
					gameMain.ChangeToScreen(item.ScreenEventIsIn);
					if (item.SystemEventOccuredAt != null)
					{
						gameMain.EmpireManager.CurrentEmpire.SelectedSystem = gameMain.EmpireManager.CurrentEmpire.SelectedSystem = item.SystemEventOccuredAt;
						gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = null;
					}
					if (item.PlanetEventOccuredAt != null)
					{
						for (int j = 0; j < item.SystemEventOccuredAt.Planets.Count; j++)
						{
							if (item.SystemEventOccuredAt.Planets[j] == item.PlanetEventOccuredAt)
							{
								gameMain.EmpireManager.CurrentEmpire.PlanetSelected = j;
							}
						}
					}
					if (item.PointEventOccuredAt != null)
					{
						gameMain.CenterGalaxyScreen(item.PointEventOccuredAt);
					}
					isVisible = false;
				}
			}*/
			return false;
		}

		public void Refresh()
		{
			/*scrollBar.SetAmountOfItems(gameMain.EmpireManager.CurrentEmpire.SitRepManager.Items.Count);
			scrollBar.TopIndex = 0;
			topIndex = 0;
			isVisible = gameMain.EmpireManager.CurrentEmpire.SitRepManager.HasItems;
			RefreshLabels(gameMain.EmpireManager.CurrentEmpire.SitRepManager);*/
		}

		public void Clear()
		{
			/*scrollBar.SetAmountOfItems(10);
			scrollBar.TopIndex = 0;
			topIndex = 0;
			isVisible = false;*/
		}

		public void ToggleVisibility()
		{
			//isVisible = !isVisible;
		}

		public void Show()
		{
			//isVisible = true;
		}

		public void Hide()
		{
			//isVisible = false;
		}

		/*private void RefreshLabels(SitRepManager sitRepManager)
		{
			int maxVisible = AMOUNT_VISIBLE;

			if (sitRepManager.Items.Count < AMOUNT_VISIBLE)
			{
				maxVisible = sitRepManager.Items.Count;
			}

			for (int i = 0; i < maxVisible; i++)
			{
				buttons[i].SetText(sitRepManager.Items[topIndex + i].EventMessage);
			}
		}*/
	}
}
