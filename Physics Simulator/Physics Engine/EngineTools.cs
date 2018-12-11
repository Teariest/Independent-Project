using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Physics_Simulator {
    

    class EngineCircle {

        private double x; // Left side's position from left of canvas in p
        private double y; // Top side's position from top of canvas in p
        private double r; // Radius
        private double m; // Mass
        private double e; // coefficient of elasticity

        public EngineCircle(double posX, double posY, double radius, double mass, double elasticity) {

            x = posX;
            y = posY;
            r = radius;
            m = mass;
            e = elasticity;
        }

        /// <summary>
        /// Move object by a certain amount of pixels
        /// </summary>
        public void Move(double dX, double dY) {
            x += dX;
            y += dY;
        }

        /// <summary>
        /// Move object by a certain vector
        /// </summary>
        public void Move(Vector v) {
            x += v.getXValue();
            y += v.getYValue();
        }

        /// <summary>
        /// Move object to a specific location
        /// </summary>
        public void MoveAbs(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public double GetXPos() { return x; }
        public double GetYPos() { return y; }
        public double GetRadius() { return r; }
        public double GetMass() { return m; }
        public double GetElasticity() { return e; }
    }

    /// <summary>
    /// A vector that can use both angle/magnitude or x/y values
    /// </summary>
    class Vector {

        double angle;
        double mag;
        double xVal;
        double yVal;

        /// <summary>
        /// Setup Vector, if either magnitude/angle OR x/y are set to 0 they will be ignored and vector will be calculate via the other set of variables
        /// </summary>
        public Vector(double magnitude, double angle, double x, double y) {

            if (mag == 0) // if given scalars
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

        private void CalcVector() {
            mag = Math.Sqrt(Math.Pow(xVal, 2) + Math.Pow(yVal, 2));
            angle = Math.Acos(xVal / mag);
        }

        private void CalcScalar() {
            xVal = mag * Math.Cos(angle);
            yVal = mag * Math.Sin(angle);
        }

        /// <summary>
        /// Vector addition
        /// </summary>
        public void Add(Vector v) {
            xVal += v.getXValue();
            yVal += v.getYValue();
            CalcVector();
        }


        /// <summary>
        /// Vector addition using raw values, only one pair of values have to be set to a value
        /// </summary>
        public void Add(double magnitude, double angle, double x, double y) {
            Add(new Vector(magnitude, angle, x, y));
        }

        /// <summary>
        /// Invert vector
        /// </summary>
        public void Invert() {
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