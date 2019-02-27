using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics_Simulator.ViewModel;

namespace Physics_Simulator {

    class HUB {

        public static XMLTree root;

        public static int config = 1;

        public static bool usingPreBuiltLessons = false;
        public static bool testingSimulator = false; // precedes usingPreBuiltLessons
    }
}
