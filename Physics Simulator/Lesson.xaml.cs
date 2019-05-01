﻿using Physics_Simulator.ViewModel;
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
        private int rowNum = 0;

        public Lesson() {
            this.InitializeComponent();

            HUB.root = XMLParser.ParseLesson("Assets/LessonData.xml");

            Classroom.BLA = BuildLesson;

            BuildLesson();
        }

        public void BuildLesson() {

            XMLTree lroot = HUB.root.children.ElementAt(0);

            GridCanvas.Children.Clear();
            GridCanvas.RowDefinitions.Clear();
            rowNum = 0;

            // config starts at 1
            if (HUB.config == 1) {
                lroot = lroot.children.ElementAt(0);
                BuildFromTree(lroot, "");
            }
        }

        private void BuildTextBox(bool title, string text, int id) { // simpler version without all the parameters

            BuildTextBox(new int[] { 0, 0, 0, 0 }, title, text, id - 1, 4);
        }

        // margin is left(1), top(2), right(3), bottom(4)       (relPox)
        private void BuildTextBox(int[] margin, bool title, string text, int relID, int relPos) {

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

            GridCanvas.RowDefinitions.Add(new RowDefinition());
            GridCanvas.RowDefinitions.ElementAt(rowNum).Height = new GridLength(1, GridUnitType.Star);
            GridCanvas.Children.Add(block);
            block.SetValue(Grid.RowProperty, rowNum);
            rowNum++;
        }

        private void BuildFromTree(XMLTree node, string p) {
            
            if (node.tagName.Equals("Title")) {

                BuildTextBox(true, node.content, 0);
                return;
            }

            else if (node.tagName.Equals("Content")) {
                int i = 1;
                foreach (XMLTree n in node.children) {

                    BuildTextBox(false, n.content, i);
                }
                return;
            }

            else if (node.tagName.Equals("SimulationData")) {

                // TODO
            }

            else if (p.Equals("SimulationData")) {

            }

            else {
                foreach(XMLTree n in node.children) {
                    BuildFromTree(n, "");
                }
            }
        }
    }
}


// source1: https://stackoverflow.com/questions/10686917/setting-the-style-property-of-a-wpf-label-in-code