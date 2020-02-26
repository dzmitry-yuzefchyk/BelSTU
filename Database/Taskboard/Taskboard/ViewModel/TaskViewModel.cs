using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Taskboard.ViewModel
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private readonly int teamId;
        private readonly int boardId;

        private int currentPage = 0;
        private readonly int pageSize = 3;

        private string taskTitle = "Title";
        private string taskContent = "MyContent";

        public string[] Types { get; set; } = { "BUG", "FEATURE", "IMPROVEMENT" };
        public string[] Priorities { get; set; } = { "LOW", "MEDIUM", "HIGH", "URGENT" };
        public string[] Severities { get; set; } = { "LOW", "NORMAL", "CRITICAL", "BLOCKER" };

        private string selectedType;
        private string selectedPriority;
        private string selectedSeverity;

        private TeamUser selectedAssignee;
        private List<TeamUser> assignees = new List<TeamUser>();
        private DateTime finishTime = DateTime.Now;
        private string filePath;

        #region props
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }
        public DateTime FinishTime
        {
            get => finishTime;
            set
            {
                finishTime = value;
                NotifyPropertyChanged("FinishTime");
            }
        }
        public List<TeamUser> Assignees
        {
            get => assignees;
            set
            {
                assignees = value;
                NotifyPropertyChanged("Assignees");
            }
        }
        public TeamUser SelectedAssignee
        {
            get => selectedAssignee;
            set
            {
                selectedAssignee = value;
                NotifyPropertyChanged("SelectedAssignee");
            }
        }
        public string SelectedSeverity
        {
            get => selectedSeverity;
            set
            {
                selectedSeverity = value;
                NotifyPropertyChanged("SelectedSeverity");
            }
        }
        public string SelectedPriority
        {
            get => selectedPriority;
            set
            {
                selectedPriority = value;
                NotifyPropertyChanged("SelectedPriority");
            }
        }
        public string SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                NotifyPropertyChanged("SelectedType");
            }
        }
        public string TaskTitle
        {
            get => taskTitle;
            set
            {
                taskTitle = value;
                NotifyPropertyChanged("TaskTitle");
            }
        }

        public string TaskContent
        {
            get => taskContent;
            set
            {
                taskContent = value;
                NotifyPropertyChanged("TaskContent");
            }
        }
        #endregion

        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }
        public RelayCommand ChooseFileCommand { get; set; }
        public RelayCommand<Window> CreateTaskCommand { get; set; }

        public TaskViewModel(int teamId, int boardId)
        {
            this.teamId = teamId;
            this.boardId = boardId;

            selectedType = Types[0];
            selectedPriority = Priorities[0];
            selectedSeverity = Severities[0];

            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            CreateTaskCommand = new RelayCommand<Window>(CreateTask);
            ChooseFileCommand = new RelayCommand(ChooseFile);

            GetAssignees();
        }
        public void ChooseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog.FileName;
            }
        }
        public void CreateTask(Window window)
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }

            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@creatorId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", this.teamId);
                SqlParameter boardId = new SqlParameter("@boardId", this.boardId);
                SqlParameter title = new SqlParameter("@title", TaskTitle);
                SqlParameter content = new SqlParameter("@content", TaskContent);
                SqlParameter type = new SqlParameter("@type", SelectedType);
                SqlParameter priority = new SqlParameter("@priority", SelectedPriority);
                SqlParameter severity = new SqlParameter("@severity", SelectedSeverity);
                SqlParameter status = new SqlParameter("@status", "CREATED");
                SqlParameter assigneeEmail = new SqlParameter("@asigneeEmail", SelectedAssignee.Email);
                SqlParameter finishTime = new SqlParameter("@finishTime", FinishTime);
                SqlParameter filePath = new SqlParameter("@filePath", FilePath);

                var result = db.Database.ExecuteSqlCommand("[Task.Create]" +
                    "@creatorId," +
                    "@teamId," +
                    "@boardId," +
                    "@title," +
                    "@content," +
                    "@type," +
                    "@priority," +
                    "@severity," +
                    "@status," +
                    "@asigneeEmail," +
                    "@finishTime," +
                    "@filePath",
                    new[] { userId, teamId, boardId, title, content, type, priority, severity, status, assigneeEmail, finishTime, filePath });

                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    window.Close();
                }
                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }
        public void NextPage()
        {
            currentPage += 1;
            GetAssignees();
        }
        public void PreviousPage()
        {
            currentPage -= currentPage == 0 ? 0 : 1;
            GetAssignees();
        }

        private void GetAssignees()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter teamId = new SqlParameter("@teamId", this.teamId);
                SqlParameter skip = new SqlParameter("@skip", currentPage * pageSize);
                SqlParameter take = new SqlParameter("@take", pageSize);

                Assignees.Clear();
                Assignees = db.Database.SqlQuery<TeamUser>("[Team.GetTeamUsers] @teamId, @skip, @take", new[] { teamId, skip, take }).ToList();
            }
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
