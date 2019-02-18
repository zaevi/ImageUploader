using System;
using System.ComponentModel;
using System.IO;

namespace ImageUploader
{
    public class UploadResult : INotifyPropertyChanged
    {
        #region [OnChanged]

        private void OnChanged(string Name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public UploadRequest Request;
        private bool _markdownFormat;

        public bool Succeed { get; set; }

        public string Url { get; set; }

        public bool MarkdownFormat { get => _markdownFormat; set { _markdownFormat = value; OnChanged(nameof(Message)); } }

        public Exception ErrorException { get; set; }

        public string Message
        {
            get
            {
                if (Succeed)
                {
                    if (!MarkdownFormat)
                        return Url;
                    else
                        return $"{(Request.IsImage ? "!" : "")}[{Path.GetFileName(Url)}]({Url})";
                }
                else
                {
                    return $"{ErrorException.GetType().Name}: {ErrorException.Message}";
                }
            }
        }
    }
}
