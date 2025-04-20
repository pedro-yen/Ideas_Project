using Backend.Challenge._1._Common.Contracts.Enum;
using System;
using System.Collections.Generic;

namespace Backend.Challenge._1._Common.Contracts.Requests.Idea
{
    public class CreateIdeiaRequest
    {
        public string Title { get; set; }
        public string SummaryHtml { get; set; }
        public string AuthorId { get; set; }
        public DateTime PublishedAt { get; set; }
        public EStatus Status { get; set; }
        public List<string> UserIds { get; set; }
    }
}

