using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSerializationLibrary
{
    public class _Collection
    {
        public string collectionName { get; set; }
        public List<string> Items { get; set; }
        public _Collection() { }
        public _Collection(string _collectionName, List<string> _Items)
        {
            collectionName = _collectionName;
            Items = _Items;
        }
    }
}
