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
            
            for (int i = 0; i < objects.Length; i++) { // change velocities

                for (int j = 0; j < objects.Length; j++) {

                    // Collisions
                    if ((Math.Pow(objects[i].GetXPos() - objects[j].GetXPos(), 2) + Math.Pow(objects[i].GetYPos() - objects[j].GetYPos(), 2)) <= ((Math.Pow((objects[i].GetRadius() + objects[j].GetRadius()), 2)))) {

                        double ma = objects[i].GetMass();
                        Vector va = velocity[i];
                        double mb = objects[j].GetMass();
                        Vector vb = velocity[j];
                        double e = objects[i].GetElasticity();

                        double xv = (ma * va.getXValue() + mb * vb.getXValue() + mb * e * (vb.getXValue() - va.getXValue())) / (ma + mb);
                        double yv = (ma * va.getYValue() + mb * vb.getYValue() + mb * e * (vb.getYValue() - va.getYValue())) / (ma + mb);

                        newV[i] = new Vector(0, 0, xv, yv);
                    }
                }
            }

            velocity = newV;

            for (int i = 0; i < objects.Length; i++) { // update positions

                objects[i].Move((velocity[i].getXValue() * tInt), (velocity[i].getYValue() * tInt));
            }
        }

        public EngineCircle[] Positions() {
            return objects;
        }
    }
}
