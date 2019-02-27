using Aliyun.OSS;
using Aliyun.OSS.Util;
using System;
using System.IO;

namespace ImageUploader.Provider
{
    [Config("Aliyun", "Oss")]
    public class AliyunStorageProvider : IStorageProvider
    {
        private OssClient _client;

        [Config]
        public string Name { get; set; }

        [Config]
        public string EndPoint { get; set; }

        [Config("SecretId")]
        public string AccessKeyId { get; set; }

        [Config("SecretKey")]
        public string AccessKeySecret { get; set; }

        [Config("Bucket")]
        public string BucketName { get; set; }

        public OssClient Client
        {
            get
            {
                _client = _client ?? new OssClient(EndPoint, AccessKeyId, AccessKeySecret);
                return _client;
            }
        }

        public string GetUrl(string key)
            => $"https://{BucketName}.{EndPoint}/{key}";

        public bool Test()
            => Client.DoesBucketExist(BucketName);

        public bool Upload(string key, Stream stream)
        {
            // To be fixed: https://github.com/aliyun/aliyun-oss-csharp-sdk/issues/77
            var metadata = new ObjectMetadata() { ContentType = HttpUtils.GetContentType(key.ToLower(), null) };
            // To be fixed: https://github.com/aliyun/aliyun-oss-csharp-sdk/issues/79
            if (key.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                metadata.ContentType = "image/gif";

            var result = Client.PutObject(BucketName, key, stream, metadata);
            return result.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
