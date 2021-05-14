using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarCalc.XAML
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderContentView : ContentView
    {
        public HeaderContentView()
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
            Header.Source = "header4.jfif";
        }

        private void DarkTheme()
        {
            Header.Source = "header_Dark2.png";
        }
    }
}