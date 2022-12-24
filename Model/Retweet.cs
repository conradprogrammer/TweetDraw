using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetDraw.Helpers;

namespace TweetDraw.Model
{
    public class RetweetResponse
    {
        public List<Retweet>? Data { get; set; }
    }
    public class Retweet
    {
        public string? Id { get; set; }
        [JsonProperty("created_at")] public DateTime? Created { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        [JsonProperty("profile_image_url")] public string? Img { get; set; }
        [JsonProperty("public_metrics")] public PublicMetrics? Metrics { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        [JsonProperty("pinned_tweet_id")]public string? RetweetId { get; set; }
        public class PublicMetrics
        {
            [JsonProperty("followers_count")] public int FollowersCount { get; set; }
            [JsonProperty("following_count")] public int FollowingCount { get; set; }
            [JsonProperty("tweet_count")] public int TweetCount { get; set; }
        }

        internal bool Match(string key)
        {
            if ((Name ?? "").Contains(key)) return true;
            if ((UserName ?? "").Contains(key)) return true;
            if ((Description ?? "").Contains(key)) return true;
            if ((Location ?? "").Contains(key)) return true;
            return false;
        }

        public bool Drawed { get; private set; }
        async public Task<string> Draw(SettingModel drawSetting)
        {
            // AccountAge { "Any", "One Week", "One Month", "Six Months", "One Year" };
            Drawed = true;
            List<string> result = new List<string>();
            if (drawSetting.Picture && (Img == null || Img == "")) result.Add("Picture");
            if (drawSetting.AccountAge > 0 && Created != null)
            {
                int timeSpan = (DateTime.Now - Created).Value.Days;
                switch (drawSetting.AccountAge)
                {
                    case 1:
                        if (timeSpan < 7) result.Add("Age");
                        break;
                    case 2:
                        if (timeSpan < 30) result.Add("Age");
                        break;
                    case 3:
                        if (timeSpan < 180) result.Add("Age");
                        break;
                    default:
                        if (timeSpan < 365) result.Add("Age");
                        break;
                }
            }
            if (drawSetting.MustFollow != null && drawSetting.MustFollow != "")
            {
                var followers = await API.GetFollowers(Id ?? "");
                if(followers != null)
                {
                    var temp = drawSetting.MustFollow.Split(',');
                    for(int i = 0; i < temp.Count(); i++)
                    {
                        if (followers.Find(f => f.UserName == temp[i]) == null)
                            result.Add($"Follow:{temp[i]}");
                    }
                }
                else
                {
                    result.Add("Follow");
                }
            }
            if (drawSetting.Location && (Location == null || Location == "")) result.Add("Location");
            if (drawSetting.Description && (Description == null || Description == "")) result.Add("Description");
            return String.Join(",", result);
        }
    }
}
