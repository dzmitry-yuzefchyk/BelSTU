using System;
using System.Collections.Generic;

namespace DataProvider.Entities
{
    public class Column
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public int Position { get; set; }
        public Guid BoardId { get; set; }
        public Board Board { get; set; }

        public Column()
        {
            Tasks = new List<Task>();
        }
    }
}
