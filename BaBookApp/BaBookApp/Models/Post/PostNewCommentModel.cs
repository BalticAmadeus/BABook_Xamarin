using Newtonsoft.Json;

namespace BaBookApp.Resources.Models.Post
{
    internal class PostNewCommentModel
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("comment")]
        public string CommentText { get; set; }
    }
}