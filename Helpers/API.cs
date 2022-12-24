using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TweetDraw.Model;

namespace TweetDraw.Helpers
{
    internal class API
    {
        const string PARM_GET_TWEET
            = "tweet.fields=author_id,created_at,entities,id,lang,public_metrics,source,text&expansions=attachments.poll_ids,attachments.media_keys,author_id,geo.place_id,in_reply_to_user_id,referenced_tweets.id,entities.mentions.username,referenced_tweets.id.author_id&user.fields=location,name,profile_image_url,username";
        const string PARM_GET_RETWEET
            = "user.fields=location,created_at,profile_image_url,pinned_tweet_id,public_metrics,description";
        const string PARM_GET_FOLLOW
            = "user.fields=username";

        internal static async Task<TweetModel?> GetTweet(string id)
        {
            string _url = $"tweets/{id}?{PARM_GET_TWEET}";
            var res = JsonConvert.DeserializeObject<TweetModel>(await Helpers.API.TwitterAPI(_url));
            return res;
        }

        internal static async Task<List<Retweet>?> GetRetweet(string id)
        {
            string _url = $"tweets/{id}/retweeted_by?{PARM_GET_RETWEET}";
            var res = JsonConvert.DeserializeObject<RetweetResponse>(await Helpers.API.TwitterAPI(_url));
            return (res != null) ? res.Data : null;
        }

        internal static async Task<List<Follower>?> GetFollowers(string id)
        {
            string _url = $"users/{id}/followers?{PARM_GET_FOLLOW}";
            var res = JsonConvert.DeserializeObject<FollowerResponse>(await TwitterAPI(_url));
            return (res != null) ? res.Data : null;
        }

        private static async Task<string> TwitterAPI(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setting.Default.Token);
                HttpResponseMessage response;
                response = await client.GetAsync($"{Setting.Default.EndPoint}{url}");
                return await response.Content.ReadAsStringAsync();
            }
        }        
    }
}
