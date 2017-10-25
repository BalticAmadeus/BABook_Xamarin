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