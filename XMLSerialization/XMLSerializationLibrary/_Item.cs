using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSerializationLibrary
{
    class _Item
    {
        public string itemName { get; set; }
        public string itemValue { get; set; } 

        public List<string> attributes { get; set; }

        public _Item() {
            itemValue = null;
            itemValue = null;

            attributes = new List<string>();
        }

        public _Item(string itemName, string itemValue)
        {
            this.itemName = itemName;
            this.itemValue = itemValue;

            attributes = new List<string>();
        }

        public _Item(string itemName, string itemValue, List<string> attributes)
        {
            this.itemName = itemName;
            this.itemValue = itemValue;
            this.attributes = attributes;
        }
    }
}
