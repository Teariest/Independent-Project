using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Navigation;



namespace Physics_Simulator {

    public sealed partial class SimulationPage : Page {

        private Ellipse[] UIObjects; // Visual Representations of object
        private EngineCircle[] eObjects; // Engine Objects
        private Vector[] vectors;

        private int fps = 60; // Frames per second
        private double g = 0.3; // Gravity of simulation

        private DispatcherTimer timer; // Timer that requests engine to calculate and move objects at every interval

        private Engine simEngine; // Physics Engine

        public SimulationPage() {
            this.InitializeComponent();

            int numObjects = 8;
            UIObjects = new Ellipse[numObjects];
            eObjects = new EngineCircle[numObjects];
            vectors = new Vector[numObjects];

            // Set up all objects

            BuildEllipse(0, 10, 10, 2, 255, 250, 100, 0, 0, 0, 10, 0, 2, 1);
            BuildEllipse(1, 20, 10, 2, 255, 0, 250, 100, 0, 0, 0, 0, 2, 1);
            BuildEllipse(2, 10, 20, 2, 255, 0, 100, 250, 0, 0, 10, 0, 2, 2);
            BuildEllipse(3, 20, 20, 2, 255, 100, 250, 0, 0, 0, 0, 0, 2, 2);
            BuildEllipse(4, 10, 30, 2, 255, 100, 0, 250, 0, 0, 10, 0, 2, 0.5);
            BuildEllipse(5, 20, 30, 2, 255, 100, 200, 100, 0, 0, 0, 0, 2, 0.5);
            BuildEllipse(6, 10, 40, 2, 255, 200, 100, 100, 0, 0, 10, 0, 2, 0);
            BuildEllipse(7, 20, 40, 2, 255, 100, 100, 200, 0, 0, 0, 0, 2, 0);


            // Build Engine
            simEngine = new Engine(eObjects, vectors, fps, g);

            // Build Dispatcher
            timer = new DispatcherTimer();
            timer.Tick += Dispatch;
            timer.Interval = new TimeSpan(0, 0, 0, 0, (1000 / fps));
            timer.Start();

        }

        /// <summary>
        /// Runs once for every frame, changes everything that has to be changed during the frame
        /// </summary>
        private void Dispatch(object sender, object e) {
            simEngine.ExecuteNext();
            RefreshDisplay();
        }

        private void RefreshDisplay() {
            EngineCircle[] pos = simEngine.Positions();

            for (int i = 0; i < pos.Length; i++) {
                UIObjects[i].SetValue(Canvas.LeftProperty, (pos[i].GetXPos() - pos[i].GetRadius()) * 10);
                UIObjects[i].SetValue(Canvas.TopProperty, (pos[i].GetYPos() - pos[i].GetRadius()) * 10);
            }
        }

        /// <summary>
        /// Variables are in meters, meters per second and degrees
        /// </summary>
        public void BuildEllipse(int index, double xPos, double yPos, double radius, byte a, byte r, byte g, byte b, double magnitude, double angle, double xV, double yV, double mass, double elasticity) {
            UIObjects[index] = new Ellipse();
            UIObjects[index].Height = radius*20;
            UIObjects[index].Width = radius*20;
            UIObjects[index].Fill = new SolidColorBrush(Color.FromArgb(a, r, g, b));
            SimCanvas.Children.Add(UIObjects[index]);
            UIObjects[index].SetValue(Canvas.LeftProperty, (xPos-radius)*10);
            UIObjects[index].SetValue(Canvas.TopProperty, (yPos-radius)*10);
            eObjects[index] = new EngineCircle(xPos, yPos, radius, mass, elasticity);
            vectors[index] = new Vector(magnitude, angle, xV, yV);
        }
    }
}
