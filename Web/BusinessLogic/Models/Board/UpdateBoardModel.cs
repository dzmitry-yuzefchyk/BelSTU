using System.Collections.Generic;

namespace BusinessLogic.Models.Board
{
    public class UpdateBoardModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Prefix { get; set; }
        public IEnumerable<ColumnInfo> Columns { get; set; }
    }

    public class ColumnInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
    }
}
