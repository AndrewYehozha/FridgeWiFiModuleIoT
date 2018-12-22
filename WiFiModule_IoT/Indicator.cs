using System;

namespace WiFiModule_IoT
{
    class Indicator
    {
        public int IdFridge { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public Nullable<decimal> Humidity { get; set; }
    }
}