using System;

namespace Backend.Challenge.Data.Models
{
    public class StatusUpdate
    {
        public string Id { get; set; } // RavenDB auto-generates if you leave this null
        public string IdeaId { get; set; }
        public string Title { get; set; }
        public string SummaryHtml { get; set; }
        public string Author { get; set; }
        public DateTime PublishedAtUtc { get; set; }
        public string Status { get; set; } // e.g. "on track", "at risk", "off track"
    }
}
