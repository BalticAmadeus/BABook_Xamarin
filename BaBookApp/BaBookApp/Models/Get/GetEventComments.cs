using Newtonsoft.Json;

namespace BaBookApp.Resources.Models.Get
{
    public class GetEventComments
    {
        [JsonProperty("name")]
        public string OwnerUser { get; set; }

        [JsonProperty("comment")]
        public string Text { get; set; }
    }
}