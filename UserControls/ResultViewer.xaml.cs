using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TweetDraw.Model;

namespace TweetDraw.UserControls
{
    public partial class ResultViewer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ResultViewer()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        List<ResultModel>? _displayList = null;
        public List<ResultModel>? DisplayList
        {
            get => _displayList;
            set
            {
                _displayList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayList)));
            }
        }

        private void ViewEntry(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = ((Button)sender).Tag.ToString() ?? "",
                UseShellExecute = true,
            });
        }

        public event RoutedEventHandler? ReAction;
        private void Reaction(object sender, RoutedEventArgs e)
        {
            ReAction?.Invoke(sender, e);
        }
    }
}
