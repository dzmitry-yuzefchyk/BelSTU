namespace BusinessLogic.Models.Project
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
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
    }
}
