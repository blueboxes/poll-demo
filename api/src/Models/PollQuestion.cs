using System.Collections.Generic;

namespace Api.Models
{
    public class PollQuestion
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }

        public List<PollOption> Options { get; set; } = new List<PollOption>();

        public List<PollResult> Results { get; set; } = new List<PollResult>();
    }

}
