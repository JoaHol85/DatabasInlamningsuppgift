using EFFinalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EFFinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        // Checkboxes
        private void cbx_LowToHigh_Checked(object sender, RoutedEventArgs e)
        {
            cbx_HighToLow.IsChecked = false;
        }
        private void cbx_HighToLow_Checked(object sender, RoutedEventArgs e)
        {
            cbx_LowToHigh.IsChecked = false;
        }
        private void cbx_Inside_Checked(object sender, RoutedEventArgs e)
        {
            cbx_Outside.IsChecked = false;
        }
        private void cbx_Outside_Checked(object sender, RoutedEventArgs e)
        {
            cbx_Inside.IsChecked = false;
        }


        // Temperature, Humidity and Moldrisk buttons
        private void btn_Temperature_Click(object sender, RoutedEventArgs e)
        {
            bool checkboxes = ValidateSortingCheckboxes();
            string place = InsideOrOutside();
            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();

            if (checkboxes)
            {
                PrintMessage("Datum\t\tTemperatur");
                if ((bool)cbx_HighToLow.IsChecked == true)
                {
                    result = EFLibrary.TempHottestToColdestDay(place);
                }
                else if ((bool)cbx_LowToHigh.IsChecked == true)
                {
                    result = EFLibrary.TempColdestToHottestDay(place);
                }

                foreach (var item in result)
                {
                    lbx_Result.Items.Add($"{item.Date.ToString("yyyy-MM-dd")}\t{Math.Round(item.AverageTemperature, 1)}");
                }
            }
        }
        private void btn_Humidity_Click(object sender, RoutedEventArgs e)
        {
            bool checkboxes = ValidateSortingCheckboxes();
            string place = InsideOrOutside();
            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();

            if (checkboxes)
            {
                PrintMessage("Datum\t\tLuftfuktighet");
                if ((bool)cbx_HighToLow.IsChecked == true)
                {
                    result = EFLibrary.HumidityMostToLeast(place);
                }
                else if ((bool)cbx_LowToHigh.IsChecked == true)
                {
                    result = EFLibrary.HumidityLeastToMost(place);
                }

                foreach (var item in result)
                {
                    lbx_Result.Items.Add($"{item.Date.ToString("yyyy-MM-dd")}\t{Math.Round(item.AverageHumidity, 1)}");
                }
            }
        }
        private void btn_MoldRisk_Click(object sender, RoutedEventArgs e)
        {
            bool checkboxes = ValidateSortingCheckboxes();
            string place = InsideOrOutside();
            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();

            if (checkboxes)
            {
                PrintMessage("Datum\t\tMögelrisk");
                if ((bool)cbx_HighToLow.IsChecked == true)
                {
                    result = EFLibrary.MoldRiskMostToLeast(place);
                }
                else if ((bool)cbx_LowToHigh.IsChecked == true)
                {
                    result = EFLibrary.MoldRiskLeastToMost(place);
                }

                foreach (var item in result)
                {
                    lbx_Result.Items.Add($"{item.Date.ToString("yyyy-MM-dd")}\t{Math.Round(item.MoldRisk, 1)}%");
                }
            }
        }


        // Meteorological Winter / Autumn buttons
        private void btn_MeteorologicalAutumn_Click(object sender, RoutedEventArgs e)
        {
            PrintMessage(EFLibrary.MeteorologicalAutumn());
        }
        private void btn_MeteorologicalWinter_Click(object sender, RoutedEventArgs e)
        {
            PrintMessage(EFLibrary.MeteorologicalWinter());
        }


        // Most / Least temperature difference buttons
        private void btn_MostTempDifference_Click(object sender, RoutedEventArgs e)
        {
            List<TemperatureDifference> tempDiffList = EFLibrary.TempDifference();

            var qu = tempDiffList
                .OrderByDescending(r => r.TempDifferenceInsideOutside)
                .Take(150);

            PrintMessage("Datum:\t\tTimme:\tTemperaturskillnad Inne/Ute");
            foreach (var item in qu)
            {
                lbx_Result.Items.Add($"{item.Time.ToString("yy-MM-dd\tHH")}\t\t{Math.Round(item.TempDifferenceInsideOutside, 1)}");
            }
        }
        private void btn_LeastTempDifference_Click(object sender, RoutedEventArgs e)
        {
            List<TemperatureDifference> tempDiffList = EFLibrary.TempDifference();

            var qu = tempDiffList
                .OrderBy(r => r.TempDifferenceInsideOutside)
                .Take(150);

            PrintMessage("Datum:\t\tTimme:\tTemperaturskillnad Inne/Ute");
            foreach (var item in qu)
            {
                lbx_Result.Items.Add($"{item.Time.ToString("yy-MM-dd\tHH")}\t\t{Math.Round(item.TempDifferenceInsideOutside, 1)}");
            }
        }


        // Search for data at specific date button
        private void btn_SearchDate_Click(object sender, RoutedEventArgs e)
        {
            bool checkboxes = true;
            lbx_Result.Items.Clear();

            if ((bool)cbx_Inside.IsChecked == false && (bool)cbx_Outside.IsChecked == false)
            {
                checkboxes = false;
                lbx_Result.Items.Add("Du måste ange plats för din sökning.\n" +
                                     "Kryssa för Inne/Ute.");
            }
            if (checkboxes)
            {
                lbx_Result.Items.Add(EFLibrary.AverageTempSearch(InsideOrOutside(), dp_SelectDate.SelectedDate.ToString()));
            }
        }
       
        
        // Save data input to database
        private void btn_SaveData_Click(object sender, RoutedEventArgs e)
        {
            string place = tbx_Place.Text.ToLower();
            bool placeInput = true;
            switch (place)
            {
                case "inne":
                    place = "Inne";
                    break;
                case "ute":
                    place = "Ute";
                    break;
                default:
                    PrintMessage("Vänligen ange platsen för mätningen\n" +
                                 "Inne eller Ute.");
                    placeInput = false;
                    break;
            }
            if (placeInput)
            {
                double temp;
                bool tempCheck = double.TryParse(tbx_Temp.Text, out temp);
                if (!tempCheck)
                {
                    PrintMessage("Temperatur värdet är inte korekt!\n" +
                                 "Vänligen ange temperaturen i siffror.");
                }
                else
                {
                    bool hum = double.TryParse(tbx_Humidity.Text, out double humidity);
                    if (!hum)
                    {
                        PrintMessage("Luftfuktighetsvärdet är inte korekt!\n" +
                                     "Vänligen ange luftfuktigheten i siffror.");
                    }
                    else
                    {
                        EFLibrary.AddDataToDatabase(place, temp, humidity);
                        PrintMessage("Data sparat!");
                    }
                }
            }
        }


        // Checks if data should be taken from inside or outside
        private string InsideOrOutside()
        {
            if ((bool)cbx_Inside.IsChecked)
                return "Inne";
            else if ((bool)cbx_Outside.IsChecked)
                return "Ute";
            return "";
        }


        // Checks so that the checkboxes are correctly filled
        private bool ValidateSortingCheckboxes()
        {
            if ((bool)cbx_HighToLow.IsChecked == false && (bool)cbx_LowToHigh.IsChecked == false || (bool)cbx_Inside.IsChecked == false && (bool)cbx_Outside.IsChecked == false)
            {
                PrintMessage("Inget/Inga sorteringsval har gjorts!\n" +
                             "Välj inne eller ute och ordningen på resultatet.");
                return false;
            }
            else
                return true;
        }


        // Read from file button
        private void btn_ReadFromFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Vill du verkligen infoga data från fil?", "Ladda upp fil till databas", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                (bool readFinished, int numberOfFiles) = EFLibrary.ReadFile("TemperaturData.csv");
                if (readFinished)
                {
                    PrintMessage($"{numberOfFiles} Filer sparade till databasen.");
                }
                if (!readFinished)
                {
                    PrintMessage("Något gick fel!\n" +
                                 "Se till att filen finns tillgänglig.");
                }
            }
        }
        
        
        // Prints messages
        private void PrintMessage(string message)
        {
            lbx_Result.Items.Clear();
            lbx_Result.Items.Add(message);
        }


    }
}
