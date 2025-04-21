using Backend.Challenge._1._Common.Contracts.Enum;
using System;
using System.Collections.Generic;

namespace Backend.Challenge.Data.Models
{
    public class StatusUpdate
    {
        public string Id { get; set; }
        public string IdeaId { get; set; }
        public string Title { get; set; }
        public string SummaryHtml { get; set; }
        public DateTime PublishedAtUtc { get; set; }
        public EStatus Status { get; set; }
        public string AuthorId { get; set; }
        public Dictionary<string, DateTime> SeenByUserIds { get; set; } = new Dictionary<string, DateTime>();
    }
}
