using System.IO;

namespace ImageUploader.Provider
{
    public interface IStorageProvider
    {
        string Name { get; set; }

        bool Upload(string key, Stream stream);

        string GetUrl(string key);

        bool Test();
    }
}
