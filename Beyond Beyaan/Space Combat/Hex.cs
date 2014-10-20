namespace Beyond_Beyaan.Space_Combat
{
	public enum HexOrientation { Flat, Pointy }

	public enum FlatVertice
	{
		UpperLeft = 0,
		UpperRight = 1,
		MiddleRight = 2,
		BottomRight = 3,
		BottomLeft = 4,
		MiddleLeft = 5,
	}

	public enum PointyVertice
	{
		Top = 0,
		UpperRight = 1,
		BottomRight = 2,
		Bottom = 3,
		BottomLeft = 4,
		TopLeft = 5,
	}

	public class Hex
	{
		#region Data Members

		private float _x;
		private float _y;
		#endregion

		#region Properties
		public System.Drawing.Color Color { get; set; }
		public HexOrientation Orientation { get; private set; }
		public PointF[] Points { get; private set; }
		public float Side { get; private set; }
		public float H { get; private set; }
		public float R { get; private set; }
		public float CenterX
		{
			get
			{
				if (Orientation == HexOrientation.Flat)
				{
					return _x + (0.5f * Side);
				}
				return _x;
			}
		}
		public float CenterY
		{
			get
			{
				if (Orientation == HexOrientation.Flat)
				{
					return _y + R;
				}
				return _y + H + (0.5f * Side);
			}
		}
		#endregion

		#region Constructors
		public Hex(float x, float y, float side, HexOrientation orientation)
		{
			Initialize(x, y, side, orientation);
		}

		public Hex(PointF point, float side, HexOrientation orientation)
		{
			Initialize(point.X, point.Y, side, orientation);
		}

		public Hex()
		{ }
		#endregion

		private void Initialize(float x, float y, float side, HexOrientation orientation)
		{
			_x = x;
			_y = y;
			Side = side;
			Orientation = orientation;
			Color = System.Drawing.Color.LightGray;
			CalculateVertices();
		}

		/// <summary>
		/// Calculates the vertices of the hex based on orientation. Assumes that points[0] contains a value.
		/// </summary>
		private void CalculateVertices()
		{
			//  
			//  h = short length (outside)
			//  r = long length (outside)
			//  side = length of a side of the hexagon, all 6 are equal length
			//
			//  h = sin (30 degrees) x side
			//  r = cos (30 degrees) x side
			//
			//		 h
			//	     ---
			//   ----     |r
			//  /    \    |          
			// /      \   |
			// \      /
			//  \____/
			//
			// Flat orientation (scale is off)
			//
			//     /\
			//    /  \
			//   /    \
			//   |    |
			//   |    |
			//   |    |
			//   \    /
			//    \  /
			//     \/
			// Pointy orientation (scale is off)

			H = CalculateH(Side);
			R = CalculateR(Side);

			switch (Orientation)
			{
				case HexOrientation.Flat:
					// x,y coordinates are top left point
					Points = new PointF[6];
					Points[0] = new PointF(_x, _y);
					Points[1] = new PointF(_x + Side, _y);
					Points[2] = new PointF(_x + Side + H, _y + R);
					Points[3] = new PointF(_x + Side, _y + R + R);
					Points[4] = new PointF(_x, _y + R + R);
					Points[5] = new PointF(_x - H, _y + R);
					break;
				case HexOrientation.Pointy:
					//x,y coordinates are top center point
					Points = new PointF[6];
					Points[0] = new PointF(_x, _y);
					Points[1] = new PointF(_x + R, _y + H);
					Points[2] = new PointF(_x + R, _y + Side + H);
					Points[3] = new PointF(_x, _y + Side + H + H);
					Points[4] = new PointF(_x - R, _y + Side + H);
					Points[5] = new PointF(_x - R, _y + H);
					break;
			}
		}

		public static double DegreesToRadians(double degrees)
		{
			return degrees * System.Math.PI / 180;
		}

		public static double RadiansToDegrees(double radians)
		{
			return radians * 180 / System.Math.PI;
		}

		public static float CalculateH(float side)
		{
			return (float) System.Math.Sin(DegreesToRadians(30)) * side;
		}

		public static float CalculateR(float side)
		{
			return (float)System.Math.Cos(DegreesToRadians(30)) * side;
		}

		public static bool InsidePolygon(PointF[] polygon, int N, PointF p)
		{
			//http://astronomy.swin.edu.au/~pbourke/geometry/insidepoly/
			//
			// Slick algorithm that checks if a point is inside a polygon.  Checks how many time a line
			// origination from point will cross each side.  An odd result means inside polygon.
			//
			int counter = 0;
			int i;
			double xinters;
			PointF p1, p2;

			p1 = polygon[0];
			for (i = 1; i <= N; i++)
			{
				p2 = polygon[i % N];
				if (p.Y > System.Math.Min(p1.Y, p2.Y))
				{
					if (p.Y <= System.Math.Max(p1.Y, p2.Y))
					{
						if (p.X <= System.Math.Max(p1.X, p2.X))
						{
							if (p1.Y != p2.Y)
							{
								xinters = (p.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
								if (p1.X == p2.X || p.X <= xinters)
									counter++;
							}
						}
					}
				}
				p1 = p2;
			}

			if (counter % 2 == 0)
				return false;

			return true;
		}
	}
}
