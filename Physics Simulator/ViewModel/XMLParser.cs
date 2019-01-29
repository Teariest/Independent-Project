using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;

namespace Physics_Simulator.ViewModel {
    class XMLParser {

        public static void ParseLesson(string filePath) {

            XmlReader reader = XmlReader.Create(filePath);
            
            int i = 1;
            while (reader.Read()) {
                //while (reader.HasValue && (!reader.IsEmptyElement)) { reader.Skip(); } // skips all junk

                Debug.WriteLine("|" + i + "|: |" + reader.Name + "|: |" + reader.HasValue + "| |" + reader.IsEmptyElement + "| |" +  reader.Value + "|");
                i++;
            }
        }

        public static void ParseSimulation() {

        }
    }
}
