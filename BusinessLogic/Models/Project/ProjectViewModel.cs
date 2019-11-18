namespace BusinessLogic.Models.Project
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool CanUserChangeProject { get; set; }
        public bool CanUserCreateBoard { get; set; }
    }
}
