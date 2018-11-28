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
            for (int itemInt = 0; itemInt < objects.Length; itemInt++)
            {
                EngineBox item = objects[itemInt];

                if (item.GetMass() == 0)
                    continue;
                
                double leftB = item.GetXPos();
                double rightB = item.GetWidth() + leftB;
                double topB = item.GetYPos();
                double botB = item.GetHeight() + topB;

                for (int subjectInt = 0; subjectInt < velocity.Length; subjectInt++)
                {
                    if (subjectInt == itemInt) continue;

                    EngineBox subject = objects[subjectInt];

                    double leftS = subject.GetXPos();
                    double rightS = subject.GetWidth() + leftS;
                    double topS = subject.GetYPos();
                    double botS = subject.GetHeight() + topS;

                    bool overlapX = !(leftB > rightS || rightB < leftS);
                    bool overlapY = !(botB < topS || topB > botS);

                    if (overlapX && overlapY)
                    {

                        double xFinalVelocity = ((velocity[subjectInt].getXValue() * subject.GetMass()) + (velocity[itemInt].getXValue() * item.GetMass())) / (subject.GetMass() + item.GetMass());
                        double yFinalVelocity = ((velocity[subjectInt].getYValue() * subject.GetMass()) + (velocity[itemInt].getYValue() * item.GetMass())) / (subject.GetMass() + item.GetMass());

                        velocity[itemInt] = new Vector(0, 0, xFinalVelocity, yFinalVelocity);
                    }
                }

                // find a different way to do gravity
                velocity[itemInt].Add(0, 0, 0, g);
                item.Move(velocity[itemInt], fps);
            }
        }
    }
}
