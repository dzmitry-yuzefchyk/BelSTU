using System;

namespace DataProvider.Entities
{
    public class Activity
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
