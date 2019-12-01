using System;
using System.Collections.Generic;

namespace DataProvider.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public int Severity { get; set; }
        public Guid? AssigneeId { get; set; }
        public User Assignee { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; }
        public int ColumnId { get; set; }
        public ICollection<TaskAttachment> Attachments { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public Task()
        {
            Attachments = new List<TaskAttachment>();
            Comments = new List<Comment>();
        }
    }
}
