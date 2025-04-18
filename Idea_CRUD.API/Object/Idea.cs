using System;

namespace Backend.Challenge.Object
{
    public class Idea
    {
        public string Id { get; set; } // RavenDB auto-generates like "ideas/1-A"
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
