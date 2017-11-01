using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLSerializationLibrary;

namespace XMLDeserTesting
{
    [TestClass]
    public class UnitTest1
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
    }
}
