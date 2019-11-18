using System;

namespace BusinessLogic.Models.Notification
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string DirectLink { get; set; }
    }
}
