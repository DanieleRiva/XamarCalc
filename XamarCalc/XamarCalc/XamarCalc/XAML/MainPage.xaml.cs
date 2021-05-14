using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarCalc
{
    public partial class MainPage : Shell
    {
        public MainPage()
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
            ShellName.BackgroundColor = Color.White;
        }

        private void DarkTheme()
        {
            
        }
    }
}