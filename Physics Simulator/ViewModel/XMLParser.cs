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

        public static XMLTree ParseLesson(string filePath) {

            XmlReaderSettings settings = new XmlReaderSettings(); // allows us to ignore comments and only access nodes and relevant information
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            XmlReader reader = XmlReader.Create(filePath, settings);

            reader.Read(); // get to correct xml tag

            XMLTree tree = new XMLTree(reader.Name);

            RParser(reader, reader.Name, tree);

            return tree;
        }

        public static void RParser(XmlReader reader, string tagName, XMLTree node) {

            while (reader.Read()) {

                if (reader.Name == tagName) { Debug.WriteLine("Close :" + tagName); return; } // IF END TAG BASECASE

                if (reader.Name == "") { // IF VALUE
                    Debug.WriteLine("Value :" + reader.Value);
                    node.content = reader.Value;
                }

                else if (reader.Name != tagName) { // IF NEW TAG
                    Debug.WriteLine("Open  :" + reader.Name);
                    XMLTree child = new XMLTree(reader.Name);
                    node.AddChild(child);
                    RParser(reader, reader.Name, child);
                }
            }

        }
    }
}
