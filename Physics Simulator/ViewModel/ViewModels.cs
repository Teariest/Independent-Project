using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics_Simulator.ViewModel {
     
    class LessonViewModel {

        public string title { get; set; }
        public string description { get; set; }

        public SimViewModel simulation { get; set; }
    }

    class SimViewModel {

        public EllipseViewModel[] objects { get; set; }


    }

    class EllipseViewModel {

    }

    class VectorViewModel {

        public double x { get; set; }
        public double y { get; set;  }
    }
}
