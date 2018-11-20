using System;
using System.Diagnostics;
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
        private double fps; // frames per second
        private double g; // in u/s/s

        private List<EngineBox> objects = new List<EngineBox>();

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
            this.fps = fps;
            g = gravity;

            this.objects = objects;

            velocity = new Vector[objects.Count];

            for (int i = 0; i < velocity.Length; i++)
            {
                velocity[i] = new Vector(0, 0, 0, 0);
            }
        }

        public void ExecuteNext()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                EngineBox item = objects.ElementAt<EngineBox>(i);

                if (item.GetMass() == 0)
                    continue;

                CheckColision(item, i);

                velocity[i].Add(0, 0, 0, (g /fps));
                item.Move(velocity[i], fps);
            }
        }

        private void CheckColision(EngineBox item, int j)
        {
            double leftB = item.GetXPos();
            double rightB = item.GetWidth() + leftB;
            double topB = item.GetYPos();
            double botB = item.GetHeight() + topB;
            
            for (int i = 0; i < velocity.Length; i++)
            {
                if (i == j) continue;

                EngineBox subject = objects.ElementAt<EngineBox>(i);

                double leftS = subject.GetXPos();
                double rightS = subject.GetWidth() + leftS;
                double topS = subject.GetYPos();
                double botS = subject.GetHeight() + topS;
                
                bool overlapX = !(leftB > rightS || rightB < leftS);
                bool overlapY = !(botB < topS || topB > botS);

                if (overlapX && overlapY) // if colision invert velocities [incorrect]
                {
                    velocity[i].Invert();
                    velocity[j].Invert();
                }
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
            rect.Fill = fill;

            x = posX;
            y = posY;
            h = height;
            w = width;
            m = mass;

            canvas.Children.Add(rect);
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
        }

        public void Move(double dX, double dY)
        {
            x += dX;
            y += dY;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
        }

        public void Move(Vector v, double fps)
        {
            x += (v.getXValue() / fps);
            y += (v.getYValue() / fps);
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

        public void Invert()
        {
            xVal = -xVal;
            yVal = -yVal;
            angle += angle > 0 ? -180 : 180;
        }

        public double getXValue() { return xVal; }
        public double getYValue() { return yVal; }
        public double getAngle() { return angle; }
        public double getMagnitude() { return mag; }
    }
}
