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
		//private BBSprite _shipSprites;

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

		//private BBLabel[][] _weaponLabels;
		//private BBLabel[][] _specialLabels;

		private BBLabel[] _costLabels;
		private BBLabel[] _costValueLabels;

		private int _maxVisible;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 350;
			int y = (gameMain.ScreenHeight / 2) - 300;
			if (!Initialize((gameMain.ScreenWidth / 2) - 370, (gameMain.ScreenHeight / 2) - 320, 740, 640, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
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
				if (!_shipBackgrounds[i].Initialize(x, y + (i * 100), 700, 100, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_shipNameLabels[i].Initialize(x + 10, y + 10 + (i * 100), string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
				if (!_scrapButtons[i].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, x + 250, y + 5 + (i * 100), 75, 35, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_scrapButtons[i].SetToolTip("ScrapShipToolTip" + i, "Scrap Ship Design", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_beamBackgrounds[i].Initialize(x + 5, y + 39 + (i * 100), 160, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_beamDefLabels[i].Initialize(x + 10, y + 43 + (i * 100), "Beam Defense:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_beamDefValueLabels[i].Initialize(x + 155, y + 43 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_missileBackgrounds[i].Initialize(x + 5, y + 66 + (i * 100), 160, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_missileDefLabels[i].Initialize(x + 10, y + 72 + (i * 100), "Missile Defense:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_missileDefValueLabels[i].Initialize(x + 155, y + 72 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_attackBackgrounds[i].Initialize(x + 165, y + 39 + (i * 100), 160, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_attackLevelLabels[i].Initialize(x + 170, y + 43 + (i * 100), "Attack Level:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_attackLevelValueLabels[i].Initialize(x + 315, y + 43 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				if (!_hitPointsBackgrounds[i].Initialize(x + 165, y + 66 + (i * 100), 160, 28, StretchableImageType.TinyButtonBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_hitPointsLabels[i].Initialize(x + 170, y + 72 + (i * 100), "Hit Points:", Color.Orange, out reason))
				{
					return false;
				}
				if (!_hitPointsValueLabels[i].Initialize(x + 315, y + 72 + (i * 100), string.Empty, Color.White, out reason))
				{
					return false;
				}
				_beamDefValueLabels[i].SetAlignment(true);
				_missileDefValueLabels[i].SetAlignment(true);
				_attackLevelValueLabels[i].SetAlignment(true);
				_hitPointsValueLabels[i].SetAlignment(true);
			}

			reason = null;
			return true;
		}

		public void LoadDesigns()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			bool moreThanOneDesign = currentEmpire.FleetManager.CurrentDesigns.Count > 1;
			_maxVisible = currentEmpire.FleetManager.CurrentDesigns.Count;
			for (int i = 0; i < _maxVisible; i++)
			{
				if (i < currentEmpire.FleetManager.CurrentDesigns.Count)
				{
					var shipDesign = currentEmpire.FleetManager.CurrentDesigns[i];
					//_shipSprites[i] = currentEmpire.EmpireRace.GetShip(shipDesign.Size, shipDesign.WhichStyle);
					_shipNameLabels[i].SetText(shipDesign.Name);
					_scrapButtons[i].Active = moreThanOneDesign;
					_beamDefValueLabels[i].SetText(shipDesign.BeamDefense.ToString());
					_missileDefValueLabels[i].SetText(shipDesign.MissileDefense.ToString());
					_attackLevelValueLabels[i].SetText(shipDesign.AttackLevel.ToString());
					_hitPointsValueLabels[i].SetText(shipDesign.MaxHitPoints.ToString());
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
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;

			for (int i = 0; i < 6; i ++)
			{
				result = _scrapButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			if (result)
			{
				//So we don't highlight the button behind the scrap button
				x = int.MinValue;
				y = int.MinValue;
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
					return true;
				}
			}
			return base.MouseUp(x, y);
		}
	}
}
