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
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;
using IoT_06.Models;
using Windows.Web.Http;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoT_06
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static readonly int[] led01pinNums = new int[3] { 13, 6, 5 };//LED01のピン番号
        static readonly int[] led02pinNums = new int[3] { 16, 12, 1 };//LED02のピン番号
        static readonly int[] irSensorPinNums = new int[4] { 18, 23, 24, 25 };//irSensor(Arduino)のピン番号（下位ビットから順番）

        private bool getFlag;

        private MyColors colors;
        private ColorLED led01;
        private ColorLED led02;
        private IrSensor sensor;
        private TempDataStorage storage;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init();
            Reset();
            base.OnNavigatedTo(e);
        }

        private async void Init()
        {
            led01 = new ColorLED(led01pinNums);
            led02 = new ColorLED(led02pinNums);
            colors = new MyColors();
            sensor = new IrSensor(irSensorPinNums);
            storage = new TempDataStorage();
            await InitOnline();
        }

        private void Reset()
        {
            getFlag = false;
            Blinker();
            GetLoop();
        }

        private async Task InitOnline()
        {
            var client = new HttpClient();
            HttpResponseMessage res;
            try
            {
                res = await client.GetAsync(new Uri("http://mspjp-iot-test.azurewebsites.net/initRpi"));
                string json = res.Content.ToString();
                json = json.Replace("\"", "");
                json = json.Replace("[", "");
                json = json.Replace(" ", "");
                json = json.Replace("]", "");
                var data = json.Split(',');
                var number = data.Count();
                for (var i = 0; i < number; i++)
                {
                    if (i % 2 == 0)
                    {
                        storage.sticks.Add(new Stick(data[i], data[i + 1]));
                    }
                }
                number = storage.sticks.Count;
                for (var i = 0; i < number; i++)
                {
                    debugBox2.Text += (storage.sticks[i].num + ":" + storage.sticks[i].color + "\n");
                }
                /*storage.sticks.Add(new Stick("1", "Red"));
                storage.sticks.Add(new Stick("12", "Blue"));
                storage.sticks.Add(new Stick("2", "Purple"));
                storage.sticks.Add(new Stick("3", "Green"));
                storage.sticks.Add(new Stick("4", "Yellow"));*/
            }
            catch (Exception ex)
            {
                debugBox.Text = ex.Message;
            }


        }
        private async Task Blinker()
        {
            while (!getFlag)
            {
                led01.ChangeColor(colors.Red);
                led02.ChangeColor(colors.Purple);
                if (getFlag)
                {
                    break;
                }
                await Task.Delay(700);
                led01.ChangeColor(colors.Orange);
                led02.ChangeColor(colors.Blue);
                if (getFlag)
                {
                    break;
                }
                await Task.Delay(700);
                led01.ChangeColor(colors.Yellow);
                led02.ChangeColor(colors.Green);
                if (getFlag)
                {
                    break;
                }
                await Task.Delay(700);
                led01.ChangeColor(colors.Green);
                led02.ChangeColor(colors.Yellow);
                if (getFlag)
                {
                    break;
                }
                await Task.Delay(700);
                led01.ChangeColor(colors.Blue);
                led02.ChangeColor(colors.Orange);
                if (getFlag)
                {
                    break;
                }
                await Task.Delay(700);
                led01.ChangeColor(colors.Purple);
                led02.ChangeColor(colors.Red);
                if (getFlag)
                {
                    break;
                }
                await Task.Delay(700);
            }
        }

        private async Task GetLoop()
        {
            var innerGetFlag = true;
            int returnValue = 0;
            while (innerGetFlag)
            {
                returnValue = sensor.CheckNum();
                if (returnValue != 0)
                {
                    innerGetFlag = false;
                    getFlag = true;
                    debugBox2.Text = returnValue.ToString();
                    break;
                }
                await Task.Delay(10);
            }

            foreach (var stick in storage.sticks)
            {
                if (stick.num == returnValue.ToString())
                {
                    SendStick(stick);
                    if (stick.color == colors.Red.colorName)
                    {
                        led01.ChangeColor(colors.Red);
                        led02.ChangeColor(colors.Red);
                    }
                    else if (stick.color == colors.Orange.colorName)
                    {
                        led01.ChangeColor(colors.Orange);
                        led02.ChangeColor(colors.Orange);
                    }
                    else if (stick.color == colors.Yellow.colorName)
                    {
                        led01.ChangeColor(colors.Yellow);
                        led02.ChangeColor(colors.Yellow);
                    }
                    else if (stick.color == colors.Green.colorName)
                    {
                        led01.ChangeColor(colors.Green);
                        led02.ChangeColor(colors.Green);
                    }
                    else if (stick.color == colors.Blue.colorName)
                    {
                        led01.ChangeColor(colors.Blue);
                        led02.ChangeColor(colors.Blue);
                    }
                    else if (stick.color == colors.Purple.colorName)
                    {
                        led01.ChangeColor(colors.Purple);
                        led02.ChangeColor(colors.Purple);
                    }
                    led01.lockFlg = true;
                    led02.lockFlg = true;
                    break;
                }
            }

            await Task.Delay(10000);
            led01.lockFlg = false;
            led02.lockFlg = false;
            Reset();
        }

        private async Task SendStick(Stick stick)
        {
            var client = new HttpClient();
            var content = new HttpMultipartFormDataContent();

            var stickNum = new HttpStringContent(stick.num);
            var raspiNum = new HttpStringContent(RasPiData.number.ToString());

            content.Add(stickNum, "stick");
            content.Add(raspiNum, "rpi");
            HttpResponseMessage res;
            try
            {
                res = await client.PostAsync(new Uri("http://mspjp-iot-test.azurewebsites.net/fromRpi"), content);

            }
            catch (Exception ex)
            {
                debugBox.Text = ex.Message;
            }
        }
    }
}