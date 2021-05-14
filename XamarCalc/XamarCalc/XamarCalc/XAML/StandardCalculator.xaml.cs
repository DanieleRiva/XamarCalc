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
        int maxCharacters = 22;

        bool possibleDecimal = true;
        char[] numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        char[] operators = { '+', '-', '×', '÷' };

        // Parenthesis
        int parenthesis = 0;
        bool openPar = true;
        bool onlySub = true;
        int degPar = 0;

        // Scientific calculator
        bool usesTrigonometry = false;
        bool usesLogarithms = false;
        string logBase = "10";
        List<string> operationList = new List<string>();

        // MathJS Post
        static readonly HttpClient client = new HttpClient();
        static string urlPost = "https://api.mathjs.org/v4/";

        History historyClass = new History();

        public StandardCalculator()
        {
            AppTheme appTheme = AppInfo.RequestedTheme;

            InitializeComponent();

            HistoryListView.ItemsSource = historyClass.HistoryOperation;

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
                operationList.Add(((Button)sender).Text); // Add to list for scientific calc
                resultText.Text = operation;

                openPar = false;
                onlySub = false;

                Debug.WriteLine(operation.Length);
            }
        }

        private void OnAddOperator(object sender, EventArgs e)
        {
            if (operation.Length == maxCharacters)
                return;
            else
            {
                if (operation.Length > 0 && !onlySub)
                {
                    if (operation.Length == 8)
                        resultText.FontSize -= fontDecrease;

                    lastChar = operation.ElementAt(operation.Length - 1);

                    if (lastChar.ToString() != ((Button)sender).Text)
                        if (lastChar == '+' || lastChar == '-' || lastChar == '×' || lastChar == '÷')
                        {
                            operation = operation.Remove(operation.Length - 1, 1) + ((Button)sender).Text;

                            operationList.RemoveAt(operationList.Count - 1); // Replace to list for scientific calc
                            operationList.Add(((Button)sender).Text);        // 
                        }
                        else
                        {
                            operation += ((Button)sender).Text;

                            operationList.Add(((Button)sender).Text); // Add to list for scientific calc
                        }

                    resultText.Text = operation;
                    possibleDecimal = true;
                    openPar = true;
                }
                else if (onlySub && ((Button)sender).Text == "-")
                {
                    operation += ((Button)sender).Text;

                    operationList.Add(((Button)sender).Text); // Add to list for scientific calc

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
                if (possibleDecimal)
                {
                    if (operation.Length == 8)
                        resultText.FontSize -= fontDecrease;

                    if (operation.Length > 0)
                    {
                        lastChar = operation.ElementAt(operation.Length - 1);

                        if (lastChar.ToString() != ",")
                            if (lastChar == '+' || lastChar == '-' || lastChar == '×' || lastChar == '÷')
                            {
                                operation += "0,";

                                operationList.Add("0."); // Add to list for scientific calc
                            }
                            else
                            {
                                operation += ",";

                                operationList.Add("."); // Add to list for scientific calc
                            }
                    }
                    else
                    {
                        operation += "0,";

                        operationList.Add("."); // Add to list for scientific calc
                    }

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
                    operationList.Add("("); // Add to list for scientific calc
                    resultText.Text = operation;

                    parenthesis += 1;
                }
                else if (openPar == false && parenthesis > 0)
                {
                    if (operation.Length == 8)
                        resultText.FontSize -= fontDecrease;

                    operation += ")";
                    operationList.Add(")"); // Add to list for scientific calc
                    resultText.Text = operation;

                    parenthesis -= 1;

                    if (parenthesis == 0)
                        openPar = true;
                    if (parenthesis == degPar && usesTrigonometry)
                    {
                        operationList.Insert(operationList.Count - 1, " deg");
                        usesTrigonometry = false;
                    }
                    else if (parenthesis == degPar && usesLogarithms)
                    {
                        operationList.Insert(operationList.Count - 1, $",{logBase}");
                        usesTrigonometry = false;
                    }
                }
            }
        }

        private void OnAddScience(object sender, EventArgs e)
        {
            if (operation.Length > 0)
                lastChar = operation.ElementAt(operation.Length - 1);

            Debug.WriteLine(lastChar);

            if (((Button)sender).Text == "sin")
            {
                operation += "sin(";

                openPar = true;
                degPar = parenthesis;
                parenthesis += 1;
                usesTrigonometry = true;

                operationList.Add("sin("); // Add to list for scientific cal

                onlySub = true;
                possibleDecimal = false;

                resultText.Text = operation;
            }
            else if (((Button)sender).Text == "cos")
            {
                operation += "cos(";

                openPar = true;
                degPar = parenthesis;
                parenthesis += 1;
                usesTrigonometry = true;

                operationList.Add("cos("); // Add to list for scientific cal

                onlySub = true;
                possibleDecimal = false;

                resultText.Text = operation;
            }
            else if (((Button)sender).Text == "tan")
            {
                operation += "tan(";

                openPar = true;
                degPar = parenthesis;
                parenthesis += 1;
                usesTrigonometry = true;

                operationList.Add("tan("); // Add to list for scientific cal

                onlySub = true;
                possibleDecimal = false;

                resultText.Text = operation;
            }
            else if (operation.Length > 0 && ((Button)sender).StyleId == "1" && numbers.Contains(lastChar) || lastChar == ')' && ((Button)sender).StyleId == "1") // x^2
            {
                Debug.WriteLine("Sono nel ^2");

                operation += "^2";

                usesTrigonometry = false;

                operationList.Add("^2"); // Add to list for scientific cal

                onlySub = false;
                possibleDecimal = false;

                resultText.Text = operation;
            }
            else if (((Button)sender).StyleId == "2" && operation.Length > 0 && numbers.Contains(lastChar) || lastChar == ')' && ((Button)sender).StyleId == "2") // x^y
            {
                operation += "^";

                usesTrigonometry = false;

                operationList.Add("^"); // Add to list for scientific cal

                onlySub = true;
                possibleDecimal = false;

                openPar = true;

                resultText.Text = operation;
            }
            else if (((Button)sender).StyleId == "3") // √(
            {
                operation += "√(";

                openPar = true;
                parenthesis += 1;
                usesTrigonometry = false;

                operationList.Add("sqrt("); // Add to list for scientific cal

                onlySub = true;
                possibleDecimal = false;

                resultText.Text = operation;
            }
            else if (((Button)sender).Text == "log" || ((Button)sender).StyleId == "4")
            {
                if (((Button)sender).StyleId == "4")
                {
                    operation += "log₂(";
                    logBase = "2";
                }
                else
                {
                    operation += "log(";
                    logBase = "10";
                }


                openPar = true;
                parenthesis += 1;
                usesTrigonometry = false;
                usesLogarithms = true;

                operationList.Add("log("); // Add to list for scientific cal

                onlySub = true;
                possibleDecimal = false;

                resultText.Text = operation;
            }
            else if (((Button)sender).Text == "ln")
            {
                operation += "ln(";
                logBase = "e";


                openPar = true;
                parenthesis += 1;
                usesTrigonometry = false;
                usesLogarithms = true;

                operationList.Add("log("); // Add to list for scientific cal

                onlySub = true;
                possibleDecimal = false;

                resultText.Text = operation;
            }
            else if (((Button)sender).Text == "!" && ((Button)sender).StyleId == "0") // !
            {
                if (operation.Length == maxCharacters)
                    return;
                else
                {
                    if (operation.Length > 0)
                        lastChar = operation.ElementAt(operation.Length - 1);

                    if (numbers.Contains(lastChar) || lastChar == ')')
                    {
                        if (operation.Length == 8)
                            resultText.FontSize -= fontDecrease;

                        operation += "!";
                        operationList.Add("!"); // Add to list for scientific cal
                        resultText.Text = operation;
                    }

                    openPar = false;
                    onlySub = false;
                    possibleDecimal = false;
                }
            }
        }

        private void OnBack(object sender, EventArgs e)
        {
            if (operationList.Count > 0)
            {
                if (operationList[operationList.Count - 1].Contains(','))
                {
                    char[] charArr = operationList[operationList.Count - 1].ToCharArray();
                    logBase = charArr[1].ToString();
                }

                if (operation.Length == 9 && resultText.FontSize != 48)
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

                int count;

                if (operationList[operationList.Count - 1].Length > 1 && operationList[operationList.Count - 1].ElementAt(operationList[operationList.Count - 1].Length - 2) == 't')
                    count = 2;
                else if (operationList[operationList.Count - 1].Length > 1 && operationList[operationList.Count - 1].ElementAt(operationList[operationList.Count - 1].Length - 2) == 'g' && logBase == "2") // If log2 discrepancy with list
                {
                    count = 5;
                    Debug.WriteLine("Count: " + count);
                    logBase = "10";

                    if (operationList[operationList.Count - 1].Contains(' ') || operationList[operationList.Count - 1].Contains(',')) // prevents deleting discrepancy with list when using trigon or logs
                        operationList.RemoveAt(operationList.Count - 1); // Remove from list for scientific calc
                }
                else if (operationList[operationList.Count - 1].Length > 1 && operationList[operationList.Count - 1].ElementAt(operationList[operationList.Count - 1].Length - 2) == 'g' && logBase == "e") // If ln discrepancy with list
                {
                    count = 3;
                    Debug.WriteLine("Count: " + count);
                    logBase = "10";

                    if (operationList[operationList.Count - 1].Contains(' ') || operationList[operationList.Count - 1].Contains(',')) // prevents deleting discrepancy with list when using trigon or logs
                        operationList.RemoveAt(operationList.Count - 1); // Remove from list for scientific calc
                }
                else
                {
                    if (operationList[operationList.Count - 1].Contains(' ') || operationList[operationList.Count - 1].Contains(',')) // prevents deleting discrepancy with list when using trigon or logs
                        operationList.RemoveAt(operationList.Count - 1); // Remove from list for scientific calc


                    count = operationList[operationList.Count - 1].Length;
                }

                //if (operationList[operationList.Count - 1] == "")
                    operation = operation.Remove(operation.Length - count, count); //

                if (operationList[operationList.Count - 1] == "sen(")
                    usesTrigonometry = false;

                operationList.RemoveAt(operationList.Count - 1); // Remove from list for scientific calc

                if (operation.EndsWith("^"))
                    onlySub = true;
                else if (operation.Length >= 2 && operation[operation.Length - 2] == '√') // Radix
                    onlySub = true;
                else if (operation.Length >= 4 && operation[operation.Length - 2] == 'n') // sin
                    onlySub = true;
                else if (operation.Length >= 2 && operation[operation.Length - 2] == 's') // cos
                    onlySub = true;
                else if (operation.Length >= 4 && operation[operation.Length - 3] == 'a') // tan
                    onlySub = true;

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
            operationList = new List<string>(); // Wipe the list for scientific calc
            resultText.Text = "0";

            possibleDecimal = true;

            onlySub = true;

            usesTrigonometry = false;

            openPar = true;
            parenthesis = 0;

            logBase = "10";

            resultText.FontSize = 48;
        }

        private async void OnCalculate(object sender, EventArgs e)
        {
            // Closes open paranthesis at the end of the
            // expression if the user left some open
            if (parenthesis != 0)
                for (int x = parenthesis; x != 0; x--)
                {
                    operation += ")";
                    operationList.Add(")"); // Add to list for scientific calc
                    parenthesis -= 1;

                    if (usesTrigonometry && x == 1)
                    {
                        Debug.WriteLine("ENTRATO");
                        operationList.Insert(operationList.Count - 1, " deg");
                        usesTrigonometry = false;
                    }
                    else if (usesLogarithms && x == 1)
                    {
                        operationList.Insert(operationList.Count - 1, $",{logBase}");
                        usesLogarithms = false;
                    }
                }

            // Post on MathJS
            finalOperation = String.Concat(operationList);
            Debug.WriteLine(finalOperation);
            finalOperation = finalOperation.Replace('÷', '/');
            finalOperation = finalOperation.Replace('×', '*');
            finalOperation = finalOperation.Replace("𝜋", "pi");

            logBase = "10";

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

                resultText.FontSize = 48;

                // Add current operation to history list
                operation += "=" + resp.result;

                // History
                historyClass.HistoryOperation.Add(operation);
                historyClass.HistoryListOperation.Add(operationList);


                operation = resp.result;
                operationList = new List<string>(); // Wipe list and add the result
                char[] respCharForList = resp.result.ToCharArray();
                foreach (var character in respCharForList)
                    operationList.Add(character.ToString());

            }
            catch (Exception)
            {
                operation = string.Empty;
                resultText.Text = "Errore";
            }
        }

        private void OnHistory(object sender, EventArgs e)
        {
            HistoryStackLayout.IsVisible = true;

            if (historyClass.HistoryOperation.Count == 0)
                EmptyHistoryText.IsVisible = true;
            else
                EmptyHistoryText.IsVisible = false;
        }

        private void OnCloseHistory(object sender, EventArgs e)
        {
            HistoryStackLayout.IsVisible = false;
        }

        private void OnTappedHistoryItem(object sender, ItemTappedEventArgs e)
        {
            int pos = historyClass.HistoryOperation.IndexOf(((sender as ListView).SelectedItem).ToString());

            operation = historyClass.HistoryOperation[pos];
            operationList = historyClass.HistoryListOperation[pos]; // Wipe the list for scientific calc
            operation = operation.Substring(0, operation.IndexOf('=')); //Remove everything after the =
            resultText.Text = operation;

            possibleDecimal = true;

            if (operation == "0")
                onlySub = true;
            else
                onlySub = false;

            usesTrigonometry = false;

            openPar = true;
            parenthesis = 0;

            logBase = "10";

            if (operation.Length <= 9)
                resultText.FontSize = 48;

            HistoryStackLayout.IsVisible = false;
        }

        public void LightTheme()
        {
            // Standard
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

            // Scientific
            bSin.BackgroundColor = Color.White;
            bSin.TextColor = Color.Black;
            bCos.BackgroundColor = Color.White;
            bCos.TextColor = Color.Black;
            bTan.BackgroundColor = Color.White;
            bTan.TextColor = Color.Black;
            bPow.BackgroundColor = Color.White;
            bPow.TextColor = Color.Black;
            bPowy.BackgroundColor = Color.White;
            bPowy.TextColor = Color.Black;
            bSqrt.BackgroundColor = Color.White;
            bSqrt.TextColor = Color.Black;
            bLog.BackgroundColor = Color.White;
            bLog.TextColor = Color.Black;
            bLog2.BackgroundColor = Color.White;
            bLog2.TextColor = Color.Black;
            bLn.BackgroundColor = Color.White;
            bLn.TextColor = Color.Black;
            bE.BackgroundColor = Color.White;
            bE.TextColor = Color.Black;
            bFatt.BackgroundColor = Color.White;
            bFatt.TextColor = Color.Black;
            bPi.BackgroundColor = Color.White;
            bPi.TextColor = Color.Black;

            // History
            bHistory.Source = "history.png";
            HistoryStackLayout.BackgroundColor = Color.White;
            EmptyHistoryText.TextColor = Color.Black;
            CloseHistoryButton.TextColor = Color.Black;
        }

        public void DarkTheme()
        {
            // Standard
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

            // Scientific
            bSin.BackgroundColor = Color.Black;
            bSin.TextColor = Color.White;
            bCos.BackgroundColor = Color.Black;
            bCos.TextColor = Color.White;
            bTan.BackgroundColor = Color.Black;
            bTan.TextColor = Color.White;
            bPow.BackgroundColor = Color.Black;
            bPow.TextColor = Color.White;
            bPowy.BackgroundColor = Color.Black;
            bPowy.TextColor = Color.White;
            bSqrt.BackgroundColor = Color.Black;
            bSqrt.TextColor = Color.White;
            bLog.BackgroundColor = Color.Black;
            bLog.TextColor = Color.White;
            bLog2.BackgroundColor = Color.Black;
            bLog2.TextColor = Color.White;
            bLn.BackgroundColor = Color.Black;
            bLn.TextColor = Color.White;
            bE.BackgroundColor = Color.Black;
            bE.TextColor = Color.White;
            bFatt.BackgroundColor = Color.Black;
            bFatt.TextColor = Color.White;
            bPi.BackgroundColor = Color.Black;
            bPi.TextColor = Color.White;

            // History
            bHistory.Source = "historyDark.png";
            HistoryStackLayout.BackgroundColor = Color.Black;
            EmptyHistoryText.TextColor = Color.White;
            CloseHistoryButton.TextColor = Color.White;
        }
    }
}