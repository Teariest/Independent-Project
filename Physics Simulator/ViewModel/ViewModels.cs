using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics_Simulator.ViewModel {

    interface ViewModel {
    }
     
    class LessonViewModel : ViewModel {

        public int rank { get { return 0; } }

        public string title { get; set; }
        public string[] content { get; set; }

        public SimViewModel[] simulation { get; set; }
    }

    class SimViewModel : ViewModel {

        public double fps { get; set; }
        public double g { get; set; }
        public double ga { get; set; }


        public EllipseViewModel[] objects { get; set; }
        public VectorViewModel[] velocities { get; set; }
    }

    class EllipseViewModel : ViewModel {
        
        public double x { get; set; }
        public double y { get; set; }
        public double r { get; set; }
        public double m { get; set; }
        public double e { get; set; }
        public ColorViewModel color { get; set; }
    }

    class VectorViewModel : ViewModel {

        public double x { get; set; }
        public double y { get; set; }
        public double a { get; set; }
        public double m { get; set; }
    }

    class ColorViewModel : ViewModel {

        public double a { get; set; };
        public double r { get; set; };
        public double g { get; set; };
        public double b { get; set; };
    }
}
