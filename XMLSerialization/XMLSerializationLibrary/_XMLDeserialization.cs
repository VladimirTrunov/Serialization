using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSerializationLibrary
{
    public enum TagContent
    {
        Content,
        Tree,
        Undefined
    }
    public class _XMLDeserialization
    {
        private _XMLTree mainTree;
        public string XML_String;
        public _XMLDeserialization()
        {
            mainTree = new _XMLTree();
        }
        public _XMLDeserialization(string XML)
        {
            XML_String = XML.Replace("\n", "");
            int stringLength;
            do
            {
                stringLength = XML_String.Length;
                XML_String = XML_String.Replace(" <", "<");
                XML_String = XML_String.Replace("> ", ">");
            } while (stringLength != XML_String.Length);

            mainTree = new _XMLTree();
            XML_String = XML_String.Trim();
        }

        public string GetMainTag(string XML_str)
        {
            string result = "";
            if (XML_str[0] == '<')
            {
                for(int i = 1; i < XML_str.Length; i++)
                {
                    if(XML_str[i] == '>' || XML_str[i] == ' ')
                    {
                        break;
                    }
                    result += XML_str[i];
                }
            }
            return result;
        }

        public TagContent DefineTagContent(string XML)
        {
            if(XML[0] == '<')
            {
                int i = 1;
                do
                {
                    i++;
                } while (XML[i] != '>');

                if (XML[i + 1] == '<')
                    return TagContent.Tree;
                else
                    return TagContent.Content;
            }
            else
            {
                return TagContent.Undefined;
            }
        }

        public string GetContent(string XML, out string remains)
        {
            string result = "";
            if(XML[0] == '<')
            {
                string openName = "";
                string closingName = "";
                int i = 1;
                do
                {
                    openName += XML[i];
                    i++;
                } while (XML[i] != ' ' && XML[i] != '>' && i < XML.Length);

                if (XML[i] == ' ')
                {
                    do
                    {
                        i++;
                    } while (XML[i] != '>' && i < XML.Length);
                }

                i++;
                do
                {
                    result += XML[i];
                    i++;
                } while (XML[i] != '<' && i < XML.Length);

                if (XML[i + 1] == '/')
                {
                    i+=2;
                    do
                    {
                        closingName += XML[i];
                        i++;
                    } while (XML[i] != '>' && i < XML.Length);

                    if (closingName == openName)
                    {
                        remains = XML.Remove(0, i + 1);
                        return result;
                    }
                    else
                        throw new Exception("Opennig and closing tags aren't equal (<" + openName + "> != <" + closingName + ">)");
                }
                else
                    throw new Exception("Couldn't find closing tag for <" + openName + "> tag!");
                
                
            }
            else
            {
                throw new Exception("XML[0] doesn't contain '<' character!");
            }
        }

        public bool isClosingTagNext(string openningTag, string XML)
        {
            if (XML[0] == '<' && XML[1] == '/')
            {
                string closingTag = "";
                int i = 2;
                do
                {
                    closingTag += XML[i];
                    i++;
                } while (XML[i] != '>' && i < XML.Length);
                if (closingTag == openningTag)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public string GetCloseTag(string openningTag, string XML)
        {
            string closingTag = "";
            if (XML[0] == '<' && XML[1] == '/')
            {
                int idx = 2;
                do
                {
                    closingTag += XML[idx];
                    idx++;
                } while (XML[idx] != '>' && idx != XML.Length);

                if (openningTag == closingTag)
                    return closingTag;
                else
                    throw new Exception("Closing tag is not equal to openning one (" + openningTag + "!=" + closingTag + ")");
            }
            else
                throw new Exception("Couldn't find closing tag");
        }
        private void FillXmlTree(_XMLTree tree, string XML, int startChar, ref int lastChar)
        {
            tree.tagOpen = GetMainTag(XML);
            TagContent content = DefineTagContent(XML);
            string remains;
            if (content == TagContent.Content)
            {
                tree.tagContent = GetContent(XML, out remains);
            }
            else if (content == TagContent.Tree)
            {
                int idx = startChar;

                string NewXml = XML.Remove(0, tree.tagOpen.Length + 2);
                idx += tree.tagOpen.Length + 2;
                do
                {
                    int lastIdx = 0;
                    tree.subTrees.Add(new _XMLTree());
                    FillXmlTree(tree.subTrees[tree.subTrees.Count - 1], NewXml, idx, ref lastIdx);
                    NewXml = NewXml.Remove(0, lastIdx - idx);
                    idx += lastIdx;
                } while (!isClosingTagNext(tree.tagOpen, NewXml));
            }
        }
    }
}
