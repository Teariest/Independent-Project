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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics_Simulator
{
    
    public sealed partial class SimulationPage : Page
    {

        private int fps = 60; // Frames per second
        private double g = 0.3; // Gravity of simulation

        private DispatcherTimer timer; // Timer that requests engine to calculate and move objects at every interval

        private Engine simEngine; // Physics Engine

        public SimulationPage()
        {
            this.InitializeComponent();


            // Objects and their Starting velocities to be passed on to Engine
            EngineBox[] objects = new EngineBox[4];
            objects[0] = new EngineBox(100, 100, 20, 20, 1, new SolidColorBrush(Color.FromArgb(255, 255, 100, 200)), SimCanvas);
            objects[1] = new EngineBox(100, 150, 20, 20, 1, new SolidColorBrush(Color.FromArgb(255, 100, 100, 255)), SimCanvas);
            objects[2] = new EngineBox(50, 200, 10, 300, 0, new SolidColorBrush(Color.FromArgb(255, 100, 255, 255)), SimCanvas);
            objects[3] = new EngineBox(200, 100, 10, 10, 1, new SolidColorBrush(Color.FromArgb(255, 100, 100, 255)), SimCanvas);

            Vector[] vectors = new Vector[4];
            vectors[0] = new Vector(0, 0, 0, 0);
            vectors[1] = new Vector(0, 0, 0, 0);
            vectors[2] = new Vector(0, 0, 0, 0);
            vectors[3] = new Vector(0, 0, -3, 0);

            // Build Engine
            simEngine = new Engine(objects, vectors, fps, g);

            // Build Dispatcher
            timer = new DispatcherTimer();
            timer.Tick += Dispatch;
            timer.Interval = new TimeSpan(0, 0, 0, 0, (1000/fps));
            timer.Start();
            
        }

        /// <summary>
        /// Runs once for every frame, changes everything that has to be changed during the frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dispatch(object sender, object e)
        {
            simEngine.ExecuteNext();
        }
    }
}
