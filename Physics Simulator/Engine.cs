using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Physics_Simulator
{
    class Engine
    {

        public Engine()
        {

        }
    }

    class EngineBox
    {
        private Rectangle rect;
        private int x;
        private int y;
        private int h;
        private int w;

        public EngineBox(int posX, int posY, int height, int width, Canvas canvas)
        {
            rect = new Rectangle();
            rect.Height = height;
            rect.Width = width;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);

            canvas.Children.Add(rect);

            x = posX;
            y = posY;
            h = height;
            w = width;
        }

        public void Move(int dX, int dY)
        {
            x += dX;
            y += dY;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
        }

        public int GetXPos() { return x; }
        public int GetYPos() { return y; }
        public int GetHeight() { return h; }
        public int GetWidth() { return w; }
    }
}
