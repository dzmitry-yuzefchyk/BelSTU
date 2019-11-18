using System;

namespace DataProvider.Entities
{
    public class ProjectSecurityPolicy
    {
        public bool UseSecretKey { get; set; }
        public string Action { get; set; }
        public bool IsAllowed { get; set; }
        public int ProjectSecuritySettingsId { get; set; }
        public ProjectSecuritySettings ProjectSecuritySettings { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
