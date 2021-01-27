using System;
using System.Collections.Generic;
using System.Text;

namespace EFFinalLibrary
{
    public class TemperatureDifference
    {
        public DateTime Time { get; set; }
        public double TempDifferenceInsideOutside { get; set; }

        public TemperatureDifference(DateTime time, double tempDiff)
        {
            Time = time;
            TempDifferenceInsideOutside = tempDiff;
        }
    }
}
