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

namespace BaBookApp.Resources.Models
{
    class PostEventModel
    {
        public int GroupId { get; set; }
        public int OwnerId { get; set; }
        public string Title { get; set; }
        public DateTime DateOfOccurance { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}