using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_06.Models
{
    public class TempDataStorage
    {
        public List<Stick> sticks { get; set; }
        public TempDataStorage()
        {
            sticks = new List<Stick>();
        }
    }

    public class Stick
    {
        public string num { get; set; }
        public string color { get; set; }
        public Stick(string num, string color)
        {
            this.num = num;
            this.color = color;
        }
    }
}
