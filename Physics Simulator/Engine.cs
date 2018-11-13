using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Physics_Simulator
{
    class Engine
    {
        private double interval;
        private double gravity;

        private List<EngineBox> staticObjects;
        private List<EngineBox> dynamicObjects;

        private double[] velocity;

        // could add limits (box bouncing etc)
        public Engine(List<EngineBox> objects, double interval, double gravityA)
        {
            this.interval = interval;
            gravity = gravityA;

            foreach (EngineBox i in objects)
            {
                if (i.GetMass() == 0)
                {
                    staticObjects.Add(i);
                }
                else
                {
                    dynamicObjects.Add(i);
                }
            }

            velocity = new double[dynamicObjects.Count];

            for (int i = 0; i < velocity.Length; i++)
            {
                velocity[i] = 0;
            }
        }

        public void ExecuteNext()
        {
            for (int i = 0; i < dynamicObjects.Count; i++)
            {
                velocity[i] += gravity / interval;
            }
        }
    }

    class EngineBox
    {
        private Rectangle rect;
        private int x;
        private int y;
        private int h;
        private int w;
        private int m;

        public EngineBox(int posX, int posY, int height, int width, int mass, Canvas canvas)
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
            m = mass;
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
        public int GetMass() { return m; }
    }
}
