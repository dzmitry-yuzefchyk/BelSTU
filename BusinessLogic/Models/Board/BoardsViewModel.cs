using System.Collections.Generic;

namespace BusinessLogic.Models.Board
{
    public class BoardsViewModel
    {
        public IEnumerable<BoardDetails> Boards { get; set; }
        public int Total { get; set; }
    }

    public class BoardDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }

    }
}
