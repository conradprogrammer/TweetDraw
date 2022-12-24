using System.Collections.Generic;

namespace TweetDraw.Model
{
    public class FollowerResponse
    {
        public List<Follower>? Data { get; set; }
    }
    public class Follower
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
    }
}
