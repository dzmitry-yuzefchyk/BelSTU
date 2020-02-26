namespace DataProvider.Entities
{
    public class TaskAttachment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }
    }
}
