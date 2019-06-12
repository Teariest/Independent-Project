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
using Physics_Simulator.ViewModel;

namespace Physics_Simulator {

    public sealed partial class SimulationPage : Page {

        // Physics Engine
        private Engine simEngine;

        // Objects
        private Ellipse[] UIObjects; // Visual Representations of object
        private EngineCircle[] eObjects; // Engine Objects
        private Vector[] vectors;

        // Environment
        private int fps = 60; // Frames per second
        private double g = -5; // Gravity of simulation
        private double gA = -Math.PI / 2; // Direction of Gravity

        // XML Root
        private XMLTree pLocalRoot;

        // Dispatch
        private DispatcherTimer timer; // Timer that requests engine to calculate and move objects at every interval
        private DispatcherTimer resetTimer; // Timer to reset engine
        private int resetTime = 7; // Reset interval in seconds
        private bool reset = true;

        // User Editing
        private int[][] targets;


        // Fields
        // ------------------------------------------------------------------------->
        // Methods



        // Constructor
        public SimulationPage() {
            this.InitializeComponent();

            Build(); // Build Sim

            // Build Dispatcher, should only be called one SimulationPage is built so keep within constructor
            timer = new DispatcherTimer();
            timer.Tick += Dispatch;
            timer.Interval = new TimeSpan(0, 0, 0, 0, (1000 / fps));
            timer.Start();

            resetTimer = new DispatcherTimer();
            resetTimer.Tick += Reset;
            resetTimer.Interval = new TimeSpan(0, 0, 0, resetTime);
            if (resetTime != 0) {
                resetTimer.Start();
            }
        }

        // Build the Sim but not the timing elements, root build method, can be used from any point
        private void Build() {

            // Sets up all of the engine's parameters
            if (HUB.testingSimulator) { SetupDebugSim(); } // Build Debug Sim
            else if (HUB.usingPreBuiltLessons) { SetupSimFromPrebuildLesson(); } // Build Pre-Built Lesson Sims
            else { SetupSim(); } // Build Sims based off of xml file

            // Build Engine
            simEngine = new Engine(eObjects, vectors, fps, g, gA);
        }
        
        // Runs once for every frame, changes everything that has to be changed during the frame, called by dispatcher
        private void Dispatch(object sender, object e) {
            
            simEngine.ExecuteNext();
            RefreshDisplay();
        }

        // Re-builds the entire simulation, gets called by dispatcher
        private void Reset(object sender, object e) {
            
            if (reset == false && resetTime != 0) { // re-launch the reset sequence
                reset = true;
                resetTimer.Tick += Reset;
                resetTimer.Start();
            }
            SimCanvas.Children.Clear();
            Build();
        }

        private void RefreshDisplay() {
            EngineCircle[] pos = simEngine.Positions(); // retrieve all object positions from the engine
            
            for (int i = 0; i < pos.Length; i++) { // Display all object positions
                UIObjects[i].SetValue(Canvas.LeftProperty, (pos[i].GetXPos() - pos[i].GetRadius()) * 10);
                UIObjects[i].SetValue(Canvas.TopProperty, (pos[i].GetYPos() - pos[i].GetRadius()) * 10);
            }
        }
        
        // Variables are in meters, meters per second, degrees and kilograms
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

