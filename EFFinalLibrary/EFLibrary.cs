using EFFinalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


// FIXA SÅ MAN KAN SE NÄR BALKONGDÖRREN ÄR ÖPPEN.
// KAN MAN TA UR FOREACH LOOPEN I METODERNA NEDAN?
// LÄGGA TILL INFO TILL DATABASEN FÖR DAGENS DATUM
// TA BORT EN VISS DATA MARKERING
// ÄNDRA EN VISS DATA MARKERING?


namespace EFFinalLibrary
{
    public class EFLibrary
    {

        // KONVERTERAR STRING TILL DATETIME - TA BORT???
        public static DateTime SearchDate(string date) // "yyyy-MM-dd"
        {
            try
            {
                DateTime time = DateTime.Parse(date);
                return time.Date;
            }
            catch
            {
                return DateTime.Now;
            }
        }
        // LÄGGER TILL EN TEMPERATUR PoST I TABELLEN I DATABASEN
        public static void AddTemp(DateTime time, string place, double temp, double humidity)
        {
            TemperatureData newData = new TemperatureData()
            {
                Time = time,
                Place = place,
                Temperature = temp,
                Humidity = humidity
            };

            using (var context = new EFFinalContext())
            {
                context.Add(newData);
                context.SaveChanges();
            }

        }
        // LÄSER HELA databasen och skriver ut - TA BORT???
        public static void ReadTable()
        {
            using (var context = new EFFinalContext())
            {
                foreach (var item in context.TempData)
                {
                    Console.WriteLine($"{item.Time} - {item.Place} - {item.Temperature} - {item.Humidity}");
                }
            }
        }
        // SORTERAR HELA DATABASEN INNE EFTER TEMPERATUR - TABORT???
        public static void ColdestToHottest()
        {
            using (var context = new EFFinalContext())
            {
                var q = context.TempData
                    .Where(q => q.Place == "Inne")
                    .OrderBy(q => q.Temperature);

                foreach (var item in q)
                {
                    Console.WriteLine(item.Temperature);
                }
            }
        }




        public static string MeteorologicalAutumn()
        {
            List<DateAndAverageNumber> averageDataEachDay = GetAverageTempAndHumidityData("Ute");

            bool noMeteorologicalAutumn = true;
            int daysInRow = 0;
            DateAndAverageNumber target = new DateAndAverageNumber();
            foreach (var item in averageDataEachDay)
            {
                if (daysInRow == 0)
                {
                    target = item;
                }
                if (item.AverageTemperature < 10 && item.AverageTemperature > 0)
                {
                    daysInRow++;

                    if (daysInRow == 5)
                    {
                        return $"Året {target.Date.Year} inföll den meteorologiska hösten: {target.Date.Day}/{target.Date.Month}" +
                               $"\nTemperaturen var då {Math.Round(target.AverageTemperature, 1)} grader";
                    }
                }
                else
                {
                    daysInRow = 0;
                }
            }
            if (noMeteorologicalAutumn)
                return "Den meteorologiska hösten inträffade aldrig.";
            else
                return "";
        }
        public static string MeteorologicalWinter()
        {
            List<DateAndAverageNumber> averageDataEachDay = GetAverageTempAndHumidityData("Ute");

            bool noMeteorologiaclWinter = true;
            int daysInRow = 0;
            DateAndAverageNumber target = new DateAndAverageNumber();
            foreach (var item in averageDataEachDay)
            {
                if (daysInRow == 0)
                {
                    target = item;
                }
                if (item.AverageTemperature < 0)
                {
                    daysInRow++;

                    if (daysInRow == 5)
                    {
                        return $"Året {target.Date.Year} inföll den meteorologiska vintern: {target.Date.Day}/{target.Date.Month}" +
                               $"\nTemperaturen var då {Math.Round(target.AverageTemperature, 1)} grader";
                    }
                }
                else
                {
                    daysInRow = 0;
                }
            }
            if (noMeteorologiaclWinter)
                return $"Den Meteorologiska vintern inträffade aldig.";
            else
                return "";
        }




        public static List<DateAndAverageNumber> HumidityMostToLeast(string place)
        {
            List<DateAndAverageNumber> averageHumidityData = GetAverageTempAndHumidityData(place);

            var q = averageHumidityData
                .OrderByDescending(a => a.AverageHumidity);

            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();
            foreach (var item in q)
            {
                result.Add(item);
            }
            return result;
        }
        public static List<DateAndAverageNumber> HumidityLeastToMost(string place)
        {
            List<DateAndAverageNumber> averageHumidityData = GetAverageTempAndHumidityData(place);

            var q = averageHumidityData
                .OrderBy(a => a.AverageHumidity);

            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();
            foreach (var item in q)
            {
                result.Add(item);
            }
            return result;
        }




        public static List<DateAndAverageNumber> TempHottestToColdestDay(string place)
        {
            List<DateAndAverageNumber> averageTempEachDay = GetAverageTempAndHumidityData(place);

            var a = averageTempEachDay
                .OrderByDescending(a => a.AverageTemperature);

            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();
            foreach (var item in a)
            {
                result.Add(item);
            }
            return result;
        }
        public static List<DateAndAverageNumber> TempColdestToHottestDay(string place)
        {
            List<DateAndAverageNumber> averageTempEachDay = GetAverageTempAndHumidityData(place);

            var a = averageTempEachDay
                .OrderBy(a => a.AverageTemperature).ToList();

            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();
            foreach (var item in a)
            {
                result.Add(item);
            }
            return result;
        }




