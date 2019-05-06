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
using System.Windows;
using System.Diagnostics;

namespace Physics_Simulator {

    /// <summary>
    /// Retrieves XML data from parser, uses data to build the page accordingly
    /// </summary>

    public sealed partial class Lesson : Page {

        private int pageWidth = 1300; // width in pixels
        private int rowNum = 0;

        public Lesson() {
            this.InitializeComponent();

            HUB.root = XMLParser.ParseLesson("Assets/LessonData.xml");

            Classroom.BLA = BuildLesson;

            GridCanvas.ColumnDefinitions.Add(new ColumnDefinition());
            GridCanvas.ColumnDefinitions.ElementAt(rowNum).Width = new GridLength(pageWidth-20);

            BuildLesson();
        }

        public void BuildLesson() {

            XMLTree lroot = HUB.root.children.ElementAt(0);

            GridCanvas.Children.Clear();
            GridCanvas.RowDefinitions.Clear();
            rowNum = 0;
            
            lroot = lroot.children.ElementAt(HUB.config - 1);
            BuildFromTree(lroot);
        }

        private void BuildTextBox(bool title, string text, int id) { // simpler version without all the parameters

            BuildTextBox(new int[] { 20, 10, 0, 20 }, title, text);
        }

        // margin is left, top, right, bottom
        private void BuildTextBox(int[] margin, bool title, string text) {

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
            GridCanvas.RowDefinitions.ElementAt(rowNum).Height = new GridLength(1, GridUnitType.Auto);
            

            GridCanvas.Children.Add(block);
            block.SetValue(Grid.RowProperty, rowNum);
            block.SetValue(Grid.ColumnProperty, 0);

            rowNum++;
        }

        private void BuildFromTree(XMLTree node) {
            
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

                foreach (XMLTree n in node.children) {

                    HUB.simRoot = n.Duplicate(); // give duplicate to make sure that there isn't funny business where I edit node in another class but use it again here assuming it wasn't changed elsewhere

                    Frame sim = new Frame();
                    sim.Navigate(typeof(SimulationPage));

                    GridCanvas.RowDefinitions.Add(new RowDefinition());
                    GridCanvas.RowDefinitions.ElementAt(rowNum).Height = new GridLength(350);


                    GridCanvas.Children.Add(sim);
                    sim.SetValue(Grid.RowProperty, rowNum);
                    sim.SetValue(Grid.ColumnProperty, 0);

                    rowNum++;
                }
            }

            else {
                foreach(XMLTree n in node.children) {
                    BuildFromTree(n);
                }
            }
        }
    }
}


// source1: https://stackoverflow.com/questions/10686917/setting-the-style-property-of-a-wpf-label-in-code