using System;

namespace DataProvider.Entities
{
    public class ProjectUser
    {
        public string Role { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
