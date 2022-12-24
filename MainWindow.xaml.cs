using Newtonsoft.Json;
using shortid;
using shortid.Configuration;
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
using TweetDraw.Helpers;
using TweetDraw.Model;
using TweetDraw.UserControls;

namespace TweetDraw
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        GenerationOptions IdOption = new GenerationOptions
        {
            Length = 12,
            UseNumbers = true
        };
        public event PropertyChangedEventHandler? PropertyChanged;

        public SettingModel DrawSetting { get; set; }

        DateTime? _drawDate = null;
        public DateTime? DrawDate
        {
            get => _drawDate;
            set
            {
                _drawDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DrawDate)));
            }
        }

        string? _drawId = null;
        public string? DrawId
        {
            get => _drawId;
            set
            {
                _drawId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DrawId)));
            }
        }

        bool _isBegun = false;
        public bool IsBegun
        {
            get => _isBegun;
            set
            {
                _isBegun = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBegun)));
            }
        }

        bool _isDrawn = false;
        public bool IsDrawn
        {
            get => _isDrawn;
            set
            {
                _isDrawn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDrawn)));
            }
        }

        bool _notLoading = true;
        public bool NotLoading
        {
            get => _notLoading;
            set
            {
                _notLoading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotLoading)));
            }
        }

        bool ShowRejected { get; set; } = false;

        List<ResultModel>? _resultShowList = null;
        public List<ResultModel>? ResultShowList
        {
            get => _resultShowList;
            set
            {
                _resultShowList = value;
                ResultRefresh();
            }
        }

        public List<ResultModel>? _resultList = null;
        public List<ResultModel>? ResultList
        {
            get => _resultList;
            set
            {
                _resultList = value;
                ResultRefresh();
            }
        }

        List<Retweet>? _retweets = null;
        public List<Retweet>? Retweets {
            get => _retweets;
            set
            {
                _retweets = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Retweets)));
            }
        }


        TweetModel? Tweet = null;
        private void ResultRefresh()
        {
            if (ShowRejected)
            {
                resultViewer.DisplayList = ResultList?.ToList();
                //PropertyChanged?.Invoke(resultViewer, new PropertyChangedEventArgs(nameof(resultViewer.DisplayList)));
            }
            if (!ShowRejected)
            {
                var temp = ResultList?.Where(r => r.Status).ToList();
                resultViewer.DisplayList = temp;
            }
        }

        public List<string> TweetCountOption { get; set; } = new List<string> { "Any", "50+", "100+", "500+", "1000+" };
        public List<string> AccountAgeOption { get; set; } = new List<string> { "Any", "One Week", "One Month", "Six Months", "One Year" };
        public List<string> LatestTweetOption { get; set; } = new List<string> { "Any", "Past Day", "Past Week", "Past Month" };

        public MainWindow()
        {
            DrawSetting = new SettingModel();
            InitializeComponent();
            this.DataContext = this;
        }

        async private void LoadTweet(object sender, RoutedEventArgs e)
        {
            if (txtUrl.Value == null || txtUrl.Value == "")
            {
                MessageBox.Show("Please input site url");
                return;
            }
            NotLoading = false;
            var _tmp = txtUrl.Value.Split('/');
            string _tweetId = _tmp[_tmp.Length - 1];
            Tweet = await API.GetTweet(_tweetId);
            Retweets = await API.GetRetweet(_tweetId);
            if (Tweet != null)
            {
                preview.Tweet = Tweet;
            }
            if (Retweets != null)
            {
                entryView.Retweets = Retweets;
                IsBegun = true;
            }
            NotLoading = true;
        }

        private void GotoTab(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Tag)
            {
                case "ToTab1":
                    tab1.IsSelected = true;
                    break;
                case "ToTab2":
                    tab2.IsSelected = true;
                    break;
                default:
                    tab3.IsSelected = true;
                    break;
            }
        }

        int[] RandomIndexes = new int[0];
        async private void Draw(object sender, RoutedEventArgs e)
        {
            if (Retweets == null || Retweets.Count == 0) return;
            DrawSetting = new SettingModel
            {
                AccountAge = cmbAgeLimit.Value,
                Banner = chkBanner.IsChecked ?? false,
                Description = chkBanner.IsChecked ?? false,
                LatestTweet = cmbLatestLimit.Value,
                Location = chkLocation.IsChecked ?? false,
                MustFollow = txtMustFollow.Value,
                Picture = chkPicture.IsChecked ?? false,
                TweetCount = cmbTweetLimit.Value,
                WinnderCount = txtWinnerCount.Value
            };
            IsDrawn = true;
            DrawDate = DateTime.Now;
            DrawId = ShortId.Generate(IdOption);
            RandomIndexes = Utils.RandomeIndexes(Retweets.Count);
            int _limit = Math.Min(DrawSetting.WinnderCount, Retweets.Count);
            Random rnd = new Random(DateTime.Now.Second);
            int _count = 0;
            if (ResultList == null) ResultList = new List<ResultModel>();
            for (int i = 0; i < Retweets.Count; i++)
            {
                int _index = RandomIndexes[i];
                string result = await Retweets[_index].Draw(DrawSetting);

                ResultList.Add(new ResultModel
                {
                    FiltersMissed = result,
                    Img = Retweets[_index].Img ?? "",
                    Name = Retweets[_index].Name ?? "",
                    Status = (result == ""),
                    UserId = Retweets[_index].Id ?? "",
                    UserName = Retweets[_index].UserName ?? "",
                    RetweetId = Retweets[_index].RetweetId ?? ""
                });
                if (result == "") _count++;
                if (_count >= _limit) break;
            }
            ResultRefresh();
        }

        private void ShowRejectedClick(object sender, RoutedEventArgs e)
        {
            ShowRejected = chkShowRejected.IsChecked ?? false;
            ResultRefresh();
        }

        private async void Reaction(object sender, RoutedEventArgs e)
        {
            if (ResultList == null || Retweets == null) return;
            string id = ((Button)sender).Tag.ToString() ?? "";
            int index = ResultList.FindIndex(r => r.UserId == id);
            ResultList[index].Status = !ResultList[index].Status;
            if (!ResultList[index].Status)
            {
                for(int i = ResultList.Count; i < Retweets.Count; i++)
                {
                    int _index = RandomIndexes[i];
                    string result = await Retweets[_index].Draw(DrawSetting);

                    ResultList.Add(new ResultModel
                    {
                        FiltersMissed = result,
                        Img = Retweets[_index].Img ?? "",
                        Name = Retweets[_index].Name ?? "",
                        Status = (result == ""),
                        UserId = Retweets[_index].Id ?? "",
                        UserName = Retweets[_index].UserName ?? "",
                        RetweetId = Retweets[_index].RetweetId ?? ""
                    });
                    if (result == "") break;
                }
            }
            ResultRefresh();
        }

        private void Reply(object sender, RoutedEventArgs e)
        {
            if (ResultList == null) return;
            string winners = "";
            foreach(var r in ResultList)
            {
                if(r.UserName != null && r.UserName != "")
                {
                    winners += $"%0A{Environment.NewLine}• @{r.UserName}";
                }
            }
            Process.Start(new ProcessStartInfo
            {
                FileName = $@"https://twitter.com/intent/tweet?in_reply_to={Tweet?.Data?.Id}&text=Winners 🏆{winners}",
                UseShellExecute = true,
            });
        }
    }
}
