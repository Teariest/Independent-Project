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
using Physics_Simulator.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics_Simulator {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Classroom : Page {

        private int PrevSelect = 1;

        public Classroom() {
            this.InitializeComponent();

            LessonFrame.Navigate(typeof(Lesson1));
            MenuItem1.IsSelected = true;

            XMLParser.ParseLesson("Assets/LessonData.xml");
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            bool reset = false;
            
            if (MenuHamburgerItem.IsSelected) {
                MenuSplitView.IsPaneOpen = !MenuSplitView.IsPaneOpen;
                MenuHamburgerItem.IsSelected = false;
                reset = true;
            }

            if (MenuItem1.IsSelected || (reset && PrevSelect == 1)) {
                LessonSimulationLiason.config = 1;
                LessonFrame.Navigate(typeof(Lesson1));
                PrevSelect = 1;
            }
            
            else if (MenuItem2.IsSelected || (reset && PrevSelect == 2)) {
                LessonSimulationLiason.config = 2;
                LessonFrame.Navigate(typeof(Lesson2));
                PrevSelect = 2;
            }

            else if (MenuItem3.IsSelected || (reset && PrevSelect == 3)) {
                LessonSimulationLiason.config = 3;
                LessonFrame.Navigate(typeof(Lesson3));
                PrevSelect = 3;
            }

            else if (MenuItem4.IsSelected || (reset && PrevSelect == 4)) {
                LessonSimulationLiason.config = 4;
                LessonFrame.Navigate(typeof(Lesson4));
                PrevSelect = 4;
            }

            else if (MenuItem5.IsSelected || (reset && PrevSelect == 5)) {
                LessonSimulationLiason.config = 5;
                LessonFrame.Navigate(typeof(Lesson5));
                PrevSelect = 5;
            }
        }
    }
}
