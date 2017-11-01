using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSerializationLibrary
{
    class _ItemTree
    {
        public string SelfName { get; set; }
        public string ParentName { get; set; }
        public List<_Item> Fields { get; set; }
        public List<_Item> Properties { get; set; }
        public List<_Collection> Collections { get; set; }

        public List<_ItemTree> subTrees { get; set; }

        public _ItemTree(): this("") { }
        public _ItemTree(string parentName)
        {
            ParentName = parentName;
            Fields = new List<_Item>();
            Properties = new List<_Item>();
            Collections = new List<_Collection>();
            subTrees = new List<_ItemTree>();
        }
    }
}
