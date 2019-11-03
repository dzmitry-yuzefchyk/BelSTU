using System;

namespace DataProvider.Entities
{
    public class UserSettings
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public bool EmailNotifications { get; set; }
        public string Theme { get; set; }
        public string Icon { get; set; }
    }
}
