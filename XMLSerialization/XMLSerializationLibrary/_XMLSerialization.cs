using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLSerializationLibrary
{
    class _XMLSerialization
    {
        _ItemTree Parent;
        List<string> SubtreeNames;

        private string XMLHeader = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>\n";
        private string mainLinks = " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n";
        private string Header;

        private string _startElement;
        private string _endMainElement;

        private string _xmlString;

        private string _currentOpenedElement;

        public string StartElement
        {
            get
            {
                return _startElement;
            }
        }

        public string EndMainElement
        {
            get
            {
                return _endMainElement;
            }
        }

        public string XmlString
        {
            get
            {
                return _xmlString;
            }
        }

        public string CurrentOpenedElement
        {
            get
            {
                return _currentOpenedElement;
            }
        }

        public _XMLSerialization(string startElement, _ItemTree tree)
        {
            _xmlString = "";
            _startElement = startElement;
            _endMainElement = "</" + startElement + ">";

            Parent = tree;

            FillSubtreeList();
            Header = "<" + _startElement + mainLinks;

            AddHeader();
            ConvertTreeToXML(Parent, startElement, 0);
        }

        private void AddHeader()
        {
            _xmlString += XMLHeader + Header;
        }
        private void Refresh()
        {
            _xmlString = "";
        }

        private void WriteStartElement(string element, int depth)
        {
            for (int i = 0; i < depth; i++)
                _xmlString += ' ';
            _xmlString += "<" + element + ">\n";
        }
        private void WriteEndElement(string element, int depth)
        {
            for (int i = 0; i < depth; i++)
                _xmlString += ' ';
            _xmlString += "</" + element + ">\n";
        }
        private void WriteElementString(string elementName, string elementValue, int depth)
        {
            for (int i = 0; i < depth + 2; i++)
                _xmlString += ' ';
            _xmlString += "<" + elementName + ">" + elementValue + "</" + elementName + ">\n";
        }

        private void FillSubtreeList()
        {
            if (Parent != null)
            {
                SubtreeNames = new List<string>();
                foreach(_ItemTree subTree in Parent.subTrees)
                {
                    bool isAbsent = true;
                    foreach (string subTreeName in SubtreeNames)
                    {
                        if(subTree.ParentName == subTreeName)
                        {
                            isAbsent = false;
                            break;
                        }
                    }
                    if(isAbsent == true)
                        SubtreeNames.Add(subTree.ParentName);
                }
            }
            else
                throw new Exception("Parent tree is null");
        }

        private _ItemTree GetCollectionSubTree(string collectionName, int idx, _ItemTree Parent)
        {
            int i = 0;
            {
                foreach(_ItemTree tree in Parent.subTrees)
                {
                    if(tree.ParentName == collectionName)
                    {
                        if (i == idx)
                            return tree;
                        else
                            i++;
                    }
                }
            }
            return null;
        }

        private void ConvertTreeToXML(_ItemTree tree, string ParentName, int depth)
        {
            if(tree.ParentName != "Master" && ParentName != "")
                WriteStartElement(ParentName, depth);

            foreach(_Collection collection in tree.Collections)
            {
                if(!SubtreeNames.Contains(collection.collectionName))
                {
                    foreach (string collitem in collection.Items)
                    {
                        WriteElementString(collection.collectionName, collitem, depth);
                    }
                }
                else
                {
                    WriteStartElement(collection.collectionName, depth + 2);
                    depth += 2;
                    int idx = 0;
                    foreach (string collectionItem in collection.Items)
                    {
                        _ItemTree subtree = GetCollectionSubTree(collection.collectionName, idx, tree);
                        if (subtree != null)
                        {
                            ConvertTreeToXML(subtree, subtree.SelfName, depth);
                        }
                        else
                            throw new Exception("Subtree wasn't found for collection!");
                        idx++;
                    }
                    depth -= 2;
                    WriteEndElement(collection.collectionName, depth + 2);
                }
            }

            foreach(_Item field in tree.Fields)
            {
                if(!SubtreeNames.Contains(field.itemName))
                {
                    WriteElementString(field.itemName, field.itemValue, depth);
                }
                else
                {
                    depth += 2;
                    _ItemTree subtree = null;
                    foreach (_ItemTree tr in tree.subTrees)
                    {
                        if(tr.ParentName == field.itemName)
                        {
                            subtree = tr;
                            break;
                        }
                    }
                    if (subtree != null)
                        ConvertTreeToXML(subtree, field.itemName, depth);
                    depth -= 2;
                }
            }

            foreach (_Item property in tree.Properties)
            {
                if(!SubtreeNames.Contains(property.itemName))
                {
                    WriteElementString(property.itemName, property.itemValue, depth);
                }
                else
                {
                    depth += 2;
                    _ItemTree subtree = null;
                    foreach (_ItemTree tr in tree.subTrees)
                    {
                        if (tr.ParentName == property.itemName)
                        {
                            subtree = tr;
                            break;
                        }
                    }
                    if (subtree != null)
                        ConvertTreeToXML(subtree, property.itemName, depth);
                    depth -= 2;
                }
            }

            if(ParentName != "")
                WriteEndElement(ParentName, depth);
        }

    }
}
