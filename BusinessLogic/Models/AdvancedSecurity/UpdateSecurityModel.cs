using System.Collections.Generic;

namespace BusinessLogic.AdvancedSecurity
{
    public class UpdateSecurityModel
    {
        public int ProjectId { get; set; }
        public List<ProjectPolicy> Policies { get; set; }
    }

    public class ProjectPolicy
    {
        public bool UseSecretKey { get; set; }
        public int Action { get; set; }
        public bool IsAllowed { get; set; }
        public string UserEmail { get; set; }
    }
}
