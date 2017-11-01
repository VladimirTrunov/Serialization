using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLSerializationLibrary;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XMLSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            PC pc = new PC("Kevin", 25, 80, "GTA");
            Serialization<PC> instance = new Serialization<PC>(pc);

            string xml = instance.GetXMLFormat();
            FileStream f1 = new FileStream("xml2.xml", FileMode.Create, FileAccess.Write);
            StreamWriter wrt1 = new StreamWriter(f1);
            wrt1.Write(xml);
            wrt1.Close();
            f1.Close();

            PC newPc = new PC();
            instance.Deserialize(ref newPc, xml);
            //Console.WriteLine(instance.GetGeneralInformation());
            //string res = instance.SerializeToXML(pc);
            //Console.WriteLine(res);
            FileStream f = new FileStream("xml.xml", FileMode.Create, FileAccess.Write);
            StreamWriter wrt = new StreamWriter(f);
            //wrt.Write(res);
            wrt.Close();
            f.Close();

            XmlSerializer serializer = new XmlSerializer(typeof(PC));
            StreamWriter wrt2 = new StreamWriter("xmlTrue.xml");

            serializer.Serialize(wrt2, pc);
            wrt2.Close();


            //instance.Deserialize(ref pc, res);


            Console.ReadKey();
        }
    }
}
