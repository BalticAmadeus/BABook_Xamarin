using System;

namespace AndroidApp.Resources.Models
{
    public class GetEventModel
    {
        //public int EventId { get; set; }
        //public string OwnerName { get; set; }
        //public string GroupName { get; set; }
        //public DateTime Date { get; set; }
        //public string Title { get; set; }
        //public string Description { get; set; }
        //public string Location { get; set; }
        //public Enums.EventResponse AttendanceStatus { get; set; }

        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime DateOfOccurance { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}