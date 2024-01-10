using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace WindowsApplication22
{

	public class Hero
	{
		public Point Position;
		static Bitmap heroImage1 = null;
		static Bitmap heroImage2 = null;
        int inc = 3;
		int LastPositionX = 0;
		int LastPositionY = 0;

        public Hero()
		{

			Position.X = 30;
			Position.Y = 35;
			if (heroImage1  == null)
			{
				heroImage1 = new Bitmap("hero-2.gif");
			}
			
			if (heroImage2  == null)
			{
				heroImage2 = new Bitmap("hero-3.gif");
			}

        }

		public Hero(int x, int y)
		{

			Position.X = x;
			Position.Y = y;
			if (heroImage1  == null)
			{
				heroImage1 = new Bitmap("hero-2.gif");
			}

			if (heroImage2  == null)
			{
				heroImage2 = new Bitmap("hero-3.gif");
			}

        }

		public Rectangle GetFrame()
		{
			Rectangle myRect = new Rectangle(Position.X, Position.Y, heroImage1.Width, heroImage1.Height);
			return myRect;
		}

		public void Draw(Graphics g)
		{

			Rectangle destR = new Rectangle(Position.X, Position.Y, heroImage1.Width, heroImage1.Height);
			Rectangle srcR = new Rectangle(0,0, heroImage1.Width, heroImage1.Height);

			if ( ((Position.X % 2 == 1) && ((Position.X - LastPositionX) != 0)) || ((Position.Y % 2 == 1) && ((Position.Y - LastPositionY) != 0)))
				g.DrawImage(heroImage1, destR, srcR, GraphicsUnit.Pixel);
			else
				g.DrawImage(heroImage2, destR, srcR, GraphicsUnit.Pixel);
            LastPositionX = Position.X;
			LastPositionY = Position.Y;

		}

		public void MoveLeft(Rectangle r)
		{
		  if (Position.X <= 0)
			  return;  // precondition

		  Position.X -= inc;
		}

		public void MoveRight(Rectangle r)
		{
			if (Position.X >= r.Width - heroImage1.Width)
				return;  // precondition

			Position.X += inc;
		}

		public void MoveUp(Rectangle r)
		{
			if (Position.Y <= 0)
				return;  // precondition

			Position.Y -= inc;
		}

		public void MoveDown(Rectangle r)
		{
			if (Position.Y >= r.Height - heroImage1.Height)
				return;  // precondition

			Position.Y += inc;
		}
    }
}
