using System;
using System.Collections.Generic;

namespace BusinessLogic.Models.Task
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public int Severity { get; set; }
        public string AssigneeTag { get; set; }
        public string AssigneeIcon { get; set; }
        public string CreatorTag { get; set; }
        public string CreatorIcon { get; set; }
        public bool CanUpdateTask { get; set; }
        public bool CanComment { get; set; }
        public IEnumerable<AttachmentView> Attachments { get; set; }
        public IEnumerable<CommentView> Comments { get; set; }
    }

    public class AttachmentView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
    }

    public class CommentView
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreatorTag { get; set; }
        public string CreatorIcon { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<AttachmentView> Attachments { get; set; }
    }
}
