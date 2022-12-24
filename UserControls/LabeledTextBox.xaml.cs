using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TweetDraw.UserControls
{
    public partial class LabeledTextBox : UserControl
    {
        public LabeledTextBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Placeholder { get; set; } = "";
        public string Label { get; set; } = "";


        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register("DynamicText", typeof(string), typeof(LabeledTextBox));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public event TextChangedEventHandler? TextChanged;
        private void inputText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }
    }
}
