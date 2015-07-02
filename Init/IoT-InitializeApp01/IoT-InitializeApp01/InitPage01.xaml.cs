using IoT_InitializeApp01.Models;
using IoT_InitializeApp01.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IoT_InitializeApp01
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitPage01 : Page
    {
        private ObservableCollection<ColorItem> colorItems;
        public InitPage01()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init();

            base.OnNavigatedTo(e);
        }

        private void Init()
        {
            colorItems = new ObservableCollection<ColorItem>();
            colorItems.Add(new ColorItem(ColorItem.MyColors.Red));
            colorItems.Add(new ColorItem(ColorItem.MyColors.Orange));
            colorItems.Add(new ColorItem(ColorItem.MyColors.Yellow));
            colorItems.Add(new ColorItem(ColorItem.MyColors.Green));
            colorItems.Add(new ColorItem(ColorItem.MyColors.Blue));
            colorItems.Add(new ColorItem(ColorItem.MyColors.Purple));
            colorGrid.DataContext = colorItems;
        }

        private async void NextButtonClicked(object sender, TappedRoutedEventArgs e)
        {
            var color = (ColorItem)colorGrid.SelectedItem;
            var name = nameBox.Text;
            var stick = stickNumBox.Text;
            var gid = gameNumBox.Text;
            if (color == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(stick) || string.IsNullOrEmpty(gid))
            {
                var message = new MessageDialog("何か忘れてませんか？", "おや？");
                await message.ShowAsync();
            }
            else
            {
                var data = new PostData();
                data.color = color;
                data.name = name;
                data.stickNum = stick;
                data.gameId = gid;
                this.Frame.Navigate(typeof(InitPage02), data);
            }
        }
    }
}
