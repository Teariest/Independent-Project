using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace Physics_Simulator
{
    class Engine
    {
        private double interval;
        private double g;

        private List<EngineBox> staticObjects = new List<EngineBox>();
        private List<EngineBox> dynamicObjects = new List<EngineBox>();

        private Vector[] velocity;

        // could add limits (box bouncing etc)
        /// <summary>
        /// Represents a 2D physics simulator
        /// </summary>
        /// <param name="objects">List of all objects</param>
        /// <param name="interval">Frames per seconds the simulation will run at</param>
        /// <param name="g">Gravity in u/s/s</param>
        public Engine(List<EngineBox> objects, int fps, double gravity)
        {
            interval = 1/fps;
            g = gravity;

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

            velocity = new Vector[dynamicObjects.Count];

            for (int i = 0; i < velocity.Length; i++)
            {
                velocity[i] = new Vector(0, 0, 0, 0);
            }
        }

        public void ExecuteNext()
        {
            for (int i = 0; i < dynamicObjects.Count; i++)
            {
                velocity[i].Add(0, 0, 0, g / interval);
                dynamicObjects.ElementAt<EngineBox>(i).Move(velocity[i], interval);
            }
        }
    }

    class EngineBox
    {
        private Rectangle rect;
        private double x;
        private double y;
        private double h;
        private double w;
        private double m;

        public EngineBox(double posX, double posY, double height, double width, double mass, Brush fill,  Canvas canvas)
        {
            rect = new Rectangle();
            rect.Height = height;
            rect.Width = width;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
            rect.Fill = fill;

            canvas.Children.Add(rect);

            x = posX;
            y = posY;
            h = height;
            w = width;
            m = mass;
        }

        public void Move(double dX, double dY)
        {
            x += dX;
            y += dY;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
        }

        public void Move(Vector v, double interval)
        {
            x += v.getXValue() / interval;
            y += v.getYValue() / interval;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
        }

        public double GetXPos() { return x; }
        public double GetYPos() { return y; }
        public double GetHeight() { return h; }
        public double GetWidth() { return w; }
        public double GetMass() { return m; }
    }

    class Vector
    {

        double angle;
        double mag;
        double xVal;
        double yVal;

        public Vector(double magnitude, double angle, double x, double y)
        {
            if (mag == 0) // if given scalar
            {
                xVal = x;
                yVal = y;
                CalcVector();
            }
            else // if given vector
            {
                mag = magnitude;
                this.angle = angle;
                CalcScalar();
            }
        }

        private void CalcVector()
        {
            mag = Math.Sqrt(Math.Pow(xVal, 2) + Math.Pow(yVal, 2));
            angle = Math.Acos(xVal/mag);
        }

        private void CalcScalar()
        {
            xVal = mag * Math.Cos(angle);
            yVal = mag * Math.Sin(angle);
        }

        public void Add(Vector v)
        {
            xVal += v.getXValue();
            yVal += v.getYValue();
            CalcVector();
        }

        public void Add(double magnitude, double angle, double x, double y)
        {
            Add(new Vector(magnitude, angle, x, y));
        }

        public double getXValue() { return xVal; }
        public double getYValue() { return yVal; }
        public double getAngle() { return angle; }
        public double getMagnitude() { return mag; }
    }
}
