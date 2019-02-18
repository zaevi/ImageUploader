using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace ImageUploader
{
    public partial class MainWindow : Window
    {

        public UploadRequest CurrentRequest
        {
            get => _currentRequest;
            set
            {
                value.Provider = value.Provider ?? (comboBoxProviders.SelectedValue as XElement);
                _currentRequest = value;
                DataContext = value;
                imageMsg.Visibility = Visibility.Collapsed;
                imageErrorMsg.Visibility = (value != null && !value.IsImage) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public UploadResult CurrentResult
        {
            get => _currentResult;
            set
            {
                _currentResult = value;
                resultPanel.DataContext = value;
                msgLabel.Text = value.Succeed ? "上传成功" : "上传失败";
            }
        }

        public XElement ProviderConfig = null;
        private UploadRequest _currentRequest;
        private UploadResult _currentResult;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ProviderConfig = XElement.Load("config.xml");
                ProviderConfig.Changed += (s, ce) => comboBoxProviders.ItemsSource = ProviderConfig.Elements();
                comboBoxProviders.ItemsSource = ProviderConfig.Elements();
            }
            catch(System.IO.FileNotFoundException)
            {
                imageMsg.Text = "未找到配置文件config.xml\n现在有了, 去配置一下吧";
                StorageProviderFactory.GetDefaultXmlConfig().Save("config.xml");
            }
            catch(System.Xml.XmlException)
            {
                imageMsg.Text = "配置文件格式有误(config.xml)";
            }
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            var request = CurrentRequest;

            if (request == null) return;
            if (request.Provider == null)
            {
                msgLabel.Text = "未选择配置";
                return;
            }

            request.Stream.Seek(0, System.IO.SeekOrigin.Begin);
            var provider = StorageProviderFactory.InstanceFromXml(request.Provider);

            var result = new UploadResult() { Request = request };

            try
            {
                result.Succeed = provider.Upload(request.Key, request.Stream);
                if (result.Succeed)
                {
                    result.Url = provider.GetUrl(request.Key);
                    result.MarkdownFormat = mdCheckBox.IsChecked == true;
                }
                else
                {
                    result.ErrorException = new Exception("上传失败");
                }
            }
            catch (Exception ex)
            {
                result.Succeed = false;
                result.ErrorException = ex;
            }
            CurrentResult = result;
        }

        private void Image_PreviewDragOver(object sender, DragEventArgs e)
        {
            var check = false;
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                check = true;
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files.Length == 1 && System.IO.File.Exists(files[0]))
                    check = true;
            }
            e.Effects = check ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void Image_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                CurrentRequest = UploadRequest.CreateFromFile(files[0]);
            }
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UploadRequest request = null;
            if (Clipboard.ContainsImage())
            {
                var files = Clipboard.GetFileDropList();
                if (files != null && files.Count == 1)
                {
                    request = UploadRequest.CreateFromFile(files[0]);
                    if (request != null)
                    {
                        CurrentRequest = request;
                        return;
                    }
                }
            }
        }

        private void Image_MouseLeftClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var dialog = new Microsoft.Win32.OpenFileDialog() { CheckFileExists = true };
                var result = dialog.ShowDialog();
                if (result == true)
                {
                    var request = UploadRequest.CreateFromFile(dialog.FileName);
                    if (request != null)
                        CurrentRequest = request;
                }
                e.Handled = true;
            }

        }

        private void About_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentResult == null || !CurrentResult.Succeed) return;
            try
            {
                Clipboard.SetText(resultTextBox.Text);
                msgLabel.Text = "已复制";
            }
            catch(Exception)
            {

            }
        }
    }
}
