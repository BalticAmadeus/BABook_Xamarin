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
        public int Status { get; set; }
    }
}