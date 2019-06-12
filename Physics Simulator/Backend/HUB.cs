using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics_Simulator.ViewModel;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Physics_Simulator {

    class HUB {

        private static MediaPlayer pCollisionPlayer;
        public static MediaPlayer collisionPlayer { get {
                if (pCollisionPlayer == null) {
                    Debug.WriteLine("Test");
                    pCollisionPlayer = new MediaPlayer();
                    pCollisionPlayer.Volume = 100.00;
                    pCollisionPlayer.Source = MediaSource.CreateFromUri(new Uri("C:/Users/s-faisandierc/source/repos/Physics Simulator/Physics Simulator/Assets/BounceSound.wav"));
                }
                return pCollisionPlayer;
            } }

        public static XMLTree root; // should start at <xml> tag
        public static XMLTree simRoot; // <SimData> node that contains next simulation to be loaded in if using lesson.xaml

        public static int config = 1; // which lesson should be displayed, starts at 1

        public static bool usingPreBuiltLessons = false;
        public static bool testingSimulator = false;
        public static bool collisionSounds = true;
        public static bool backgroundMusic = true;
    }
}