        // Build the simulation for the pre-built lesson xaml pages
        public void SetupSimFromPrebuildLesson() {

                int n = 0;

                switch (HUB.config) {

                    case 1:
                        n = 2;
                        UIObjects = new Ellipse[n];
                        eObjects = new EngineCircle[n];
                        vectors = new Vector[n];
                        //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                        BuildEllipse(00, 10, 20, 2, 255, 255, 087, 051, 0, 0, 10, 00, 1, 1.0);
                        BuildEllipse(01, 80, 30, 2, 255, 051, 087, 255, 0, 0, -10, 00, 1, 1.0);
                        g = 0;
                        break;

                    case 2:
                        n = 2;
                        UIObjects = new Ellipse[n];
                        eObjects = new EngineCircle[n];
                        vectors = new Vector[n];
                        //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                        BuildEllipse(00, 35, 20, 2, 255, 255, 087, 051, 0, 0, 00, 00, 1, 1.0);
                        BuildEllipse(01, 40, 15, 2, 255, 255, 087, 051, 0, 0, 00, -10, 1, 1.0);
                        g = -5;
                        break;

                    case 3:
                        n = 4;
                        UIObjects = new Ellipse[n];
                        eObjects = new EngineCircle[n];
                        vectors = new Vector[n];
                        //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                        BuildEllipse(00, 35, 20, 2, 255, 255, 087, 051, 10, 1, 00, 00, 1, 1.0);
                        BuildEllipse(01, 40, 15, 1, 255, 255, 051, 087, 10, -2, 00, 00, 1, 1.0);
                        BuildEllipse(02, 35, 30, 3, 255, 087, 051, 255, 10, -0.75, 00, 00, 1, 1.0);
                        BuildEllipse(03, 45, 18, 2, 255, 051, 087, 255, 10, -0.3, 00, 00, 1, 1.0);
                        g = -5;
                        break;

                    case 4:
                        n = 1;
                        UIObjects = new Ellipse[n];
                        eObjects = new EngineCircle[n];
                        vectors = new Vector[n];
                        //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                        BuildEllipse(00, 35, 20, 2, 255, 255, 087, 051, 0, 0, 10, -10, 1, 1.0);
                        g = -10;
                        break;

                    case 5:
                        n = 4;
                        UIObjects = new Ellipse[n];
                        eObjects = new EngineCircle[n];
                        vectors = new Vector[n];
                        //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                        BuildEllipse(00, 35, 20, 2, 255, 051, 087, 255, 0, 0, 20, 00, 1, 0);
                        BuildEllipse(01, 70, 20, 2, 255, 255, 087, 051, 0, 0, -20, 00, 1, 0);
                        BuildEllipse(02, 35, 30, 2, 255, 051, 087, 255, 0, 0, 20, 00, 1, 0);
                        BuildEllipse(03, 50, 30, 2, 255, 255, 087, 051, 0, 0, 0, 00, 1, 0);
                        g = 0;
                        break;
                }
            

            
            }

