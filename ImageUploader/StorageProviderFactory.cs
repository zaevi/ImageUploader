using System.Collections.Generic;
using ImageUploader.Provider;
using System.Xml.Linq;
using System.Linq;

namespace ImageUploader
{
    public static class StorageProviderFactory
    {
        public static Dictionary<string, XmlReader> ReaderMap = new Dictionary<string, XmlReader>();
        
        static StorageProviderFactory()
        {
            foreach(var reader in typeof(StorageProviderFactory).Assembly.GetExportedTypes()
                                    .Where(t => t.IsClass && typeof(IStorageProvider).IsAssignableFrom(t))
                                    .Select(t=>new XmlReader(t)))
                reader.TypeNames.ToList().ForEach(n => ReaderMap.Add(n, reader));
        }

        public static IStorageProvider InstanceFromXml(XElement element)
        {
            if (ReaderMap.TryGetValue(element.Name.LocalName, out var reader))
                return reader.Read(element) as IStorageProvider;
            return null;
        }

        public static bool TryInstanceFromXml(XElement element, out IStorageProvider provider)
        {
            provider = InstanceFromXml(element);
            return provider != null;
        }

        public static XElement GetDefaultXmlConfig()
        {
            return new XElement("ImageUploaderConfig", ReaderMap.Values.Distinct().Select(r => r.Default).ToArray());
        }
    }
}
