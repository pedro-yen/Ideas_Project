using System;

namespace Backend.Challenge._2._Data.Models
{
    public class StatusUpdateView
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string StatusUpdateId { get; set; }
        public DateTime ViewedAtUtc { get; set; }
    }
}
