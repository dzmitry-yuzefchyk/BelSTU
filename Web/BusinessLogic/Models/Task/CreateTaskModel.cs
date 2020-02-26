using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BusinessLogic.Models.Task
{
    public class CreateTaskModel
    {
        public int ColumnId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public int Severity { get; set; }
        public string AssigneeEmail { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
