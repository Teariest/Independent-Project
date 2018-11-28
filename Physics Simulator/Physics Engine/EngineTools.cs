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

    /// <summary>
    /// Represents a box object used by the Engine object
    /// </summary>
    class EngineBox
    {
        private Rectangle rect; // Rectangle
        private double x; // Left side's position from left of canvas in p
        private double y; // Top side's position from top of canvas in p
        private double h; // Height
        private double w; // Width
        private double m; // Mass

        /// <summary>
        /// Setup EngineBox params and add rectangle to canvas at specified location
        /// </summary>
        /// <param name="posX">X-Var of Position from top-left corner of canvas (pixel)</param>
        /// <param name="posY">Y-Var of Position from top-left corner of canvas (pixel)</param>
        /// <param name="height">Height of box (pixel)</param>
        /// <param name="width">Height of box (pixel)</param>
        /// <param name="mass">Mass of box (relative)</param>
        /// <param name="fill">Color of box</param>
        /// <param name="canvas">Canvas the box will be inside of</param>
        public EngineBox(double posX, double posY, double height, double width, double mass, Brush fill, Canvas canvas)
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

        /// <summary>
        /// Move object by a certain amount of pixels
        /// </summary>
        public void Move(double dX, double dY)
        {
            x += dX;
            y += dY;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
        }

        /// <summary>
        /// Move object by a certain vector
        /// </summary>
        public void Move(Vector v)
        {
            x += v.getXValue();
            y += v.getYValue();
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
        }

        public void ReboundMove(Vector v, double fps)
        {
            // TODO : rebound object so that it is at impact point
        }

        /// <summary>
        /// Move object to a specific location
        /// </summary>
        public void MoveAbs(double x, double y)
        {
            this.x += x;
            this.y += y;
            rect.SetValue(Canvas.LeftProperty, x);
            rect.SetValue(Canvas.TopProperty, y);
        }

        public double GetXPos() { return x; }
        public double GetYPos() { return y; }
        public double GetHeight() { return h; }
        public double GetWidth() { return w; }
        public double GetMass() { return m; }
    }


    /// <summary>
    /// A vector that can use both angle/magnitude or x/y values
    /// </summary>
    class Vector
    {

        double angle;
        double mag;
        double xVal;
        double yVal;

        /// <summary>
        /// Setup Vector, if either magnitude/angle OR x/y are set to 0 they will be ignored and vector will be calculate via the other set of variables
        /// </summary>
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
            angle = Math.Acos(xVal / mag);
        }

        private void CalcScalar()
        {
            xVal = mag * Math.Cos(angle);
            yVal = mag * Math.Sin(angle);
        }

        /// <summary>
        /// Vector addition
        /// </summary>
        public void Add(Vector v)
        {
            xVal += v.getXValue();
            yVal += v.getYValue();
            CalcVector();
        }


        /// <summary>
        /// Vector addition using raw values, only one pair of values have to be set to a value
        /// </summary>
        public void Add(double magnitude, double angle, double x, double y)
        {
            Add(new Vector(magnitude, angle, x, y));
        }

        /// <summary>
        /// Invert vector
        /// </summary>
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