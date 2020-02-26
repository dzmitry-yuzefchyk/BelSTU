using System.Collections.Generic;

namespace BusinessLogic.Models.Project
{
    public class UpdatePoliciesModel
    {
        public IEnumerable<ProjectUserModel> Users { get; set; }
        public int ProjectId { get; set; }
    }
}
