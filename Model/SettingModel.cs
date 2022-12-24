using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetDraw.Model
{
    public class SettingModel
    {
        public int WinnderCount { get; set; } = 1;
        public string MustFollow { get; set; } = "";
        public bool Description { get; set; } = true;
        public bool Picture { get; set; } = true;
        public bool Banner { get; set; } = true;
        public bool Location { get; set; } = false;
        public int AccountAge { get; set; } = 2;
        public int LatestTweet { get; set; } = 3;
        public int TweetCount { get; set; } = 1;
    }
    
}
