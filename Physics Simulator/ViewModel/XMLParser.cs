using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

namespace Physics_Simulator.ViewModel {
    class XMLParser {

        public static LessonViewModel ParseLesson(string filePath) {

            LessonViewModel model = new LessonViewModel();

            XmlReaderSettings settings = new XmlReaderSettings(); // allows us to ignore comments and only access nodes and relevant information
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            XmlReader reader = XmlReader.Create(filePath, settings);

            string nodeName = ""; // name of node containing value
            List<LessonViewModel> Classroom = new List<LessonViewModel>();

            while (reader.Read()) {
                

                switch (reader.Name) {

                    case "ClassroomData":

                        break;
                }





                /*
                //Debug.WriteLine("|" + i + "|: |" + reader.Name + "|: |" + reader.HasValue + "| |" + reader.IsEmptyElement + "| |" +  reader.Value + "|");

                if (reader.Name.Length == 0) { // if a value set value within model
                    model.GetType().GetProperty(nodeName).SetValue(model, reader.Value);
                }
                else { // if a node possibly containing a value
                    nodeName = reader.Name;
                }*/
            }

            return model;
        }

        public static void ParseSimulation() {

        }
    }
}
