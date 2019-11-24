using BusinessLogic.Contstants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Models.Project
{
    public class PoliciesModel
    {
        public IEnumerable<Task<ProjectUserModel>> Users { get; set; }
        public int Total { get; set; }
    }

    public class ProjectUserModel
    {
        public string Email { get; set; }
        public string Tag { get; set; }
        public Dictionary<UserAction, bool> Actions { get; set; }
    }
}
