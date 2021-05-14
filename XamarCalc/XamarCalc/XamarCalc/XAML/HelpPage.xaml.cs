using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarCalc.XAML
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPage : ContentPage
    {
        public HelpPage()
        {
            AppTheme appTheme = AppInfo.RequestedTheme;

            InitializeComponent();

            // Synchronize with phone's theme
            if (appTheme == AppTheme.Light || appTheme == AppTheme.Unspecified)
                LightTheme();
            else if (appTheme == AppTheme.Dark)
                DarkTheme();
        }

        private void LightTheme()
        {
            StackLayout.BackgroundColor = Color.White;
            Text1.TextColor = Color.Black;
            Text2.TextColor = Color.Black;
            Text3.TextColor = Color.Black;
            Image1.Source = "helpMenu.png";
            Image2.Source = "helpCalc.png";
        }

        private void DarkTheme()
        {
            StackLayout.BackgroundColor = Color.Black;
            Text1.TextColor = Color.White;
            Text2.TextColor = Color.White;
            Text3.TextColor = Color.White;
            Image1.Source = "helpMenu_Dark.png";
            Image2.Source = "helpCalc_Dark.png";
        }
    }
}