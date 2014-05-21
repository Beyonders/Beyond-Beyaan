using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using GorgonLibrary.Graphics;
using Image = GorgonLibrary.Graphics.Image;

namespace Beyond_Beyaan.Data_Modules
{
	internal enum StretchMode { STRETCH, REPEAT }
	public class BaseSprite
	{
		public List<Sprite> Frames { get; private set; }
		public List<string> FrameLength { get; private set; }
		public string Name { get; private set; }
		private StretchMode stretchMode;

		public BaseSprite()
		{
			Frames = new List<Sprite>();
			FrameLength = new List<string>();
			stretchMode = StretchMode.STRETCH;
		}

		public bool LoadSprite(XElement spriteValues, string graphicDirectory, out string reason)
		{
			string filename = null;
			Name = null;
			foreach (var attribute in spriteValues.Attributes())
			{
				//Get the graphic file
				switch (attribute.Name.ToString().ToLower())
				{
					case "file":
					{
						filename = attribute.Value;
						break;
					}
					case "name":
					{
						Name = attribute.Value;
						break;
					}
					case "stretchmode":
					{
						if (attribute.Value == "repeat")
						{
							stretchMode = StretchMode.REPEAT;
						}
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(filename))
			{
				reason = "No sprite name or file location.";
				return false;
			}
			Sprite graphicFile;
			if (ImageCache.Images.Contains(Name))
			{
				graphicFile = new Sprite(Name, ImageCache.Images[Name]);
			}
			else
			{
				string path = Path.Combine(graphicDirectory, filename);
				if (!File.Exists(path))
				{
					reason = "Graphic File doesn't exist: " + path;
					return false;
				}
				graphicFile = new Sprite(Name, Image.FromFile(Path.Combine(graphicDirectory, filename)));
			}

			int frameCount = 0;
			foreach (var element in spriteValues.Elements())
			{
				int x = 0;
				int y = 0;
				int width = 0;
				int height = 0;
				int axisX = 0;
				int axisY = 0;
				string frameLength = null;
				//Process each frame
				foreach (var attribute in element.Attributes())
				{
					switch (attribute.Name.ToString().ToLower())
					{
						case "x":
						{
							x = int.Parse(attribute.Value);
							break;
						}
						case "y":
						{
							y = int.Parse(attribute.Value);
							break;
						}
						case "width":
						{
							width = int.Parse(attribute.Value);
							break;
						}
						case "height":
						{
							height = int.Parse(attribute.Value);
							break;
						}
						case "framelength":
						{
							frameLength = attribute.Value;
							break;
						}
						case "axisx":
						{
							axisX = int.Parse(attribute.Value);
							break;
						}
						case "axisy":
						{
							axisY = int.Parse(attribute.Value);
							break;
						}
					}
				}
				var frame = new Sprite(Name + frameCount, graphicFile.Image, x, y, width, height, axisX,
				                                              axisY);
				if (stretchMode == StretchMode.REPEAT)
				{
					frame.WrapMode = ImageAddressing.Wrapping;
				}
				Frames.Add(frame);
				FrameLength.Add(frameLength);
				frameCount++;
			}
			reason = null;
			return true;
		}
	}

	public class BBSprite
	{
		private BaseSprite _baseSprite;
		private int _currentFrame;
		private float _frameTimer;
		private bool _animated;

		public float Width
		{
			get { return _baseSprite.Frames[0].Width; }
		}
		public float Height
		{
			get { return _baseSprite.Frames[0].Height; }
		}

		public BBSprite(BaseSprite baseSprite, Random r)
		{
			_baseSprite = baseSprite;
			_currentFrame = 0;
			_animated = _baseSprite.Frames.Count > 1;
			if (_animated)
			{
				_frameTimer = Utility.GetIntValue(_baseSprite.FrameLength[_currentFrame], r) / 1000.0f;
			}
		}

		public void Update(float time, Random r)
		{
			if (!_animated)
			{
				return;
			}
			_frameTimer -= time;
			if (_frameTimer <= 0)
			{
				//Advance to next frame
				_currentFrame++;
				if (_currentFrame >= _baseSprite.Frames.Count)
				{
					_currentFrame = 0;
				}
				_frameTimer = Utility.GetIntValue(_baseSprite.FrameLength[_currentFrame], r) / 1000.0f;
			}
		}

		public void Draw(float x, float y)
		{
			Draw(x, y, 1.0f, 1.0f, Color.White);
		}

		public void Draw(float x, float y, float scaleX, float scaleY)
		{
			Draw(x, y, scaleX, scaleY, Color.White);
		}

		public void Draw(float x, float y, float scaleX, float scaleY, byte alpha)
		{
			Draw(x, y, scaleX, scaleY, Color.FromArgb(alpha, Color.White));
		}

		public void Draw(float x, float y, float scaleX, float scaleY, Color color)
		{
			Draw(x, y, scaleX, scaleY, color, 0);
		}

		public void Draw(float x, float y, float scaleX, float scaleY, Color color, float angle)
		{
			var frame = _baseSprite.Frames[_currentFrame];
			frame.Rotation = angle;
			frame.SetPosition(x, y);
			float oldWidth = frame.Width;
			if (frame.HorizontalWrapMode == ImageAddressing.Wrapping)
			{
				frame.SetScale(1, scaleY);
				frame.Width = frame.Width * scaleX;
			}
			else
			{
				frame.SetScale(scaleX, scaleY);
			}
			frame.Color = color;
			frame.Draw();
			frame.Width = oldWidth;
		}
	}
}
