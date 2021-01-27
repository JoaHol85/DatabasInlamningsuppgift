using System;
using System.Collections.Generic;
using System.Text;

namespace EFFinalLibrary.Models
{
    class TemperatureData
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Place { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }
}
