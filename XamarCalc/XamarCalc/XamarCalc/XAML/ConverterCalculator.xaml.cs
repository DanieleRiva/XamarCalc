using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XamarCalc.CLASSES;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarCalc.XAML
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConverterCalculator : ContentPage
    {
        // MathJS Post
        static readonly HttpClient client = new HttpClient();
        static string urlPost = "https://api.mathjs.org/v4/";

        string[] fromPickers = { "1", "3", "5", "7", "9" };
        string[] toPickers = { "2", "4", "6", "8", "10" };
        string operation = string.Empty;
        List<string> lungList = new List<string>();

        public ConverterCalculator()
        {
            AppTheme appTheme = AppInfo.RequestedTheme;

            InitializeComponent();

            // Synchronize with phone's theme
            if (appTheme == AppTheme.Light || appTheme == AppTheme.Unspecified)
                LightTheme();
            else if (appTheme == AppTheme.Dark)
                DarkTheme();
            
            lungList.Add("km");
            lungList.Add("hm");
            lungList.Add("dam");
            lungList.Add("m");
            lungList.Add("dm");
            lungList.Add("cm");
            lungList.Add("mm");
        }

        private void OnChangedType(object sender, EventArgs e)
        {
            if ((typePicker.SelectedItem.ToString() == "Lunghezza"))
            {
                fromEntry.IsVisible = false;
                fromEntry.Text = string.Empty;
                toEntry.IsVisible = false;
                toEntry.Text = string.Empty;

                lengthFromPicker.SelectedItem = null;
                lengthToPicker.SelectedItem = null;
                lengthFromPicker.IsVisible = true;
                lengthToPicker.IsVisible = true;

                capacityFromPicker.IsVisible = false;
                capacityToPicker.IsVisible = false;

                timeFromPicker.IsVisible = false;
                timeToPicker.IsVisible = false;

                weightFromPicker.IsVisible = false;
                weightToPicker.IsVisible = false;

                tempFromPicker.IsVisible = false;
                tempToPicker.IsVisible = false;

                convertButton.IsVisible = true;
            }
            else if ((typePicker.SelectedItem.ToString() == "Peso"))
            {
                fromEntry.IsVisible = false;
                fromEntry.Text = string.Empty;
                toEntry.IsVisible = false;
                toEntry.Text = string.Empty;

                lengthFromPicker.IsVisible = false;
                lengthToPicker.IsVisible = false;

                weightFromPicker.SelectedItem = null;
                weightToPicker.SelectedItem = null;
                weightFromPicker.IsVisible = true;
                weightToPicker.IsVisible = true;

                capacityFromPicker.IsVisible = false;
                capacityToPicker.IsVisible = false;

                timeFromPicker.IsVisible = false;
                timeToPicker.IsVisible = false;

                tempFromPicker.IsVisible = false;
                tempToPicker.IsVisible = false;

                convertButton.IsVisible = true;
            }
            else if ((typePicker.SelectedItem.ToString() == "Capacità"))
            {
                fromEntry.IsVisible = false;
                fromEntry.Text = string.Empty;
                toEntry.IsVisible = false;
                toEntry.Text = string.Empty;

                lengthFromPicker.IsVisible = false;
                lengthToPicker.IsVisible = false;

                weightFromPicker.IsVisible = false;
                weightToPicker.IsVisible = false;

                capacityFromPicker.SelectedItem = null;
                capacityToPicker.SelectedItem = null;
                capacityFromPicker.IsVisible = true;
                capacityToPicker.IsVisible = true;

                timeFromPicker.IsVisible = false;
                timeToPicker.IsVisible = false;

                tempFromPicker.IsVisible = false;
                tempToPicker.IsVisible = false;

                convertButton.IsVisible = true;
            }
            else if ((typePicker.SelectedItem.ToString() == "Tempo"))
            {
                fromEntry.IsVisible = false;
                fromEntry.Text = string.Empty;
                toEntry.IsVisible = false;
                toEntry.Text = string.Empty;

                lengthFromPicker.IsVisible = false;
                lengthToPicker.IsVisible = false;

                weightFromPicker.IsVisible = false;
                weightToPicker.IsVisible = false;

                capacityFromPicker.IsVisible = false;
                capacityToPicker.IsVisible = false;

                timeFromPicker.IsVisible = true;
                timeToPicker.IsVisible = true;
                timeFromPicker.SelectedItem = null;
                timeToPicker.SelectedItem = null;

                tempFromPicker.IsVisible = false;
                tempToPicker.IsVisible = false;

                convertButton.IsVisible = true;
            }
            else if ((typePicker.SelectedItem.ToString() == "Temperatura"))
            {
                fromEntry.IsVisible = false;
                fromEntry.Text = string.Empty;
                toEntry.IsVisible = false;
                toEntry.Text = string.Empty;

                lengthFromPicker.IsVisible = false;
                lengthToPicker.IsVisible = false;

                weightFromPicker.IsVisible = false;
                weightToPicker.IsVisible = false;

                capacityFromPicker.IsVisible = false;
                capacityToPicker.IsVisible = false;

                timeFromPicker.IsVisible = false;
                timeToPicker.IsVisible = false;

                tempFromPicker.IsVisible = true;
                tempToPicker.IsVisible = true;
                tempFromPicker.SelectedItem = null;
                tempToPicker.SelectedItem = null;

                convertButton.IsVisible = true;
            }
        }

        private void OnFromEntryChanged(object sender, TextChangedEventArgs e)
        {
            if (fromEntry.Text.Length > 0)
                convertButton.IsEnabled = true;
            else
                convertButton.IsEnabled = false;
        }

        private void OnChosenUnit(object sender, EventArgs e)
        {
            if (((Picker)sender).SelectedItem != null)
                if (fromPickers.Contains(((Picker)sender).StyleId)) //FromPicker
                    fromEntry.IsVisible = true;
                else if (toPickers.Contains(((Picker)sender).StyleId)) //ToPicker
                    toEntry.IsVisible = true;
        }

        private async void OnConvert(object sender, EventArgs e)
        {
            if (typePicker.SelectedItem.ToString() == "Lunghezza")
                operation = $"{fromEntry.Text} {lengthFromPicker.SelectedItem} in {lengthToPicker.SelectedItem}";
            else if (typePicker.SelectedItem.ToString() == "Peso")
                operation = $"{fromEntry.Text} {weightFromPicker.SelectedItem} in {weightToPicker.SelectedItem}";
            else if (typePicker.SelectedItem.ToString() == "Capacità")
                operation = $"{fromEntry.Text} {capacityFromPicker.SelectedItem} in {capacityToPicker.SelectedItem}";
            else if (typePicker.SelectedItem.ToString() == "Tempo")
            {
                string from = string.Empty;
                string into = string.Empty;

                if (timeFromPicker.SelectedItem.ToString() == "Anni")
                    from = "year";
                else if (timeFromPicker.SelectedItem.ToString() == "Mesi")
                    from = "month";
                else if (timeFromPicker.SelectedItem.ToString() == "Settimane")
                    from = "week";
                else if (timeFromPicker.SelectedItem.ToString() == "Giorni")
                    from = "day";
                else if (timeFromPicker.SelectedItem.ToString() == "Ore")
                    from = "hour";
                else if (timeFromPicker.SelectedItem.ToString() == "Minuti")
                    from = "minute";
                else if (timeFromPicker.SelectedItem.ToString() == "Secondi")
                    from = "second";
                else if (timeFromPicker.SelectedItem.ToString() == "Millisecondi")
                    from = "millisecond";

                if (timeToPicker.SelectedItem.ToString() == "Anni")
                    into = "year";
                else if (timeToPicker.SelectedItem.ToString() == "Mesi")
                    into = "month";
                else if (timeToPicker.SelectedItem.ToString() == "Settimane")
                    into = "week";
                else if (timeToPicker.SelectedItem.ToString() == "Giorni")
                    into = "day";
                else if (timeToPicker.SelectedItem.ToString() == "Ore")
                    into = "hour";
                else if (timeToPicker.SelectedItem.ToString() == "Minuti")
                    into = "minute";
                else if (timeToPicker.SelectedItem.ToString() == "Secondi")
                    into = "second";
                else if (timeToPicker.SelectedItem.ToString() == "Millisecondi")
                    into = "millisecond";

                operation = $"{fromEntry.Text} {from} in {into}";
            }
            else if (typePicker.SelectedItem.ToString() == "Temperatura")
                operation = $"{fromEntry.Text} deg{tempFromPicker.SelectedItem.ToString().Remove(tempFromPicker.SelectedItem.ToString().Length - 1, 1)} in deg{tempToPicker.SelectedItem.ToString().Remove(tempToPicker.SelectedItem.ToString().Length - 1, 1)}";

            Post newPost = new Post()
            {
                expr = operation,
                precision = 5
            };

            try
            {
                Debug.WriteLine(newPost.ToString());
                string jsonData = JsonConvert.SerializeObject(newPost);

                // Send post to the server
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urlPost, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                Response resp = JsonConvert.DeserializeObject<Response>(jsonResponse);
                toEntry.Text = resp.result.Split(' ')[0];
            }
            catch (Exception)
            {
                toEntry.Text = "Errore";
            }
        }

        public void LightTheme()
        {
            Grid.BackgroundColor = Color.White;
            fromEntry.TextColor = Color.Black;
            lengthFromPicker.TextColor = Color.Black;
            weightFromPicker.TextColor = Color.Black;
            capacityFromPicker.TextColor = Color.Black;
            timeFromPicker.TextColor = Color.Black;
            tempFromPicker.TextColor = Color.Black;

            lengthToPicker.TextColor = Color.Black;
            weightToPicker.TextColor = Color.Black;
            capacityToPicker.TextColor = Color.Black;
            timeToPicker.TextColor = Color.Black;
            tempToPicker.TextColor = Color.Black;

            typePicker.TextColor = Color.Black;
        }

        public void DarkTheme()
        {
            Grid.BackgroundColor = Color.Black;
            fromEntry.TextColor = Color.White;
            lengthFromPicker.TextColor = Color.White;
            weightFromPicker.TextColor = Color.White;
            capacityFromPicker.TextColor = Color.White;
            timeFromPicker.TextColor = Color.White;
            tempFromPicker.TextColor = Color.White;

            lengthToPicker.TextColor = Color.White;
            weightToPicker.TextColor = Color.White;
            capacityToPicker.TextColor = Color.White;
            timeToPicker.TextColor = Color.White;
            tempToPicker.TextColor = Color.White;

            typePicker.TextColor = Color.White;
        }
    }
}