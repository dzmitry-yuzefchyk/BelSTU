namespace BusinessLogic.Models.Notification
{
    public class CreateNotificationModel
    {
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string DirectLink { get; set; }
    }
}
