using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_06.Models
{
    public class MyColor
    {
        public MyColor(string colorName, bool[] bools)
        {
            this.colorName = colorName;
            this.colorData = bools;
        }
        public string colorName { get; set; }
        public bool[] colorData { get; set; }
    }

    public class MyColors
    {
        public MyColor Red;
        public MyColor Orange;
        public MyColor Yellow;
        public MyColor Green;
        public MyColor Blue;
        public MyColor Purple;

        public MyColors()
        {
            Red = new MyColor("Red", new bool[3] { true, false, false });
            Orange = new MyColor("Orange", new bool[3] { true, true, false });
            Yellow = new MyColor("Yellow", new bool[3] { false, true, true });
            Green = new MyColor("Green", new bool[3] { false, true, false });
            Blue = new MyColor("Blue", new bool[3] { false, false, true });
            Purple = new MyColor("Purple", new bool[3] { true, false, true });
        }
    }
}
