using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft;
using Newtonsoft.Json;
using System.Net.Http;
using XamarCalc.CLASSES;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;

namespace XamarCalc.XAML
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StandardCalculator : ContentPage
    {
        // full operation shown on calc screen
        string operation = string.Empty;
        string finalOperation = string.Empty;
        char lastChar;
        int fontDecrease = 20;
        int maxCharacters = 23;
        List<string> history = new List<string>();

        bool possibleDecimal = true;

        // Parenthesis
        int parenthesis = 0;
        bool openPar = true;

        // MathJS Post
        static readonly HttpClient client = new HttpClient();
        static string urlPost = "https://api.mathjs.org/v4/";

        public StandardCalculator()
        {
            AppTheme appTheme = AppInfo.RequestedTheme;

            InitializeComponent();

            // Synchronize with phone's theme
            if (appTheme == AppTheme.Light || appTheme == AppTheme.Unspecified)
                LightTheme();
            else if (appTheme == AppTheme.Dark)
                DarkTheme();
        }

        private void OnAddNumber(object sender, EventArgs e)
        {
            if (operation.Length == maxCharacters)
                return;
            else
            {
                if (operation.Length == 8)
                    resultText.FontSize -= fontDecrease;

                operation += ((Button)sender).Text;
                resultText.Text = operation;
                openPar = false;

                Debug.WriteLine(operation.Length);
            }
        }

        private void OnAddOperator(object sender, EventArgs e)
        {
            if (operation.Length == maxCharacters)
                return;
            else
            {
                if (operation.Length == 8)
                    resultText.FontSize -= fontDecrease;

                if (operation.Length > 0)
                {
                    char lastChar = operation.ElementAt(operation.Length - 1);

                    if (lastChar.ToString() != ((Button)sender).Text)
                        if (lastChar == '+' || lastChar == '-' || lastChar == '×' || lastChar == '÷')
                            operation = operation.Remove(operation.Length - 1, 1) + ((Button)sender).Text;
                        else
                            operation += ((Button)sender).Text;

                    resultText.Text = operation;
                    possibleDecimal = true;
                    openPar = true;
                }
                else if (((Button)sender).Text == "-")
                {
                    operation += ((Button)sender).Text;
                    resultText.Text = operation;
                    possibleDecimal = true;
                    openPar = true;
                }
            }
        }

        private void OnAddDecimal(object sender, EventArgs e)
        {
            if (operation.Length == maxCharacters)
                return;
            else
            {
                if (operation.Length == 8)
                    resultText.FontSize -= fontDecrease;

                if (possibleDecimal)
                {
                    if (operation.Length > 0)
                    {
                        lastChar = operation.ElementAt(operation.Length - 1);

                        if (lastChar.ToString() != ",")
                            if (lastChar == '+' || lastChar == '-' || lastChar == '×' || lastChar == '÷')
                                operation = "0,";
                            else
                                operation += ",";
                    }
                    else
                        operation += "0,";

                    resultText.Text = operation;
                    possibleDecimal = false;
                }
            }
        }

        private void OnParenthesis(object sender, EventArgs e)
        {
            if (operation.Length == maxCharacters)
                return;
            else
            {
                if (operation.Length > 0)
                    lastChar = operation.ElementAt(operation.Length - 1);

                if (openPar)
                {
                    if (operation.Length == 8)
                        resultText.FontSize -= fontDecrease;

                    operation += "(";
                    resultText.Text = operation;

                    parenthesis += 1;
                }
                else if (openPar == false && parenthesis > 0)
                {
                    if (operation.Length == 8)
                        resultText.FontSize -= fontDecrease;

                    operation += ")";
                    resultText.Text = operation;

                    parenthesis -= 1;

                    if (parenthesis == 0)
                        openPar = true;
                }
            }
        }

        private void OnBack(object sender, EventArgs e)
        {
            if (operation.Length > 0)
            {
                if (operation.Length == 9)
                    resultText.FontSize += fontDecrease;

                if (operation.EndsWith(","))
                    possibleDecimal = true;
                else if (operation.EndsWith(")"))
                {
                    parenthesis += 1;
                    openPar = false;
                }
                else if (operation.EndsWith("("))
                {
                    parenthesis -= 1;
                    openPar = true;
                }

                operation = operation.Remove(operation.Length - 1, 1);

                if (operation.EndsWith("("))
                    openPar = true;

                if (operation.Length == 0)
                {
                    resultText.Text = "0";
                    openPar = true;
                }
                else
                    resultText.Text = operation;
            }
        }

        private void OnClear(object sender, EventArgs e)
        {
            operation = string.Empty;
            resultText.Text = "0";

            possibleDecimal = true;

            openPar = true;
            parenthesis = 0;

            resultText.FontSize = 48;
        }

        private async void OnCalculate(object sender, EventArgs e)
        {
            // Closes open paranthesis at the end of the
            // expression if the user left some open
            if (parenthesis != 0)
                for (int x = parenthesis; x != 0; x--)
                    operation += ")";

            // Add current operation to history list
            history.Add(operation);

            // Post on MathJS
            finalOperation = operation.Replace('÷', '/');
            finalOperation = finalOperation.Replace('×', '*');
            finalOperation = finalOperation.Replace(',', '.');

            operation = string.Empty;

            Post newPost = new Post()
            {
                expr = finalOperation,
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
                resultText.Text = resp.result.Replace('.', ',');
                operation = resp.result;
            }
            catch (Exception)
            {
                operation = string.Empty;
                resultText.Text = "Errore";
            }
        }

        public void LightTheme()
        {
            Grid.BackgroundColor = Color.White;
            resultText.BackgroundColor = Color.White;
            resultText.TextColor = Color.Green;
            b0.BackgroundColor = Color.White;
            b0.TextColor = Color.Black;
            b1.BackgroundColor = Color.White;
            b1.TextColor = Color.Black;
            b2.BackgroundColor = Color.White;
            b2.TextColor = Color.Black;
            b3.BackgroundColor = Color.White;
            b3.TextColor = Color.Black;
            b4.BackgroundColor = Color.White;
            b4.TextColor = Color.Black;
            b5.BackgroundColor = Color.White;
            b5.TextColor = Color.Black;
            b6.BackgroundColor = Color.White;
            b6.TextColor = Color.Black;
            b7.BackgroundColor = Color.White;
            b7.TextColor = Color.Black;
            b8.BackgroundColor = Color.White;
            b8.TextColor = Color.Black;
            b9.BackgroundColor = Color.White;
            b9.TextColor = Color.Black;
            bC.BackgroundColor = Color.White;
            bPar.BackgroundColor = Color.White;
            bPar.TextColor = Color.Green;
            bBack.BackgroundColor = Color.White;
            bBack.TextColor = Color.Green;
            bDiv.BackgroundColor = Color.White;
            bDiv.TextColor = Color.Green;
            bMult.BackgroundColor = Color.White;
            bMult.TextColor = Color.Green;
            bAdd.BackgroundColor = Color.White;
            bAdd.TextColor = Color.Green;
            bSub.BackgroundColor = Color.White;
            bSub.TextColor = Color.Green;
            bRes.BackgroundColor = Color.Green;
            bHistory.BackgroundColor = Color.White;
            bDec.BackgroundColor = Color.White;
            bDec.TextColor = Color.Black;
        }

        public void DarkTheme()
        {
            Grid.BackgroundColor = Color.Black;
            resultText.BackgroundColor = Color.Black;
            resultText.TextColor = Color.Green;
            b0.BackgroundColor = Color.Black;
            b0.TextColor = Color.White;
            b1.BackgroundColor = Color.Black;
            b1.TextColor = Color.White;
            b2.BackgroundColor = Color.Black;
            b2.TextColor = Color.White;
            b3.BackgroundColor = Color.Black;
            b3.TextColor = Color.White;
            b4.BackgroundColor = Color.Black;
            b4.TextColor = Color.White;
            b5.BackgroundColor = Color.Black;
            b5.TextColor = Color.White;
            b6.BackgroundColor = Color.Black;
            b6.TextColor = Color.White;
            b7.BackgroundColor = Color.Black;
            b7.TextColor = Color.White;
            b8.BackgroundColor = Color.Black;
            b8.TextColor = Color.White;
            b9.BackgroundColor = Color.Black;
            b9.TextColor = Color.White;
            bC.BackgroundColor = Color.Black;
            bPar.BackgroundColor = Color.Black;
            bPar.TextColor = Color.Green;
            bBack.BackgroundColor = Color.Black;
            bBack.TextColor = Color.Green;
            bDiv.BackgroundColor = Color.Black;
            bDiv.TextColor = Color.Green;
            bMult.BackgroundColor = Color.Black;
            bMult.TextColor = Color.Green;
            bAdd.BackgroundColor = Color.Black;
            bAdd.TextColor = Color.Green;
            bSub.BackgroundColor = Color.Black;
            bSub.TextColor = Color.Green;
            bRes.BackgroundColor = Color.Green;
            bHistory.BackgroundColor = Color.Black;
            bDec.BackgroundColor = Color.Black;
            bDec.TextColor = Color.White;
        }
    }
}