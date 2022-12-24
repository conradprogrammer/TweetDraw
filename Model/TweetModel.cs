using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TweetDraw.Model
{
    public class TweetModel
    {
        public Datum? Data { get; set; }
        public Include? Includes { get; set; }
        public class Datum
        {
            public string? Id { get; set; }
            [JsonProperty("lang")] public string? Language { get; set; }
            public Entity? Entities { get; set; }
            public string? Text { get; set; }
            [JsonProperty("author_id")] public string? AuthorId { get; set; }
            [JsonProperty("created_at")] public DateTime Created { get; set; }
            public string? Source { get; set; }
            [JsonProperty("public_metrics")] public PublicMetrics? Metrics { get; set; } = new PublicMetrics();
            public class PublicMetrics
            {
                [JsonProperty("retweet_count")] public int RetweetCount { get; set; }
                [JsonProperty("reply_count")] public int ReplyCount { get; set; }
                [JsonProperty("like_count")] public int LikeCount { get; set; }
                [JsonProperty("quote_count")] public int QuoteCount { get; set; }
            }
            public class Entity
            {
                public List<URL> Urls { get; set; } = new List<URL>();
                public class URL
                {
                    public string? Url { get; set; }
                    public string? Title { get; set; }
                    public string? Description { get; set; }
                    public List<Image> Images { get; set; } = new List<Image>();
                    public class Image
                    {
                        public string? Url { get; set; }
                        public int Width { get; set; }
                        public int Height { get; set; }
                    }
                }
            }
        }
        public class Include
        {
            public List<User>? Users { get; set; }
            public class User
            {
                public string? Location { get; set; }
                public string? Id { get; set; }
                public string? Name { get; set; }
                public string? UserName { get; set; }
                [JsonProperty("profile_image_url")]public string? Img { get; set; }
            }
        }
    }
}
