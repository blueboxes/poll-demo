using Newtonsoft.Json;

namespace Api.Models
{

    public class PollResult
    {
        [JsonProperty("answerId")]
        public int AnswerId { get; set; }

        [JsonProperty("percentage")]
        public int? Percentage { get; set; }
    }
}
