using Newtonsoft.Json;

namespace Api.Models
{
    public class Connection
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }
    }

}
