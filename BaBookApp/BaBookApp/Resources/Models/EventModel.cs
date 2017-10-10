using System;

namespace AndroidApp.Resources.Models
{
    public class EventViewModel
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime DateOfOccurance { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}