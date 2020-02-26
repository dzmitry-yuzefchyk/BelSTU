using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DataProvider.Entities
{
    public class User : IdentityUser<Guid>
    {
        public UserSettings Settings { get; set; }
        public UserProfile Profile { get; set; }
        public ICollection<Board> Boards { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<ProjectUser> Projects { get; set; }
        public ICollection<ProjectSecurityPolicy> Policies { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
        public ICollection<Task> CreatedTasks { get; set; }
        public ICollection<Activity> Activities { get; set; }

        public User()
        {
            Boards = new List<Board>();
            Comments = new List<Comment>();
            Notifications = new List<Notification>();
            Projects = new List<ProjectUser>();
            Policies = new List<ProjectSecurityPolicy>();
            AssignedTasks = new List<Task>();
            CreatedTasks = new List<Task>();
            Activities = new List<Activity>();
        }
    }
}
