using System;
using System.IO.Ports;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace WiFiModule_IoT
{
    class Program
    {
        SerialPort sp = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);

        static void Main(string[] args)
        {
            new Program();
        }

        private Program()
        {
            while (true)
                if (SerialPort.GetPortNames().Length > 0)
                {
                    if (!sp.IsOpen)
                    {

                        sp.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
                        sp.Open();
                        Console.Read();
                    }
                }
                else { Console.WriteLine("Please connect arduino."); Console.ReadKey(); }
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(1000);

            string result = sp.ReadExisting();

            if (!String.IsNullOrEmpty(result))
                postIndicators(result);
        }

        private async void postIndicators(string indicator)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    await client.PostAsync(
                        "https://medicalfridgeserver.azurewebsites.net/api/Indicators/",
                        new StringContent(indicator, Encoding.UTF8, "application/json")
                        );
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
