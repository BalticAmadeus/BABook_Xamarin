﻿using System;
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

namespace BaBookApp.Resources.Models
{
    public class PutEventModel
    {
        [JsonProperty("eventId")]
        public int EventId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("date")]
        public DateTime DateOfOccurance { get; set; }
        [JsonProperty("comment")]
        public string Description { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
    }
}