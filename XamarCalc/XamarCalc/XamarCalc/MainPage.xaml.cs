using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace XamarCalc
{
    public partial class MainPage : ContentPage
    {
        // Phone mode (portait - landscape)
        //private double width = 0;
        //private double height = 0;

        // full operation shown on calc screen
        string operation = string.Empty;
        char lastChar;
        int fontDecrease = 20;
        int maxCharacters = 23;

        bool possibleDecimal = true;

        // Parenthesis
        int parenthesis = 0;
        bool openPar = true;

        public MainPage()
        {
            AppTheme appTheme = AppInfo.RequestedTheme;

            InitializeComponent();

            // Synchronize with phone's theme
            if (appTheme == AppTheme.Light || appTheme == AppTheme.Unspecified)
                LightTheme(null, null);
            else if (appTheme == AppTheme.Dark)
                DarkTheme(null, null);
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

        private void OnSelectOperator(object sender, EventArgs e)
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

                if (operation.Length == 8)
                    resultText.FontSize -= fontDecrease;


                if (openPar)
                {
                    operation += "(";
                    resultText.Text = operation;

                    parenthesis += 1;
                }
                else if (parenthesis > 0)
                {
                    operation += ")";
                    resultText.Text = operation;

                    parenthesis -= 1;
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
                    openPar = true;
                }

                operation = operation.Remove(operation.Length - 1, 1);

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

        private void OnCalculate(object sender, EventArgs e)
        {
            // Send operation to the api

            // Closes open paranthesis at the end of the
            // expression if the user left some open
            if (parenthesis != 0)
                for (int x = parenthesis; x != 0; x--)
                    operation += ")";

            resultText.Text = operation;
        }

        //OnOperationChanged()
        //{
        //    if (operation.Length > 10)
        //        resultText.FontSize -= 5;
        //}

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    Debug.WriteLine("Orizzontale");
        //    Debug.WriteLine("Orizzontale");

        //    base.OnSizeAllocated(width, height); //must be called

        //    if (this.width != width || this.height != height)
        //    {
        //        this.width = width;
        //        this.height = height;

        //        //reconfigure layout
        //        if (width > height) // Landscape
        //            Debug.WriteLine("Orizzontale");
        //        //resultText.Text = "Orizzontale";
        //        else // Portrait
        //            Debug.WriteLine("Orizzontale");
        //        //resultText.Text = "Verticale";
        //    }
        //}

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
            bDec.BackgroundColor = Color.White;
            bDec.TextColor = Color.Black;
        }

        private void DarkTheme(object sender, EventArgs e)
        {
            Grid.BackgroundColor = Color.Black;
            resultText.BackgroundColor = Color.Black;
            resultText.TextColor = Color.MediumSlateBlue;
            b0.BackgroundColor = Color.MediumSlateBlue;
            b0.TextColor = Color.White;
            b1.BackgroundColor = Color.MediumSlateBlue;
            b1.TextColor = Color.White;
            b2.BackgroundColor = Color.MediumSlateBlue;
            b2.TextColor = Color.White;
            b3.BackgroundColor = Color.MediumSlateBlue;
            b3.TextColor = Color.White;
            b4.BackgroundColor = Color.MediumSlateBlue;
            b4.TextColor = Color.White;
            b5.BackgroundColor = Color.MediumSlateBlue;
            b5.TextColor = Color.White;
            b6.BackgroundColor = Color.MediumSlateBlue;
            b6.TextColor = Color.White;
            b7.BackgroundColor = Color.MediumSlateBlue;
            b7.TextColor = Color.White;
            b8.BackgroundColor = Color.MediumSlateBlue;
            b8.TextColor = Color.White;
            b9.BackgroundColor = Color.MediumSlateBlue;
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
            bDec.BackgroundColor = Color.Black;
            bDec.TextColor = Color.White;
        }
    }
}