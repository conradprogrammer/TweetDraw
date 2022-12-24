using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetDraw.Model
{
    public class ResultModel
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Img { get; set; } = "";
        public bool Status { get; set; } = false;
        public string FiltersMissed { get; set; } = "None";
        public string RetweetId { get; set; } = "";
    }
}
