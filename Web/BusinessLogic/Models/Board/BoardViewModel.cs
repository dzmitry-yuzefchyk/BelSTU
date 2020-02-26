using System.Collections.Generic;

namespace BusinessLogic.Models.Board
{
    public class BoardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TaskPrefix { get; set; }
        public int TaskIndex { get; set; }
        public IEnumerable<ColumnView> Columns { get; set; }
        public bool CanAddColumn { get; set; }
        public bool CanCreateTask { get; set; }
    }

    public class ColumnView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
        public IEnumerable<TaskView> Tasks { get; set; }
    }

    public class TaskView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public int Severity { get; set; }
        public string AssigneeTag { get; set; }
        public string AssigneeIcon { get; set; }
        public string CreatorTag { get; set; }
        public string CreatorIcon { get; set; }
    }
}
