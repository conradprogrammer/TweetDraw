using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TweetDraw.Model;

namespace TweetDraw.UserControls
{
    public partial class PagingGrid : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public List<string> PageSizes { get; set; } = new List<string>() { "5", "10", "20", "50"};

        public PagingGrid()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        internal void PagingRefresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartPos)));
            if (_filteredList == null)
                DisplayList = null;
            else
            {
                btnFirst.Visibility = btnLast.Visibility = (Count > PageSize * 3) ? Visibility.Visible : Visibility.Collapsed;
                btnNext.Visibility = btnNext.Visibility = (Count > PageSize) ? Visibility.Visible : Visibility.Collapsed;
                btnFirst.IsEnabled = btnPrev.IsEnabled = CurrentPage > 1;
                btnLast.IsEnabled = btnNext.IsEnabled = CurrentPage < PageCount;

                int _pageGroupStartpage = (PageGroupIndex - 1) * 5 + 1;
                btn1.Visibility = (PageCount >= _pageGroupStartpage) ? Visibility.Visible : Visibility.Collapsed;
                btn2.Visibility = (PageCount >= _pageGroupStartpage + 1) ? Visibility.Visible : Visibility.Collapsed;
                btn3.Visibility = (PageCount >= _pageGroupStartpage + 2) ? Visibility.Visible : Visibility.Collapsed;
                btn4.Visibility = (PageCount >= _pageGroupStartpage + 3) ? Visibility.Visible : Visibility.Collapsed;
                btn5.Visibility = (PageCount >= _pageGroupStartpage + 4) ? Visibility.Visible : Visibility.Collapsed;
                btn1.Tag = btn1.Content = _pageGroupStartpage.ToString();
                btn2.Tag = btn2.Content = (_pageGroupStartpage + 1).ToString();
                btn3.Tag = btn3.Content = (_pageGroupStartpage + 2).ToString();
                btn4.Tag = btn4.Content = (_pageGroupStartpage + 3).ToString();
                btn5.Tag = btn5.Content = (_pageGroupStartpage + 4).ToString();

                DisplayList = _filteredList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            }

        }

        int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                PagingRefresh();
            }
        }

        int _pageSize = 1;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                PagingRefresh();
            }
        }

        public int PageGroupIndex { get => (int)Math.Ceiling((float)CurrentPage / 5); }
        public int StartPos { get => (CurrentPage - 1) * PageSize; }
        public int Count { get => (FilteredList == null) ? 1 : FilteredList.Count; }
        public int PageCount { get => (int)Math.Ceiling((float)Count / PageSize); }

        List<Retweet>? _displayList = null;
        public List<Retweet>? DisplayList
        {
            get => _displayList;
            set
            {
                _displayList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayList)));
            }
        }

        private List<Retweet>? _filteredList = null;
        public List<Retweet>? FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
                PagingRefresh();
            }
        }

        private List<Retweet>? _retweets = null;
        public List<Retweet>? Retweets
        {
            get => _retweets;
            set
            {
                _retweets = value;
                if (_retweets != null)
                {
                    _pageSize = int.Parse(PageSizes[cmdPageSize.Value]);
                    FilteredList = _retweets;
                }

            }
        }

        // Event Handlers
        private void SearchKeyChanged(object sender, TextChangedEventArgs e)
        {
            if (_retweets == null) return;
            string key = ((TextBox)sender).Text;
            if (key == null || key == "")
            {
                FilteredList = _retweets;
            }
            else
            {
                _currentPage = 1;
                FilteredList = _retweets.Where(e => e.Match(key)).ToList();
            }
        }
        private void PageSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((ComboBox)sender).SelectedIndex;
            CurrentPage = 1;
            PageSize = int.Parse(PageSizes[index]);
        }
        private void GotoFirst(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
        }
        private void Prev(object sender, RoutedEventArgs e)
        {
            CurrentPage--;
        }
        private void GotoPage(object sender, RoutedEventArgs e)
        {
            CurrentPage = int.Parse(((Button)sender).Tag.ToString() ?? "0");
        }
        private void Next(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
        }
        private void GotoLast(object sender, RoutedEventArgs e)
        {
            CurrentPage = Count / PageSize + 1;
        }
        private void ViewEntry(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = ((Button)sender).Tag.ToString() ?? "",
                UseShellExecute = true,
            });
        }
    }
}