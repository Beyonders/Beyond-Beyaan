using System;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class WindowInterface
	{
		protected GameMain _gameMain;

		protected int _xPos;
		protected int _yPos;
		protected int _mouseX;
		protected int _mouseY;
		protected int _origX;
		protected int _origY;
		protected int _windowWidth;
		protected int _windowHeight;
		protected bool _moving;
		protected bool _moveable;

		protected BBStretchableImage _backGroundImage;

		public bool Initialize(int x, int y, int width, int height, StretchableImageType backgroundImage, GameMain gameMain, bool moveable, Random r, out string reason)
		{
			_xPos = x;
			_yPos = y;
			_windowWidth = width;
			_windowHeight = height;
			this._moveable = moveable;

			_moving = false;
			_gameMain = gameMain;

			_backGroundImage = new BBStretchableImage();
			if (!_backGroundImage.Initialize(x, y, width, height, backgroundImage, r, out reason))
			{
				return false;
			}
			reason = null;
			return true;
		}

		public virtual void Draw()
		{
			_backGroundImage.Draw();
		}

		public virtual bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (_moving)
			{
				_xPos = (x - _mouseX) + _origX;
				_yPos = (y - _mouseY) + _origY;

				if (_xPos + _windowWidth > _gameMain.ScreenWidth)
				{
					_xPos = _gameMain.ScreenWidth - _windowWidth;
				}
				if (_yPos + _windowHeight > _gameMain.ScreenHeight)
				{
					_yPos = _gameMain.ScreenHeight - _windowHeight;
				}
				if (_xPos < 0)
				{
					_xPos = 0;
				}
				if (_yPos < 0)
				{
					_yPos = 0;
				}
				MoveWindow();
				return true;
			}
			if (x >= _xPos && x < _xPos + _windowWidth && y >= _yPos && y < _yPos + _windowHeight)
			{
				//Don't want items behind this window to be highlighted
				return true;
			}
			return false;
		}

		public virtual bool MouseDown(int x, int y)
		{
			if (x >= _xPos && x < _xPos + _windowWidth && y >= _yPos && y < _yPos + _windowHeight)
			{
				if (_moveable)
				{
					_moving = true;
					_mouseX = x;
					_mouseY = y;
					_origX = _xPos;
					_origY = _yPos;
				}
				return true;
			}
			return false;
		}

		public virtual bool MouseUp(int x, int y)
		{
			if (_moving)
			{
				//If it was moving, no matter what, it should capture the mouse up since it's releasing the moving grip
				_moving = false;
				return true;
			}
			if (x >= _xPos && x < _xPos + _windowWidth && y >= _yPos && y < _yPos + _windowHeight)
			{
				return true;
			}
			return false;
		}

		public virtual bool KeyDown(KeyboardInputEventArgs e)
		{
			return false;
		}

		public virtual void MoveWindow()
		{
			_backGroundImage.MoveTo(_xPos, _yPos);
		}
	}
}