        public static List<DateAndAverageNumber> MoldRiskLeastToMost(string place)
        {
            List<DateAndAverageNumber> averageData = GetAverageTempAndHumidityData(place);
            CalculateMoldRisk(averageData);

            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();
            var q = averageData
                .OrderBy(o => o.MoldRisk)
                .ThenBy(o => o.Date);

            foreach (var item in q)
            {
                result.Add(item);
            }
            return result;
        }
        public static List<DateAndAverageNumber> MoldRiskMostToLeast(string place)
        {
            List<DateAndAverageNumber> averageData = GetAverageTempAndHumidityData(place);
            CalculateMoldRisk(averageData);

            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();
            var q = averageData
                .OrderByDescending(o => o.MoldRisk)
                .ThenBy(o => o.Date);

            foreach (var item in q)
            {
                result.Add(item);
            }
            return result;
        }
        private static void CalculateMoldRisk(List<DateAndAverageNumber> averageData)
        {
            foreach (var item in averageData)
            {
                double moldRisk = ((item.AverageHumidity - 78) * (item.AverageTemperature / 15)) / 0.22;
                if (moldRisk < 0)
                    item.MoldRisk = 0;
                else
                    item.MoldRisk = moldRisk;
            }
        }



        ////UTRÄKNING FÖR BALKONGDÖRREN!
        public static void BalconyOpenEachDay()
        {
            List<DateAndAverageNumber> dataList = new List<DateAndAverageNumber>();
            List<List<DateAndAverageNumber>> listOfLists = new List<List<DateAndAverageNumber>>();
            using (var context = new EFFinalContext())
            {
                var q = context.TempData
                    .Where(t => t.Place == "Inne")
                    .OrderBy(t => t.Time)
                    .AsEnumerable()
                    .GroupBy(t => t.Time.Date);

                foreach (var group in q)
                {
                    dataList.Clear();
                    foreach (var item in group)
                    {
                        dataList.Add(new DateAndAverageNumber(item.Time, item.Temperature, 0));
                    }
                    listOfLists.Add(dataList);
                }
            }

            double latestTemp = 0;
            foreach (var list in listOfLists)
            {
                DateTime timePeriod;
                double temp = 0;
                var period = list.First();
                foreach (var item in list)
                {
                    if (latestTemp == 0)
                        latestTemp = item.AverageTemperature;
                    else
                    {
                        temp = item.AverageTemperature;
                        if (temp < latestTemp - 0.5)
                        {
                            timePeriod = item.Date;
                            latestTemp = item.AverageTemperature;
                        }
                        if (temp > latestTemp + 0.5)
                        {

                        }
                    }
                }
            }
        }



        public static string AverageTempSearch(string place, string dateInput)
        {
            List<DateAndAverageNumber> averageTempEachDay = GetAverageTempAndHumidityData(place);
            bool foundDate = false;
            foreach (var item in averageTempEachDay)
            {
                if (item.Date.Date == SearchDate(dateInput).Date)
                {
                    return $"Datum\t\tTemperatur\tLuftfuktighet\n" +
                           $"{item.Date.ToString("yyyy-MM-dd")}\t{Math.Round(item.AverageTemperature, 1)}\t\t{Math.Round(item.AverageHumidity, 1)}%";
                }
            }
            if (!foundDate)
                return "Datumet fann inte registrerat i databasen.";
            return "";
        }



        public static List<DateAndAverageNumber> GetAverageTempAndHumidityData(string place)
        {
            List<DateAndAverageNumber> averageTempAndHumidityEachDay = new List<DateAndAverageNumber>();
            using (var context = new EFFinalContext())
            {
                var q = context.TempData
                    .Where(d => d.Place == place)
                    .OrderBy(d => d.Time)
                    .AsEnumerable()
                    .GroupBy(d => d.Time.Date);

                foreach (var group in q)
                {
                    double avgHumidity = group
                        .Average(t => t.Humidity);

                    double avgTemperature = group
                        .Average(t => t.Temperature);

                    var dateData = group.First();
                    DateTime time = dateData.Time.Date;

                    averageTempAndHumidityEachDay.Add(new DateAndAverageNumber(time, avgTemperature, avgHumidity));
                }
            }
            return averageTempAndHumidityEachDay;
        }



        public static void AddDataToDatabase(string place, double temp, double humidity)
        {
            using (var context = new EFFinalContext())
            {
                TemperatureData data = new TemperatureData();
                data.Place = place;
                data.Humidity = humidity;
                data.Temperature = temp;
                data.Time = DateTime.Now;

                context.Add(data);
                context.SaveChanges();
            }
        }
        public static (bool, int) ReadFile(string filePath)
        {
            NumberFormatInfo provider = new NumberFormatInfo();             // Tvungen att använda för att konvertera från string till double. 
            provider.NumberDecimalSeparator = ".";                          // Då decimaltalen använder sig av "." och inte "," .
            provider.NumberGroupSeparator = ",";

            try
            {
                List<TemperatureData> importedData = new List<TemperatureData>();
                string[] dataLines = File.ReadAllLines(filePath);

                foreach (var line in dataLines)
                {
                    string[] splitLine = line.Split(',');
                    TemperatureData temp = new TemperatureData();

                    temp.Time = DateTime.Parse(splitLine[0]);
                    temp.Place = splitLine[1];
                    temp.Temperature = Convert.ToDouble(splitLine[2], provider); //double.Parse(splitLine[2].Replace(".", ","))    // Behöver 'NumberFormaInfo' för att konvertera denna, se ovan!
                    temp.Humidity = double.Parse(splitLine[3]);

                    importedData.Add(temp);
                }

                int numberOfDataFiles = importedData.Count();
                using (var context = new EFFinalContext())
                {
                    try
                    {
                        foreach (var item in importedData)
                        {
                            context.Add(item);
                        }
                        context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return (false, 0);
                    }
                }
                return (true, numberOfDataFiles);
            }
            catch(Exception)
            {
                return (false, 0);
            }
        }
    }
}
