namespace DataProvider.Entities
{
    public class CommentAttachment
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
