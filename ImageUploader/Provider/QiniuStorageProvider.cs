using Qiniu.Storage;
using Qiniu.Util;
using System.IO;

namespace ImageUploader.Provider
{
    [Config("Qiniu")]
    public class QiniuStorageProvider : IStorageProvider
    {
        [Config]
        public string Name { get; set; }

        [Config]
        public string Domain { get; set; }

        [Config("SecretId")]
        public string AccessKey { get; set; }

        [Config]
        public string SecretKey { get; set; }

        [Config]
        public string Bucket { get; set; }

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
    }
}
