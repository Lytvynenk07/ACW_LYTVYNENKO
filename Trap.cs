using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApplication22
{
    internal class Trap
    {
        public Point Position;
        static Brush trapBrush = new SolidBrush(Color.Red);
        private bool isActive = true;

        public bool IsActive
        {
            get { return isActive; }
        }
        public void Deactivate()
        {
            isActive = false;
        }
        public Trap()
        {
            Position.X = 0;
            Position.Y = 0;
        }

        public Trap(int x, int y)
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
            g.FillRectangle(trapBrush, destR);
        }
    }
}
