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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoT_InitializeApp01
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Storyboard> storyboards;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init();
            RunAnimation();
            base.OnNavigatedTo(e);
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
        }

        private async Task RunAnimation()
        {
            while (true)
            {
                foreach (var board in storyboards)
                {
                    board.Begin();
                    await Task.Delay(5000);
                }
            }
        }

        private void StartButtonClicked(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InitPage01));
        }
    }
}
