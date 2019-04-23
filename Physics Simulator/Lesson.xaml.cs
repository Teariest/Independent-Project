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
                BuildTextBox(new int[] { 0, 0, 0, 0 }, true, "hello world");
            }

            if (HUB.config == 2) {
                BuildTextBox(new int[] { 10, 0, 0, 0 }, true, "hello world 2");
            }

            if (HUB.config == 3) {
                BuildTextBox(new int[] { 20, 40, 0, 0 }, false, "hello world");
            }
        }

        private void BuildTextBox(int[] margin, bool title, string text) { // margin is left, top, right, bottom

            if (margin.Length != 4 || string.IsNullOrEmpty(text)) {
                throw new System.Exception("Illegal parameter");
            }

            TextBlock block = new TextBlock();

            block.Text = text;
            block.Margin = new Thickness(margin[0], margin[1], margin[2], margin[3]);        
            // Get style from xaml document and apply it here to the block
            if (title) {
                block.Style = (Style) this.Resources.Where(nab => nab.Key.ToString() == "TitleText").FirstOrDefault().Value; // source1
            }
            else {
                block.Style = (Style)this.Resources.Where(nab => nab.Key.ToString() == "ContentText").FirstOrDefault().Value; // source1
            }
            RelativeCanvas.Children.Add(block);
        }
    }
}


// source1: https://stackoverflow.com/questions/10686917/setting-the-style-property-of-a-wpf-label-in-code