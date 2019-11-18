using System.Collections.Generic;

namespace DataProvider.Entities
{
    public class ProjectSecuritySettings
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public string SecretKey { get; set; }
        public ICollection<ProjectSecurityPolicy> Policies { get; set; }

        public ProjectSecuritySettings()
        {
            Policies = new List<ProjectSecurityPolicy>();
        }
    }
}
