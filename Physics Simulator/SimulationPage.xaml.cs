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
        private double g = 30;

        private DispatcherTimer timer;

        private Engine simEngine;

        public SimulationPage()
        {
            this.InitializeComponent();

            List<EngineBox> objects = new List<EngineBox>();

            objects.Add(new EngineBox(100, 100, 20, 20, 1, new SolidColorBrush(Color.FromArgb(255, 255, 100, 200)), SimCanvas));
            objects.Add(new EngineBox(50, 600, 10, 300, 0, new SolidColorBrush(Color.FromArgb(255, 100, 255, 255)), SimCanvas));

            simEngine = new Engine(objects, fps, g);

            timer = new DispatcherTimer();

            timer.Tick += Dispatch;
            timer.Interval = new TimeSpan(0, 0, 0, 0, (1000/fps));
            timer.Start();
            
        }

        private void Dispatch(object sender, object e)
        {
            simEngine.ExecuteNext();
        }
    }
}
