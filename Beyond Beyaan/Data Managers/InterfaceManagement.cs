using System;
using System.Collections.Generic;
using System.Drawing;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary;
using GorgonLibrary.Graphics;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan
{
	public enum ButtonTextAlignment { LEFT, CENTER, RIGHT }

	public class BBButton
	{
		#region Member Variables
		private BBToolTip _toolTip;
		private BBSprite _backgroundSprite;
		private BBSprite _foregroundSprite;
		private int _xPos;
		private int _yPos;
		private int _width;
		private int _height;
		private BBLabel _label;
		private int _xTextOffset;
		private int _yTextOffset;
		private ButtonTextAlignment _alignment;
		#endregion

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public bool Initialize(string backgroundSprite, string foregroundSprite, string buttonText, string font, ButtonTextAlignment alignment, int xPos, int yPos, int width, int height, Random r, out string reason, int xTextOffset = 0, int yTextOffset = 0)
		{
			_backgroundSprite = SpriteManager.GetSprite(backgroundSprite, r);
			_foregroundSprite = SpriteManager.GetSprite(foregroundSprite, r);
			if (backgroundSprite == null || foregroundSprite == null)
			{
				reason = string.Format("One of those sprites does not exist in sprites.xml: \"{0}\" or \"{1}\"", backgroundSprite, foregroundSprite);
				return false;
			}
			_xPos = xPos;
			_yPos = yPos;
			_width = width;
			_height = height;
			_xTextOffset = xTextOffset;
			_yTextOffset = yTextOffset;
			_alignment = alignment;

			_label = new BBLabel();
			if (string.IsNullOrEmpty(font))
			{
				if (!_label.Initialize(0, 0, buttonText, Color.White, out reason))
				{
					return false;
				}
			}
			else
			{
				if (!_label.Initialize(0, 0, buttonText, Color.White, font, out reason))
				{
					return false;
				}
			}
			SetText(buttonText);

			Reset();
			reason = null;
			return true;
		}
		public bool Initialize(string backgroundSprite, string foregroundSprite, string buttonText, ButtonTextAlignment alignment, int xPos, int yPos, int width, int height, Random r, out string reason)
		{
			return Initialize(backgroundSprite, foregroundSprite, buttonText, string.Empty, alignment, xPos, yPos, width, height, r, out reason);
		}
		#endregion

		#region Functions
		public void Reset()
		{
			pulse = 0;
			direction = false;
			Active = true;
			pressed = false;
			Selected = false;
		}

		public void SetText(string text)
		{
			_label.SetText(text);
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5 + _xTextOffset, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos) + _yTextOffset);
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos) + _xTextOffset, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos) + _yTextOffset);
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()) + _xTextOffset, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos) + _yTextOffset);
					break;
			}
		}
		public void SetTextColor(Color color, Color outline)
		{
			_label.SetColor(color, outline);
		}
		public bool SetToolTip(string name, string text, int screenWidth, int screenHeight, Random r, out string reason)
		{
			_toolTip = new BBToolTip();
			if (!_toolTip.Initialize(name, text, screenWidth, screenHeight, r, out reason))
			{
				return false;
			}
			return true;
		}

		public void MoveTo(int x, int y)
		{
			_xPos = x;
			_yPos = y;
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5 + _xTextOffset, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos) + _yTextOffset);
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos) + _xTextOffset, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos) + _yTextOffset);
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()) + _xTextOffset, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos) + _yTextOffset);
					break;
			}
		}

		public void Resize(int width, int height)
		{
			this._width = width;
			this._height = height;
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (pulse < 0.6f)
				{
					pulse = 0.9f;
				}
				if (Active)
				{
					if (direction)
					{
						pulse += frameDeltaTime / 2;
						if (pulse > 0.9f)
						{
							direction = !direction;
							pulse = 0.9f;
						}
					}
					else
					{
						pulse -= frameDeltaTime / 2;
						if (pulse < 0.6f)
						{
							direction = !direction;
							pulse = 0.6f;
						}
					}
				}
				if (_toolTip != null)
				{
					_toolTip.SetShowing(true);
					_toolTip.MouseHover(x, y, frameDeltaTime);
				}
				return true;
			}
			if (pulse > 0)
			{
				pulse -= frameDeltaTime * 2;
				if (pulse < 0)
				{
					pulse = 0;
				}
			}
			if (_toolTip != null)
			{
				_toolTip.SetShowing(false);
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (_toolTip != null)
				{
					_toolTip.SetShowing(false);
				}
				if (Active)
				{
					pressed = true;
				}
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				if (_toolTip != null)
				{
					_toolTip.SetShowing(false);
				}
				pressed = false;
				if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
				{
					return true;
				}
			}
			return false;
		}

		public void Draw()
		{
			if (Active)
			{
				if (pressed || Selected)
				{
					_foregroundSprite.Draw(_xPos, _yPos, _foregroundSprite.Width / _width, _foregroundSprite.Height / _height);
				}
				else if (!Selected)
				{
					_backgroundSprite.Draw(_xPos, _yPos, _foregroundSprite.Width / _width, _foregroundSprite.Height / _height);
					if (pulse > 0)
					{
						_foregroundSprite.Draw(_xPos, _yPos, _foregroundSprite.Width / _width, _foregroundSprite.Height / _height, (byte)(255 * pulse));
					}
				}
			}
			else
			{
				_backgroundSprite.Draw(_xPos, _yPos, _foregroundSprite.Width / _width, _foregroundSprite.Height / _height, System.Drawing.Color.Tan);
				if (Selected)
				{
					_foregroundSprite.Draw(_xPos, _yPos, _foregroundSprite.Width / _width, _foregroundSprite.Height / _height, System.Drawing.Color.Tan);
				}
			}
			if (_label.Text.Length > 0)
			{
				_label.Draw();
			}
		}
		public void DrawToolTip()
		{
			if (_toolTip != null)
			{
				_toolTip.Draw();
			}
		}
		#endregion
	}

	public class BBUniStretchButton
	{
		#region Member Variables
		private BBUniStretchableImage backgroundImage;
		private BBUniStretchableImage foregroundImage;
		private BBLabel _label;
		private ButtonTextAlignment _alignment;

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;

		private int _xPos;
		private int _yPos;
		private int _width;
		private int _height;
		private int _fixedSize;
		private int _variableSize;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public bool Initialize(List<string> backgroundSections, List<string> foregroundSections, bool isHorizontal, string buttonText, ButtonTextAlignment alignment, int xPos, int yPos, int width, int height, int fixedSize, int variableSize, Random r, out string reason)
		{
			_xPos = xPos;
			_yPos = yPos;
			_width = width;
			_height = height;
			_fixedSize = fixedSize;
			_variableSize = variableSize;
			_alignment = alignment;

			backgroundImage = new BBUniStretchableImage();
			foregroundImage = new BBUniStretchableImage();

			if (!backgroundImage.Initialize(xPos, yPos, width, height, fixedSize, variableSize, isHorizontal, backgroundSections, r, out reason))
			{
				return false;
			}
			if (!foregroundImage.Initialize(xPos, yPos, width, height, fixedSize, variableSize, isHorizontal, foregroundSections, r, out reason))
			{
				return false;
			}

			_label = new BBLabel();
			if (!_label.Initialize(0, 0, buttonText, Color.White, out reason))
			{
				return false;
			}

			Reset();

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Reset()
		{
			pulse = 0;
			direction = false;
			Active = true;
			pressed = false;
			Selected = false;
		}

		public void SetButtonText(string text)
		{
			_label.SetText(text);
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
		}

		public void MoveTo(int x, int y)
		{
			_xPos = x;
			_yPos = y;
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
			backgroundImage.MoveTo(x, y);
			foregroundImage.MoveTo(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this._width = width;
			this._height = height;
			backgroundImage.Resize(width, height);
			foregroundImage.Resize(width, height);
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (pulse < 0.6f)
				{
					pulse = 0.9f;
				}
				if (Active)
				{
					if (direction)
					{
						pulse += frameDeltaTime / 2;
						if (pulse > 0.9f)
						{
							direction = !direction;
							pulse = 0.9f;
						}
					}
					else
					{
						pulse -= frameDeltaTime / 2;
						if (pulse < 0.6f)
						{
							direction = !direction;
							pulse = 0.6f;
						}
					}
				}
				return true;
			}
			if (pulse > 0)
			{
				pulse -= frameDeltaTime * 2;
				if (pulse < 0)
				{
					pulse = 0;
				}
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (Active)
				{
					pressed = true;
				}
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				pressed = false;
				if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
				{
					return true;
				}
			}
			return false;
		}

		public void Draw()
		{
			if (Active)
			{
				if (pressed || Selected)
				{
					foregroundImage.Draw();
				}
				else if (!Selected)
				{
					backgroundImage.Draw();
					if (pulse > 0)
					{
						foregroundImage.Draw((byte)(255 * pulse));
					}
				}
			}
			else
			{
				backgroundImage.Draw(Color.Tan, 255);
				if (Selected)
				{
					foregroundImage.Draw(Color.Tan, 255);
				}
			}
			if (_label.Text.Length > 0)
			{
				_label.Draw();
			}
		}
		#endregion
	}

	public class BBStretchButton
	{
		#region Member Variables
		private BBToolTip _toolTip;
		protected BBStretchableImage _backgroundImage;
		protected BBStretchableImage _foregroundImage;
		protected BBLabel _label;
		protected ButtonTextAlignment _alignment;

		//Button state information
		protected bool _pressed;
		protected float _pulse;
		protected bool _direction;

		protected int _xPos;
		protected int _yPos;
		protected int _width;
		protected int _height;
		protected bool _doubleClicked;
		protected float _timeSinceClick;
		#endregion

		#region Properties
		public bool Enabled { get; set; }
		public bool Selected { get; set; }
		public string Text { get; private set; }
		public bool DoubleClicked
		{
			get
			{
				var value = _doubleClicked;
				_doubleClicked = false; //Reset it
				return value;
			}
		}
		#endregion

		#region Constructors
		public bool Initialize(string buttonText, ButtonTextAlignment alignment, StretchableImageType background, StretchableImageType foreground, int xPos, int yPos, int width, int height, Random r, out string reason)
		{
			_xPos = xPos;
			_yPos = yPos;
			_width = width;
			_height = height;
			_alignment = alignment;

			_backgroundImage = new BBStretchableImage();
			_foregroundImage = new BBStretchableImage();

			if (!_backgroundImage.Initialize(xPos, yPos, width, height, background, r, out reason))
			{
				return false;
			}
			if (!_foregroundImage.Initialize(xPos, yPos, width, height, foreground, r, out reason))
			{
				return false;
			}

			_label = new BBLabel();
			if (!_label.Initialize(0, 0, string.Empty, Color.White, out reason))
			{
				return false;
			}
			SetText(buttonText);

			Reset();
			_timeSinceClick = 10; //10 seconds will exceed any double-click max interval
			_doubleClicked = false;

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Reset()
		{
			_pulse = 0;
			_direction = false;
			Enabled = true;
			_pressed = false;
			Selected = false;
			_timeSinceClick = 10;
			_doubleClicked = false;
		}

		public void SetText(string text)
		{
			_label.SetText(text);
			Text = text;
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
		}
		public bool SetToolTip(string name, string text, int screenWidth, int screenHeight, Random r, out string reason)
		{
			_toolTip = new BBToolTip();
			if (!_toolTip.Initialize(name, text, screenWidth, screenHeight, r, out reason))
			{
				return false;
			}
			return true;
		}
		public void SetToolTipText(string text)
		{
			_toolTip.SetText(text);
		}
		public void SetTextColor(Color color, Color outline)
		{
			_label.SetColor(color, outline);
		}

		public void MoveTo(int x, int y)
		{
			_xPos = x;
			_yPos = y;
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
			_backgroundImage.MoveTo(x, y);
			_foregroundImage.MoveTo(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this._width = width;
			this._height = height;
			_backgroundImage.Resize(width, height);
			_foregroundImage.Resize(width, height);
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (_timeSinceClick < 10)
			{
				_timeSinceClick += frameDeltaTime;
			}
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (_pulse < 0.6f)
				{
					_pulse = 0.9f;
				}
				if (Enabled)
				{
					if (_direction)
					{
						_pulse += frameDeltaTime / 2;
						if (_pulse > 0.9f)
						{
							_direction = !_direction;
							_pulse = 0.9f;
						}
					}
					else
					{
						_pulse -= frameDeltaTime / 2;
						if (_pulse < 0.6f)
						{
							_direction = !_direction;
							_pulse = 0.6f;
						}
					}
				}
				if (_toolTip != null)
				{
					_toolTip.SetShowing(true);
					_toolTip.MouseHover(x, y, frameDeltaTime);
				}
				return true;
			}
			if (_pulse > 0)
			{
				_pulse -= frameDeltaTime * 2;
				if (_pulse < 0)
				{
					_pulse = 0;
				}
			}
			if (_toolTip != null)
			{
				_toolTip.SetShowing(false);
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (_toolTip != null)
				{
					_toolTip.SetShowing(false);
				}
				if (Enabled)
				{
					_pressed = true;
				}
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Enabled && _pressed)
			{
				if (_toolTip != null)
				{
					_toolTip.SetShowing(false);
				}
				_pressed = false;
				if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
				{
					if (_timeSinceClick < 0.3)
					{
						_doubleClicked = true;
					}
					_timeSinceClick = 0;
					return true;
				}
			}
			return false;
		}

		public void Draw()
		{
			if (Enabled)
			{
				if (_pressed || Selected)
				{
					_foregroundImage.Draw();
				}
				else if (!Selected)
				{
					_backgroundImage.Draw();
					if (_pulse > 0)
					{
						_foregroundImage.Draw((byte)(255 * _pulse));
					}
				}
			}
			else
			{
				_backgroundImage.Draw(Color.Tan, 255);
				if (Selected)
				{
					_foregroundImage.Draw(Color.Tan, 255);
				}
			}
			if (_label.Text.Length > 0)
			{
				_label.Draw();
			}
		}
		public void DrawToolTip()
		{
			if (_toolTip != null)
			{
				_toolTip.Draw();
			}
		}
		#endregion
	}

	public class BBInvisibleStretchButton : BBStretchButton
	{
		private bool _visible;

		new public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			_visible = base.MouseHover(x, y, frameDeltaTime);
			return _visible;
		}

		new public void Draw()
		{
			if (Enabled && (_visible || Selected))
			{
				if (_pressed || Selected)
				{
					_foregroundImage.Draw();
				}
				else if (!Selected)
				{
					_backgroundImage.Draw();
					if (_pulse > 0)
					{
						_foregroundImage.Draw((byte)(255 * _pulse));
					}
				}
			}
			if (_label.Text.Length > 0)
			{
				_label.Draw();
			}
		}
	}

	public class BBComboBox
	{
		#region Member Variables
		//ComboBox drawing information
		private BBSprite _downArrowSprite;
		private int _xPos;
		private int _yPos;
		private int _width;
		private int _height;
		private List<string> _items;
		private List<BBInvisibleStretchButton> _buttons;
		private BBStretchableImage _dropBackground;
		private BBScrollBar _scrollBar;

		//ComboBox state information
		private bool _haveScroll;
		private int _selectedIndex;

		#endregion

		#region Properties
		public bool Enabled { get; set; }
		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set 
			{ 
				_selectedIndex = value;
				_buttons[0].SetText(_items[_selectedIndex]);
			}
		}

		public bool Dropped { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		/// <param name="xPos"></param>
		/// <param name="yPos"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="maxVisible"></param>
		/// <param name="r"></param>
		/// <param name="reason"></param>
		public bool Initialize(List<string> items, int xPos, int yPos, int width, int height, int maxVisible, Random r, out string reason)
		{
			_items = items;
			_xPos = xPos;
			_yPos = yPos;
			_width = width;
			_height = height;

			_selectedIndex = 0;
			Dropped = false;
			_downArrowSprite = SpriteManager.GetSprite("ScrollDownBGButton", r);

			if (items.Count < maxVisible)
			{
				_haveScroll = false;
				maxVisible = items.Count;
			}
			else if (items.Count > maxVisible)
			{
				_haveScroll = true;
			}

			Enabled = true;

			_buttons = new List<BBInvisibleStretchButton>();
			_dropBackground = new BBStretchableImage();
			_scrollBar = new BBScrollBar();
			
			for (int i = 0; i <= maxVisible; i++)
			{
				BBInvisibleStretchButton button = new BBInvisibleStretchButton();
				if (!button.Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos, _yPos + (i * height), _width, _height, r, out reason))
				{
					return false;
				}
				_buttons.Add(button);
			}
			if (!_dropBackground.Initialize(_xPos, _yPos, width, height, StretchableImageType.ThinBorderBG, r, out reason))
			{
				return false;
			}
			if (!_scrollBar.Initialize(_xPos + _width, _yPos + _height, maxVisible * _height, maxVisible, items.Count, false, false, r, out reason))
			{
				return false;
			}
			RefreshSelection();
			RefreshLabels();
			return true;
		}
		#endregion

		#region Functions
		public void MoveTo(int x, int y)
		{
			_xPos = x;
			_yPos = y;

			for (int i = 0; i < _buttons.Count; i++)
			{
				_buttons[i].MoveTo(x, y + (i * _height));
			}

			_scrollBar.MoveTo(_xPos + _width, _yPos + _height);
			_dropBackground.MoveTo(_xPos, _yPos);
		}

		public void Draw()
		{
			if (_items.Count <= 0)
			{
				_buttons[0].Enabled = false;
			}
			else
			{
				_buttons[0].Enabled = Enabled;
			}
			if (!Dropped)
			{
				_dropBackground.Draw();
				_buttons[0].Draw();
				_downArrowSprite.Draw(_xPos + _width - 25, _yPos + (_height / 2) - (_downArrowSprite.Height / 2));
			}
			else
			{
				_dropBackground.Draw();
				for (int i = 0; i < _buttons.Count; i++)
				{
					_buttons[i].Draw();
				}
				if (_haveScroll)
				{
					_scrollBar.Draw();
				}
			}
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (Enabled)
			{
				if (!Dropped)
				{
					return _buttons[0].MouseHover(x, y, frameDeltaTime);
				}
				bool result = false;
				foreach (var button in _buttons)
				{
					result = button.MouseHover(x, y, frameDeltaTime) || result;
				}
				if (_scrollBar.MouseHover(x, y, frameDeltaTime))
				{
					//Need to refresh the button text
					RefreshLabels();
					result = true;
				}
				return result;
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (Enabled)
			{
				if (!Dropped)
				{
					return _buttons[0].MouseDown(x, y);
				}
				for (int i = 0; i < _buttons.Count; i++)
				{
					if (_buttons[i].MouseDown(x, y))
					{
						return true;
					}
				}
				return _scrollBar.MouseDown(x, y);
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Enabled)
			{
				if (!Dropped)
				{
					if (_buttons[0].MouseUp(x, y))
					{
						Dropped = true;
						_dropBackground.Resize(_width + 30, _height * (_items.Count + 1) + 20);
						_dropBackground.MoveTo(_xPos - 10, _yPos - 10);
						return true;
					}
				}
				else
				{
					for (int i = 0; i < _buttons.Count; i++)
					{
						if (_buttons[i].MouseUp(x, y))
						{
							if (i > 0)
							{
								_selectedIndex = i + _scrollBar.TopIndex - 1;
								_buttons[0].SetText(_buttons[i].Text);
							}
							Dropped = false;
							_dropBackground.Resize(_width, _height);
							_dropBackground.MoveTo(_xPos, _yPos);
							return true;
						}
					}
					if (_scrollBar.MouseUp(x, y))
					{
						RefreshLabels();
						return true;
					}
					//At this point, even if the mouse is not over the UI, we want to capture the mouse up so the user don't click on something else
					Dropped = false;
					return true;
				}
			}
			return false;
		}

		private void RefreshLabels()
		{
			for (int i = 1; i < _buttons.Count; i++)
			{
				_buttons[i].SetText(_items[_scrollBar.TopIndex + (i - 1)]);
			}
		}
		private void RefreshSelection()
		{
			_buttons[0].SetText(_items[_selectedIndex]);
			Dropped = false;
		}
		#endregion
	}

	public class BBScrollBar
	{
		// TODO: Fix scrollbar so if I say 100 items, I don't need to specify 101 for amountOfItems
		#region Member Variables
		//Variables that are defined in constructor
		private int xPos;
		private int yPos;
		private BBButton Up;
		private BBButton Down;
		private BBUniStretchButton Scroll;
		private BBSprite scrollBar;
		private BBSprite highlightedScrollBar;
		private int amountOfItems;
		private int amountVisible;
		private int scrollBarLength;

		private int topIndex; //Which topmost item is visible
		private int scrollPos;
		private bool scrollSelected; //is the scroll button selected? if so, drag it
		private int initialMousePos;
		private int initialScrollPos;
		private bool isHorizontal;
		private bool isSlider; //Is the scroll bar's behavior like a scroll bar or a slider?
		private bool isEnabled;

		//Variables that are calculated from the values passed into the constructor
		private int scrollButtonLength;

		#endregion

		#region Properties
		public int TopIndex
		{
			get { return topIndex; }
			set
			{
				topIndex = value;
				SetScrollButtonPosition();
			}
		}
		#endregion

		#region Constructors
		public bool Initialize(int xPos, int yPos, int length, int amountOfVisibleItems, int amountOfItems, bool isHorizontal, bool isSlider, Random r, out string reason)
		{
			this.xPos = xPos;
			this.yPos = yPos;
			Up = new BBButton();
			Down = new BBButton();
			Scroll = new BBUniStretchButton();
			
			this.amountOfItems = amountOfItems;
			this.amountVisible = amountOfVisibleItems;
			this.isSlider = isSlider;
			this.isHorizontal = isHorizontal;

			scrollBarLength = length - 32;

			if (!isSlider)
			{
				scrollButtonLength = (int)(((float)amountOfVisibleItems / amountOfItems) * scrollBarLength);
				if (!isHorizontal)
				{
					if (!Up.Initialize("ScrollUpBGButton", "ScrollUpFGButton", "", ButtonTextAlignment.CENTER, xPos, yPos, 16, 16, r, out reason))
					{
						return false;
					}
					if (!Down.Initialize("ScrollDownBGButton", "ScrollDownFGButton", "", ButtonTextAlignment.CENTER, xPos, yPos + length - 16, 16, 16, r, out reason))
					{
						return false;
					}
					if (!Scroll.Initialize(new List<string> { "ScrollVerticalBGButton1", "ScrollVerticalBGButton2", "ScrollVerticalBGButton3" }, 
										   new List<string> { "ScrollVerticalFGButton1", "ScrollVerticalFGButton2", "ScrollVerticalFGButton3" },
										   false, "", ButtonTextAlignment.LEFT, xPos, yPos + 16, 16, 7, 2, scrollButtonLength, r, out reason))
					{
						return false;
					}
					scrollBar = SpriteManager.GetSprite("ScrollVerticalBar", r);
					if (scrollBar == null)
					{
						reason = "\"ScrollVerticalBar\" sprite does not exist";
						return false;
					}
				}
				else
				{
					if (!Up.Initialize("ScrollLeftBGButton", "ScrollLeftFGButton", "", ButtonTextAlignment.CENTER, xPos, yPos, 16, 16, r, out reason))
					{
						return false;
					}
					if (!Down.Initialize("ScrollRightBGButton", "ScrollRightFGButton", "", ButtonTextAlignment.CENTER, xPos + length - 16, yPos, 16, 16, r, out reason))
					{
						return false;
					}
					if (!Scroll.Initialize(new List<string> { "ScrollHorizontalBGButton1", "ScrollHorizontalBGButton2", "ScrollHorizontalBGButton3" },
										   new List<string> { "ScrollHorizontalFGButton1", "ScrollHorizontalFGButton2", "ScrollHorizontalFGButton3" },
										   false, "", ButtonTextAlignment.LEFT, xPos + 16, yPos, 16, 7, 2, scrollButtonLength, r, out reason))
					{
						return false;
					}
					scrollBar = SpriteManager.GetSprite("ScrollHorizontalBar", r);
					if (scrollBar == null)
					{
						reason = "\"ScrollHorizontalBar\" sprite does not exist";
						return false;
					}
				}
			}
			else
			{
				scrollButtonLength = 16;
				if (!Up.Initialize("ScrollLeftBGButton", "ScrollLeftFGButton", "", ButtonTextAlignment.CENTER, xPos, yPos, 16, 16, r, out reason))
				{
					return false;
				}
				if (!Down.Initialize("ScrollRightBGButton", "ScrollRightFGButton", "", ButtonTextAlignment.CENTER, xPos + length - 16, yPos, 16, 16, r, out reason))
				{
					return false;
				}
				if (!Scroll.Initialize(new List<string> { "SliderHorizontalBGButton1", "SliderHorizontalBGButton2", "SliderHorizontalBGButton3" },
									   new List<string> { "SliderHorizontalFGButton1", "SliderHorizontalFGButton2", "SliderHorizontalFGButton3" },
									   true, "", ButtonTextAlignment.LEFT, xPos + 16, yPos, 16, 7, 2, scrollButtonLength, r, out reason))
				{
					return false;
				}
				scrollBar = SpriteManager.GetSprite("SliderBGBar", r);
				if (scrollBar == null)
				{
					reason = "\"SliderBGBar\" sprite does not exist";
					return false;
				}
				highlightedScrollBar = SpriteManager.GetSprite("SliderFGBar", r);
				if (highlightedScrollBar == null)
				{
					reason = "\"SliderFGBar\" sprite does not exist";
					return false;
				}
			}

			topIndex = 0;
			scrollPos = 0; //relative to the scrollbar itself
			scrollSelected = false;
			isEnabled = true;
			reason = null;
			return true;
		}
		#endregion

		#region Private Functions
		private void SetScrollButtonPosition()
		{
			scrollPos = (int)(((float)topIndex / (amountOfItems - amountVisible)) * (scrollBarLength - scrollButtonLength));
			if (scrollPos < 0)
			{
				scrollPos = 0;
			}
			else if (scrollPos > (scrollBarLength - scrollButtonLength))
			{
				scrollPos = scrollBarLength - scrollButtonLength;
			}
			if (isHorizontal)
			{
				Scroll.MoveTo(xPos + 16 + scrollPos, yPos);
			}
			else
			{
				Scroll.MoveTo(xPos, yPos + 16 + scrollPos);
			}
		}
		#endregion

		#region Public Functions
		public void Draw()
		{
			System.Drawing.Color enabledColor = isEnabled ? System.Drawing.Color.White : System.Drawing.Color.Tan;
			if (!isSlider)
			{
				if (!isHorizontal)
				{
					scrollBar.Draw(xPos, yPos + 16, 1, scrollBarLength / scrollBar.Height, enabledColor);
				}
				else
				{
					scrollBar.Draw(xPos + 16, yPos, scrollBarLength / scrollBar.Width, 1, enabledColor);
				}
			}
			else
			{
				scrollBar.Draw(xPos + 16, yPos, scrollBarLength / scrollBar.Width, 1, enabledColor);
				highlightedScrollBar.Draw(xPos + 16, yPos, scrollPos / highlightedScrollBar.Width, 1, enabledColor);
			}
			Up.Draw();
			Down.Draw();
			Scroll.Draw();
		}

		public bool MouseDown(int x, int y)
		{
			if (isEnabled)
			{
				if (Up.MouseDown(x, y))
				{
					return true;
				}
				if (Down.MouseDown(x, y))
				{
					return true;
				}
				if (!isSlider && Scroll.MouseDown(x, y))
				{
					scrollSelected = true;
					if (isHorizontal)
					{
						initialMousePos = x;
					}
					else
					{
						initialMousePos = y;
					}
					initialScrollPos = scrollPos;
					return true;
				}
				//at this point, only the scroll bar itself is left
				if ((isHorizontal && (x >= xPos + 16 && x < xPos + 16 + scrollBarLength && yPos <= y && y < yPos + 16))
					|| (!isHorizontal && (x >= xPos && x < xPos + 16 && yPos + 16 <= y && y < yPos + 16 + scrollBarLength)))
				{
					if (!isSlider)
					{
						//clicked on the bar itself, jump up one page
						if ((!isHorizontal && y < yPos + 16 + scrollPos) || (isHorizontal && x < xPos + 16 + scrollPos))
						{
							topIndex -= amountVisible;
							if (topIndex < 0)
							{
								topIndex = 0;
							}
						}
						else
						{
							//since up is checked already, jump down one page
							topIndex += amountVisible;
							if (topIndex > (amountOfItems - amountVisible))
							{
								topIndex = (amountOfItems - amountVisible);
							}
						}
						SetScrollButtonPosition();
					}
					else
					{
						scrollSelected = true;
					}
					return true;
				}
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (isEnabled)
			{
				bool changed = false;
				if (Up.MouseUp(x, y))
				{
					if (TopIndex > 0)
					{
						TopIndex--;
					}
					changed = true;
				}
				else if (Down.MouseUp(x, y))
				{
					if (TopIndex < amountOfItems - amountVisible)
					{
						TopIndex++;
					}
					changed = true;
				}
				if (changed || scrollSelected)
				{
					scrollSelected = false;
					if (!isSlider)
					{
						SetScrollButtonPosition();
						Scroll.MouseUp(x, y);
					}
					return true;
				}
				if (x >= xPos && x < xPos + 16 + (isHorizontal ? scrollBarLength : 0) && yPos + 16 <= y && y < yPos + 16 + (isHorizontal ? 0 : scrollBarLength))
				{
					return true;
				}
			}
			return false;
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (isEnabled)
			{
				Scroll.MouseHover(x, y, frameDeltaTime);
				if (scrollSelected)
				{
					int newPosition = 0;
					if (isHorizontal)
					{
						newPosition = initialScrollPos + (x - (isSlider ? (xPos + 16 + (scrollButtonLength / 2)) : initialMousePos));
					}
					else
					{
						newPosition = initialScrollPos + (y - (isSlider ? (yPos + 16 + (scrollButtonLength / 2)) : initialMousePos));
					}
					if (newPosition < 0)
					{
						newPosition = 0;
					}
					else if (newPosition > (scrollBarLength - scrollButtonLength))
					{
						newPosition = scrollBarLength - scrollButtonLength;
					}
					float itemsPerIncrement = ((float)(amountOfItems - amountVisible) / (float)(scrollBarLength - scrollButtonLength));
					int oldIndex = topIndex;
					topIndex = (int)((itemsPerIncrement * newPosition) + 0.5f);
					SetScrollButtonPosition();
					return !(oldIndex == topIndex);
				}
				Up.MouseHover(x, y, frameDeltaTime);
				Down.MouseHover(x, y, frameDeltaTime);
				return false;
			}
			return false;
		}

		public void MoveTo(int x, int y)
		{
			Up.MoveTo(x, y);
			if (isHorizontal)
			{
				Down.MoveTo(x + scrollBarLength + 16, y);
				Scroll.MoveTo(x + 16 + scrollPos, y);
			}
			else
			{
				Down.MoveTo(x, y + scrollBarLength + 16);
				Scroll.MoveTo(x, y + 16 + scrollPos);
			}
			xPos = x;
			yPos = y;
		}

		public void SetAmountOfItems(int amount)
		{
			topIndex = 0;
			amountOfItems = amount;
			if (!isSlider)
			{
				scrollButtonLength = (int)(((float)amountVisible / amountOfItems) * scrollBarLength);
				if (scrollButtonLength < 16)
				{
					scrollButtonLength = 16;
				}
				if (isHorizontal)
				{
					Scroll.ResizeButton(scrollButtonLength, 16);
				}
				else
				{
					Scroll.ResizeButton(16, scrollButtonLength);
				}
			}
			SetScrollButtonPosition();
		}

		public void SetEnabledState(bool enabled)
		{
			Up.Active = enabled;
			Down.Active = enabled;
			Scroll.Active = enabled;
			isEnabled = enabled;
		}
		#endregion
	}

	public class BBScrollBarNoArrows
	{
		// TODO: Fix scrollbar so if I say 100 items, I don't need to specify 101 for _amountOfItems
		#region Member Variables
		//Variables that are defined in constructor
		private int _xPos;
		private int _yPos;
		private BBUniStretchButton _scroll;
		private BBSprite _scrollBar;
		private BBSprite _highlightedScrollBar;
		private int _amountOfItems;
		private int _amountVisible;
		private int _scrollBarLength;

		private int _topIndex; //Which topmost item is visible
		private int _scrollPos;
		private bool _scrollSelected; //is the scroll button selected? if so, drag it
		private int _initialMousePos;
		private int _initialScrollPos;
		private bool _isHorizontal;
		private bool _isSlider; //Is the scroll bar's behavior like a scroll bar or a slider?
		private bool _isEnabled;

		//Variables that are calculated from the values passed into the constructor
		private int _scrollButtonLength;

		#endregion

		#region Properties
		public int TopIndex
		{
			get { return _topIndex; }
			set
			{
				_topIndex = value;
				SetScrollButtonPosition();
			}
		}
		#endregion

		#region Constructors
		public bool Initialize(int xPos, int yPos, int length, int amountOfVisibleItems, int amountOfItems, bool isHorizontal, bool isSlider, Random r, out string reason)
		{
			_xPos = xPos;
			_yPos = yPos;
			_scroll = new BBUniStretchButton();

			_amountOfItems = amountOfItems;
			_amountVisible = amountOfVisibleItems;
			_isSlider = isSlider;
			_isHorizontal = isHorizontal;

			_scrollBarLength = length;

			if (!isSlider)
			{
				_scrollButtonLength = (int)(((float)amountOfVisibleItems / amountOfItems) * _scrollBarLength);
				if (!isHorizontal)
				{
					if (!_scroll.Initialize(new List<string> { "TinyScrollVerticalBGButton1", "TinyScrollVerticalBGButton2", "TinyScrollVerticalBGButton3" },
										   new List<string> { "TinyScrollVerticalFGButton1", "TinyScrollVerticalFGButton2", "TinyScrollVerticalFGButton3" },
										   isHorizontal, "", ButtonTextAlignment.LEFT, xPos, yPos, 5, _scrollButtonLength, 3, 1, r, out reason))
					{
						return false;
					}
					_scrollBar = SpriteManager.GetSprite("TinyScrollVerticalBar", r);
					if (_scrollBar == null)
					{
						reason = "\"TinyScrollVerticalBar\" sprite does not exist";
						return false;
					}
				}
				else
				{
					if (!_scroll.Initialize(new List<string> { "TinyScrollHorizontalBGButton1", "TinyScrollHorizontalBGButton2", "TinyScrollHorizontalBGButton3" },
										   new List<string> { "TinyScrollHorizontalFGButton1", "TinyScrollHorizontalFGButton2", "TinyScrollHorizontalFGButton3" },
										   isHorizontal, "", ButtonTextAlignment.LEFT, xPos, yPos, _scrollButtonLength, 5, 3, 1, r, out reason))
					{
						return false;
					}
					_scrollBar = SpriteManager.GetSprite("TinyScrollHorizontalBar", r);
					if (_scrollBar == null)
					{
						reason = "\"TinyScrollHorizontalBar\" sprite does not exist";
						return false;
					}
				}
			}
			else
			{
				_scrollButtonLength = 16;
				if (!_scroll.Initialize(new List<string> { "TinySliderHorizontalBGButton1", "TinySliderHorizontalBGButton2", "TinySliderHorizontalBGButton3" },
									   new List<string> { "TinySliderHorizontalFGButton1", "TinySliderHorizontalFGButton2", "TinySliderHorizontalFGButton3" },
									   true, "", ButtonTextAlignment.LEFT, xPos, yPos, _scrollButtonLength, 5, 3, 1, r, out reason))
				{
					return false;
				}
				_scrollBar = SpriteManager.GetSprite("TinySliderBGBar", r);
				if (_scrollBar == null)
				{
					reason = "\"TinySliderBGBar\" sprite does not exist";
					return false;
				}
				_highlightedScrollBar = SpriteManager.GetSprite("TinySliderFGBar", r);
				if (_highlightedScrollBar == null)
				{
					reason = "\"SliderFGBar\" sprite does not exist";
					return false;
				}
			}

			_topIndex = 0;
			_scrollPos = 0; //relative to the scrollbar itself
			_scrollSelected = false;
			_isEnabled = true;
			reason = null;
			return true;
		}
		#endregion

		#region Private Functions
		private void SetScrollButtonPosition()
		{
			_scrollPos = (int)(((float)_topIndex / (_amountOfItems - _amountVisible)) * (_scrollBarLength - _scrollButtonLength));
			if (_scrollPos < 0)
			{
				_scrollPos = 0;
			}
			else if (_scrollPos > (_scrollBarLength - _scrollButtonLength))
			{
				_scrollPos = _scrollBarLength - _scrollButtonLength;
			}
			if (_isHorizontal)
			{
				_scroll.MoveTo(_xPos + _scrollPos, _yPos);
			}
			else
			{
				_scroll.MoveTo(_xPos, _yPos + _scrollPos);
			}
		}
		#endregion

		#region Public Functions
		public void Draw()
		{
			Color enabledColor = _isEnabled ? Color.White : Color.Tan;
			if (!_isSlider)
			{
				if (!_isHorizontal)
				{
					_scrollBar.Draw(_xPos, _yPos, 1, _scrollBarLength / _scrollBar.Height, enabledColor);
				}
				else
				{
					_scrollBar.Draw(_xPos, _yPos, _scrollBarLength / _scrollBar.Width, 1, enabledColor);
				}
			}
			else
			{
				_scrollBar.Draw(_xPos, _yPos, _scrollBarLength / _scrollBar.Width, 1, enabledColor);
				_highlightedScrollBar.Draw(_xPos, _yPos, _scrollPos / _highlightedScrollBar.Width, 1, enabledColor);
			}
			_scroll.Draw();
		}

		public bool MouseDown(int x, int y)
		{
			if (_isEnabled)
			{
				if (!_isSlider && _scroll.MouseDown(x, y))
				{
					_scrollSelected = true;
					if (_isHorizontal)
					{
						_initialMousePos = x;
					}
					else
					{
						_initialMousePos = y;
					}
					_initialScrollPos = _scrollPos;
					return true;
				}
				//at this point, only the scroll bar itself is left
				if ((_isHorizontal && (x >= _xPos && x < _xPos + _scrollBarLength && _yPos <= y && y < _yPos + 5))
					|| (!_isHorizontal && (x >= _xPos && x < _xPos + 5 && _yPos <= y && y < _yPos + _scrollBarLength)))
				{
					if (!_isSlider)
					{
						//clicked on the bar itself, jump up one page
						if ((!_isHorizontal && y < _yPos + _scrollPos) || (_isHorizontal && x < _xPos + _scrollPos))
						{
							_topIndex -= _amountVisible;
							if (_topIndex < 0)
							{
								_topIndex = 0;
							}
						}
						else
						{
							//since up is checked already, jump down one page
							_topIndex += _amountVisible;
							if (_topIndex > (_amountOfItems - _amountVisible))
							{
								_topIndex = (_amountOfItems - _amountVisible);
							}
						}
						SetScrollButtonPosition();
					}
					else
					{
						_scrollSelected = true;
					}
					return true;
				}
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (_isEnabled)
			{
				if (_scrollSelected)
				{
					_scrollSelected = false;
					if (!_isSlider)
					{
						SetScrollButtonPosition();
						_scroll.MouseUp(x, y);
					}
					return true;
				}
				if (x >= _xPos && x < _xPos + (_isHorizontal ? _scrollBarLength : 5) && _yPos <= y && y < _yPos + (_isHorizontal ? 5 : _scrollBarLength))
				{
					return true;
				}
			}
			return false;
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (_isEnabled)
			{
				_scroll.MouseHover(x, y, frameDeltaTime);
				if (_scrollSelected)
				{
					int newPosition;
					if (_isHorizontal)
					{
						newPosition = _initialScrollPos + (x - (_isSlider ? (_xPos + (_scrollButtonLength / 2)) : _initialMousePos));
					}
					else
					{
						newPosition = _initialScrollPos + (y - (_isSlider ? (_yPos + (_scrollButtonLength / 2)) : _initialMousePos));
					}
					if (newPosition < 0)
					{
						newPosition = 0;
					}
					else if (newPosition > (_scrollBarLength - _scrollButtonLength))
					{
						newPosition = _scrollBarLength - _scrollButtonLength;
					}
					float itemsPerIncrement = ((_amountOfItems - _amountVisible) / (float)(_scrollBarLength - _scrollButtonLength));
					int oldIndex = _topIndex;
					_topIndex = (int)((itemsPerIncrement * newPosition) + 0.5f);
					SetScrollButtonPosition();
					return oldIndex != _topIndex;
				}
				return false;
			}
			return false;
		}

		public void MoveTo(int x, int y)
		{
			if (_isHorizontal)
			{
				_scroll.MoveTo(x + _scrollPos, y);
			}
			else
			{
				_scroll.MoveTo(x, y + _scrollPos);
			}
			_xPos = x;
			_yPos = y;
		}

		public void SetAmountOfItems(int amount)
		{
			_topIndex = 0;
			_amountOfItems = amount;
			if (!_isSlider)
			{
				_scrollButtonLength = (int)(((float)_amountVisible / _amountOfItems) * _scrollBarLength);
				if (_scrollButtonLength < 16)
				{
					_scrollButtonLength = 16;
				}
				if (_isHorizontal)
				{
					_scroll.ResizeButton(_scrollButtonLength, 16);
				}
				else
				{
					_scroll.ResizeButton(16, _scrollButtonLength);
				}
			}
			SetScrollButtonPosition();
		}

		public void SetEnabledState(bool enabled)
		{
			_scroll.Active = enabled;
			_isEnabled = enabled;
		}

		public void ScrollToBottom()
		{
			TopIndex = (_amountOfItems - _amountVisible);
			SetScrollButtonPosition();
		}
		#endregion
	}

	public class BBNumericUpDown
	{
		#region Member Variables
		private BBButton _upButton;
		private BBButton _downButton;
		private int _minimum;
		private int _maximum;
		private BBLabel _valueLabel;
		private int _incrementAmount;
		private int _width;
		private bool _enabled;
		private bool _upButtonEnabled;
		#endregion

		#region Properties

		public int Value { get; private set; }
		public bool Enabled
		{
			get { return _enabled; } 
			set
			{
				_enabled = value;
				RefreshButtons();
			}
		}
		public bool UpButtonEnabled
		{
			get { return _upButtonEnabled; }
			set
			{
				_upButtonEnabled = value;
				RefreshButtons();
			}
		}

		#endregion

		#region Constructors
		public bool Initialize(int xPos, int yPos, int width, int min, int max, int initialAmount, Random r, out string reason)
		{
			_enabled = true;
			_upButtonEnabled = true;

			_width = width;

			_upButton = new BBButton();
			_downButton = new BBButton();
			_valueLabel = new BBLabel();

			if (!_upButton.Initialize("ScrollUpBGButton", "ScrollUpFGButton", string.Empty, ButtonTextAlignment.LEFT, xPos + width - 16, yPos, 16, 16, r, out reason))
			{
				return false;
			}
			if (!_downButton.Initialize("ScrollDownBGButton", "ScrollDownFGButton", string.Empty, ButtonTextAlignment.LEFT, xPos, yPos, 16, 16, r, out reason))
			{
				return false;
			}
			if (!_valueLabel.Initialize(xPos + width - 20, yPos, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_valueLabel.SetAlignment(true);

			_minimum = min;
			_maximum = max;
			Value = initialAmount;
			CheckAmount(); //Just in case

			_incrementAmount = 1;

			return true;
		}

		public bool Initialize(int xPos, int yPos, int width, int min, int max, int initialAmount, int incrementAmount, Random r, out string reason)
		{
			if (!Initialize(xPos, yPos, width, min, max, initialAmount, r, out reason))
			{
				return false;
			}
			_incrementAmount = incrementAmount;
			return true;
		}
		#endregion

		#region Functions
		public bool MouseUp(int x, int y)
		{
			if (_upButton.MouseUp(x, y))
			{
				Value += _incrementAmount;
				CheckAmount();
				return true;
			}
			if (_downButton.MouseUp(x, y))
			{
				Value -= _incrementAmount;
				CheckAmount();
				return true;
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (_upButton.MouseDown(x, y))
			{
				return true;
			}
			if (_downButton.MouseDown(x, y))
			{
				return true;
			}
			return false;
		}
		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			if (_upButton.MouseHover(x, y, frameDeltaTime))
			{
				result = true;
			}
			if (_downButton.MouseHover(x, y, frameDeltaTime))
			{
				result = true;
			}
			return result;
		}

		public void MoveTo(int x, int y)
		{
			_upButton.MoveTo(x + _width - 16, y);
			_downButton.MoveTo(x, y);
			_valueLabel.MoveTo(x + 20, y);
		}

		public void Draw()
		{
			_upButton.Draw();
			_downButton.Draw();
			_valueLabel.Draw();
		}

		public void SetMin(int min)
		{
			_minimum = min;
			CheckAmount();
		}

		public void SetMax(int max)
		{
			_maximum = max;
			CheckAmount();
		}

		public void SetValue(int value)
		{
			this.Value = value;
			CheckAmount();
		}

		private void CheckAmount()
		{
			if (_minimum >= 0 && Value < _minimum)
			{
				Value = _minimum;
			}
			if (_maximum >= 0 && Value > _maximum)
			{
				Value = _maximum;
			}
			_valueLabel.SetText(Value.ToString());
			RefreshButtons();
		}

		private void RefreshButtons()
		{
			_downButton.Active = Value > _minimum && _enabled;
			_upButton.Active = Value < _maximum && _enabled && _upButtonEnabled;
		}

		#endregion
	}

	public enum StretchableImageType
	{
		ThickBorder,
		MediumBorder,
		ThinBorderBG,
		ThinBorderFG,
		TinyButtonBG,
		TinyButtonFG,
		TextBox
	}
	public class BBStretchableImage
	{
		#region Member Variables
		private int xPos;
		private int yPos;
		private int width;
		private int height;
		private int sectionWidth;
		private int sectionHeight;
		private int horizontalStretchLength;
		private int verticalStretchLength;
		private List<BBSprite> sections;
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public bool Initialize(int x, int y, int width, int height, StretchableImageType type, Random r, out string reason)
		{
			xPos = x;
			yPos = y;

			switch (type)
			{
				case StretchableImageType.TextBox:
					{
						sectionWidth = 30;
						sectionHeight = 13;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("TextTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					}
					break;
				case StretchableImageType.ThinBorderBG:
				{
					sectionWidth = 30;
					sectionHeight = 13;
					sections = new List<BBSprite>();
					var tempSprite = SpriteManager.GetSprite("ThinBorderBGTL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGTC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGTR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGCL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGCC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGCR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGBL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGBC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGBR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
				} break;
				case StretchableImageType.ThinBorderFG:
					{
						sectionWidth = 30;
						sectionHeight = 13;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("ThinBorderFGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					} break;
				case StretchableImageType.TinyButtonBG:
					{
						sectionWidth = 10;
						sectionHeight = 10;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("TinyButtonBGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					} break;
				case StretchableImageType.TinyButtonFG:
					{
						sectionWidth = 10;
						sectionHeight = 10;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("TinyButtonFGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					} break;
				case StretchableImageType.MediumBorder:
					{
						sectionWidth = 60;
						sectionHeight = 60;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("MediumBorderBGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					} break;
				case StretchableImageType.ThickBorder:
					{
						sectionWidth = 200;
						sectionHeight = 200;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("ThickBorderBGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					}
					break;
			}
			
			horizontalStretchLength = (width - (sectionWidth * 2));
			verticalStretchLength = (height - (sectionHeight * 2));

			if (horizontalStretchLength < 0)
			{
				horizontalStretchLength = 0;
			}
			if (verticalStretchLength < 0)
			{
				verticalStretchLength = 0;
			}

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Draw()
		{
			Draw(System.Drawing.Color.White, 255);
		}

		public void Draw(byte alpha)
		{
			Draw(System.Drawing.Color.White, alpha);
		}

		public void Draw(Color color, byte alpha)
		{
			sections[0].Draw(xPos, yPos, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[1].Draw(xPos + sectionWidth, yPos, horizontalStretchLength / sections[1].Width, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[2].Draw(xPos + sectionWidth + horizontalStretchLength, yPos, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[3].Draw(xPos, yPos + sectionHeight, 1, verticalStretchLength / sections[3].Height, System.Drawing.Color.FromArgb(alpha, color));
			sections[4].Draw(xPos + sectionWidth, yPos + sectionHeight, horizontalStretchLength / sections[4].Width, verticalStretchLength / sections[4].Height, System.Drawing.Color.FromArgb(alpha, color));
			sections[5].Draw(xPos + sectionWidth + horizontalStretchLength, yPos + sectionHeight, 1, verticalStretchLength / sections[5].Height, System.Drawing.Color.FromArgb(alpha, color));
			sections[6].Draw(xPos, yPos + sectionHeight + verticalStretchLength, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[7].Draw(xPos + sectionWidth, yPos + sectionHeight + verticalStretchLength, horizontalStretchLength / sections[7].Width, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[8].Draw(xPos + sectionWidth + horizontalStretchLength, yPos + sectionHeight + verticalStretchLength, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
		}
		public void Resize(int width, int height)
		{
			this.width = width;
			this.height = height;
			horizontalStretchLength = (width - (sectionWidth * 2));
			verticalStretchLength = (height - (sectionHeight * 2));
		}
		public void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
		}
		#endregion
	}

	public class BBUniStretchableImage
	{
		#region Member Variables
		private int x;
		private int y;
		private int width;
		private int height;
		private int mainLength;
		private int stretchLength;
		private int stretchAmount;
		private List<BBSprite> sections;
		private bool isHorizontal;
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public bool Initialize(int x, int y, int width, int height, int mainLength, int stretchLength, bool isHorizontal, List<string> sections, Random r, out string reason)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.mainLength = mainLength;
			this.stretchLength = stretchLength;
			this.sections = new List<BBSprite>();
			foreach (string spriteName in sections)
			{
				BBSprite sprite = SpriteManager.GetSprite(spriteName, r);
				if (sprite == null)
				{
					reason = string.Format("Can't find sprite \"{0}\".", spriteName);
					return false;
				}
				this.sections.Add(sprite);
			}
			this.isHorizontal = isHorizontal;
			if (isHorizontal)
			{
				stretchAmount = (width - (mainLength * 2));
			}
			else
			{
				stretchAmount = (height - (mainLength * 2));
			}

			if (stretchAmount < 0)
			{
				stretchAmount = 0;
			}

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Draw()
		{
			Draw(System.Drawing.Color.White, 255);
		}

		public void Draw(byte alpha)
		{
			Draw(System.Drawing.Color.White, alpha);
		}

		public void Draw(System.Drawing.Color color, byte alpha)
		{
			sections[0].Draw(x, y, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			if (isHorizontal)
			{
				sections[1].Draw(x + mainLength, y, stretchAmount / sections[1].Width, 1, System.Drawing.Color.FromArgb(alpha, color));
				sections[2].Draw(x + mainLength + stretchAmount, y, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			}
			else
			{
				sections[1].Draw(x, y + mainLength, 1, stretchAmount / sections[1].Height, System.Drawing.Color.FromArgb(alpha, color));
				sections[2].Draw(x, y + mainLength + stretchAmount, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			}
		}

		public void Resize(int width, int height)
		{
			this.width = width;
			this.height = height;
			if (isHorizontal)
			{
				stretchAmount = (width - (mainLength * 2));
			}
			else
			{
				stretchAmount = (height - (mainLength * 2));
			}

			if (stretchAmount < 0)
			{
				stretchAmount = 0;
			}
		}

		public void MoveTo(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		#endregion
	}

	public class BBLabel
	{
		#region Member Variables
		private int _x;
		private int _y;
		private bool _isRightAligned;
		private TextSprite _textSprite;
		private Color _color;
		private Color _outlineColor;
		private string _font = string.Empty;
		#endregion

		#region Properties
		public string Text { get; private set; }
		public float CharacterWidth { get { return _textSprite.Width / Text.Length; } }
		public Vector2D Position { get { return _textSprite.Position; } }
		#endregion

		#region Constructor
		public bool Initialize(int x, int y, string label, Color color, out string reason)
		{
			_x = x;
			_y = y;
			_color = color;
			if (!SetText(label))
			{
				reason = "Default font not found";
				return false;
			}
			reason = null;
			return true;
		}
		public bool Initialize(int x, int y, string label, Color color, string fontName, out string reason)
		{
			_x = x;
			_y = y;
			_color = color;
			if (!SetText(label, fontName))
			{
				reason = fontName + " font not found";
				return false;
			}
			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public bool SetText(string text)
		{
			//If font has been specified, use that, otherwise use default.  This function merely changes text.
			RefreshText(text);
			return true;
		}
		public bool SetText(string text, string fontName)
		{
			_font = fontName;
			RefreshText(text);
			return true;
		}
		private void RefreshText(string text)
		{
			GorgonLibrary.Graphics.Font font;
			if (string.IsNullOrEmpty(_font))
			{
				font = FontManager.GetDefaultFont();
			}
			else
			{
				font = FontManager.GetFont(_font);
			}
			_textSprite = new TextSprite("Arial", text, font, _color);
			if (!_outlineColor.IsEmpty)
			{
				_textSprite.ShadowColor = _outlineColor;
				_textSprite.ShadowDirection = FontShadowDirection.LowerRight;
				_textSprite.Shadowed = true;
			}
			SetAlignment(_isRightAligned);
			Text = text;
		}
		public void SetAlignment(bool isRightAligned)
		{
			_isRightAligned = isRightAligned;
			if (_textSprite != null)
			{
				if (isRightAligned)
				{
					_textSprite.SetPosition(_x - _textSprite.Width, _y);
				}
				else
				{
					_textSprite.SetPosition(_x, _y);
				}
			}
		}

		public float GetWidth()
		{
			return _textSprite.Width > 0 ? _textSprite.Width : 1;
		}
		public float GetHeight()
		{
			return _textSprite.Height > 0 ? _textSprite.Height : 1;
		}
		public void MoveTo(int x, int y)
		{
			this._x = x;
			this._y = y;
			_textSprite.SetPosition(x - (_isRightAligned ? _textSprite.Width : 0), y);
		}
		public void SetColor(Color color, Color outline)
		{
			_color = color;
			_outlineColor = outline;
			//Update the font sprite
			RefreshText(Text);
		}
		public void Draw()
		{
			_textSprite.Draw();
		}
		#endregion
	}

	public class BBSingleLineTextBox
	{
		#region Member Variables
		private BBLabel text;
		private BBStretchableImage background;
		private int xPos;
		private int yPos;
		private int width;
		private int height;

		private bool isReadOnly;
		private bool isSelected;
		private bool pressed;
		private bool blink;
		private float timer;
		private float highlightWidth = 0.0f;
		private float highlightHeight = 0.0f;
		private string selectedText = string.Empty;
		#endregion

		#region Properties
		public string Text { get; private set; }
		public string SelectedText
		{
			get { return selectedText; }
			private set
			{
				selectedText = value;
				highlightWidth = selectedText.Length * text.CharacterWidth;
				highlightHeight = text.GetHeight();
			}
		}
		public int SelectedTextIndex { get; private set; }
		public Color HighlightColor { get; set; }
		#endregion

		#region Constructors
		public bool Initialize(string text, int x, int y, int width, int height, bool isReadOnly, Random r, out string reason)
		{
			xPos = x;
			yPos = y;
			this.width = width;
			this.height = height;
			HighlightColor = Color.FromArgb(125, 0, 125, 125);

			//Allows us to have transparency on our highlight rectangle
			Gorgon.Screen.BlendingMode = BlendingModes.Modulated;

			background = new BBStretchableImage();
			if (!background.Initialize(x, y, width, height, StretchableImageType.TextBox, r, out reason))
			{
				return false;
			}

			Text = string.Empty;
			this.text = new BBLabel();
			if (!this.text.Initialize(x + 6, y + 7, text, Color.White, out reason))
			{
				return false;
			}
			pressed = false;
			isSelected = false;
			blink = true;
			this.isReadOnly = isReadOnly;

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public bool MouseDown(int x, int y)
		{
			if (isReadOnly)
			{
				return false;
			}
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				pressed = true;
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (isReadOnly)
			{
				return false;
			}
			if (pressed)
			{
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					blink = true;
					isSelected = true;
				}
				else
				{
					isSelected = false;
				}
			}
			else
			{
				isSelected = false;
			}
			if (!isSelected)
			{
				blink = false;
				text.SetText(Text);
			}
			return isSelected;
		}

		public void Update(float frameDeltaTime)
		{
			if (isSelected)
			{
				timer += frameDeltaTime;
				if (timer >= 0.25f)
				{
					blink = !blink;
					text.SetText(Text + (blink ? "|" : ""));
					timer -= 0.25f;
				}
			}
		}

		public void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			background.MoveTo(x, y);
			text.MoveTo(x + 6, y + 7);
		}

		public void DeleteSelectedText()
		{
			if (Text.Length == SelectedText.Length)
			{
				SelectedText = string.Empty;
				SetText(string.Empty);
				return;
			}

			string beforeSelected = Text.Substring(0, SelectedTextIndex);
			int selectedTextIndexOfLast = SelectedTextIndex + (SelectedText.Length - 1);
			string afterSelected = Text.Substring(selectedTextIndexOfLast + 1, Text.Length - 1 - selectedTextIndexOfLast);
			SelectedText = string.Empty;
			SetText(beforeSelected + afterSelected);
		}

		public void ReplaceSelectedText(string replacement)
		{
			if (Text.Length == SelectedText.Length)
			{
				SelectedText = string.Empty;
				SetText(replacement);
				return;
			}

			string beforeSelected = Text.Substring(0, SelectedTextIndex);
			int selectedTextIndexOfLast = SelectedTextIndex + (SelectedText.Length - 1);
			string afterSelected = Text.Substring(selectedTextIndexOfLast + 1, Text.Length - 1 - selectedTextIndexOfLast);
			SelectedText = string.Empty;
			SetText(beforeSelected + replacement + afterSelected);
		}

		public void Draw()
		{
			background.Draw();
			if (SelectedText.Length > 0)
			{
				float x = text.Position.X + ( SelectedTextIndex * text.CharacterWidth );
				float y = text.Position.Y;
				DrawHighlight(x, y);
			}
			text.Draw();
		}

		private void DrawHighlight(float x, float y)
		{
			Gorgon.Screen.FilledRectangle(x, y, highlightWidth, highlightHeight, HighlightColor);
		}

		public void SetText(string text)
		{
			Text = text;
			this.text.SetText(text);
		}

		public void SetReadOnly(bool readOnly)
		{
			isReadOnly = readOnly;
		}

		public void SetTextAttributes(Color color, Color outline)
		{
			text.SetColor(color, outline);
		}

		public void Select()
		{
			//Sets the text field to be selected and editable
			if (isReadOnly)
			{
				return;
			}
			blink = true;
			isSelected = true;
		}

		#region Keys
		public bool KeyDown(KeyboardInputEventArgs e)
		{
			if (isSelected)
			{
				if (e.Key == KeyboardKeys.Enter || e.Key == KeyboardKeys.Return)
				{
					isSelected = false;
				}
				else if (e.Key == KeyboardKeys.Back)
				{
					if (SelectedText.Length > 0)
					{
						DeleteSelectedText();
					}
					else if (Text.Length > 0)
					{
						Text = Text.Substring(0, Text.Length - 1);
					}
				}
				else if (e.Key == KeyboardKeys.Delete)
				{
					if (SelectedText.Length > 0)
					{
						DeleteSelectedText();
					}
				}
				else if (e.Key == KeyboardKeys.A && e.ModifierKeys == KeyboardKeys.Control)
				{
					SelectedText = Text;
					SelectedTextIndex = 0;
				}
				else if (char.IsLetterOrDigit(e.CharacterMapping.Character) ||
					char.IsLetterOrDigit(e.CharacterMapping.Shifted) ||
					char.IsSymbol(e.CharacterMapping.Character) ||
					char.IsSymbol(e.CharacterMapping.Shifted) ||
					char.IsPunctuation(e.CharacterMapping.Character) ||
					char.IsPunctuation(e.CharacterMapping.Shifted) ||
					char.IsWhiteSpace(e.CharacterMapping.Character))
				{
					if (SelectedText.Length > 0)
					{
						ReplaceSelectedText(e.Shift ? e.CharacterMapping.Shifted.ToString() : e.CharacterMapping.Character.ToString());
						return true;
					}

					string prevText = Text;
					Text = Text + (e.Shift ? e.CharacterMapping.Shifted : e.CharacterMapping.Character);
					text.SetText(Text);
					if (text.GetWidth() > width - 8)
					{
						text.SetText(prevText);
						Text = prevText;
					}
				}
				else if (e.Key == KeyboardKeys.Decimal)
				{
					if (SelectedText.Length > 0)
					{
						ReplaceSelectedText(".");
						return true;
					}
					string prevText = Text;
					Text = Text + ".";
					text.SetText(Text);
					if (text.GetWidth() > width - 8)
					{
						text.SetText(prevText);
						Text = prevText;
					}
				}
				return true;
			}
			return false;
		}
		#endregion
		#endregion
	}

	public class BBTextBox
	{
		#region Member Variables
		private Viewport _wrapView;
		private TextSprite _textSprite;
		private BBScrollBarNoArrows _textScrollBar;
		private RenderImage _target;
		private bool _scrollbarVisible;
		private bool _wrapText;
		private bool _allowScrollbar;
		private int _x;
		private int _y;
        private object _lockObject = new object();
		#endregion

		private int _maxWidth;
		public int Width { get; private set; }
		public int Height { get; private set; }
		public string Text { get; private set; }

		public bool Initialize(int xPos, int yPos, int width, int height, bool wrapText, bool allowScrollbar, string name, Random r, out string reason)
		{
			//If using scrollbar, then shrink the actual width by 16 to allow for scrollbar, even if it's not visible
			_x = xPos;
			_y = yPos;
			_maxWidth = width;
			Width = width;
			Height = height == 0 ? 1 : height;
			_wrapText = wrapText;
			_scrollbarVisible = false;
			_textScrollBar = new BBScrollBarNoArrows();
			_allowScrollbar = allowScrollbar;
			if (_allowScrollbar)
			{
				if (_wrapText)
				{
					if (!_textScrollBar.Initialize(xPos + _maxWidth - 5, yPos, Height, Height, 1, false, false, r, out reason))
					{
						return false;
					}
					_wrapView = new Viewport(0, 0, _maxWidth - 5, Height);

					_target = new RenderImage(name + "render", _maxWidth - 5, Height, ImageBufferFormats.BufferRGB888A8);
				}
				else
				{
					if (!_textScrollBar.Initialize(xPos, yPos + Height - 5, _maxWidth, _maxWidth, 1, true, false, r, out reason))
					{
						return false;
					}
					_wrapView = new Viewport(0, 0, _maxWidth, Height - 5);

					_target = new RenderImage(name + "render", _maxWidth, Height - 5, ImageBufferFormats.BufferRGB888A8);
				}
			}
			else
			{
				_wrapView = new Viewport(0, 0, _maxWidth, Height);

				_target = new RenderImage(name + "render", _maxWidth, Height, ImageBufferFormats.BufferRGB888A8);
			}
			_textSprite = new TextSprite(name, string.Empty, FontManager.GetDefaultFont());
			_textSprite.WordWrap = _wrapText;
			if (_allowScrollbar || _wrapText)
			{
				_textSprite.Bounds = _wrapView;
			}
			_target.BlendingMode = BlendingModes.Modulated;
			reason = null;
			return true;
		}

		public void SetText(string text)
		{
            lock (_lockObject)
            {
                Text = text;
                if (string.IsNullOrEmpty(text))
                {
                    text = " "; //Blank so it don't crash with 0 width
                }
                _textSprite.Text = text;
                if (_allowScrollbar)
                {
                    _textScrollBar.TopIndex = 0;
                    if (_wrapText)
                    {
                        if (_textSprite.Height > Height)
                        {
                            _scrollbarVisible = true;
                            _textScrollBar.SetAmountOfItems((int) _textSprite.Height);
                        }
                        else
                        {
                            if (_textSprite.Width < _maxWidth)
                            {
                                Width = (int) _textSprite.Width;
                            }
                            else
                            {
                                Width = _maxWidth;
                            }
                            _scrollbarVisible = false;
                        }
                    }
                    else
                    {
                        if (_textSprite.Width > _maxWidth)
                        {
                            _scrollbarVisible = true;
                            _textScrollBar.SetAmountOfItems((int) _textSprite.Width);
                            _textScrollBar.MoveTo(_x, (int) (_y + _textSprite.Height));
                        }
                        else
                        {
                            _scrollbarVisible = false;
                        }
                    }
                }
                else
                {
                    //Expand the size to the actual text sprite's size
                    if (_wrapText)
                    {
                        Height = (int) _textSprite.Height;
                        if (_textSprite.Width < _maxWidth)
                        {
                            Width = (int) _textSprite.Width;
                        }
                        else
                        {
                            Width = _maxWidth;
                        }
                        _target.Height = Height;
                    }
                    else
                    {
                        Width = (int) _textSprite.Width;
                        _target.Width = Width;
                    }
                }
                RefreshText();
            }
		}

		public void Draw()
		{
            lock (_lockObject)
            {
                if (_allowScrollbar && _scrollbarVisible)
                {
                    _textScrollBar.Draw();
                }
                //Already rendered when text was set
                _target.Blit(_x, _y);
            }
		}

		public bool MouseDown(int x, int y)
		{
			if (_allowScrollbar && _scrollbarVisible)
			{
				if (_textScrollBar.MouseDown(x, y))
				{
					RefreshText();
					return true;
				}
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (_allowScrollbar && _scrollbarVisible)
			{
				if (_textScrollBar.MouseUp(x, y))
				{
					RefreshText();
					return true;
				}
			}
			return false;
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (_allowScrollbar && _scrollbarVisible)
			{
				if (_textScrollBar.MouseHover(x, y, frameDeltaTime))
				{
					RefreshText();
					return true;
				}
			}
			return false;
		}

		public void MoveTo(int x, int y)
		{
			_x = x;
			_y = y;
			if (_allowScrollbar)
			{
				if (_wrapText)
				{
					_textScrollBar.MoveTo(_x + Width - 5, _y);
				}
				else
				{
					_textScrollBar.MoveTo(_x, (int)(_y + _textSprite.Height));
				}
			}
		}

		public void ScrollToBottom()
		{
			if (_allowScrollbar && _scrollbarVisible)
			{
				//Move the scroll to last position
				_textScrollBar.ScrollToBottom();
				RefreshText();
			}
		}

		private void RefreshText()
		{
			//Draw it once onto _target for performance reasons
			_target.Clear(Color.FromArgb(0, Color.Black));
			RenderTarget old = GorgonLibrary.Gorgon.CurrentRenderTarget;
			GorgonLibrary.Gorgon.CurrentRenderTarget = _target;
			if (_allowScrollbar)
			{
				if (_wrapText)
				{
					_textSprite.SetPosition(0, -_textScrollBar.TopIndex);
				}
				else
				{
					_textSprite.SetPosition(-_textScrollBar.TopIndex, 0);
				}
			}
			else
			{
				_textSprite.SetPosition(0, 0);
			}
			_textSprite.Draw();
			GorgonLibrary.Gorgon.CurrentRenderTarget = old;
		}
	}

	public class BBToolTip
	{
		#region Member Variables
		private const int WIDTH = 260;
		private int _actualWidth;

		private BBStretchableImage _background;
		private BBTextBox _text;

		private float _delayBeforeShowing;
		private bool _showing;

		private int _screenWidth;
		private int _screenHeight;

		private int _totalHeight;
		#endregion

		public bool Initialize(string name, string text, int screenWidth, int screenHeight, Random r, out string reason)
		{
			_text = new BBTextBox();
			if (!_text.Initialize(0, 0, WIDTH - 30, 0, true, false, name, r, out reason))
			{
				return false;
			}
			_text.SetText(text);
			if (_text.Width < WIDTH - 30)
			{
				_actualWidth = _text.Width + 30;
			}
			else
			{
				_actualWidth = WIDTH;
			}

			_totalHeight = _text.Height + 15;

			_background = new BBStretchableImage();
			if (!_background.Initialize(0, 0, _actualWidth, _totalHeight, StretchableImageType.ThinBorderBG, r, out reason))
			{
				return false;
			}

			_showing = false;
			_delayBeforeShowing = 0; //1 second will turn on showing

			_screenWidth = screenWidth;
			_screenHeight = screenHeight;

			reason = null;
			return true;
		}

		public void SetText(string text)
		{
			_text.SetText(text);

			if (_text.Width < WIDTH - 30)
			{
				_actualWidth = _text.Width + 30;
			}
			else
			{
				_actualWidth = WIDTH;
			}
			_totalHeight = _text.Height + 15;

			_background.Resize(_actualWidth, _totalHeight);
		}

		public void Draw()
		{
			if (_showing && _delayBeforeShowing >= 1)
			{
				_background.Draw();
				_text.Draw();
			}
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (_showing)
			{
				int modifiedX = 0;
				int modifiedY = 0;
				if (x < _screenWidth - (WIDTH + 24))
				{
					modifiedX = x + 24;
				}
				else
				{
					modifiedX = x - WIDTH;
				}
				if (y < _screenHeight - _totalHeight)
				{
					modifiedY = y;
				}
				else
				{
					modifiedY = y - _totalHeight;
				}
				_background.MoveTo(modifiedX, modifiedY);
				_text.MoveTo(modifiedX + 15, modifiedY + 7);

				if (_delayBeforeShowing < 1.0)
				{
					_delayBeforeShowing += frameDeltaTime;
				}
			}
			else
			{
				_delayBeforeShowing = 0;
			}
			return true;
		}

		public void SetShowing(bool showing)
		{
			_showing = showing;
			if (!_showing)
			{
				_delayBeforeShowing = 0;
			}
		}
	}
}
