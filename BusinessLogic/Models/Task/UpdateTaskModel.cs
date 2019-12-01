namespace BusinessLogic.Models.Task
{
    public class UpdateTaskModel
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public int Severity { get; set; }
        public string AssigneeEmail { get; set; }
    }
}
