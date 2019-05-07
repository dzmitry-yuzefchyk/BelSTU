using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskboard.ViewModel
{
    public class TaskViewModel
    {
        private int teamId;
        private int boardId;

        public TaskViewModel(int teamId, int boardId)
        {
            this.teamId = teamId;
            this.boardId = boardId;
        }
    }
}
