using System;
using System.IO.Ports;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

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
            try
            {
                sp.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
                sp.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(1000);
            string result = sp.ReadExisting();
            Indicator json = JsonConvert.DeserializeObject<Indicator>(result);
            postIndicators(json);
        }

        private async void postIndicators(Indicator indicator)
        {
            string json = "{" + $"\"IdFridge\":{indicator.IdFridge}," +
                                $"\"Temperature\":{indicator.Temperature.ToString().Replace(',', '.')}," +
                                $"\"Humidity\":{indicator.Humidity.ToString().Replace(',', '.')}," +
                          "}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    await client.PostAsync(
                        "https://medicalfridgeserver.azurewebsites.net/api/Indicators",
                        new StringContent(json, Encoding.UTF8, "application/json")
                        );
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
