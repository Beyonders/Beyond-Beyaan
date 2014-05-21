using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	interface ScreenInterface
	{
		bool Initialize(GameMain gameMain, out string reason);

		void DrawScreen();

		void Update(int x, int y, float frameDeltaTime);

		void MouseDown(int x, int y, int whichButton);

		void MouseUp(int x, int y, int whichButton);

		void MouseScroll(int direction, int x, int y);

		void KeyDown(KeyboardInputEventArgs e);
	}
}
