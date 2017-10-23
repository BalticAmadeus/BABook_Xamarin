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
    public class PostAttendenceModel
    {
            [JsonProperty("eventId")]
            public int EventId { get; set; }

            [JsonProperty("userId")]
            public string UserId { get; set; }

            [JsonProperty("status")]
            public int  Status { get; set; }
    }
}