using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Qiniu.Storage;
using Qiniu.Util;

namespace ImageUploader.Provider
{
    [Config("Qiniu")]
    public class QiniuStorageProvider : IStorageProvider
    {
        [Config]
        public string Name { get; set; } = "Qiniu Storage";

        [Config]
        public string Domain { get; set; } = "YourDomain";

        [Config("SecretId")]
        public string AccessKey { get; set; } = "YourAccessKey";

        [Config]
        public string SecretKey { get; set; } = "YourSecretKey";

        [Config]
        public string Bucket { get; set; } = "YourBucket";

        public string Token => Auth.CreateUploadToken(
            new Mac(AccessKey, SecretKey),
            new PutPolicy() { Scope = Bucket }.ToJsonString()
            );

        public string GetUrl(string key)
            => $"http://{Domain}/{key}";

        public bool Test()
        {
            var result = new BucketManager(new Mac(AccessKey, SecretKey), new Config()).Buckets(true);
            return result.Result.Contains(Bucket);
        }

        public bool Upload(string key, Stream stream)
        {
            var config = new Config();
            var result = new FormUploader(config).UploadStream(stream, key, Token, null);

            return result.Code == 200;
        }

        #region [Xml Read & Write]

        public static QiniuStorageProvider FromXml(XElement element)
        {
            try
            {
                var provider = new QiniuStorageProvider()
                {
                    Name = element.Attribute(nameof(Name)).Value,
                    Domain = element.Attribute(nameof(Domain)).Value,
                    AccessKey = element.Attribute(nameof(AccessKey)).Value,
                    SecretKey = element.Attribute(nameof(SecretKey)).Value,
                    Bucket = element.Attribute(nameof(Bucket)).Value,
                };
                return provider;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public XElement ToXml()
            => new XElement(nameof(QiniuStorageProvider),
                new XAttribute(nameof(Name), Name),
                new XAttribute(nameof(Domain), Domain),
                new XAttribute(nameof(AccessKey), AccessKey),
                new XAttribute(nameof(SecretKey), SecretKey),
                new XAttribute(nameof(Bucket), Bucket)
                );

        #endregion
    }
}
