using System;
using System.Drawing;

namespace WindowsApplication22
{
    internal class Heart
    {
        public Point Position;
        static Brush heartBrush = new SolidBrush(Color.Magenta);
        private bool isActive = true;

        public bool IsActive
        {
            get { return isActive; }
        }

        public void Deactivate()
        {
            isActive = false;
        }

        public Heart()
        {
            Position.X = 0;
            Position.Y = 0;
        }

        public Heart(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public Rectangle GetFrame()
        {
            Rectangle myRect = new Rectangle(Position.X, Position.Y, 20, 20);
            return myRect;
        }

        public void Draw(Graphics g)
        {
            Rectangle destR = new Rectangle(Position.X, Position.Y, 20, 20);
            g.FillEllipse(heartBrush, destR);
        }
    }
}
