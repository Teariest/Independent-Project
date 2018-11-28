using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Physics_Simulator
{
    class Engine
    {
        private double fps; // frames per second
        private double g; // in u/s/s

        private EngineBox[] objects; // All objects within simulation, each with a unique index

        private Vector[] velocity; // Velocities per object, same index as reference object's index within 'objects'

        /// <summary>
        /// Setup Engine and fields
        /// </summary>
        /// <param name="objects">List of objects used by Engine</param>
        /// <param name="fps">Frames per second simulation will be running at</param>
        /// <param name="gravity">Simulation Gravity</param>
        /// <param name="startV">Velocities that each object should start with</param>
        public Engine(EngineBox[] objects, Vector[] startV, int fps, double gravity)
        {
            this.fps = fps;
            g = gravity;

            this.objects = objects;

            velocity = startV;
        }

        /// <summary>
        /// Calculate physics and move all objects accordingly over pre-determined amount of time
        /// </summary>
        public void ExecuteNext()
        {
            
            for (int itemInt = 0; itemInt < objects.Length; itemInt++) // Loop goes over every item, but ignores static objects
            {
                EngineBox item = objects[itemInt];
                
                // item boundaries
                double leftB = item.GetXPos();
                double rightB = item.GetWidth() + leftB;
                double topB = item.GetYPos();
                double botB = item.GetHeight() + topB;

                for (int subjectInt = itemInt+1; subjectInt < velocity.Length; subjectInt++) // Loop goes over every object after the target object
                {

                    EngineBox subject = objects[subjectInt];

                    // subject boundaries
                    double leftS = subject.GetXPos();
                    double rightS = subject.GetWidth() + leftS;
                    double topS = subject.GetYPos();
                    double botS = subject.GetHeight() + topS;

                    if (leftB < rightS && rightB > leftS && topB < botS && botB > topS) // if there is overlap [rectangle]
                    {
                        // re-bound

                        item.ReboundMove(velocity[itemInt], fps);
                        subject.ReboundMove(velocity[subjectInt], fps);

                        // change velocities
                        double nVX = (velocity[itemInt].getXValue() * item.GetMass() + velocity[subjectInt].getXValue() * subject.GetMass()) / (item.GetMass() + subject.GetMass()); // new x velocity
                        double nVY = (velocity[itemInt].getYValue() * item.GetMass() + velocity[subjectInt].getYValue() * subject.GetMass()) / (item.GetMass() + subject.GetMass()); // new x velocity

                        velocity[itemInt] = new Vector(0, 0, nVX, nVY);
                        velocity[subjectInt] = new Vector(0, 0, nVX, nVY);
                    }
                }

                if (item.GetMass() == 0) // do not affect static objects
                    continue;
                 
                velocity[itemInt].Add(0, 0, 0, g);
                item.Move(velocity[itemInt], fps);
            }
        }
    }
}
