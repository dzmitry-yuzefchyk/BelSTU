using System;

namespace DataProvider.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public Guid RecipientId { get; set; }
        public User Recipient { get; set; }
        public bool IsDelivered { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string DirectLink { get; set; }
    }
}
