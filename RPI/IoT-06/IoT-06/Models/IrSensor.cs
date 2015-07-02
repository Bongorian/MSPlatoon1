using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace IoT_06.Models
{
    class IrSensor
    {
        List<GpioPin> irPins { get; set; }
        public IrSensor(int[] pinNums)
        {
            irPins = new List<GpioPin>();
            var controller = GpioController.GetDefault();
            foreach (var pinNum in pinNums)
            {
                irPins.Add(controller.OpenPin(pinNum));
            }
            foreach (var pin in irPins)
            {
                pin.SetDriveMode(GpioPinDriveMode.Input);
            }
        }

        public int CheckNum()
        {
            var inputs = new GpioPinValue[4];
            inputs[0] = irPins[0].Read();
            inputs[1] = irPins[1].Read();
            inputs[2] = irPins[2].Read();
            inputs[3] = irPins[3].Read();
            int returnValue = 0;

            if (inputs[0] == GpioPinValue.High)
            {
                returnValue += 1;
            }
            if (inputs[1] == GpioPinValue.High)
            {
                returnValue += 2;
            }
            if (inputs[2] == GpioPinValue.High)
            {
                returnValue += 4;
            }
            if (inputs[3] == GpioPinValue.High)
            {
                returnValue += 8;
            }

            return returnValue;
        }
    }
}
