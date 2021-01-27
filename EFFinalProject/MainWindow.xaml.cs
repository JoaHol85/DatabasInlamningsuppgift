using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EFFinalLibrary;

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

        private void btn_Temperature_Click(object sender, RoutedEventArgs e)
        {
            lbx_Result.Items.Clear();
            bool checkboxes = ValidateSortingCheckboxes();
            string place = InsideOrOutside();
            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();

            if (checkboxes)
            {
                lbx_Result.Items.Add("Datum\t\tTemperatur");
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
            lbx_Result.Items.Clear();
            bool checkboxes = ValidateSortingCheckboxes();
            string place = InsideOrOutside();
            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();

            if (checkboxes)
            {
                lbx_Result.Items.Add("Datum\t\tLuftfuktighet");
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
            lbx_Result.Items.Clear();
            bool checkboxes = ValidateSortingCheckboxes();
            string place = InsideOrOutside();
            List<DateAndAverageNumber> result = new List<DateAndAverageNumber>();

            if (checkboxes)
            {
                lbx_Result.Items.Add("Datum\t\tMögelrisk");
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

        private string InsideOrOutside()
        {
            if ((bool)cbx_Inside.IsChecked)
                return "Inne";
            else if ((bool)cbx_Outside.IsChecked)
                return "Ute";
            return "";
        }
        private bool ValidateSortingCheckboxes()
        {
            if ((bool)cbx_HighToLow.IsChecked == false && (bool)cbx_LowToHigh.IsChecked == false || (bool)cbx_Inside.IsChecked == false && (bool)cbx_Outside.IsChecked == false)
            {
                lbx_Result.Items.Add("Inget/Inga sorteringsval har gjorts!\n" +
                                     "Välj inne eller ute och ordningen på resultatet.");
                return false;
            }
            else
                return true;
        }


        private void btn_ReadFromFile_Click(object sender, RoutedEventArgs e)
        {
            (bool readFinished, int numberOfFiles) = EFLibrary.ReadFile("TemperaturData.csv");
            if (readFinished)
            {
                lbx_Result.Items.Add($"{numberOfFiles} Filer sparade till databasen.");
            }
            if (!readFinished)
            {
                lbx_Result.Items.Add("Något gick fel!\n" +
                                     "Se till att filen finns tillgänglig.");
            }
        }
    }
}
