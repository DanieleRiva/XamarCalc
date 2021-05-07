using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarCalc
{
    public partial class MainPage : ContentPage
    {
        string operation = "";

        bool possibleDecimal = true;
        //string[] splitOperation;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnAddNumber(object sender, EventArgs e)
        {
            operation += ((Button)sender).Text;
            resultText.Text = operation;
        }

        private void OnSelectOperator(object sender, EventArgs e)
        {
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
            }
        }

        private void OnAddDecimal(object sender, EventArgs e)
        {
            if (possibleDecimal)
            {
                if (operation.Length > 0)
                {
                    char lastChar = operation.ElementAt(operation.Length - 1);

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

        private void OnParenthesis(object sender, EventArgs e)
        {
            //  !! Handle the single parenthesis from the user !!
        }

        private void OnBack(object sender, EventArgs e)
        {
            if (operation.Length > 0)
            {
                if (operation.EndsWith(","))
                    possibleDecimal = true;
                operation = operation.Remove(operation.Length - 1, 1);

                if (operation.Length == 0)
                    resultText.Text = "0";
                else
                    resultText.Text = operation;
            }
        }

        private void OnClear(object sender, EventArgs e)
        {
            operation = "";
            resultText.Text = "0";
            possibleDecimal = true;
        }

        private void OnCalculate(object sender, EventArgs e)
        {
            // Send operation to the api
        }

        private void LightTheme(object sender, EventArgs e)
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
            bLightTheme.BackgroundColor = Color.White;
            bDarkTheme.BackgroundColor = Color.White;
        }

        private void DarkTheme(object sender, EventArgs e)
        {
            Grid.BackgroundColor = Color.Black;
            resultText.BackgroundColor = Color.Black;
            resultText.TextColor = Color.MediumSlateBlue;
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
            bPar.TextColor = Color.MediumSlateBlue;
            bBack.BackgroundColor = Color.Black;
            bBack.TextColor = Color.MediumSlateBlue;
            bDiv.BackgroundColor = Color.Black;
            bDiv.TextColor = Color.MediumSlateBlue;
            bMult.BackgroundColor = Color.Black;
            bMult.TextColor = Color.MediumSlateBlue;
            bAdd.BackgroundColor = Color.Black;
            bAdd.TextColor = Color.MediumSlateBlue;
            bSub.BackgroundColor = Color.Black;
            bSub.TextColor = Color.MediumSlateBlue;
            bRes.BackgroundColor = Color.MediumSlateBlue;
            bHistory.BackgroundColor = Color.Black;
            bLightTheme.BackgroundColor = Color.Black;
            bDarkTheme.BackgroundColor = Color.Black;
        }

        
    }
}
