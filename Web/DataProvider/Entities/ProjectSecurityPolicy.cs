using System;

namespace DataProvider.Entities
{
    public class ProjectSecurityPolicy
    {
        public int Id { get; set; }
        public int Action { get; set; }
        public bool IsAllowed { get; set; }
        public int ProjectSettingsId { get; set; }
        public ProjectSettings ProjectSettings { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