        public void SetupSim() {

            if (pLocalRoot == null) {
                pLocalRoot = HUB.simRoot.Duplicate();
            }
            XMLTree rn = pLocalRoot.Duplicate(); // sim node (not sim list but <Simulation>)

            // setup environment
            fps = int.Parse(rn.children.ElementAt(0).content);
            g = double.Parse(rn.children.ElementAt(1).content);
            gA = double.Parse(rn.children.ElementAt(2).content);
            resetTime = int.Parse(rn.children.ElementAt(3).content);

            XMLTree o = rn.children.ElementAt(4); // object list

            // setup build ellipse
            int c = o.children.Count;
            UIObjects = new Ellipse[c];
            eObjects = new EngineCircle[c];
            vectors = new Vector[c];

            int i = 0;
            foreach (XMLTree n in o.children) { // foreach object in simulation, build ellipse

                BuildEllipse(i, // index
                    double.Parse(n.children.ElementAt(0).content), // xPos
                    double.Parse(n.children.ElementAt(1).content), // yPos
                    double.Parse(n.children.ElementAt(2).content), // radius
                    byte.Parse(n.children.ElementAt(5).children.ElementAt(0).content), // a
                    byte.Parse(n.children.ElementAt(5).children.ElementAt(1).content), // r
                    byte.Parse(n.children.ElementAt(5).children.ElementAt(2).content), // g
                    byte.Parse(n.children.ElementAt(5).children.ElementAt(3).content), // b
                    double.Parse(n.children.ElementAt(6).children.ElementAt(2).content), // mag
                    double.Parse(n.children.ElementAt(6).children.ElementAt(3).content), // angle
                    double.Parse(n.children.ElementAt(6).children.ElementAt(0).content), // xV
                    double.Parse(n.children.ElementAt(6).children.ElementAt(1).content), // yV
                    double.Parse(n.children.ElementAt(3).content), // mass
                    double.Parse(n.children.ElementAt(4).content)); // e
                i++;
            }

            // UserEdits

            SimStackPanel.Children.Clear();

            if (rn.children.Count == 6) { // If simulation has UserEdits

                o = rn.children.ElementAt(5); // UserEdits list

                List<int[]> targetList = new List<int[]>(o.children.Count); // List of arrays that will be added to int[][] targets
                
                foreach(XMLTree n in o.children) { // for each object available for edit

                    int objectID; // ObjectID from xml

                    // If the ObjectID from the xml is not valid, then throw an exception
                    if ((!int.TryParse(n.children.ElementAt(0).content, out objectID)) || objectID >= eObjects.Length) {
                        throw new InvalidDataException("XML format error: <objectID>" + n.children.ElementAt(0).content + "</objectID>. " + n.children.ElementAt(0).content + " is not a valid objectID.");
                    }
                    
                    int[] tempArray = new int[n.children.Count]; // array to be added to targetList

                    tempArray[0] = objectID;

                    for (i = 1; i < n.children.Count; i++) { // goes through each target of object available for edit

                        int target; // targeted value ID of taret object from xml

                        // If the target tag's content from the xml is not valid then throw an exception
                        if ((!int.TryParse(n.children.ElementAt(i).content, out target)) || target > 8) {
                            throw new InvalidDataException("XML format error: <target>" + n.children.ElementAt(i).content + "</target>. " + n.children.ElementAt(i).content + " is not a valid target.");
                        }

                        tempArray[i] = target; // add <target> value to array

                        // Setup the TextBox user will input values into
                        TextBox b = new TextBox();
                        b.Width = 100; // set width of box to 80 pixels
                        b.CharacterReceived += TextBoxHandler; // call handler when new character is received
                        // add objectID # to box access key, to the left of the '|' the integer is the objectID,
                        //to the right it is the target value, access key is like a name for the textbox
                        b.AccessKey += objectID + "|" + target;

                        switch(target) {
                            case 0:
                                b.PlaceholderText = "x pos";
                                break;
                            case 1:
                                b.PlaceholderText = "y pos";
                                break;
                            case 2:
                                b.PlaceholderText = "radius";
                                break;
                            case 3:
                                b.PlaceholderText = "mass";
                                break;
                            case 4:
                                b.PlaceholderText = "elasticiy";
                                break;
                            case 5:
                                b.PlaceholderText = "x velocity";
                                break;
                            case 6:
                                b.PlaceholderText = "y velocity";
                                break;
                            case 7:
                                b.PlaceholderText = "velocity";
                                break;
                            case 8:
                                b.PlaceholderText = "direction";
                                break;
                        }

                        SimStackPanel.Children.Add(b);
                    }

                    targetList.Add(tempArray); // add array to list
                }

                targets = targetList.ToArray(); // targets = list
            }

            // Add a reset Button

            Button resetB = new Button();
            resetB.Content = "Reset";
            resetB.Width = 100;
            resetB.Click += Reset;
            SimStackPanel.Children.Add(resetB);
        }

