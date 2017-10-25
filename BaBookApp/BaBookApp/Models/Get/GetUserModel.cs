using Newtonsoft.Json;

namespace BaBookApp.Resources.Models.Get
{
    public class GetUserModel
    {
        [JsonProperty("id")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string Username { get; set; }
    }
}