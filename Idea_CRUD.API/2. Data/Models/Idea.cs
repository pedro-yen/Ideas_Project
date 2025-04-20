using System;
using System.Collections.Generic;

namespace Backend.Challenge.Data.Models
{
    public class Idea
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public List<string> UserIds { get; set; } = [];
        public string LatestStatus { get; set; }
        public DateTime PublishedAtUtc { get; set; }
    }
}
