using Backend.Challenge._1._Common.Contracts.Enum;
using System.Collections.Generic;
using System;

namespace Backend.Challenge._1._Common.Contracts.Requests.Idea
{
    public class CreateStatusUpdateRequest
    {
        public string? Title { get; set; }
        public string? SummaryHtml { get; set; }
        public string? AuthorId { get; set; }
        public EStatus? Status { get; set; }
        public List<string>? UserIds { get; set; }
    }
}
