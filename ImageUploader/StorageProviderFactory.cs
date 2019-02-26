using ImageUploader.Provider;
using System.Xml.Linq;

namespace ImageUploader
{
    public static class StorageProviderFactory
    {
        public static IStorageProvider InstanceFromXml(XElement element)
        {
            IStorageProvider provider = null;
            switch(element.Name.LocalName)
            {
                case "Oss":
                case nameof(AliyunStorageProvider):
                    provider = AliyunStorageProvider.FromXml(element);
                    break;
                case "Qiniu":
                case nameof(QiniuStorageProvider):
                    provider = QiniuStorageProvider.FromXml(element);
                    break;
            }
            return provider;
        }

        public static bool TryInstanceFromXml(XElement element, out IStorageProvider provider)
        {
            provider = InstanceFromXml(element);
            return provider != null;
        }

        public static XElement GetDefaultXmlConfig()
        {
            return new XElement("ImageUploaderConfig",
                    new AliyunStorageProvider().ToXml(),
                    new QiniuStorageProvider().ToXml()
                );
        }
    }
}
