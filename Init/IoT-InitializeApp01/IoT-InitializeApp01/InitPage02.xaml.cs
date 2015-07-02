using IoT_InitializeApp01.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IoT_InitializeApp01
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitPage02 : Page
    {
        Windows.Media.Capture.MediaCapture _capture;
        private PostData data;
        public InitPage02()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetCapturePreview();
            data = (PostData)e.Parameter;
            base.OnNavigatedTo(e);
        }

        private async Task GetCapturePreview()
        {
            _capture = new MediaCapture();
            var Videodevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            //var rearCamera = Videodevices.FirstOrDefault(item => item.EnclosureLocation != null && item.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);
            var frontCamera = Videodevices.FirstOrDefault(item => item.EnclosureLocation != null && item.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
            //TODO: カメラを切り替えられるようにする。



            try
            {
                await _capture.InitializeAsync(new MediaCaptureInitializationSettings
                {
                    VideoDeviceId = frontCamera.Id
                });
            }
            catch (Exception ex)
            {
                var message = new MessageDialog(ex.Message, "おや？なにかがおかしいようです。");
                await message.ShowAsync();
            }

            capturePreview.Source = _capture;

            await _capture.StartPreviewAsync();
        }

        private async void PhotoButtonClicked(object sender, RoutedEventArgs e)
        {
            if (capturePreview.Source == null)
                return;

            // プレビューに付けた MediaCapture オブジェクトを取り出す
            MediaCapture capture = capturePreview.Source;

            // 保存する画像ファイルのフォーマット（.pngファイルを指定）
            var imageProperties
              = Windows.Media.MediaProperties.ImageEncodingProperties.CreatePng();

            // 撮影する。一度ファイルに保存する
            StorageFile file
              = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("test.png", CreationCollisionOption.GenerateUniqueName);
            try
            {
                await capture.CapturePhotoToStorageFileAsync(imageProperties, file);
                // キャプチャ開始から撮影までの間に許可を取り消されると、ここで例外が出る
            }
            catch (Exception ex)
            {
                return;
            }

            using (var stream = await file.OpenReadAsync())
            {
                var bi = new BitmapImage();
                bi.SetSource(stream);
                ////photoPreview.Source = bi;
                data.image = bi;
                var realStream = stream.AsStreamForRead();
                var byteArray = ReadToEnd(realStream);
                data.imageByte = byteArray;
                data.imageBuffer = byteArray.AsBuffer();
            }
            this.Frame.Navigate(typeof(InitPage02_2), data);
            //photoPreviewFlyout.ShowAt(sender as FrameworkElement);
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
