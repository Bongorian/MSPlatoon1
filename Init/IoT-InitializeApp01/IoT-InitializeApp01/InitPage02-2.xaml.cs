using IoT_InitializeApp01.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IoT_InitializeApp01
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitPage02_2 : Page
    {
        private PostData data;
        public InitPage02_2()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            data = (PostData)e.Parameter;
            capturedPhotoPreview.Source = data.image;
            base.OnNavigatedTo(e);
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InitPage02), data);
        }

        private void NextButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InitPage03), data);
        }
    }
}
