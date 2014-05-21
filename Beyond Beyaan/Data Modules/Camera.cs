using System;

namespace Beyond_Beyaan
{
	class Camera
	{
		#region Member Variables

		private int _windowWidth;
		private int _windowHeight;

		private float _xVel; //for camera movement
		private float _yVel; //for camera movement

		private float _cameraX, _cameraY; //The position where the player is looking at
		private int _moveToX, _moveToY; //The position where camera needs to move to
		private bool _moving;
		private bool _leftOfX;
		private bool _aboveOfY;

		#endregion

		#region Auto Properties

		public int Width { get; private set; }
		public int Height { get; private set; }

		public float CameraX { get { return _cameraX; } }
		public float CameraY { get { return _cameraY; } }

		public float ZoomDistance { get; private set; }
		public float MaxZoom { get; private set; }

		#endregion

		#region Constructors
		public Camera(int width, int height, int windowWidth, int windowHeight)
		{
			Width = width;
			Height = height;

			_windowWidth = windowWidth;
			_windowHeight = windowHeight;

			MaxZoom = 1.0f;
			while (true)
			{
				MaxZoom -= 0.05f;
				if (width * MaxZoom < windowWidth && height * MaxZoom < windowHeight)
				{
					//Get a maxZoom that have the items fill the screen
					float temp = windowWidth / (float)width;
					MaxZoom = windowHeight / (float)height;
					if (temp < MaxZoom)
					{
						MaxZoom = temp;
					}
					break;
				}
				if (MaxZoom <= 0)
				{
					MaxZoom = 0.05f;
					break;
				}
			}

			ZoomDistance = 1.0f;
		}
		#endregion

		#region Functions
		public void ScrollToPosition(int x, int y)
		{
			//Smoothly move to the new position
			_moveToX = (int)(x - (_windowWidth / ZoomDistance) / 2);
			_moveToY = (int)(y - (_windowHeight / ZoomDistance) / 2);

			if (_moveToX > (Width - ((_windowWidth / ZoomDistance) * 0.9f)))
			{
				_moveToX = (int)(Width - ((_windowWidth / ZoomDistance) * 0.9f));
			}
			if (_moveToX < ((_windowWidth / ZoomDistance) / -10))
			{
				_moveToX = (int)((_windowWidth / ZoomDistance) / -10);
			}
			if (_moveToY > (Height - ((_windowHeight / ZoomDistance) * 0.9f)))
			{
				_moveToY = (int)(Height - ((_windowHeight / ZoomDistance) * 0.9f));
			}
			if (_moveToY < ((_windowHeight / ZoomDistance) / -10))
			{
				_moveToY = (int)((_windowHeight / ZoomDistance) / -10);
			}

			_moving = true;
			//We want only a second to move to the position
			float xDis = _moveToX - _cameraX;
			float yDis = _moveToY - _cameraY;
			float moveSpeed = (float)Math.Sqrt(xDis * xDis + yDis * yDis) * 2;
			float angle = (float)(Math.Atan2(yDis, xDis));
			_xVel = (float)(moveSpeed * Math.Cos(angle));
			_yVel = (float)(moveSpeed * Math.Sin(angle));
			_leftOfX = _moveToX < _cameraX;
			_aboveOfY = _moveToY < _cameraY;
		}
		public void CenterCamera(int x, int y, float zoomDis)
		{
			//Instantly move to the new position (used in start of turn after processing turn is done)
			ZoomDistance = zoomDis;
			if (ZoomDistance < MaxZoom)
			{
				ZoomDistance = MaxZoom;
			}

			_cameraX = x - (_windowWidth / ZoomDistance) / 2;
			_cameraY = y - (_windowHeight / ZoomDistance) / 2;

			CheckPosition();
		}

		public void HandleUpdate(float frameDeltaTime)
		{
			if (_moving)
			{
				_cameraX += frameDeltaTime * _xVel;
				_cameraY += frameDeltaTime * _yVel;
				if ((_cameraX < _moveToX && _leftOfX) ||
					(_cameraX > _moveToX && !_leftOfX) ||
					(_cameraY < _moveToY && _aboveOfY) ||
					(_cameraY > _moveToY && !_aboveOfY))
				{
					//close enough, snap to position
					_cameraX = _moveToX;
					_cameraY = _moveToY;
					_moving = false;
					CheckPosition();
				}
			}
		}

		public void MouseWheel(int direction, float mouseX, float mouseY)
		{
			if (direction > 0)
			{
				if (ZoomDistance < 1)
				{
					float oldScale = ZoomDistance;
					ZoomDistance += 0.05f;
					if (ZoomDistance >= 1)
					{
						ZoomDistance = 1;
					}

					float xScale = mouseX / _windowWidth;
					float yScale = mouseY / _windowHeight;

					_cameraX += ((_windowWidth / oldScale) - (_windowWidth / ZoomDistance)) * xScale;
					_cameraY += ((_windowHeight / oldScale) - (_windowHeight / ZoomDistance)) * yScale;
				}
			}
			else
			{
				if (ZoomDistance > MaxZoom)
				{
					float oldScale = ZoomDistance;
					ZoomDistance -= 0.05f;
					if (ZoomDistance < MaxZoom)
					{
						ZoomDistance = MaxZoom;
					}

					_cameraX -= ((_windowWidth / ZoomDistance) - (_windowWidth / oldScale)) / 2;
					_cameraY -= ((_windowHeight / ZoomDistance) - (_windowHeight / oldScale)) / 2;
				}
			}
			CheckPosition();
		}

		private void CheckPosition()
		{
			if (_cameraX > (Width - ((_windowWidth / ZoomDistance) * 0.9f)))
			{
				_cameraX = (Width - ((_windowWidth / ZoomDistance) * 0.9f));
			}
			if (_cameraX < ((_windowWidth / ZoomDistance) / -10))
			{
				_cameraX = ((_windowWidth / ZoomDistance) / -10);
			}
			if (_cameraY > (Height - ((_windowHeight / ZoomDistance) * 0.9f)))
			{
				_cameraY = (Height - ((_windowHeight / ZoomDistance) * 0.9f));
			}
			if (_cameraY < ((_windowHeight / ZoomDistance) / -10))
			{
				_cameraY = ((_windowHeight / ZoomDistance) / -10);
			}
		}
		#endregion
	}
}
