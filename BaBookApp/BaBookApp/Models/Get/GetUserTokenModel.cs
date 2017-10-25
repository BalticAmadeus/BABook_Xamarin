using Newtonsoft.Json;

namespace BaBookApp.Resources.Models.Get
{
    public class GetUserTokenModel
    {
        [JsonProperty("access_token")]
        public string UserAccessToken { get; set; }
    }
}