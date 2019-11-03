using System;

namespace DataProvider.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
        public User Recipient { get; set; }
        public bool IsDeliveredOverEmail { get; set; }
        public bool IsDeliveredOverWeb { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string DirectLink { get; set; }
    }
}
