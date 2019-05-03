using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics_Simulator.ViewModel {

    class XMLTree {

        public XMLTree (string tagName) {
            this.tagName = tagName;
        }

        public XMLTree(string tagName, string content) {
            this.tagName = tagName;
            this.content = content;
        }

        public XMLTree(List<XMLTree> children, string tagName, string content) {
            this.children = children;
            this.tagName = tagName;
            this.content = content;
        }

        public void AddChild(XMLTree child) {
            this.children.Add(child);
        }

        public void AddChild(string tagName) {
            this.children.Add(new XMLTree(tagName));
        }
        
        public void AddChild(string tagName, string content) {
            this.children.Add(new XMLTree(tagName, content));
        }

        public XMLTree Duplicate() {
            return new XMLTree(this.children, this.tagName, this.content);
        }

        public List<XMLTree> children = new List<XMLTree>();
        public string tagName;
        public string content;
    }
}
