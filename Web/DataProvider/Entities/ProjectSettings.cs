using System.Collections.Generic;

namespace DataProvider.Entities
{
    public class ProjectSettings
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public string Preview { get; set; }
        public bool UseAdvancedSecuritySettings { get; set; }
        public int AccessToChangeProject { get; set; }
        public int AccessToChangeBoard { get; set; }
        public int AccessToChangeTask { get; set; }
        public ICollection<ProjectSecurityPolicy> Policies { get; set; }

        public ProjectSettings()
        {
            Policies = new List<ProjectSecurityPolicy>();
        }
    }
}
