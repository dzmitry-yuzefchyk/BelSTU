using System;
using System.Collections.Generic;

namespace DataProvider.Entities
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Column> Columns { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public BoardSettings Settings { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; }
        public string TaskPrefix { get; set; }
        public int TaskIndex { get; set; }

        public Board()
        {
            Columns = new List<Column>();
        }
    }
}
