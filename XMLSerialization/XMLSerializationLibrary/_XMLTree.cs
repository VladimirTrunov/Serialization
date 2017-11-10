using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSerializationLibrary
{
    public class _XMLTree
    {
        public string tagOpen { get; set; }
        public string tagClose { get; set; }
        public string tagContent { get; set; }
        public List<_XMLTree> subTrees { get; set; }

        public _XMLTree()
        {
            subTrees = new List<_XMLTree>();
        }
    }
}
