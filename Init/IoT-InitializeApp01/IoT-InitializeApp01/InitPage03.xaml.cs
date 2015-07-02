using IoT_InitializeApp01.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using IoT_InitializeApp01.ViewModel;
using Windows.Web.Http;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IoT_InitializeApp01
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitPage03 : Page
    {
        private List<Storyboard> storyboards;
        private int dockState;
        private PostData data;
        private bool buttonLockFlag;
        public InitPage03()
        {
            this.InitializeComponent();
        }

        public void Init()
        {
            storyboards = new List<Storyboard>();
            storyboards.Add(buttonStoryBoard01);
            storyboards.Add(buttonStoryBoard02);
            storyboards.Add(buttonStoryBoard03);
            storyboards.Add(buttonStoryBoard04);
            storyboards.Add(buttonStoryBoard05);
            storyboards.Add(buttonStoryBoard06);
            storyboards.Add(buttonStoryBoard07);
            dockState = 0;
            buttonLockFlag = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init();
            RunAnimation();
            data = (PostData)e.Parameter;
            base.OnNavigatedTo(e);
        }

        private async Task RunAnimation()
        {
            NextButton.Content = "準備はいいですか?\nタップしてください。";
            while (dockState == 0)
            {
                foreach (var board in storyboards)
                {
                    if (dockState == 0)
                    {
                        board.Begin();
                        await Task.Delay(3000);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private async void NextButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!buttonLockFlag)
            {
                ++dockState;
                buttonLockFlag = true;
                switch (dockState)
                {
                    case 1:
                        switch (data.color.color)
                        {
                            case "Red":
                                NextButton.Content = "赤チームとして登録します...";
                                storyboards[4].Begin();
                                await Task.Delay(2000);
                                break;
                            case "Orange":
                                NextButton.Content = "橙チームとして登録します...";
                                storyboards[3].Begin();
                                await Task.Delay(2000);
                                break;
                            case "Yellow":
                                NextButton.Content = "黄チームとして登録します...";
                                storyboards[2].Begin();
                                await Task.Delay(2000);
                                break;
                            case "Green":
                                NextButton.Content = "緑チームとして登録します...";
                                storyboards[0].Begin();
                                await Task.Delay(2000);
                                break;
                            case "Blue":
                                NextButton.Content = "青チームとして登録します...";
                                storyboards[6].Begin();
                                await Task.Delay(2000);
                                break;
                            case "Purple":
                                NextButton.Content = "紫チームとして登録します...";
                                storyboards[5].Begin();
                                await Task.Delay(2000);
                                break;
                        }

                        NextButton.Content = "データをサーバーに登録中です...\nもうすぐ楽しめます。";
                        await SendData();
                        NextButton.Content = "データは正常に登録されました！";
                        await Task.Delay(1000);
                        buttonLockFlag = false;
                        break;
                    case 2:
                        this.Frame.Navigate(typeof(MainPage));
                        break;
                    default:
                        break;
                }
            }
        }

        public async Task SendData()
        {
            var client = new HttpClient();
            var content = new HttpMultipartFormDataContent();
            var imageContent = new HttpBufferContent(data.imageBuffer);
            content.Add(imageContent, "userpic", "userpic.bmp");
            var name = new HttpStringContent(data.name);
            content.Add(name, "name");
            var color = new HttpStringContent(data.color.color.ToString());
            content.Add(color, "color");
            var stick = new HttpStringContent(data.stickNum);
            content.Add(stick, "stick");
            var gid = new HttpStringContent(data.gameId);
            content.Add(gid, "gid");
            try
            {
                var res = await client.PostAsync(new Uri("http://mspjp-iot-test.azurewebsites.net/insertUser"), content);
                if (!res.IsSuccessStatusCode)
                {
                    throw new Exception("Access failed." + res.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                var message = new MessageDialog(ex.Message, "ふむ。最初からやり直しですな。");
                await message.ShowAsync();
                this.Frame.Navigate(typeof(InitPage01));
            }
        }
    }
}

