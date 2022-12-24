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
    public partial class TweetPreview : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TweetProperty
            = DependencyProperty.Register("Data", typeof(TweetModel), typeof(TweetPreview));

        public event PropertyChangedEventHandler? PropertyChanged;

        public TweetModel? Tweet
        {
            get => (TweetModel)GetValue(TweetProperty);
            set
            {
                SetValue(TweetProperty, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tweet)));
            }
        }
        public TweetPreview()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OpenTweet(object sender, MouseButtonEventArgs e)
        {
            if(Tweet != null && Tweet.Includes != null && Tweet.Includes.Users != null && Tweet.Data != null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"https://twitter.com/{Tweet.Includes.Users[0].UserName}/status/{Tweet.Data.Id}",
                    UseShellExecute = true,
                });
            }
        }
    }
}
