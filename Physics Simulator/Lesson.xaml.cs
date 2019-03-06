using Physics_Simulator.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;



namespace Physics_Simulator {

    /// <summary>
    /// Retrieves XML data from parser, uses data to build the page accordingly
    /// </summary>

    public sealed partial class Lesson : Page {

        private int pageWidth = 1280; // width in pixels

        public Lesson() {
            this.InitializeComponent();

            HUB.root = XMLParser.ParseLesson("Assets/LessonData.xml");

            Classroom.BLA = BuildLesson;

            BuildLesson();
        }

        public void BuildLesson() {

            RelativeCanvas.Children.Clear();

            if (HUB.config == 1) {
                BuildTextBox(0, true, "hello world");
            }

            if (HUB.config == 2) {
                BuildTextBox(0, true, "hello world 2");
            }

            if (HUB.config == 3) {
                BuildTextBox(0, false, "hello world");
            }
        }

        private void BuildTextBox(int hAlignment, bool title, string text) {

            TextBlock block = new TextBlock();

            block.Text = text;

            if (title) {
                block.Style = (Style) this.Resources.Where(nab => nab.Key.ToString() == "TitleText").FirstOrDefault().Value; // source1
            }
            else {
                block.Style = (Style)this.Resources.Where(nab => nab.Key.ToString() == "ContentText").FirstOrDefault().Value; // source1
            }
            Debug.WriteLine("Test 1534");
            RelativeCanvas.Children.Add(block);
        }
    }
}


// source1: https://stackoverflow.com/questions/10686917/setting-the-style-property-of-a-wpf-label-in-code