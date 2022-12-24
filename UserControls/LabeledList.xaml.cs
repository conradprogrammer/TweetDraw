using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TweetDraw.UserControls
{
    public partial class LabeledList : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ItemListProperty
            = DependencyProperty.Register("ItemList", typeof(List<string>), typeof(LabeledList));

        public event PropertyChangedEventHandler? PropertyChanged;

        public LabeledList()
        {
            InitializeComponent();
            
        }
        public string Label { get; set; } = "";
        public List<string> ItemList {
            get => (List<string>)GetValue(ItemListProperty);
            set { 
                SetValue(ItemListProperty, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            } 
        }

        private static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register(nameof(Value), typeof(int), typeof(LabeledList));
        public int Value
        {
            get => (int)GetValue(ValueProperty); 
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public event SelectionChangedEventHandler? SelectChanged;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectChanged?.Invoke(sender, e);
        }
    }
}
