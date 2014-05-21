using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	public class SystemView : WindowInterface
	{
		private BBSingleLineTextBox _name;
		private BBStretchableImage _infrastructureBackground;
		private BBStretchableImage _researchBackground;
		private BBStretchableImage _environmentBackground;
		private BBStretchableImage _defenseBackground;
		private BBStretchButton _constructionProjectButton;

		private BBStretchableImage _generalPurposeBackground; //Used for unexplored planets, enemy-occupied planets, empty planets, population transferring, or ship relocating

		private BBScrollBar _infrastructureSlider;
		private BBScrollBar _researchSlider;
		private BBScrollBar _environmentSlider;
		private BBScrollBar _defenseSlider;
		private BBScrollBar _constructionSlider;

		private BBScrollBar _popTransferSlider;

		private BBButton _infrastructureLockButton;
		private BBButton _researchLockButton;
		private BBButton _environmentLockButton;
		private BBButton _defenseLockButton;
		private BBButton _constructionLockButton;

		private BBButton _relocateToButton;
		private BBButton _transferToButton;

		private BBLabel _infrastructureLabel;
		private BBLabel _researchLabel;
		private BBLabel _environmentLabel;
		private BBLabel _defenseLabel;
		private BBLabel _constructionLabel;

		private BBLabel _terrainLabel;
		private BBLabel _popLabel;
		private BBLabel _productionLabel;

		private BBTextBox _generalPurposeText; //Used for unexplored, etc...
		private BBLabel _transferLabel;

		private BBSprite _infrastructureIcon;
		private BBSprite _defenseIcon;
		private BBSprite _researchIcon;
		private BBSprite _environmentIcon;
		private BBSprite _constructionIcon;
		private StarSystem _currentSystem;
		private Empire _currentEmpire;

		private bool _isOwnedSystem; //To determine whether or not to display/handle sliders
		private bool _isExplored;

		private bool _isRelocating;
		public bool IsRelocating
		{
			get { return _isRelocating; }
			set
			{
				_isRelocating = value;
				if (!_isRelocating)
				{
					_relocateToButton.MoveTo(_xPos + 130, _yPos + 435);
				}
				else
				{
					_relocateToButton.MoveTo(_xPos + 215, _yPos + 435);
				}
			}
		}
		public bool IsTransferring { get; set; }
		private TravelNode _relocateSystem;
		public TravelNode RelocateSystem
		{
			get { return _relocateSystem; }
			set
			{
				_relocateSystem = value;
				if (_relocateSystem != null && !_relocateSystem.IsValid)
				{
					_relocateToButton.Active = false;
				}
				else
				{
					_relocateToButton.Active = true;
				}
			}
		}
		private TravelNode _transferSystem;
		public TravelNode TransferSystem
		{
			get { return _transferSystem; }
			set
			{
				_transferSystem = value;
				if (_transferSystem != null && !_transferSystem.IsValid)
				{
					_transferToButton.Active = false;
				}
				else
				{
					_transferToButton.Active = true;
				}
			}
		}

		#region Constructor
		public bool Initialize(GameMain gameMain, string identifier, out string reason)
		{
			_isExplored = false;
			_isOwnedSystem = false;
			if (!base.Initialize(gameMain.ScreenWidth - 300, gameMain.ScreenHeight / 2 - 240, 300, 480, StretchableImageType.ThinBorderBG, gameMain, true, gameMain.Random, out reason))
			{
				return false;
			}
			_infrastructureIcon = SpriteManager.GetSprite("InfrastructureIcon", gameMain.Random);
			_defenseIcon = SpriteManager.GetSprite("MilitaryIcon", gameMain.Random);
			_researchIcon = SpriteManager.GetSprite("ResearchIcon", gameMain.Random);
			_environmentIcon = SpriteManager.GetSprite("EnvironmentIcon", gameMain.Random);
			_constructionIcon = SpriteManager.GetSprite("ConstructionIcon", gameMain.Random);

			if (_infrastructureIcon == null || _defenseIcon == null || _researchIcon == null || _environmentIcon == null || _constructionIcon == null)
			{
				reason = "One or more of the following sprites does not exist: InfrastructureIcon, MilitaryIcon, ResearchIcon, EnvironmentIcon, and/or ConstructionIcon";
				return false;
			}

			_name = new BBSingleLineTextBox();
			if (!_name.Initialize(string.Empty, _xPos + 10, _yPos + 15, 280, 35, false, gameMain.Random, out reason))
			{
				return false;
			}
			_generalPurposeBackground = new BBStretchableImage();
			_infrastructureBackground = new BBStretchableImage();
			_researchBackground = new BBStretchableImage();
			_environmentBackground = new BBStretchableImage();
			_defenseBackground = new BBStretchableImage();
			_constructionProjectButton = new BBStretchButton();
			_popLabel = new BBLabel();
			_terrainLabel = new BBLabel();
			_productionLabel = new BBLabel();

			_infrastructureLabel = new BBLabel();
			_researchLabel = new BBLabel();
			_environmentLabel = new BBLabel();
			_defenseLabel = new BBLabel();
			_constructionLabel = new BBLabel();

			_generalPurposeText = new BBTextBox();
			_transferLabel = new BBLabel();

			_infrastructureSlider = new BBScrollBar();
			_researchSlider = new BBScrollBar();
			_environmentSlider = new BBScrollBar();
			_defenseSlider = new BBScrollBar();
			_constructionSlider = new BBScrollBar();
			_popTransferSlider = new BBScrollBar();

			_infrastructureLockButton = new BBButton();
			_researchLockButton = new BBButton();
			_environmentLockButton = new BBButton();
			_defenseLockButton = new BBButton();
			_constructionLockButton = new BBButton();

			_relocateToButton = new BBButton();
			_transferToButton = new BBButton();

			if (!_generalPurposeBackground.Initialize(_xPos + 10, _yPos + 130, 280, 300, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_generalPurposeText.Initialize(_xPos + 20, _yPos + 140, 260, 260, true, false, "PlanetUIText" + identifier, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_infrastructureBackground.Initialize(_xPos + 10, _yPos + 130, 280, 60, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_researchBackground.Initialize(_xPos + 10, _yPos + 190, 280, 60, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_environmentBackground.Initialize(_xPos + 10, _yPos + 250, 280, 60, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_defenseBackground.Initialize(_xPos + 10, _yPos + 310, 280, 60, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_constructionProjectButton.Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 10, _yPos + 370, 280, 60, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_terrainLabel.Initialize(_xPos + 55, _yPos + 60, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_popLabel.Initialize(_xPos + 55, _yPos + 80, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_productionLabel.Initialize(_xPos + 55, _yPos + 100, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}

			if (!_infrastructureLabel.Initialize(_xPos + 65, _yPos + 140, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_infrastructureSlider.Initialize(_xPos + 65, _yPos + 160, 200, 0, 100, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_infrastructureLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, _xPos + 267, _yPos + 160, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_researchLabel.Initialize(_xPos + 65, _yPos + 200, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_researchSlider.Initialize(_xPos + 65, _yPos + 220, 200, 0, 100, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_researchLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, _xPos + 267, _yPos + 220, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_environmentLabel.Initialize(_xPos + 65, _yPos + 260, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_environmentSlider.Initialize(_xPos + 65, _yPos + 280, 200, 0, 100, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_environmentLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, _xPos + 267, _yPos + 280, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_defenseLabel.Initialize(_xPos + 65, _yPos + 320, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_defenseSlider.Initialize(_xPos + 65, _yPos + 340, 200, 0, 100, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_defenseLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, _xPos + 267, _yPos + 340, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_constructionLabel.Initialize(_xPos + 65, _yPos + 380, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_constructionSlider.Initialize(_xPos + 65, _yPos + 400, 200, 0, 100, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_constructionLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, _xPos + 267, _yPos + 400, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_transferLabel.Initialize(_xPos + 20, _yPos + 370, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_popTransferSlider.Initialize(_xPos + 20, _yPos + 400, 260, 0, 1, true, true, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_relocateToButton.Initialize("RelocateToBG", "RelocateToFG", string.Empty, ButtonTextAlignment.CENTER, _xPos + 130, _yPos + 435, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_relocateToButton.SetToolTip("RelocateToolTip" + identifier, "Set a friendly system as the destination of newly built ships", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_transferToButton.Initialize("TransferToBG", "TransferToFG", string.Empty, ButtonTextAlignment.CENTER, _xPos + 215, _yPos + 435, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_transferToButton.SetToolTip("TransferToToolTip" + identifier, "Send up to half of the population to another occupied system", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}

			reason = null;
			return true;
		}
		#endregion

		public void LoadSystem()
		{
			LoadSystem(_gameMain.EmpireManager.CurrentEmpire.SelectedSystem, _gameMain.EmpireManager.CurrentEmpire);
		}
		public void LoadSystem(StarSystem system, Empire currentEmpire)
		{
			_currentSystem = system;
			_currentEmpire = currentEmpire;
			if (_currentSystem.IsThisSystemExploredByEmpire(_currentEmpire))
			{
				_isExplored = true;
				var planet = _currentSystem.Planets[0];
				_name.SetText(_currentSystem.Name);
				_isOwnedSystem = _currentSystem.Planets[0].Owner == _currentEmpire;
				_name.SetTextAttributes(_currentSystem.Planets[0].Owner != null ? _currentSystem.Planets[0].Owner.EmpireColor : System.Drawing.Color.White, System.Drawing.Color.Empty);
				_popLabel.SetText(planet.Owner != null ? string.Format("{0:0.0}/{1:0} B", planet.TotalPopulation, planet.TotalMaxPopulation - planet.Waste) : string.Format("{0:0} B", planet.TotalMaxPopulation - planet.Waste));
				_terrainLabel.SetText(Utility.PlanetTypeToString(_currentSystem.Planets[0].PlanetType));
				if (_isOwnedSystem)
				{
					_name.SetReadOnly(false);
					_productionLabel.SetText(string.Format("{0:0.0} ({1:0.0}) Industry", _currentSystem.Planets[0].ActualProduction, _currentSystem.Planets[0].TotalProduction));
					_infrastructureLabel.SetText(_currentSystem.Planets[0].InfrastructureStringOutput);
					_researchLabel.SetText(_currentSystem.Planets[0].ResearchStringOutput);
					_environmentLabel.SetText(_currentSystem.Planets[0].EnvironmentStringOutput);
					_defenseLabel.SetText(_currentSystem.Planets[0].DefenseStringOutput);
					_constructionLabel.SetText(_currentSystem.Planets[0].ConstructionStringOutput);
					_infrastructureSlider.TopIndex = planet.InfrastructureAmount;
					_researchSlider.TopIndex = planet.ResearchAmount;
					_environmentSlider.TopIndex = planet.EnvironmentAmount;
					_defenseSlider.TopIndex = planet.DefenseAmount;
					_constructionSlider.TopIndex = planet.ConstructionAmount;

					_infrastructureLockButton.Selected = planet.InfrastructureLocked;
					_infrastructureSlider.SetEnabledState(!planet.InfrastructureLocked);
					_researchLockButton.Selected = planet.ResearchLocked;
					_researchSlider.SetEnabledState(!planet.ResearchLocked);
					_environmentLockButton.Selected = planet.EnvironmentLocked;
					_environmentSlider.SetEnabledState(!planet.EnvironmentLocked);
					_defenseLockButton.Selected = planet.DefenseLocked;
					_defenseSlider.SetEnabledState(!planet.DefenseLocked);
					_constructionLockButton.Selected = planet.ConstructionLocked;
					_constructionSlider.SetEnabledState(!planet.ConstructionLocked);

					if (_currentSystem.Planets[0].TransferSystem.Key.StarSystem != null)
					{
						_transferLabel.SetText("Moving " + _currentSystem.Planets[0].TransferSystem.Value + " Pop");
						_transferLabel.MoveTo(_xPos + 10, _yPos + 440);
					}
					else
					{
						_transferLabel.SetText(string.Empty);
					}
				}
				else if (_currentSystem.Planets[0].Owner != null)
				{
					_generalPurposeText.SetText("Colonized by " + _currentSystem.Planets[0].Owner.EmpireRace.RaceName + " Empire");
					_name.SetReadOnly(true);
				}
				else
				{
					_generalPurposeText.SetText("No colony");
					_name.SetReadOnly(true);
				}
			}
			else
			{
				_isExplored = false;
				_name.SetText("Unexplored");
				_name.SetTextAttributes(System.Drawing.Color.White, System.Drawing.Color.Empty);
				_generalPurposeText.SetText(_currentSystem.Description);
				_popLabel.SetText(string.Empty);
				_terrainLabel.SetText(string.Empty);
				_productionLabel.SetText(string.Empty);
				_infrastructureLabel.SetText(string.Empty);
				_researchLabel.SetText(string.Empty);
				_environmentLabel.SetText(string.Empty);
				_defenseLabel.SetText(string.Empty);
				_constructionLabel.SetText(string.Empty);
				_name.SetReadOnly(true);
			}
		}

		public override void Draw()
		{
			base.Draw();
			_name.Draw();
			if (_isExplored)
			{
				_currentSystem.Planets[0].SmallSprite.Draw(_xPos + 10, _yPos + 60);
				_popLabel.Draw();
				_terrainLabel.Draw();
				if (_isOwnedSystem)
				{
					if (IsTransferring)
					{
						_generalPurposeBackground.Draw();
						_generalPurposeText.Draw();
						_popTransferSlider.Draw();
						_transferLabel.Draw();
						_transferToButton.Draw();
						_transferToButton.DrawToolTip();
					}
					else if (IsRelocating)
					{
						_generalPurposeBackground.Draw();
						_generalPurposeText.Draw();
						_relocateToButton.Draw();
						_relocateToButton.DrawToolTip();
					}
					else
					{
						_productionLabel.Draw();
						_infrastructureBackground.Draw();
						_researchBackground.Draw();
						_environmentBackground.Draw();
						_defenseBackground.Draw();
						_constructionProjectButton.Draw();
						_infrastructureIcon.Draw(_xPos + 20, _yPos + 140);
						_researchIcon.Draw(_xPos + 20, _yPos + 200);
						_environmentIcon.Draw(_xPos + 20, _yPos + 260);
						_defenseIcon.Draw(_xPos + 20, _yPos + 320);
						_constructionIcon.Draw(_xPos + 20, _yPos + 380);
						_infrastructureLabel.Draw();
						_infrastructureSlider.Draw();
						_infrastructureLockButton.Draw();
						_researchLabel.Draw();
						_researchSlider.Draw();
						_researchLockButton.Draw();
						_environmentLabel.Draw();
						_environmentSlider.Draw();
						_environmentLockButton.Draw();
						_defenseLabel.Draw();
						_defenseSlider.Draw();
						_defenseLockButton.Draw();
						_constructionLabel.Draw();
						_constructionSlider.Draw();
						_constructionLockButton.Draw();
						_relocateToButton.Draw();
						_transferToButton.Draw();
						_relocateToButton.DrawToolTip();
						_transferToButton.DrawToolTip();
						if (_currentSystem.Planets[0].TransferSystem.Key.StarSystem != null)
						{
							_transferLabel.Draw();
						}
					}
				}
				else
				{
					_generalPurposeBackground.Draw();
					_generalPurposeText.Draw();
				}
			}
			else
			{
				_generalPurposeBackground.Draw();
				_generalPurposeText.Draw();
			}
		}

		public override void MoveWindow()
		{
			base.MoveWindow();
			_name.MoveTo(_xPos + 10, _yPos + 15);
			_terrainLabel.MoveTo(_xPos + 55, _yPos + 60);
			_popLabel.MoveTo(_xPos + 55, _yPos + 80);
			_infrastructureBackground.MoveTo(_xPos + 10, _yPos + 130);
			_researchBackground.MoveTo(_xPos + 10, _yPos + 190);
			_environmentBackground.MoveTo(_xPos + 10, _yPos + 250);
			_defenseBackground.MoveTo(_xPos + 10, _yPos + 310);
			_constructionProjectButton.MoveTo(_xPos + 10, _yPos + 370);
			_productionLabel.MoveTo(_xPos + 55, _yPos + 100);
			_infrastructureLabel.MoveTo(_xPos + 65, _yPos + 140);
			_infrastructureSlider.MoveTo(_xPos + 65, _yPos + 160);
			_infrastructureLockButton.MoveTo(_xPos + 267, _yPos + 160);
			_researchLabel.MoveTo(_xPos + 65, _yPos + 200);
			_researchSlider.MoveTo(_xPos + 65, _yPos + 220);
			_researchLockButton.MoveTo(_xPos + 267, _yPos + 220);
			_environmentLabel.MoveTo(_xPos + 65, _yPos + 260);
			_environmentSlider.MoveTo(_xPos + 65, _yPos + 280);
			_environmentLockButton.MoveTo(_xPos + 267, _yPos + 280);
			_defenseLabel.MoveTo(_xPos + 65, _yPos + 320);
			_defenseSlider.MoveTo(_xPos + 65, _yPos + 340);
			_defenseLockButton.MoveTo(_xPos + 267, _yPos + 340);
			_constructionLabel.MoveTo(_xPos + 65, _yPos + 380);
			_constructionSlider.MoveTo(_xPos + 65, _yPos + 400);
			_constructionLockButton.MoveTo(_xPos + 267, _yPos + 400);
			if (IsRelocating)
			{
				_relocateToButton.MoveTo(_xPos + 215, _yPos + 435);
			}
			else
			{
				_relocateToButton.MoveTo(_xPos + 130, _yPos + 435);
			}
			_transferToButton.MoveTo(_xPos + 215, _yPos + 435);
		}

		public override bool MouseDown(int x, int y)
		{
			if (!_isOwnedSystem)
			{
				return base.MouseDown(x, y);
			}
			bool result;
			if (IsTransferring)
			{
				result = _popTransferSlider.MouseDown(x, y);
				if (!result)
				{
					result = _transferToButton.MouseDown(x, y);
				}
				if (!result)
				{
					result = base.MouseDown(x, y);
				}
				return result;
			}
			if (IsRelocating)
			{
				if (_relocateToButton.MouseDown(x, y))
				{
					return true;
				}
				return base.MouseDown(x, y);
			}
			result = _name.MouseDown(x, y);
			if (!result)
			{
				result = _infrastructureSlider.MouseDown(x, y);
			}
			if (!result)
			{
				result = _researchSlider.MouseDown(x, y);
			}
			if (!result)
			{
				result = _environmentSlider.MouseDown(x, y);
			}
			if (!result)
			{
				result = _defenseSlider.MouseDown(x, y);
			}
			if (!result)
			{
				result = _constructionSlider.MouseDown(x, y);
			}
			if (!result)
			{
				result = _infrastructureLockButton.MouseDown(x, y);
			}
			if (!result)
			{
				result = _researchLockButton.MouseDown(x, y);
			}
			if (!result)
			{
				result = _environmentLockButton.MouseDown(x, y);
			}
			if (!result)
			{
				result = _defenseLockButton.MouseDown(x, y);
			}
			if (!result)
			{
				result = _constructionLockButton.MouseDown(x, y);
			}
			if (!result)
			{
				result = _relocateToButton.MouseDown(x, y);
			}
			if (!result)
			{
				result = _transferToButton.MouseDown(x, y);
			}
			if (!result)
			{
				result = _constructionProjectButton.MouseDown(x, y);
			}
			if (!result)
			{
				result = base.MouseDown(x, y);
			}
			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			if (!_isOwnedSystem)
			{
				return base.MouseUp(x, y);
			}
			if (IsTransferring)
			{
				if (_popTransferSlider.MouseUp(x, y))
				{
					_transferLabel.SetText("Moving " + _popTransferSlider.TopIndex + " Population");
					return true;
				}
				if (_transferToButton.MouseUp(x, y))
				{
					if (TransferSystem != null && TransferSystem.IsValid)
					{
						if (TransferSystem.StarSystem == _currentSystem)
						{
							_currentSystem.Planets[0].TransferSystem = new KeyValuePair<TravelNode, int>(new TravelNode(), 0);
						}
						else
						{
							_currentSystem.Planets[0].TransferSystem = new KeyValuePair<TravelNode, int>(TransferSystem, _popTransferSlider.TopIndex);
						}
						_transferLabel.SetText("Moving " + _currentSystem.Planets[0].TransferSystem.Value + " Pop");
						_transferLabel.MoveTo(_xPos + 10, _yPos + 440);
						TransferSystem = null;
						//Done setting transfer
					}
					IsTransferring = false;
					return true;
				}
			}
			if (IsRelocating)
			{
				if (_relocateToButton.MouseUp(x, y))
				{
					if (RelocateSystem.IsValid)
					{
						_currentSystem.Planets[0].RelocateToSystem = RelocateSystem;
						IsRelocating = false;
						RelocateSystem = null;
					}
				}
			}
			if (_name.MouseUp(x, y))
			{
				return true;
			}
			if (_infrastructureSlider.MouseUp(x, y))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.INFRASTRUCTURE, _infrastructureSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_researchSlider.MouseUp(x, y))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.RESEARCH, _researchSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_environmentSlider.MouseUp(x, y))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, _environmentSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_defenseSlider.MouseUp(x, y))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.DEFENSE, _defenseSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_constructionSlider.MouseUp(x, y))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, _constructionSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_infrastructureLockButton.MouseUp(x, y))
			{
				_currentSystem.Planets[0].InfrastructureLocked = !_currentSystem.Planets[0].InfrastructureLocked;
				_infrastructureSlider.SetEnabledState(!_currentSystem.Planets[0].InfrastructureLocked);
				_infrastructureLockButton.Selected = _currentSystem.Planets[0].InfrastructureLocked;
				return true;
			}
			if (_researchLockButton.MouseUp(x, y))
			{
				_currentSystem.Planets[0].ResearchLocked = !_currentSystem.Planets[0].ResearchLocked;
				_researchSlider.SetEnabledState(!_currentSystem.Planets[0].ResearchLocked);
				_researchLockButton.Selected = _currentSystem.Planets[0].ResearchLocked;
				return true;
			}
			if (_environmentLockButton.MouseUp(x, y))
			{
				_currentSystem.Planets[0].EnvironmentLocked = !_currentSystem.Planets[0].EnvironmentLocked;
				_environmentSlider.SetEnabledState(!_currentSystem.Planets[0].EnvironmentLocked);
				_environmentLockButton.Selected = _currentSystem.Planets[0].EnvironmentLocked;
				return true;
			}
			if (_defenseLockButton.MouseUp(x, y))
			{
				_currentSystem.Planets[0].DefenseLocked = !_currentSystem.Planets[0].DefenseLocked;
				_defenseSlider.SetEnabledState(!_currentSystem.Planets[0].DefenseLocked);
				_defenseLockButton.Selected = _currentSystem.Planets[0].DefenseLocked;
				return true;
			}
			if (_constructionLockButton.MouseUp(x, y))
			{
				_currentSystem.Planets[0].ConstructionLocked = !_currentSystem.Planets[0].ConstructionLocked;
				_constructionSlider.SetEnabledState(!_currentSystem.Planets[0].ConstructionLocked);
				_constructionLockButton.Selected = _currentSystem.Planets[0].ConstructionLocked;
				return true;
			}
			if (_relocateToButton.MouseUp(x, y))
			{
				IsRelocating = true;
				_generalPurposeText.SetText("Select a friendly system to send newly built ships");
				_relocateToButton.Active = false;
				return true;
			}
			if (_transferToButton.MouseUp(x, y))
			{
				IsTransferring = true;
				_transferLabel.MoveTo(_xPos + 20, _yPos + 370);
				_transferLabel.SetText("Moving 0 Population");
				_popTransferSlider.SetAmountOfItems((int)(_currentSystem.Planets[0].TotalPopulation / 2));
				_generalPurposeText.SetText("Select a colonized planet to send population");
				return true;
			}
			if (_constructionProjectButton.MouseUp(x, y))
			{
				_currentSystem.Planets[0].ShipBeingBuilt = _currentEmpire.FleetManager.GetNextShipDesign(_currentSystem.Planets[0].ShipBeingBuilt);
				Refresh();
			}
			return base.MouseUp(x, y);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (!_isOwnedSystem)
			{
				return base.MouseHover(x, y, frameDeltaTime);
			}
			if (IsTransferring)
			{
				if (_popTransferSlider.MouseHover(x, y, frameDeltaTime))
				{
					_transferLabel.SetText("Moving " + _popTransferSlider.TopIndex + " Population");
					return true;
				}
				if (_transferToButton.MouseHover(x, y, frameDeltaTime))
				{
					return true;
				}
				return base.MouseHover(x, y, frameDeltaTime);
			}
			if (IsRelocating)
			{
				if (_relocateToButton.MouseHover(x, y, frameDeltaTime))
				{
					return true;
				}
				return base.MouseHover(x, y, frameDeltaTime);
			}
			_name.Update(frameDeltaTime);
			if (_infrastructureSlider.MouseHover(x, y, frameDeltaTime))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.INFRASTRUCTURE, _infrastructureSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_researchSlider.MouseHover(x, y, frameDeltaTime))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.RESEARCH, _researchSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_environmentSlider.MouseHover(x, y, frameDeltaTime))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, _environmentSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_defenseSlider.MouseHover(x, y, frameDeltaTime))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.DEFENSE, _defenseSlider.TopIndex, false);
				Refresh();
				return true;
			}
			if (_constructionSlider.MouseHover(x, y, frameDeltaTime))
			{
				_currentSystem.Planets[0].SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, _constructionSlider.TopIndex, false);
				Refresh();
				return true;
			}
			_infrastructureLockButton.MouseHover(x, y, frameDeltaTime);
			_researchLockButton.MouseHover(x, y, frameDeltaTime);
			_environmentLockButton.MouseHover(x, y, frameDeltaTime);
			_defenseLockButton.MouseHover(x, y, frameDeltaTime);
			_constructionLockButton.MouseHover(x, y, frameDeltaTime);
			_relocateToButton.MouseHover(x, y, frameDeltaTime);
			_transferToButton.MouseHover(x, y, frameDeltaTime);
			if (_constructionLockButton.MouseHover(x, y, frameDeltaTime))
			{
				return true;
			}
			_constructionProjectButton.MouseHover(x, y, frameDeltaTime);
			return base.MouseHover(x, y, frameDeltaTime);
		}

		public override bool KeyDown(GorgonLibrary.InputDevices.KeyboardInputEventArgs e)
		{
			if (_name.KeyDown(e))
			{
				_currentSystem.Name = _name.Text;
				return true;
			}
			return false;
		}

		private void Refresh()
		{
			//Occurs when a slider value changes
			_infrastructureSlider.TopIndex = _currentSystem.Planets[0].InfrastructureAmount;
			_researchSlider.TopIndex = _currentSystem.Planets[0].ResearchAmount;
			_environmentSlider.TopIndex = _currentSystem.Planets[0].EnvironmentAmount;
			_defenseSlider.TopIndex = _currentSystem.Planets[0].DefenseAmount;
			_constructionSlider.TopIndex = _currentSystem.Planets[0].ConstructionAmount;

			_infrastructureLabel.SetText(_currentSystem.Planets[0].InfrastructureStringOutput);
			_researchLabel.SetText(_currentSystem.Planets[0].ResearchStringOutput);
			_environmentLabel.SetText(_currentSystem.Planets[0].EnvironmentStringOutput);
			_defenseLabel.SetText(_currentSystem.Planets[0].DefenseStringOutput);
			_constructionLabel.SetText(_currentSystem.Planets[0].ConstructionStringOutput);
		}
	}
}
