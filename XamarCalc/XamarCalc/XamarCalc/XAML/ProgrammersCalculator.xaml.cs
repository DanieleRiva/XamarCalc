using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class ProgrammersCalculator : ContentPage
    {
        // MathJS Post
        static readonly HttpClient client = new HttpClient();
        static string urlPost = "https://api.mathjs.org/v4/";

        string operation = string.Empty;
        string[] types = { "AND", "OR", "XOR" };

        public ProgrammersCalculator()
        {
            AppTheme appTheme = AppInfo.RequestedTheme;

            InitializeComponent();

            // Synchronize with phone's theme
            if (appTheme == AppTheme.Light || appTheme == AppTheme.Unspecified)
                LightTheme();
            else if (appTheme == AppTheme.Dark)
                DarkTheme();
        }

        private void OnChangedType(object sender, EventArgs e)
        {
            if (types.Contains(typePicker.SelectedItem.ToString()))
            {
                binaryOne.IsVisible = true;
                binaryTwo.IsVisible = true;
                convertButton.IsVisible = true;
                binaryResult.IsVisible = true;
                decimalResult.IsVisible = true;

                binarySingle.IsVisible = false;
                singleResult.IsVisible = false;
            }
            else if (typePicker.SelectedItem.ToString() == "NOT")
            {
                binarySingle.IsVisible = true;
                singleResult.IsVisible = true;

                binaryOne.IsVisible = false;
                binaryTwo.IsVisible = false;
                convertButton.IsVisible = true;
                binaryResult.IsVisible = false;
                decimalResult.IsVisible = false;
            }
        }

        private async void OnExecute(object sender, EventArgs e)
        {
            if (typePicker.SelectedItem.ToString() == "AND")
                operation = $"0b{binaryOne.Text}&0b{binaryTwo.Text}";
            else if (typePicker.SelectedItem.ToString() == "OR")
                operation = $"0b{binaryOne.Text}|0b{binaryTwo.Text}";
            else if (typePicker.SelectedItem.ToString() == "XOR")
                operation = $"0b{binaryOne.Text}^|0b{binaryTwo.Text}";
            else if (typePicker.SelectedItem.ToString() == "NOT")
                operation = $"~0b{binarySingle.Text}";


            Post newPost = new Post()
            {
                expr = operation,
                precision = 5
            };

            try
            {
                binaryResult.Text = string.Empty;

                string jsonData = JsonConvert.SerializeObject(newPost);

                // Send post to the server
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urlPost, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                Response resp = JsonConvert.DeserializeObject<Response>(jsonResponse);

                string bin = Convert.ToString(int.Parse(resp.result), 2);

                if (bin.Length < 8)
                    for (int i = 8 - bin.Length; i > 0; i--)
                        binaryResult.Text += "0";

                binaryResult.Text += bin;

                if (typePicker.SelectedItem.ToString() != "NOT")
                    decimalResult.Text = resp.result;
                else
                    singleResult.Text = resp.result;

                operation = string.Empty;
            }
            catch (Exception)
            {
                operation = string.Empty;
            }
        }

        private void OnDecToBinary(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(fromDec.Text) <= 255)
                {
                    string bin = Convert.ToString(int.Parse(fromDec.Text), 2);

                    if (bin.Length < 8)
                        for (int i = 8 - bin.Length; i > 0; i--)
                            toBinary.Text += "0";

                    toBinary.Text += bin;
                }
            }
            catch (Exception)
            {
                fromBinary.Text = string.Empty;
            }
        }

        private void OnBinaryToDec(object sender, EventArgs e)
        {
            try
            {
                toDec.Text = Convert.ToInt32(fromBinary.Text, 2).ToString();
            }
            catch (Exception)
            {
                fromBinary.Text = string.Empty;
            }
        }

        private void DarkTheme()
        {
            Grid.BackgroundColor = Color.Black;
            typePicker.TextColor = Color.White;
            binaryOne.TextColor = Color.White;
            binaryTwo.TextColor = Color.White;
            decimalResult.TextColor = Color.White;
            binaryResult.TextColor = Color.White;
            fromDec.TextColor = Color.White;
            fromBinary.TextColor = Color.White;
        }

        private void LightTheme()
        {
            Grid.BackgroundColor = Color.White;
            typePicker.TextColor = Color.Black;
            binaryOne.TextColor = Color.Black;
            binaryTwo.TextColor = Color.Black;
            decimalResult.TextColor = Color.Black;
            binaryResult.TextColor = Color.Black;
            fromDec.TextColor = Color.Black;
            fromBinary.TextColor = Color.Black;
        }
    }
}