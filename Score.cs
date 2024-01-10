using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsApplication22
{
	public class Score
	{
		int Count = 0;
		public Point Position = new Point(0,0);
		public Font MyFont = new Font("Compact", 20.0f, GraphicsUnit.Pixel );

		public int Value
		{
			get
			{
				 return Count;
			}
		}

		public Score(int x, int y)
		{
			Position.X = x;
			Position.Y = y;
		}



		public void Draw(Graphics g)
		{
			g.DrawString(Count.ToString(), MyFont, Brushes.RoyalBlue, Position.X, Position.Y, new StringFormat());
		}

		public Rectangle GetFrame()
		{
			Rectangle myRect = new Rectangle(Position.X, Position.Y, (int)MyFont.SizeInPoints*Count.ToString().Length, MyFont.Height);
			return myRect;
		}



		public void Reset()
		{
			Count = 0;
		}


		public void Increment()
		{
			Count++;
		}
	}
}
