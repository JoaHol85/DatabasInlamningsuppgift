using System;
using System.Collections.Generic;
using System.Text;

namespace EFFinalLibrary
{
    public class DateAndAverageNumber
    {
        public DateTime Date { get; set; }
        public double AverageTemperature { get; set; }
        public double AverageHumidity { get; set; }
        public double MoldRisk { get; set; }

        public DateAndAverageNumber(DateTime date, double temp, double humidity)
        {
            Date = date;
            AverageTemperature = temp;
            AverageHumidity = humidity;
        }
        public DateAndAverageNumber()
        {

        }
    }
}
