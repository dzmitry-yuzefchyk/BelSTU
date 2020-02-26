using System.Collections.Generic;

namespace BusinessLogic.Models.Project
{
    public class ProjectAccessViewModel : ProjectViewModel
    {
        public bool CanUpdateProject { get; set; }
        public bool CanChangeSecurity { get; set; }
        public bool CanDeleteProject { get; set; }
        public bool CanCreateBoard { get; set; }
        public bool CanDeleteBoard { get; set; }
        public bool CanUpdateBoard { get; set; }
        public bool CanCreateTask { get; set; }
        public bool CanUpdateTask { get; set; }
        public bool CanDeleteTask { get; set; }
        public bool CanComment { get; set; }
        public bool CanAddBoard { get; set; }
        public IEnumerable<BoardView> Boards { get; set; }
    }

    public class BoardView
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
