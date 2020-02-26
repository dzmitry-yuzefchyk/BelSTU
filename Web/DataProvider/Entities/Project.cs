using System.Collections.Generic;

namespace DataProvider.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Board> Boards { get; set; }
        public ICollection<ProjectUser> Users { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ProjectSettings Settings { get; set; }

        public Project()
        {
            Boards = new List<Board>();
            Users = new List<ProjectUser>();
            Activities = new List<Activity>();
        }
    }
}
