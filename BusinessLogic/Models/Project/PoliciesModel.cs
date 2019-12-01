using BusinessLogic.Contstants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Models.Project
{
    public class PoliciesModel
    {
        public IEnumerable<ProjectUserModel> Users { get; set; }
        public int Total { get; set; }
    }

    public class ProjectUserModel
    {
        public string Email { get; set; }
        public string Tag { get; set; }
        public List<Actions> Actions { get; set; }
        public bool ChangingBlocked { get; set; }
    }

    public class Actions
    {
        public UserAction Action { get; set; }
        public bool Allowed { get; set; }
    }
}
