using System;
using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class ResearchScreen : WindowInterface
	{
		public Action CloseWindow;

		private BBStretchableImage _fieldsBackground;
		private BBStretchableImage _technologyListBackground;

		//The top UI part:
		private BBLabel[] _techFieldLabels;
		private BBLabel[] _techNamesBeingResearchedLabels;
		private BBLabel[] _techProgressLabels;
		private BBScrollBar[] _techSliders;
		private BBButton[] _techLockButtons;
		private BBLabel _totalResearchPointsLabel;

		private BBTextBox _researchedTechnologyDescriptions;
		private BBStretchButton[] _techFieldButtons;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			if (!base.Initialize(x, y, 800, 600, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_fieldsBackground = new BBStretchableImage();
			_technologyListBackground = new BBStretchableImage();
			if (!_fieldsBackground.Initialize(x + 20, y + 20, 760, 230, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_technologyListBackground.Initialize(x + 20, y + 300, 760, 280, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}

			_techFieldLabels = new BBLabel[6];
			_techNamesBeingResearchedLabels = new BBLabel[6];
			_techProgressLabels = new BBLabel[6];
			_techSliders = new BBScrollBar[6];
			_techLockButtons = new BBButton[6];
			for (int i = 0; i < 6; i++)
			{
				_techFieldLabels[i] = new BBLabel();
				_techNamesBeingResearchedLabels[i] = new BBLabel();
				_techProgressLabels[i] = new BBLabel();
				_techSliders[i] = new BBScrollBar();
				_techLockButtons[i] = new BBButton();
			}
			_totalResearchPointsLabel = new BBLabel();

			if (!_techFieldLabels[0].Initialize(x + 135, y + 35, "Computers:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[1].Initialize(x + 135, y + 65, "Construction:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[2].Initialize(x + 135, y + 95, "Force Fields:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[3].Initialize(x + 135, y + 125, "Planetology:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[4].Initialize(x + 135, y + 155, "Propulsion:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[5].Initialize(x + 135, y + 185, "Weapons:", System.Drawing.Color.White, out reason))
			{
				return false;
			}

			for (int i = 0; i < 6; i++)
			{
				_techFieldLabels[i].SetAlignment(true);
				if (!_techNamesBeingResearchedLabels[i].Initialize(x + 140, y + 35 + (i * 30), "None", System.Drawing.Color.White, out reason))
				{
					return false;
				}
				if (!_techProgressLabels[i].Initialize(x + 545, y + 35 + (i * 30), "N/A", System.Drawing.Color.White, out reason))
				{
					return false;
				}
				_techProgressLabels[i].SetAlignment(true);
				if (!_techSliders[i].Initialize(x + 550, y + 35 + (i * 30), 200, 0, 100, true, true, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_techLockButtons[i].Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.LEFT, x + 755, y + 35 + (i * 30), 16, 16, gameMain.Random, out reason))
				{
					return false;
				}
			}

			if (!_totalResearchPointsLabel.Initialize(x + 765, y + 215, "Total Research Points: 0", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			_totalResearchPointsLabel.SetAlignment(true);

			_researchedTechnologyDescriptions = new BBTextBox();
			if (!_researchedTechnologyDescriptions.Initialize(x + 30, y + 310, 740, 260, true, true, "TechnologyListDescriptions", gameMain.Random, out reason))
			{
				return false;
			}

			_techFieldButtons = new BBStretchButton[6];
			for (int i = 0; i < 6; i++)
			{
				_techFieldButtons[i] = new BBStretchButton();
			}

			if (!_techFieldButtons[0].Initialize("Computers", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 20, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[1].Initialize("Construction", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 147, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[2].Initialize("Force Fields", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 274, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[3].Initialize("Planetology", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 401, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[4].Initialize("Propulsion", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 528, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[5].Initialize("Weapons", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 655, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}

			reason = null;
			return true;
		}

		public override void Draw()
		{
			base.Draw();

			_fieldsBackground.Draw();
			_technologyListBackground.Draw();

			for (int i = 0; i < 6; i++)
			{
				_techFieldLabels[i].Draw();
				_techNamesBeingResearchedLabels[i].Draw();
				_techProgressLabels[i].Draw();
				_techSliders[i].Draw();
				_techLockButtons[i].Draw();
				_techFieldButtons[i].Draw();
			}
			_totalResearchPointsLabel.Draw();
			_researchedTechnologyDescriptions.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			foreach (var button in _techFieldButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			foreach (var button in _techLockButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			for (int i = 0; i < _techSliders.Length; i++)
			{
				if (_techSliders[i].MouseHover(x, y, frameDeltaTime))
				{
					TechField whichField = TechField.COMPUTER;
					switch (i)
					{
						case 1: whichField = TechField.CONSTRUCTION;
							break;
						case 2: whichField = TechField.FORCE_FIELD;
							break;
						case 3: whichField = TechField.PLANETOLOGY;
							break;
						case 4: whichField = TechField.PROPULSION;
							break;
						case 5: whichField = TechField.WEAPON;
							break;
					}
					_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(whichField, _techSliders[i].TopIndex);
					RefreshSliders();
					RefreshProgressLabels();
				}
			}
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = false;
			foreach (var button in _techFieldButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			foreach (var slider in _techSliders)
			{
				result = slider.MouseDown(x, y) || result;
			}
			foreach (var button in _techLockButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			return base.MouseDown(x, y) || result;
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = false;
			for (int i = 0; i < _techFieldButtons.Length; i++)
			{
				if (_techFieldButtons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 0: RefreshResearchedTechs(TechField.COMPUTER);
							break;
						case 1: RefreshResearchedTechs(TechField.CONSTRUCTION);
							break;
						case 2: RefreshResearchedTechs(TechField.FORCE_FIELD);
							break;
						case 3: RefreshResearchedTechs(TechField.PLANETOLOGY);
							break;
						case 4: RefreshResearchedTechs(TechField.PROPULSION);
							break;
						case 5: RefreshResearchedTechs(TechField.WEAPON);
							break;
					}
					result = true;
				}
			}
			for (int i = 0; i < _techLockButtons.Length; i++)
			{
				if (_techLockButtons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 0:
							_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ComputerLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ComputerLocked;
							break;
						case 1:
							_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ConstructionLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ConstructionLocked;
							break;
						case 2:
							_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ForceFieldLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ForceFieldLocked;
							break;
						case 3:
							_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.PlanetologyLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.PlanetologyLocked;
							break;
						case 4:
							_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.PropulsionLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.PropulsionLocked;
							break;
						case 5:
							_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.WeaponLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.WeaponLocked;
							break;
					}
					RefreshLockedStatus();
				}
			}
			for (int i = 0; i < _techSliders.Length; i++)
			{
				if (_techSliders[i].MouseUp(x, y))
				{
					TechField whichField = TechField.COMPUTER;
					switch (i)
					{
						case 1: whichField = TechField.CONSTRUCTION;
							break;
						case 2: whichField = TechField.FORCE_FIELD;
							break;
						case 3: whichField = TechField.PLANETOLOGY;
							break;
						case 4: whichField = TechField.PROPULSION;
							break;
						case 5: whichField = TechField.WEAPON;
							break;
					}
					_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(whichField, _techSliders[i].TopIndex);
					RefreshSliders();
					RefreshProgressLabels();
				}
			}
			if (!base.MouseUp(x, y))
			{
				//Clicked outside window, close the window
				if (CloseWindow != null)
				{
					CloseWindow();
					return true;
				}
			}
			return result;
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public override bool KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape && CloseWindow != null)
			{
				CloseWindow();
				return true;
			}
			return false;
		}

		public void Load()
		{
			_gameMain.EmpireManager.CurrentEmpire.UpdateResearchPoints(); //To ensure that we have an accurate amount of points
			RefreshFields();
		}

		private void RefreshFields()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;

			_totalResearchPointsLabel.SetText(string.Format("Total Research Points: {0:0.00}", currentEmpire.ResearchPoints));

			if (currentEmpire.TechnologyManager.WhichComputerBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[0].SetText(currentEmpire.TechnologyManager.WhichComputerBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[0].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichConstructionBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[1].SetText(currentEmpire.TechnologyManager.WhichConstructionBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[1].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichForceFieldBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[2].SetText(currentEmpire.TechnologyManager.WhichForceFieldBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[2].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichPlanetologyBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[3].SetText(currentEmpire.TechnologyManager.WhichPlanetologyBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[3].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichPropulsionBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[4].SetText(currentEmpire.TechnologyManager.WhichPropulsionBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[4].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichWeaponBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[5].SetText(currentEmpire.TechnologyManager.WhichWeaponBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[5].SetText("None");
			}

			RefreshSliders();

			RefreshProgressLabels();

			RefreshLockedStatus();

			RefreshResearchedTechs(TechField.COMPUTER);
		}

		private void RefreshLockedStatus()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			_techLockButtons[0].Selected = currentEmpire.TechnologyManager.ComputerLocked;
			_techLockButtons[1].Selected = currentEmpire.TechnologyManager.ConstructionLocked;
			_techLockButtons[2].Selected = currentEmpire.TechnologyManager.ForceFieldLocked;
			_techLockButtons[3].Selected = currentEmpire.TechnologyManager.PlanetologyLocked;
			_techLockButtons[4].Selected = currentEmpire.TechnologyManager.PropulsionLocked;
			_techLockButtons[5].Selected = currentEmpire.TechnologyManager.WeaponLocked;

			_techSliders[0].SetEnabledState(!currentEmpire.TechnologyManager.ComputerLocked);
			_techSliders[1].SetEnabledState(!currentEmpire.TechnologyManager.ConstructionLocked);
			_techSliders[2].SetEnabledState(!currentEmpire.TechnologyManager.ForceFieldLocked);
			_techSliders[3].SetEnabledState(!currentEmpire.TechnologyManager.PlanetologyLocked);
			_techSliders[4].SetEnabledState(!currentEmpire.TechnologyManager.PropulsionLocked);
			_techSliders[5].SetEnabledState(!currentEmpire.TechnologyManager.WeaponLocked);
		}

		private void RefreshProgressLabels()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			float amount = currentEmpire.ResearchPoints;

			_techProgressLabels[0].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.COMPUTER, amount));
			_techProgressLabels[1].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.CONSTRUCTION, amount));
			_techProgressLabels[2].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.FORCE_FIELD, amount));
			_techProgressLabels[3].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.PLANETOLOGY, amount));
			_techProgressLabels[4].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.PROPULSION, amount));
			_techProgressLabels[5].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.WEAPON, amount));
		}

		private void RefreshSliders()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;

			_techSliders[0].TopIndex = currentEmpire.TechnologyManager.ComputerPercentage;
			_techSliders[1].TopIndex = currentEmpire.TechnologyManager.ConstructionPercentage;
			_techSliders[2].TopIndex = currentEmpire.TechnologyManager.ForceFieldPercentage;
			_techSliders[3].TopIndex = currentEmpire.TechnologyManager.PlanetologyPercentage;
			_techSliders[4].TopIndex = currentEmpire.TechnologyManager.PropulsionPercentage;
			_techSliders[5].TopIndex = currentEmpire.TechnologyManager.WeaponPercentage;
		}

		private void RefreshResearchedTechs(TechField whichField)
		{
			for (int i = 0; i < _techFieldButtons.Length; i++)
			{
				_techFieldButtons[i].Selected = false;
			}
			string techDescriptions = string.Empty;
			List<Technology> researchedTechs = new List<Technology>();
			switch (whichField)
			{
				case TechField.COMPUTER:
					{
						_techFieldButtons[0].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedComputerTechs;
					} break;
				case TechField.CONSTRUCTION:
					{
						_techFieldButtons[1].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedConstructionTechs;
					} break;
				case TechField.FORCE_FIELD:
					{
						_techFieldButtons[2].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedForceFieldTechs;
					} break;
				case TechField.PLANETOLOGY:
					{
						_techFieldButtons[3].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedPlanetologyTechs;
					} break;
				case TechField.PROPULSION:
					{
						_techFieldButtons[4].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedPropulsionTechs;
					} break;
				case TechField.WEAPON:
					{
						_techFieldButtons[5].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedWeaponTechs;
					} break;
			}
			foreach (var researchedTech in researchedTechs)
			{
				techDescriptions += researchedTech.TechName + " -\r\n" + researchedTech.TechDescription + "\r\n\r\n\r\n";
			}
			_researchedTechnologyDescriptions.SetText(techDescriptions);
			_researchedTechnologyDescriptions.ScrollToBottom();
		}
	}
}
