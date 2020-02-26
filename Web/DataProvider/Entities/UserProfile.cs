using System;

namespace DataProvider.Entities
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Icon { get; set; }
    }
}
