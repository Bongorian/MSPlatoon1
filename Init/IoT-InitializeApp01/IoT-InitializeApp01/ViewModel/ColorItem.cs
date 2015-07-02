using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_InitializeApp01.ViewModel
{
    public class ColorItem
    {
        public enum MyColors { Red, Orange, Yellow, Green, Blue, Purple }
        public ColorItem(MyColors color)
        {
            this.color = color.ToString();
        }
        public string color { get; set; }
    }
}
