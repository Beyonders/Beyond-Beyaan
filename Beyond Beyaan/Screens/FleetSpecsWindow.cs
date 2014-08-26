using System;
using System.Drawing;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class FleetSpecsWindow : WindowInterface
	{
		private BBStretchableImage[] _shipBackgrounds;
		private BBStretchableImage[] _beamBackgrounds;
		private BBStretchableImage[] _missileBackgrounds;
		private BBStretchableImage[] _attackBackgrounds;
		private BBStretchableImage[] _hitPointsBackgrounds;
		private BBStretchableImage[] _shieldBackgrounds;
		private BBStretchableImage[] _galaxySpeedBackgrounds;
		private BBStretchableImage[] _combatSpeedBackgrounds;
		private BBStretchableImage[] _weaponsBackgrounds;
		private BBStretchableImage[] _specialsBackgrounds;

		private BBButton[] _scrapButtons;
		private BBSprite _shipSprite;
		private bool _previewVisible;
		private float[] _empireColor;
		private BBStretchableImage _shipBackground;
		private Point _shipPoint;

		private BBLabel[] _shipNameLabels;

		private BBLabel[] _beamDefLabels;
		private BBLabel[] _beamDefValueLabels;

		private BBLabel[] _missileDefLabels;
		private BBLabel[] _missileDefValueLabels;

		private BBLabel[] _attackLevelLabels;
		private BBLabel[] _attackLevelValueLabels;

		private BBLabel[] _hitPointsLabels;
		private BBLabel[] _hitPointsValueLabels;

		private BBLabel[] _shieldLabels;
		private BBLabel[] _shieldValueLabels;

		private BBLabel[] _galaxySpeedLabels;
		private BBLabel[] _galaxySpeedValueLabels;

		private BBLabel[] _combatSpeedLabels;
		private BBLabel[] _combatSpeedValueLabels;

		private BBLabel[][] _weaponLabels;
		private BBLabel[][] _specialLabels;

		private BBLabel[] _costLabels;
		private BBLabel[] _costValueLabels;

		private BBLabel[] _amountLabels;
		private BBLabel[] _amountValueLabels;

		private int _maxVisible;
		private int _x;
		private int _y;
		private FleetManager _fleetManager;

		public Action ScrapAction;

		public bool Initialize(GameMain gameMain, string name, out string reason)
		{
			_x = (gameMain.ScreenWidth / 2) - 430;
			_y = (gameMain.ScreenHeight / 2) - 300;
			if (!Initialize((gameMain.ScreenWidth / 2) - 450, (gameMain.ScreenHeight / 2) - 320, 900, 640, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}
			_shipBackground = new BBStretchableImage(); //Used for sprite preview
			if (!_shipBackground.Initialize(0,0, 170, 170, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			_shipBackgrounds = new BBStretchableImage[6];
			_beamBackgrounds = new BBStretchableImage[6];
			_missileBackgrounds = new BBStretchableImage[6];
			_attackBackgrounds = new BBStretchableImage[6];
			_hitPointsBackgrounds = new BBStretchableImage[6];
			_shieldBackgrounds = new BBStretchableImage[6];
			_galaxySpeedBackgrounds = new BBStretchableImage[6];
			_combatSpeedBackgrounds = new BBStretchableImage[6];
			_weaponsBackgrounds = new BBStretchableImage[6];
			_specialsBackgrounds = new BBStretchableImage[6];
			_scrapButtons = new BBButton[6];
			_shipNameLabels = new BBLabel[6];
			_beamDefLabels = new BBLabel[6];
			_beamDefValueLabels = new BBLabel[6];
			_missileDefLabels = new BBLabel[6];
			_missileDefValueLabels = new BBLabel[6];
			_attackLevelLabels = new BBLabel[6];
			_attackLevelValueLabels = new BBLabel[6];
			_hitPointsLabels = new BBLabel[6];
			_hitPointsValueLabels = new BBLabel[6];
			_shieldLabels = new BBLabel[6];
			_shieldValueLabels = new BBLabel[6];
			_galaxySpeedLabels = new BBLabel[6];
			_galaxySpeedValueLabels = new BBLabel[6];
			_combatSpeedLabels = new BBLabel[6];
			_combatSpeedValueLabels = new BBLabel[6];
			_costLabels = new BBLabel[6];
			_costValueLabels = new BBLabel[6];
			_amountLabels = new BBLabel[6];
			_amountValueLabels = new BBLabel[6];
			_weaponLabels = new BBLabel[6][];
			_specialLabels = new BBLabel[6][];
			for (int i = 0; i < 6; i++)
			{
				_shipBackgrounds[i] = new BBStretchableImage();
				_scrapButtons[i] = new BBButton();
				_shipNameLabels[i] = new BBLabel();
				_beamBackgrounds[i] = new BBStretchableImage();
				_missileBackgrounds[i] = new BBStretchableImage();
				_attackBackgrounds[i] = new BBStretchableImage();
				_hitPointsBackgrounds[i] = new BBStretchableImage();
				_shieldBackgrounds[i] = new BBStretchableImage();
				_galaxySpeedBackgrounds[i] = new BBStretchableImage();
				_combatSpeedBackgrounds[i] = new BBStretchableImage();
				_weaponsBackgrounds[i] = new BBStretchableImage();
				_specialsBackgrounds[i] = new BBStretchableImage();
				_beamDefLabels[i] = new BBLabel();
				_beamDefValueLabels[i] = new BBLabel();
				_missileDefLabels[i] = new BBLabel();
				_missileDefValueLabels[i] = new BBLabel();
				_attackLevelLabels[i] = new BBLabel();
				_attackLevelValueLabels[i] = new BBLabel();
				_hitPointsLabels[i] = new BBLabel();
				_hitPointsValueLabels[i] = new BBLabel();
				_shieldLabels[i] = new BBLabel();
				_shieldValueLabels[i] = new BBLabel();
				_galaxySpeedLabels[i] = new BBLabel();
				_galaxySpeedValueLabels[i] = new BBLabel();
				_combatSpeedLabels[i] = new BBLabel();
				_combatSpeedValueLabels[i] = new BBLabel();
				_costLabels[i] = new BBLabel();
				_costValueLabels[i] = new BBLabel();
				_amountLabels[i] = new BBLabel();
				_amountValueLabels[i] = new BBLabel();
				if (!_shipBackgrounds[i].Initialize(_x, _y + (i * 100), 860, 100, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_shipNameLabels[i].Initialize(_x + 10, _y + 10 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_scrapButtons[i].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, _x + 250, _y + 5 + (i * 100), 75, 35, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_scrapButtons[i].SetToolTip(name + "ScrapShipToolTip" + i, "Scrap Ship Design", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_beamBackgrounds[i].Initialize(_x + 5, _y + 39 + (i * 100), 160, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_beamDefLabels[i].Initialize(_x + 10, _y + 43 + (i * 100), "Beam Defense:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_beamDefValueLabels[i].Initialize(_x + 155, _y + 43 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_missileBackgrounds[i].Initialize(_x + 5, _y + 66 + (i * 100), 160, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_missileDefLabels[i].Initialize(_x + 10, _y + 72 + (i * 100), "Missile Defense:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_missileDefValueLabels[i].Initialize(_x + 155, _y + 72 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_attackBackgrounds[i].Initialize(_x + 165, _y + 39 + (i * 100), 160, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_attackLevelLabels[i].Initialize(_x + 170, _y + 43 + (i * 100), "Attack Level:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_attackLevelValueLabels[i].Initialize(_x + 315, _y + 43 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_hitPointsBackgrounds[i].Initialize(_x + 165, _y + 66 + (i * 100), 160, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_hitPointsLabels[i].Initialize(_x + 170, _y + 72 + (i * 100), "Hit Points:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_hitPointsValueLabels[i].Initialize(_x + 315, _y + 72 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_shieldBackgrounds[i].Initialize(_x + 325, _y + 5 + (i * 100), 140, 35, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_shieldLabels[i].Initialize(_x + 330, _y + 12 + (i * 100), "Shield Level:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_shieldValueLabels[i].Initialize(_x + 455, _y + 12 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_galaxySpeedBackgrounds[i].Initialize(_x + 325, _y + 39 + (i * 100), 140, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_galaxySpeedLabels[i].Initialize(_x + 330, _y + 43 + (i * 100), "Galaxy Speed:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_galaxySpeedValueLabels[i].Initialize(_x + 455, _y + 43 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_combatSpeedBackgrounds[i].Initialize(_x + 325, _y + 66 + (i * 100), 140, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_combatSpeedLabels[i].Initialize(_x + 330, _y + 72 + (i * 100), "Combat Speed:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_combatSpeedValueLabels[i].Initialize(_x + 455, _y + 72 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_weaponsBackgrounds[i].Initialize(_x + 470, _y + (i * 100), 200, 100, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_specialsBackgrounds[i].Initialize(_x + 670, _y + (i * 100), 190, 100, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_amountLabels[i].Initialize(_x + 675, _y + (i * 100) + 63, "Amt:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_amountValueLabels[i].Initialize(_x + 755, _y + (i * 100) + 63, string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_costLabels[i].Initialize(_x + 760, _y + (i * 100) + 63, "Cost: ", Color.Orange, out reason))
				{
					return false;
				}
				if (!_costValueLabels[i].Initialize(_x + 855, _y + (i * 100) + 63, string.Empty, Color.White, out reason))
				{
					return false;
				}
				_beamDefValueLabels[i].SetAlignment(true);
				_missileDefValueLabels[i].SetAlignment(true);
				_attackLevelValueLabels[i].SetAlignment(true);
				_hitPointsValueLabels[i].SetAlignment(true);
				_shieldValueLabels[i].SetAlignment(true);
				_galaxySpeedValueLabels[i].SetAlignment(true);
				_combatSpeedValueLabels[i].SetAlignment(true);
				_costValueLabels[i].SetAlignment(true);
				_amountValueLabels[i].SetAlignment(true);
				_weaponLabels[i] = new BBLabel[4];
				for (int j = 0; j < _weaponLabels[i].Length; j++)
				{
					_weaponLabels[i][j] = new BBLabel();
					if (!_weaponLabels[i][j].Initialize(_x + 475, _y + 7 + (i * 100 + j * 21), string.Empty, Color.White, out reason))
					{
						return false;
					}
				}
				_specialLabels[i] = new BBLabel[3];
				for (int j = 0; j < _specialLabels[i].Length; j++)
				{
					_specialLabels[i][j] = new BBLabel();
					if (!_specialLabels[i][j].Initialize(_x + 675, _y + 7 + (i * 100 + j * 21), string.Empty, Color.White, out reason))
					{
						return false;
					}
				}
			}

			reason = null;
			return true;
		}

		public void LoadDesigns()
		{
			_fleetManager = _gameMain.EmpireManager.CurrentEmpire.FleetManager;
			bool moreThanOneDesign = _fleetManager.CurrentDesigns.Count > 1;
			_maxVisible = _fleetManager.CurrentDesigns.Count;
			for (int i = 0; i < _maxVisible; i++)
			{
				if (i < _fleetManager.CurrentDesigns.Count)
				{
					var shipDesign = _fleetManager.CurrentDesigns[i];
					//_shipSprites[i] = currentEmpire.EmpireRace.GetShip(shipDesign.Size, shipDesign.WhichStyle);
					_shipNameLabels[i].SetText(shipDesign.Name);
					_scrapButtons[i].Active = moreThanOneDesign;
					_beamDefValueLabels[i].SetText(shipDesign.BeamDefense.ToString());
					_missileDefValueLabels[i].SetText(shipDesign.MissileDefense.ToString());
					_attackLevelValueLabels[i].SetText(shipDesign.AttackLevel.ToString());
					_hitPointsValueLabels[i].SetText(shipDesign.MaxHitPoints.ToString());
					_shieldValueLabels[i].SetText(shipDesign.ShieldLevel.ToString());
					_galaxySpeedValueLabels[i].SetText(shipDesign.GalaxySpeed.ToString());
					_combatSpeedValueLabels[i].SetText(shipDesign.ManeuverSpeed.ToString());
					_costValueLabels[i].SetText(string.Format("{0:0.0}", shipDesign.Cost));
					int j = 0;
					foreach (var weapon in shipDesign.Weapons)
					{
						if (weapon.Key != null)
						{
							_weaponLabels[i][j].SetText(string.Format("{0:00} {1}", weapon.Value, weapon.Key.DisplayName));
						}
						j++;
					}
					for (; j < 4; j++)
					{
						//Set the rest of unused weapon slots to blank
						_weaponLabels[i][j].SetText(string.Empty);
					}
					for (j = 0; j < 3; j++)
					{
						if (shipDesign.Specials[j] != null)
						{
							_specialLabels[i][j].SetText(shipDesign.Specials[j].DisplayName);
						}
						else
						{
							_specialLabels[i][j].SetText(string.Empty);
						}
					}
					_amountValueLabels[i].SetText(_fleetManager.GetShipCount(shipDesign).ToString());
				}
			}
		}

		public override void Draw()
		{
			base.Draw();
			for (int i = 0; i < _maxVisible; i++)
			{
				_shipBackgrounds[i].Draw();
				_shipNameLabels[i].Draw();
				_scrapButtons[i].Draw();
				_beamBackgrounds[i].Draw();
				_beamDefLabels[i].Draw();
				_beamDefValueLabels[i].Draw();
				_missileBackgrounds[i].Draw();
				_missileDefLabels[i].Draw();
				_missileDefValueLabels[i].Draw();
				_attackBackgrounds[i].Draw();
				_attackLevelLabels[i].Draw();
				_attackLevelValueLabels[i].Draw();
				_hitPointsBackgrounds[i].Draw();
				_hitPointsLabels[i].Draw();
				_hitPointsValueLabels[i].Draw();
				_shieldBackgrounds[i].Draw();
				_shieldLabels[i].Draw();
				_shieldValueLabels[i].Draw();
				_galaxySpeedBackgrounds[i].Draw();
				_galaxySpeedLabels[i].Draw();
				_galaxySpeedValueLabels[i].Draw();
				_combatSpeedBackgrounds[i].Draw();
				_combatSpeedLabels[i].Draw();
				_combatSpeedValueLabels[i].Draw();
				_weaponsBackgrounds[i].Draw();
				_specialsBackgrounds[i].Draw();
				for (int j = 0; j < 4; j++)
				{
					_weaponLabels[i][j].Draw();
				}
				for (int j = 0; j < 3; j++)
				{
					_specialLabels[i][j].Draw();
				}
				_amountLabels[i].Draw();
				_amountValueLabels[i].Draw();
				_costLabels[i].Draw();
				_costValueLabels[i].Draw();
			}
			if (_previewVisible)
			{
				_shipBackground.Draw();
				GorgonLibrary.Gorgon.CurrentShader = _gameMain.ShipShader;
				_gameMain.ShipShader.Parameters["EmpireColor"].SetValue(_empireColor);
				_shipSprite.Draw(_shipPoint.X, _shipPoint.Y);
				GorgonLibrary.Gorgon.CurrentShader = null;
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;

			for (int i = 0; i < 6; i ++)
			{
				result = _scrapButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			_previewVisible = false;
			if (x >= _x && x < _x + 860)
			{
				for (int i = 0; i < _maxVisible; i++)
				{
					if (y >= _y + (i * 100) && y < _y + ((i + 1) * 100))
					{
						var ship = _fleetManager.CurrentDesigns[i];
						_shipSprite = ship.Owner.EmpireRace.GetShip(ship.Size, ship.WhichStyle);
						_empireColor = ship.Owner.ConvertedColor;
						_previewVisible = true;
						_shipBackground.MoveTo(_x - 170, _y + (i * 100) - 35);
						_shipPoint.X = _x - 85;
						_shipPoint.Y = _y + (i * 100) + 50;
					}
				}
			}
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = false;

			for (int i = 0; i < 6; i++)
			{
				result = _scrapButtons[i].MouseDown(x, y) || result;
			}
			return base.MouseDown(x, y) || result;
		}

		public override bool MouseUp(int x, int y)
		{
			for (int i = 0; i < 6; i++)
			{
				if (_scrapButtons[i].MouseUp(x, y))
				{
					_gameMain.EmpireManager.CurrentEmpire.FleetManager.ObsoleteShipDesign(_gameMain.EmpireManager.CurrentEmpire.FleetManager.CurrentDesigns[i]);
					LoadDesigns();
					if (ScrapAction != null)
					{
						ScrapAction();
					}
					return true;
				}
			}
			return base.MouseUp(x, y);
		}
	}
}
