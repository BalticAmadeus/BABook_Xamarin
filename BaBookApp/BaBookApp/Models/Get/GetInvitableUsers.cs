using Newtonsoft.Json;

namespace BaBookApp.Resources.Models.Get
{
    public class GetInvitableUsers
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}