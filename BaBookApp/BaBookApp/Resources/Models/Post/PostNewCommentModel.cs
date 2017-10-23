using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace BaBookApp.Resources.Models.Post
{
    class PostNewCommentModel
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("comment")]
        public string CommentText { get; set; }
    }
}