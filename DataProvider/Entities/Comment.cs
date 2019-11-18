using System;
using System.Collections.Generic;

namespace DataProvider.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<CommentAttachment> Attachments { get; set; }

        public Comment()
        {
            Attachments = new List<CommentAttachment>();
        }
    }
}
