using System;

namespace DataProvider.Entities
{
    public class TaskAttachment
    {
        public Guid Id { get; set; }
        public Task Task { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string AttachedFilePath { get; set; }
    }
}
