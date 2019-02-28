using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Model.Bucket;

namespace ImageUploader.Provider
{
    [Config("QCloud", "Cos")]
    public class QCloudStorageProvider : IStorageProvider
    {
        [Config]
        public string Name { get; set; }

        [Config]
        public string Domain { get; set; }

        [Config]
        public string AppId
        {
            get
            {
                if (_appId == null)
                    _appId = Bucket.Split('-')[1];
                return _appId;
            }
            set => _appId = value;
        }

        [Config]
        public string Region
        {
            get
            {
                if (_region == null)
                    _region = Domain.Split('.')[2];
                return _region;
            }
            set => _region = value;
        }

        [Config]
        public string SecretId { get; set; }

        [Config]
        public string SecretKey { get; set; }

        [Config]
        public string Bucket { get; set; }

        private CosXmlServer _client;
        private string _region;
        private string _appId;

        public CosXmlServer Client
        {
            get
            {
                if (_client == null)
                {
                    var auth = new DefaultQCloudCredentialProvider(SecretId, SecretKey, 600);
                    var config = new CosXmlConfig.Builder().SetRegion(Region).SetAppid(AppId).Build();
                    _client = new CosXmlServer(config, auth);
                }
                return _client;
            }
        }

        public string GetUrl(string key)
            => $"https://{Domain}/{key}";

        public bool Test()
        {
            var result = Client.GetBucket(new GetBucketRequest(Bucket));
            return result.httpCode == 200;
        }

        public bool Upload(string key, Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            var request = new PutObjectRequest(Bucket, key, bytes);
            var result = Client.PutObject(request);
            return result.httpCode == 200;
        }
    }
}
