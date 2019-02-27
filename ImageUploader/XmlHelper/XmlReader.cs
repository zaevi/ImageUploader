using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

namespace ImageUploader
{
    public class XmlReader
    {

        public Type Type;

        public string[] TypeNames;

        public Dictionary<string, PropertyInfo> PropertyMap = new Dictionary<string, PropertyInfo>();

        public XElement Default;

        public XmlReader(Type type)
        {
            Type = type;
            TypeNames = new[] { type.Name }.Concat(type.GetCustomAttribute<ConfigAttribute>().Names).ToArray();
            Default = new XElement(type.Name);
            foreach(var property in type.GetProperties())
            {
                var list = property.GetCustomAttribute<ConfigAttribute>()?.Names.ToList();
                if (list == null) continue;
                Default.SetAttributeValue(property.Name, "");
                list.ForEach(n => PropertyMap.Add(n, property));
                PropertyMap.Add(property.Name, property);
            }
        }

        public object Read(XElement element)
        {
            if (!TypeNames.Contains(element.Name.LocalName))
                throw new Exception();
            var obj = Activator.CreateInstance(Type);
            foreach(var attr in element.Attributes())
                if (PropertyMap.TryGetValue(attr.Name.LocalName, out var property))
                    property.SetValue(obj, Convert.ChangeType(attr.Value, property.PropertyType));
            return obj;
        }
    }
}
