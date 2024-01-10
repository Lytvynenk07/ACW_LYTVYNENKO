using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WindowsApplication22
{

    public class Lives
    {
        private int numberOfLives = 3;
        public Point Position = new Point(0, 0);
        public Font MyFont = new Font("Compact", 20.0f, GraphicsUnit.Pixel);

        public int NumberOfLives
        {
            get { return numberOfLives; }
            set { numberOfLives = value; }
        }
        public Lives(int initialLives)
        {
            numberOfLives = initialLives;
        }
        public void DecreaseLife()
        {
            numberOfLives--;
        }
        public void IncreaseLife()
        {
            numberOfLives++;
        }
        public void ResetLives()
        {

            numberOfLives = 3; 
        }

        public Lives(int x, int y)
        {

            Position.X = x;
            Position.Y = y;
        }



        public void Draw(Graphics g)
        {
            g.DrawString(numberOfLives.ToString(), MyFont, Brushes.RoyalBlue, Position.X, Position.Y, new StringFormat());
        }

        public Rectangle GetFrame()
        {
            Rectangle myRect = new Rectangle(Position.X, Position.Y, (int)MyFont.SizeInPoints * numberOfLives.ToString().Length, MyFont.Height);
            return myRect;
        }
    }
}
