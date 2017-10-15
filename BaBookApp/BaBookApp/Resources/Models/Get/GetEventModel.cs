using Newtonsoft.Json;
using System;

namespace AndroidApp.Resources.Models
{
    public class GetEventModel
    {
        [JsonProperty("eventId")]
        public int EventId { get; set; }
        [JsonProperty("creatorName")]
        public string OwnerName { get; set; }
        [JsonProperty("groupName")]
        public string GroupName { get; set; }
        [JsonProperty("date")]
        public DateTime DateOfOccurance { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("comment")]
        public string Description { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("status")]
        public int AttendanceStatus { get; set; }
    }
}