using System;

namespace DataProvider.Entities
{
    public class ProjectUser
    {
        public int Role { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
