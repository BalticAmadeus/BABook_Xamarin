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

namespace BaBookApp.Resources.Models.Get
{
    public class GetGroupModel
    {
            [JsonProperty("groupId")]
            public int GroupId { get; set; }
            [JsonProperty("name")]
            public string GroupName { get; set; }
    }
}