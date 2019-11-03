using System;

namespace DataProvider.Entities
{
    public class ProjectSettings
    {
        public Guid Id { get; set; }
        public Project Project { get; set; }
        public string PathToBackground { get; set; }
        public bool UseAdvancedSecuritySettings { get; set; }
        public int AccessToDeleteBoard { get; set; }
        public int AccessToChangeProject { get; set; }
        public int AccessToCreateBoard { get; set; }
    }
}
