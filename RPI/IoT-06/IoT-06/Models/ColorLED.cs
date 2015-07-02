using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace IoT_06.Models
{
    public class ColorLED
    {
        List<GpioPin> ledPins { get; set; }
        public bool lockFlg { get; set; }

        public ColorLED(int[] pinNums)
        {
            lockFlg = false;
            ledPins = new List<GpioPin>();
            var controller = GpioController.GetDefault();
            foreach (var pinNum in pinNums)
            {
                ledPins.Add(controller.OpenPin(pinNum));
            }
            foreach (var pin in ledPins)
            {
                pin.SetDriveMode(GpioPinDriveMode.Output);
                pin.Write(GpioPinValue.Low);
            }
        }

        public void ChangeColor(MyColor color)
        {
            if (!lockFlg)
            {
                for (var i = 0; i < 3; i++)
                {
                    if (color.colorData[i] == true)
                    {
                        ledPins[i].Write(GpioPinValue.High);
                    }
                    else
                    {
                        ledPins[i].Write(GpioPinValue.Low);
                    }
                }
            }
        }
    }
}
