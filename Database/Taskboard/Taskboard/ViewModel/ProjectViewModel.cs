using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskboard.View;

namespace Taskboard.ViewModel
{
    public class ProjectBoard
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    class ProjectViewModel : INotifyPropertyChanged
    {
        private int pageSize = 20;
        private int projectId;
        private int teamId;

        private List<ProjectBoard> boards = new List<ProjectBoard>();
        private ProjectBoard selectedBoard;

        private string boardTitle = "MyBoard";
        private int boardId = 0;

        private int currentPage = 0;
        #region props
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }
        public int BoardId
        {
            get => boardId;
            set
            {
                boardId = value;
                NotifyPropertyChanged("BoardId");
            }
        }
        public string BoardTitle
        {
            get => boardTitle;
            set
            {
                boardTitle = value;
                NotifyPropertyChanged("BoardTitle");
            }
        }

        public ProjectBoard SelectedBoard
        {
            get => selectedBoard;
            set
            {
                selectedBoard = value;
                NotifyPropertyChanged("SelectedBoard");
            }
        }

        public List<ProjectBoard> Boards
        {
            get => boards;
            set
            {
                boards = value;
                NotifyPropertyChanged("Boards");
            }
        }
        public int ProjectId
        {
            get => projectId;
            set
            {
                projectId = value;
                NotifyPropertyChanged("ProjectId");
            }
        }
        #endregion

        public RelayCommand GetBoardWindowCommand { get; set; }
        public RelayCommand CreateBoardCommand { get; set; }
        public RelayCommand DeleteBoardCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }

        public ProjectViewModel(int projectId, int teamId)
        {
            this.projectId = projectId;
            this.teamId = teamId;

            GetBoardWindowCommand = new RelayCommand(GetBoardWindow);
            CreateBoardCommand = new RelayCommand(CreateBoard);
            DeleteBoardCommand = new RelayCommand(DeleteBoard);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);

            GetBoards();
        }
        public void GetBoards()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", this.teamId);
                SqlParameter projectId = new SqlParameter("@projectId", ProjectId);
                SqlParameter skip = new SqlParameter("@skip", currentPage * pageSize);
                SqlParameter take = new SqlParameter("@take", pageSize);

                Boards.Clear();
                Boards = db.Database.SqlQuery<ProjectBoard>("[Board.Get] @userId, @teamId, @projectId, @skip, @take", new[] { userId, teamId, projectId, skip, take }).ToList();
            }
        }

        public void NextPage()
        {
            CurrentPage += 1;
            GetBoards();
        }
        public void PreviousPage()
        {
            CurrentPage -= CurrentPage == 0 ? 0 : 1;
            GetBoards();
        }
        public void DeleteBoard()
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }

            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter boardId = new SqlParameter("@boardId", BoardId);
                SqlParameter teamId = new SqlParameter("@teamId", this.teamId);

                var result = db.Database.ExecuteSqlCommand("[Board.Delete] @userId, @boardId, @teamId", new[] { userId, boardId, teamId });
                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    GetBoards();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }
        public void CreateBoard()
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }

            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter title = new SqlParameter("@title", BoardTitle);
                SqlParameter projectId = new SqlParameter("@projectId", ProjectId);
                SqlParameter teamId = new SqlParameter("@teamId", this.teamId);

                var result = db.Database.ExecuteSqlCommand("[Board.Create] @userId, @title, @projectId, @teamId", new[] { userId, title, projectId, teamId });
                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    GetBoards();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }
        public void GetBoardWindow()
        {
            BoardView window = new BoardView(SelectedBoard.Id, teamId);
            window.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