        // Build Simulation to Debug the engine
        public void SetupDebugSim() {

            int numObjects = 11;

            UIObjects = new Ellipse[numObjects];
            eObjects = new EngineCircle[numObjects];
            vectors = new Vector[numObjects];

            // Set up all objects
            //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
            BuildEllipse(00, 10, 05, 2, 255, 000, 000, 000, 0, 0, 10, 00, 1, 1.0); // constant
            BuildEllipse(01, 10, 10, 2, 255, 250, 000, 000, 0, 0, 10, 00, 1, 0.0); // 0 elasticity
            BuildEllipse(02, 20, 10, 2, 255, 250, 000, 000, 0, 0, 00, 00, 1, 0.0);
            BuildEllipse(03, 10, 20, 2, 255, 000, 250, 000, 0, 0, 10, 00, 1, 0.5); // 0.5 elasticity
            BuildEllipse(04, 20, 20, 2, 255, 000, 250, 000, 0, 0, 00, 00, 1, 0.5);
            BuildEllipse(05, 10, 30, 2, 255, 000, 000, 250, 0, 0, 10, 00, 1, 1.0); // 1 elasticity
            BuildEllipse(06, 20, 30, 2, 255, 000, 000, 250, 0, 0, 00, 00, 1, 1.0);
            BuildEllipse(07, 10, 40, 2, 255, 000, 250, 250, 0, 0, 10, 00, 1, 2.0); // 2 elasticity
            BuildEllipse(08, 20, 40, 2, 255, 000, 250, 250, 0, 0, 00, 00, 1, 2.0);
            BuildEllipse(09, 10, 50, 2, 255, 250, 250, 000, 0, 0, 10, 10, 1, 1.0); // 1 elasticity diagonal
            BuildEllipse(10, 20, 60, 2, 255, 250, 250, 000, 0, 0, 00, 00, 1, 1.0);
            // | CHANGE SIZE OF ARRAY WHEN ADDING OR REMOVING OBJECT |
        }

        // Handles inputs from TextBox
        private void TextBoxHandler(UIElement element, CharacterReceivedRoutedEventArgs argument) {

            if (((TextBox)element).Text.Length == 0) { return; } // if nothing is entered, don't do anything

            if (argument.Character.GetHashCode() == 851981) { // If user presses enter into the textbox
                double value;
                if (double.TryParse(((TextBox)element).Text, out value)) {
                    ChangeSimulation(element, value);
                }
            }
            else if (!Char.IsNumber(argument.Character) && !argument.Character.Equals('.') && !argument.Character.Equals('\b') && !argument.Character.Equals('-')) { // If the user input isn't a number then remove it
                ((TextBox)element).Text = ((TextBox)element).Text.Substring(0, ((TextBox)element).Text.Length - 1);
            }
        }

        // Changes simulation data given UI Element user interacted with and it's value.
        private void ChangeSimulation(UIElement box, double value) {

            /* 0:x
             * 1:y
             * 2:r
             * 3:mass
             * 4:e
             * 5:vx
             * 6:vy
             * 7:mag
             * 8:angle
            */

            if (reset == true) { // stop auto-reset
                reset = false;
                resetTimer.Tick -= Reset;
                resetTimer.Stop();
            }

            int splitIndex = box.AccessKey.IndexOf('|');
            int objectID = int.Parse(box.AccessKey.Substring(0, splitIndex));
            int targetID = int.Parse(box.AccessKey.Substring(++splitIndex));
            
            switch (targetID) {
                case 0: // x pos
                    eObjects[objectID].x = value;
                    break;
                case 1: // y pos
                    eObjects[objectID].y = value;
                    break;
                case 2: // radius
                    value *= 20;
                    eObjects[objectID].r = value;
                    UIObjects[objectID].Height = value;
                    UIObjects[objectID].Width = value;
                    eObjects[objectID].x -= value * 10;
                    eObjects[objectID].y -= value * 10;
                    break;
                case 3: // mass
                    eObjects[objectID].m = value;
                    break;
                case 4: // elasticity
                    eObjects[objectID].e = value;
                    break;
                case 5: // x vector
                    vectors[objectID].xVal = value;
                    break;
                case 6: // y vector
                    vectors[objectID].yVal = value;
                    break;
                case 7: // magnitude
                    vectors[objectID].mag = value;
                    break;
                case 8: // angle
                    vectors[objectID].angle = value;
                    break;
            }
        }
    }
}
