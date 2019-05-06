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

namespace Physics_Simulator {

    /// <summary>
    /// ROOT FRAME AS OF NOW FOR APP
    /// 
    /// It's job is to be a platform to navigate accross the different pages.
    /// </summary>

    public delegate void BuildLessonAction();

    public sealed partial class Classroom : Page {

        public static BuildLessonAction BLA;
        private int PrevSelect = 1;

        /// <summary>
        /// Set up splitview, listbox menu and frame.
        /// </summary>
        
        public Classroom() {
            this.InitializeComponent();

            if (HUB.usingPreBuiltLessons) {
                LessonFrame.Navigate(typeof(Lesson1));
            }

            else {
                LessonFrame.Navigate(typeof(Lesson));
            }
            MenuItem1.IsSelected = true;
        }

        /// <summary>
        /// Whenever the listbox menu is clicked, navigate correctly or expand the panel
        /// </summary>
        
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            bool reset = false;

            if (MenuHamburgerItem.IsSelected) {
                MenuSplitView.IsPaneOpen = !MenuSplitView.IsPaneOpen;
                MenuHamburgerItem.IsSelected = false;
                reset = true;
            }

            if (MenuItem1.IsSelected || (reset && PrevSelect == 1)) {
                HUB.config = 1;
                if (HUB.usingPreBuiltLessons) LessonFrame.Navigate(typeof(Lesson1));
                else { BLA(); }
                PrevSelect = 1;
            }

            else if (MenuItem2.IsSelected || (reset && PrevSelect == 2)) {
                HUB.config = 2;
                if (HUB.usingPreBuiltLessons) LessonFrame.Navigate(typeof(Lesson2));
                else { BLA(); }
                PrevSelect = 2;
            }

            else if (MenuItem3.IsSelected || (reset && PrevSelect == 3)) {
                HUB.config = 3;
                if (HUB.usingPreBuiltLessons) LessonFrame.Navigate(typeof(Lesson4));
                else { BLA(); }
                PrevSelect = 3;
            }

            /*
            else if (MenuItem4.IsSelected || (reset && PrevSelect == 4)) {
                HUB.config = 4;
                if (HUB.usingPreBuiltLessons) LessonFrame.Navigate(typeof(Lesson3));
                else { BLA(); }
                PrevSelect = 4;
            }

            else if (MenuItem5.IsSelected || (reset && PrevSelect == 5)) {
                HUB.config = 5;
                if (HUB.usingPreBuiltLessons) LessonFrame.Navigate(typeof(Lesson5));
                else { BLA(); }
                PrevSelect = 5;
            }
            */
        }
    }
}
