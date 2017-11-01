using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using System.Collections;

namespace XMLSerializationLibrary
{
    public class Serialization<T>
    {
        Type classType;
        _ItemTree Parent;

        public Serialization(T obj) {
            classType = typeof(T);
            Parent = new _ItemTree();
            Serialiaze(classType, obj, Parent);

            //Deserialize(obj, )
        }
        public string GetGeneralInformation()
        {
            string result = "";
            foreach(MemberInfo info in classType.GetMembers())
            {
                result += (info.DeclaringType + " " + info.MemberType + " " + info.Name);
                result += "\n";
            }
            return result;
        }

       /* private void Deserialize(ref T obj, string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("PC");

            int a = Convert.ToInt32(node.SelectSingleNode("a").InnerText);
            a++;
            Type type = typeof(T);
            FieldInfo[] fInfo = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            foreach(var field in fInfo)
            {
                if(field.Name == "a")
                {
                    field.SetValue(obj, a);
                }
            }

            XmlTextWriter writer = new XmlTextWriter(Console.Out);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartElement("Stcok");
        }*/

        public string GetXMLFormat()
        {
            string result;

            result = new _XMLSerialization(classType.Name.ToString(), Parent).XmlString;

            return result;
        }


        #region Main methods for serialization
        /// <summary>
        /// Gets collections from a specified object
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="searchObj">The main object to search in</param>
        /// <param name="excludeMembers">Members shouldn't be described (were described before)</param>
        /// <returns></returns>
        private List<_Collection> GetCollections(Type type, object searchObj, _ExcludeMembers excludeMembers)
        {
            List<_Collection> result = new List<_Collection>();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            foreach(FieldInfo field in fieldInfos)
            {
                if (field.Name.IndexOf("k__BackingField") == -1 && !excludeMembers.Contains(field.Name.ToString()))
                {
                    object obj = field.GetValue(searchObj);
                    if (obj is IEnumerable && !(obj is string))
                    {
                        string CollectionName = field.Name;
                        List<string> collectionItems = new List<string>();

                        foreach (var item in obj as IEnumerable)
                        {
                            collectionItems.Add(Convert.ToString(item));
                        }
                        result.Add(new _Collection(CollectionName, collectionItems));
                        excludeMembers.Add(CollectionName);
                    }
                }
            }

            foreach (PropertyInfo prop in propertyInfos)
            {
                if (prop.Name.IndexOf("k__BackingField") == -1 && !excludeMembers.Contains(prop.Name.ToString()))
                {
                    object obj = prop.GetValue(searchObj);
                    if (obj is IEnumerable && !(obj is string))
                    {
                        string CollectionName = prop.Name;
                        List<string> collectionItems = new List<string>();

                        foreach (var item in obj as IEnumerable)
                        {
                            collectionItems.Add((string)item);
                        }
                        result.Add(new _Collection(CollectionName, collectionItems));
                        excludeMembers.Add(CollectionName);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Gets fields from a specified object
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="searchObj">The main object to search in</param>
        /// <param name="excludeMembers">Members shouldn't be described (were described before)</param>
        /// <returns></returns>
        private List<_Item> GetFields(Type type, object searchObj, _ExcludeMembers excludeMembers)
        {
            List<_Item> result = new List<_Item>();

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            foreach (FieldInfo field in fieldInfos)
            {
                if (field.Name.IndexOf("k__BackingField") == -1 && !excludeMembers.Contains(field.Name.ToString()))
                {
                    if (!(searchObj is IEnumerable && !(searchObj is string)))
                    {
                        string FieldName = field.Name;
                        object FieldValue = field.GetValue(searchObj).ToString();
                        result.Add(new _Item(FieldName, FieldValue.ToString()));
                        excludeMembers.Add(FieldName);
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Gets properties from a specified object
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="searchObj">The main object to search in</param>
        /// <param name="excludeMembers">Members shouldn't be described (were described before)</param>
        /// <returns></returns>
        private List<_Item> GetProperties(Type type, object searchObj, _ExcludeMembers excludeMembers)
        {
            List<_Item> result = new List<_Item>();

            PropertyInfo[] propInfos = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            foreach (PropertyInfo prop in propInfos)
            {
                if (prop.Name.IndexOf("k__BackingField") == -1 && !excludeMembers.Contains(prop.Name.ToString()))
                {
                    if (!(searchObj is IEnumerable && !(searchObj is string)))
                    {
                        string FieldName = prop.Name;
                        string FieldValue = Convert.ToString(prop.GetValue(searchObj));
                        result.Add(new _Item(FieldName, FieldValue));
                        excludeMembers.Add(FieldName);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Serialization method fills the tree
        /// </summary>
        /// <param name="type">Type of object to serialize</param>
        /// <param name="obj">The main object to serialize</param>
        /// <param name="itemTree">Tree to fill</param>
        private void Serialiaze(Type type, object obj, _ItemTree itemTree)
        {
            if (itemTree.ParentName == "")
                itemTree.ParentName = "Master";
            itemTree.SelfName = type.Name.ToString();
            _ExcludeMembers excludeMembers = new _ExcludeMembers();
            itemTree.Collections = GetCollections(type, obj, excludeMembers);
            itemTree.Fields = GetFields(type, obj, excludeMembers);
            itemTree.Properties = GetProperties(type, obj, excludeMembers);

            Type subType;
            string assemblyName;

            foreach (_Item item in itemTree.Fields)
            {
                int dotIdx = item.itemValue.LastIndexOf(".");
                if (dotIdx >= 0)
                {
                    assemblyName = item.itemValue.Substring(0, item.itemValue.LastIndexOf("."));
                    subType = Type.GetType(item.itemValue + ", " + assemblyName);
                    if (subType != null)
                    {
                        itemTree.subTrees.Add(new _ItemTree(item.itemName));
                        FieldInfo field = GetFieldInfo(item.itemName, type);
                        Serialiaze(subType, field.GetValue(obj), itemTree.subTrees[itemTree.subTrees.Count - 1]);
                    }
                }
            }

            foreach (_Item item in itemTree.Properties)
            {
                int dotIdx = item.itemValue.LastIndexOf(".");
                if (dotIdx >= 0)
                {
                    assemblyName = item.itemValue.Substring(0, item.itemValue.LastIndexOf("."));
                    subType = Type.GetType(item.itemValue + ", " + assemblyName);
                    if (subType != null)
                    {
                        itemTree.subTrees.Add(new _ItemTree(item.itemName));
                        PropertyInfo prop = GetPropertyInfo(item.itemName, type);
                        Serialiaze(subType, prop.GetValue(obj), itemTree.subTrees[itemTree.subTrees.Count - 1]);
                    }
                }
            }

            foreach(_Collection collection in itemTree.Collections)
            {
                int i = 0;
                foreach(string item in collection.Items)
                {
                    int dotIdx = item.LastIndexOf(".");
                    if(dotIdx >= 0)
                    {
                        assemblyName = item.Substring(0, item.LastIndexOf("."));
                        subType = Type.GetType(item + ", " + assemblyName);
                        if (subType != null)
                        {                            
                            itemTree.subTrees.Add(new _ItemTree(collection.collectionName));
                            object itemObject = GetObjectFromCollection(type, obj, collection.collectionName, i);
                            Serialiaze(subType, itemObject, itemTree.subTrees[itemTree.subTrees.Count - 1]);
                        }
                    }
                    i++;
                }
            }
        }
        /// <summary>
        /// Gets field info by its name
        /// </summary>
        /// <param name="FieldName">Name of field</param>
        /// <param name="typeFrom">Type to search field in</param>
        /// <returns></returns>
        private FieldInfo GetFieldInfo(string FieldName, Type typeFrom)
        {
            FieldInfo F_info = null;
            FieldInfo[] fieldInfos = typeFrom.GetFields(BindingFlags.NonPublic | BindingFlags.Public 
                | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fieldInfos)
            {
                if (field.Name.ToString() == FieldName)
                {
                    F_info = field;
                    break;
                }
            }
            return F_info;
        }
        /// <summary>
        /// Gets property info by its name
        /// </summary>
        /// <param name="PropertyName">Name of property</param>
        /// <param name="typeFrom">Type to search property in</param>
        /// <returns></returns>
        private PropertyInfo GetPropertyInfo(string PropertyName, Type typeFrom)
        {
            PropertyInfo P_info = null;
            PropertyInfo[] PropertyInfos = typeFrom.GetProperties(BindingFlags.NonPublic | BindingFlags.Public
                | BindingFlags.Static | BindingFlags.Instance);
            foreach (var prop in PropertyInfos)
            {
                if (prop.Name.ToString() == PropertyName)
                {
                    P_info = prop;
                    break;
                }
            }
            return P_info;
        }
        /// <summary>
        /// Gets object from specified collection under specified index
        /// </summary>
        /// <param name="searchType">Type to search collection item in</param>
        /// <param name="searchObject"> Object to search collection item in</param>
        /// <param name="collectionName">Name of collection</param>
        /// <param name="idx">idx of object in collection</param>
        /// <returns></returns>
        private object GetObjectFromCollection(Type searchType, object searchObject, string collectionName, int idx)
        {
            object result = null;

            PropertyInfo[] listPropertyInfo = searchType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public
                | BindingFlags.Static | BindingFlags.Instance);
            FieldInfo[] listFieldInfo = searchType.GetFields(BindingFlags.NonPublic | BindingFlags.Public
                | BindingFlags.Static | BindingFlags.Instance);

            PropertyInfo collectionProperty = null;
            foreach (PropertyInfo prop in listPropertyInfo)
            {
                if(prop.Name == collectionName)
                {
                    collectionProperty = prop;
                    break;
                }
            }

            if(collectionProperty != null)
            {
                object collectObject = collectionProperty.GetValue(searchObject);
                int i = 0;
                foreach(var item in collectObject as IEnumerable)
                {
                    if(i == idx)
                    {
                        result = item;
                        break;
                    }
                    i++;
                }
            }
            else
            {
                FieldInfo collectionField = null;
                foreach(FieldInfo field in listFieldInfo)
                {
                    if(field.Name == collectionName)
                    {
                        collectionField = field;
                        break;
                    }
                }

                if (collectionField != null)
                {
                    object collectObject = collectionField.GetValue(searchObject);
                    int i = 0;
                    foreach (var item in collectObject as IEnumerable)
                    {
                        if (i == idx)
                        {
                            result = item;
                            break;
                        }
                        i++;
                    }
                }
            }
            return result;
        }
        #endregion

        #region Main methods for deserialization
        public void Deserialize(ref T obj, string xml)
        {
            _XMLDeserialization des = new _XMLDeserialization(xml);

        }
        #endregion
    }
}
