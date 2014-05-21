using System;
using System.Drawing;

namespace Beyond_Beyaan.Screens
{
	public class SystemInfoWindow : WindowInterface
	{
		private BBLabel _informationText;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!this.Initialize((gameMain.ScreenWidth / 2) - 200, (gameMain.ScreenHeight / 2) - 200, 400, 100, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_informationText = new BBLabel();
			if (!_informationText.Initialize((gameMain.ScreenWidth / 2), (gameMain.ScreenHeight / 2) - 150, string.Empty, Color.White, out reason))
			{
				return false;
			}
			return true;
		}

		public void LoadExploredSystem(StarSystem system)
		{
			_informationText.SetText(string.Format("{0} System has been explored", system.Name));
			_informationText.MoveTo(_xPos + 200 - (int)(_informationText.GetWidth() / 2), _yPos + 50 - (int)(_informationText.GetHeight() / 2));
		}

		public override void Draw()
		{
			base.Draw();
			_informationText.Draw();
		}
	}
}
