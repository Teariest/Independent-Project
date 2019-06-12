using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace Physics_Simulator {
    
    class Engine {

        private double tInt; // time interval in seconds
        private Vector g; // in u/s/s

        private EngineCircle[] objects; // All objects within simulation, each with a unique index
        private Vector[] velocity; // Velocities per object, same index as reference object's index within 'objects'

        
        public Engine(EngineCircle[] objects, Vector[] startV, int fps, double gravity, double gravityA) {
            tInt = 1.0/fps;
            g = new Vector(gravity * tInt, gravityA, 0, 0);

            this.objects = objects;

            velocity = startV;
        }

        /// <summary>
        /// Calculate physics and move all objects accordingly over pre-determined amount of time
        /// </summary>
        public void ExecuteNext() {

            Vector[] newV = new Vector[velocity.Length];
            Vector[] rebound = new Vector[velocity.Length];

            // Collisions
            
            for (int i = 0; i < objects.Length; i++) { // change velocities

                newV[i] = new Vector(0, 0, 0, 0);
                rebound[i] = new Vector(0, 0, 0, 0);

                for (int j = 0; j < objects.Length; j++) {

                    if (i == j) // do not check yourself
                        continue;

                    double XDiff = Diff(objects[i].GetXPos(), objects[j].GetXPos());
                    double YDiff = Diff(objects[i].GetYPos(), objects[j].GetYPos());
                    double minDist = objects[i].GetRadius() + objects[j].GetRadius();

                    double distSqrd = Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2);
                    double minDistSqrd = Math.Pow(minDist, 2);

                    // Collisions
                    if (distSqrd <= minDistSqrd) {
                        
                        // values to be used when calculating change in velocity
                        double ma = objects[i].GetMass();
                        Vector va = velocity[i];
                        double mb = objects[j].GetMass();
                        Vector vb = velocity[j];
                        double e = objects[i].GetElasticity();

                        // change in x and y velocities source: https://en.wikipedia.org/wiki/Coefficient_of_restitution
                        double dx = ((ma * va.getXValue()) + (mb * vb.getXValue()) + (mb * e * (vb.getXValue() - va.getXValue()))) / (ma + mb); // change in x using elasticity
                        double dy = ((ma * va.getYValue()) + (mb * vb.getYValue()) + (mb * e * (vb.getYValue() - va.getYValue()))) / (ma + mb); // change in y using elasticity

                        // change in velocity
                        newV[i].Add(0, 0, dx, dy);

                        // rebound opperations

                        double xDiff = (objects[i].GetXPos() - objects[j].GetXPos());
                        double yDiff = (objects[i].GetYPos() - objects[j].GetYPos());

                        double rAngle;
                        if (yDiff != 0)
                            rAngle = Math.Atan(xDiff / yDiff);

                        else
                            rAngle = xDiff >= 0 ? 0 : Math.PI;
                        
                        rebound[i].Add((minDist - Math.Sqrt(distSqrd))/2, rAngle, 0, 0);
                        
                        if (HUB.collisionSounds) {
                            HUB.collisionPlayer.Play();
                        }
                    }
                }
            }

            // Updates

            for (int i = 0; i < objects.Length; i++) { // update positions and velocities

                if (newV[i].getMagnitude() != 0) // if change in velocity then apply it
                    velocity[i] = newV[i];

                if (objects[i].GetMass() != 0) // add gravity if objects is not static
                    velocity[i].Add(g);

                // move ball based on new velocity (rebound included)
                objects[i].Move((velocity[i].getXValue() * tInt) + rebound[i].getXValue(), (velocity[i].getYValue() * tInt) + rebound[i].getYValue());
            }
        }

        public EngineCircle[] Positions() {
            return objects;
        }

        // Returns difference between two numbers
        public double Diff(double num1, double num2) {
            return num1 > num2 ? num1 - num2 : num2 - num1;
        }
    }
}
