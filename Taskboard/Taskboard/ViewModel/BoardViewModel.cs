using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using Taskboard.View;

namespace Taskboard.ViewModel
{
    public class BoardTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
    }

    public class BoardViewModel : INotifyPropertyChanged
    {
        private int pageSize = 20;
        private int boardId;
        private readonly int teamId;

        private List<BoardTask> tasks = new List<BoardTask>();
        private BoardTask selectedTask;

        private int currentPage = 0;

        private string searchQuery = "";
        #region props
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                NotifyPropertyChanged("SearchQuery");

                if (searchQuery != "")
                {
                    SearchForTasks();
                }
            }
        }
        public BoardTask SelectedTask
        {
            get => selectedTask;
            set
            {
                selectedTask = value;
                NotifyPropertyChanged("SelectedTask");
            }
        }
        public List<BoardTask> Tasks
        {
            get => tasks;
            set
            {
                tasks = value;
                NotifyPropertyChanged("Tasks");
            }
        }
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

        #endregion

        public RelayCommand GetTaskWindowCommand { get; set; }
        public RelayCommand CreateTaskCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }
        public RelayCommand ReloadPageCommand { get; set; }

        public BoardViewModel(int boardId, int teamId)
        {
            this.boardId = boardId;
            this.teamId = teamId;

            GetTaskWindowCommand = new RelayCommand(GetEditTaskWindow);
            CreateTaskCommand = new RelayCommand(GetTaskWindow);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            ReloadPageCommand = new RelayCommand(ReloadPage);

            GetTasks();
        }

        public void ReloadPage()
        {
            if (SearchQuery != "")
            {
                SearchForTasks();
            }

            else
            {
                GetTasks();
            }
        }
        public void NextPage()
        {
            CurrentPage += 1;
            ReloadPage();
        }
        public void PreviousPage()
        {
            CurrentPage -= CurrentPage == 0 ? 0 : 1;
            ReloadPage();
        }
        public async void GetTasks()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", this.teamId);
                SqlParameter boardId = new SqlParameter("@boardId", BoardId);
                SqlParameter skip = new SqlParameter("@skip", currentPage * pageSize);
                SqlParameter take = new SqlParameter("@take", pageSize);

                Tasks.Clear();
                Tasks = await db.Database.SqlQuery<BoardTask>("[Task.Get] @userId, @teamId, @boardId, @skip, @take", new[] { userId, teamId, boardId, skip, take }).ToListAsync();
            }
        }
        public async void SearchForTasks()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", this.teamId);
                SqlParameter boardId = new SqlParameter("@boardId", BoardId);
                SqlParameter searchQuery = new SqlParameter("@searchQuery", SearchQuery);
                SqlParameter skip = new SqlParameter("@skip", currentPage * pageSize);
                SqlParameter take = new SqlParameter("@take", pageSize);

                Tasks.Clear();
                Tasks = await db.Database.SqlQuery<BoardTask>("[Task.Search] @userId, @teamId, @boardId, @searchQuery, @skip, @take", new[] { userId, teamId, boardId, searchQuery, skip, take }).ToListAsync();
            }
        }
        public void GetEditTaskWindow()
        {
            TaskEditWindow window = new TaskEditWindow(teamId, BoardId, SelectedTask);
            window.Show();
        }
        public void GetTaskWindow()
        {
            TaskView window = new TaskView(teamId, BoardId);
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
