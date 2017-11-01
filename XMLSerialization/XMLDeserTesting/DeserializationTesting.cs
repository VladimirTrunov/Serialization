using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLSerializationLibrary;

namespace XMLDeserTesting
{
    [TestClass]
    public class DeserializationTesting
    {
        [TestMethod]
        public void TestGetMainTag()
        {
            string xml = "<a type = \"open\">123</a>";
            string expected = "a";
            _XMLDeserialization deserialization = new _XMLDeserialization(xml);
            string fact = deserialization.GetMainTag(deserialization.XML_String);

            Assert.AreEqual(expected, fact);
        }
        [TestMethod]
        public void TestGetMainTag2()
        {
            string xml = "<a>123</a>";
            string expected = "a";
            _XMLDeserialization deserialization = new _XMLDeserialization(xml);
            string fact = deserialization.GetMainTag(deserialization.XML_String);

            Assert.AreEqual(expected, fact);
        }
        [TestMethod]
        public void TestContentDefinition()
        {
            string xml = "<a>123</a>";
            TagContent content = TagContent.Content;
            _XMLDeserialization deserialization = new _XMLDeserialization(xml);
            TagContent factContent = deserialization.DefineTagContent(deserialization.XML_String);

            Assert.AreEqual(content, factContent);
        }
        [TestMethod]
        public void TestContentDefinition2()
        {
            string xml = "<a><b>123</b></a>";
            TagContent content = TagContent.Tree;
            _XMLDeserialization deserialization = new _XMLDeserialization(xml);
            TagContent factContent = deserialization.DefineTagContent(deserialization.XML_String);

            Assert.AreEqual(content, factContent);
        }
    }
}
