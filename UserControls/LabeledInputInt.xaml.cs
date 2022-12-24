using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TweetDraw.UserControls
{
    public partial class LabeledInputInt : UserControl, INotifyPropertyChanged
    {
        public LabeledInputInt()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public string Label { get; set; } = "";
        public int Value { get; set; } = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Plus(object sender, RoutedEventArgs e)
        {
            Value++;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
        private void Minus(object sender, RoutedEventArgs e)
        {
            if (Value == 1) return;
            Value--;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}
