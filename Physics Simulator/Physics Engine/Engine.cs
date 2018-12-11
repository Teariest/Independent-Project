using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Physics_Simulator {
    
    class Engine {

        private double tInt; // time interval in seconds
        private double g; // in u/s/s

        private EngineCircle[] objects; // All objects within simulation, each with a unique index
        private Vector[] velocity; // Velocities per object, same index as reference object's index within 'objects'

        
        public Engine(EngineCircle[] objects, Vector[] startV, int fps, double gravity) {
            this.tInt = 1.0/fps;
            g = gravity;

            this.objects = objects;

            velocity = startV;
        }

        /// <summary>
        /// Calculate physics and move all objects accordingly over pre-determined amount of time
        /// </summary>
        public void ExecuteNext() {

            Vector[] newV = new Vector[velocity.Length];
            Vector[] rebound = new Vector[velocity.Length];
            
            for (int i = 0; i < objects.Length; i++) { // change velocities

                for (int j = 0; j < objects.Length; j++) {

                    if (i == j) // do not check yourself
                        continue;

                    double distSqrd = Math.Pow(Diff(objects[i].GetXPos(), objects[j].GetXPos()), 2) + Math.Pow(Diff(objects[i].GetYPos(), objects[j].GetYPos()), 2);
                    double minDistSqrd = Math.Pow(objects[i].GetRadius() + objects[j].GetRadius(), 2);
                    Debug.WriteLine("I: {2}, Dist:{0}, Min Dist:{1}", distSqrd, minDistSqrd, i);
                    // Collisions
                    if (distSqrd <= minDistSqrd) {

                        Debug.WriteLine("Collision");

                        double ma = objects[i].GetMass();
                        Vector va = velocity[i];
                        double mb = objects[j].GetMass();
                        Vector vb = velocity[j];
                        double e = objects[i].GetElasticity();

                        double xv = (ma * va.getXValue() + mb * vb.getXValue() + mb * e * (vb.getXValue() - va.getXValue())) / (ma + mb);
                        double yv = (ma * va.getYValue() + mb * vb.getYValue() + mb * e * (vb.getYValue() - va.getYValue())) / (ma + mb);

                        newV[i] = new Vector(0, 0, xv, yv);
                        rebound[i] = new Vector(Math.Atan2(objects[i].GetXPos() - objects[j].GetXPos(), objects[i].GetYPos() - objects[j].GetYPos()), Math.Sqrt(distSqrd) - Math.Sqrt(minDistSqrd), 0, 0);
                        //Debug.WriteLine("Rebound for {0}, X:{1}, Y:{2}", i, rebound[i].getXValue(), rebound[i].getYValue());
                    }
                }
            }

            velocity = newV;

            for (int i = 0; i < objects.Length; i++) { // update positions

                objects[i].Move(((velocity[i].getXValue() + rebound[i].getXValue()) * tInt), ((velocity[i].getYValue() + rebound[i].getYValue()) * tInt)); // error
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
