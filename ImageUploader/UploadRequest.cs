using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ImageUploader
{
    public class UploadRequest : INotifyPropertyChanged
    {
        private string _key;

        public string SourceName { get; set; }

        public string Key { get => _key; set => _key = value; }

        public XElement Provider { get; set; }

        public MemoryStream Stream;

        public bool IsImage { get; set; }

        public BitmapImage Image { get; set; }

        public static UploadRequest CreateFromFile(string path)
        {
            if (!File.Exists(path))
                return null;
            var request = new UploadRequest()
            {
                SourceName = Path.GetFileName(path),
                Key = Path.GetFileName(path),
            };

            using (var stream = File.OpenRead(path))
            {
                request.Stream = new MemoryStream();
                stream.CopyTo(request.Stream);
                request.Stream.Position = 0;
            }

            request.TryLoadImage(path);
            return request;
        }

        private void TryLoadImage(string path)
        {
            try
            {
                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = Stream;
                imageSource.EndInit();
                IsImage = true;
                Image = imageSource;
            }
            catch (NotSupportedException)
            {
                IsImage = false;
                Image = null;
            }
        }

        #region [OnChanged]

        private void OnChanged(string Name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
