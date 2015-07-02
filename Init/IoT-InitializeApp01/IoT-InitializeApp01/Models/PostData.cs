using IoT_InitializeApp01.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace IoT_InitializeApp01.Models
{
    public class PostData
    {
        public string name { get; set; }
        public ColorItem color { get; set; }
        public string stickNum { get; set; }
        public string gameId { get; set; }
        public BitmapImage image { get; set; }
        public byte[] imageByte { get; set; }
        public Windows.Storage.Streams.IBuffer imageBuffer { get; set; }
    }
}
