using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using XamarCalc.CLASSES;
using XamarCalc.XAML;
using Xamarin.Forms;

namespace XamarCalc.CLASSES
{
    public class History
    {
        public ObservableCollection<string> HistoryOperation { get; set; }
        public ObservableCollection<List<string>> HistoryListOperation { get; set; }

        public History()
        {
            HistoryOperation = new ObservableCollection<string>();
            HistoryListOperation = new ObservableCollection<List<string>>();
        }
    }
}
