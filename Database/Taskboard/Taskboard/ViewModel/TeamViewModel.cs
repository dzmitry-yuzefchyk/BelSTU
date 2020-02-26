using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Taskboard.View;

namespace Taskboard.ViewModel
{
    public class TeamProject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
    }
    public class TeamUser
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class TeamViewModel : INotifyPropertyChanged
    {
        private int pageSize = 15;
        private int teamId;

        private string projectTitle = "MyProject";
        private string projectAbout = "MyAbout";

        private int projectId;

        private TeamProject selectedProject;
        private List<TeamProject> projects = new List<TeamProject>();

        private TeamUser selectedUser;
        private List<TeamUser> users = new List<TeamUser>();


        private string userEmail = "k.yuzefchik@gmail.com";
        private int currentPage = 0;

        #region Props
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }

        public string UserEmail
        {
            get => userEmail;
            set
            {
                userEmail = value;
                NotifyPropertyChanged("UserEmail");
            }
        }

        public List<TeamUser> Users
        {
            get => users;
            set
            {
                users = value;
                NotifyPropertyChanged("Users");
            }
        }
        public TeamUser SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                NotifyPropertyChanged("SelectedUser");
            }
        }

        public TeamProject SelectedProject
        {
            get => selectedProject;
            set
            {
                selectedProject = value;
                NotifyPropertyChanged("SelectedProject");
            }
        }
        public List<TeamProject> Projects
        {
            get => projects;
            set
            {
                projects = value;
                NotifyPropertyChanged("Projects");
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

        public string ProjectTitle
        {
            get => projectTitle;
            set
            {
                projectTitle = value;
                NotifyPropertyChanged("ProjectTitle");
            }
        }

        public string ProjectAbout
        {
            get => projectAbout;
            set
            {
                projectAbout = value;
                NotifyPropertyChanged("ProjectAbout");
            }
        }

        public int TeamId
        {
            get => teamId;
            set
            {
                teamId = value;
                NotifyPropertyChanged("TeamId");
            }
        }
        #endregion

        public RelayCommand GetProjectWindowCommand { get; set; }
        public RelayCommand CreateProjectCommand { get; set; }
        public RelayCommand DeleteProjectCommand { get; set; }
        public RelayCommand AddUserToTeamCommand { get; set; }
        public RelayCommand RemoveUserFromTeamCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }

        public TeamViewModel(int teamId)
        {
            this.teamId = teamId;
            GetProjectWindowCommand = new RelayCommand(GetProjectWindow);
            CreateProjectCommand = new RelayCommand(CreateProject);
            DeleteProjectCommand = new RelayCommand(DeleteProject);
            AddUserToTeamCommand = new RelayCommand(AddUserToTeam);
            RemoveUserFromTeamCommand = new RelayCommand(RemoveUserFromTeam);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);

            GetTeamUsers();
            GetProjects();
        }

        public void GetProjects()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", TeamId);
                SqlParameter skip = new SqlParameter("@skip", currentPage * pageSize);
                SqlParameter take = new SqlParameter("@take", pageSize);


                Projects.Clear();
                Stopwatch stopwatch = Stopwatch.StartNew();
                Projects = db.Database.SqlQuery<TeamProject>("[Project.Get] @userId, @teamId, @skip, @take", new[] { userId, teamId, skip, take }).ToList();
                stopwatch.Stop();
                MessageBoxResult re = MessageBox.Show(stopwatch.Elapsed.ToString(),
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            }
        }
        public void GetTeamUsers()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter teamId = new SqlParameter("@teamId", TeamId);
                SqlParameter skip = new SqlParameter("@skip", currentPage * pageSize);
                SqlParameter take = new SqlParameter("@take", pageSize);

                Users.Clear();
                Users = db.Database.SqlQuery<TeamUser>("[Team.GetTeamUsers] @teamId, @skip, @take", new[] { teamId, skip, take }).ToList();
            }
        }
        public void NextPage()
        {
            CurrentPage += 1;
            GetTeamUsers();
            GetProjects();
        }
        public void PreviousPage()
        {
            CurrentPage -= CurrentPage == 0 ? 0 : 1;
            GetTeamUsers();
            GetProjects();
        }
        public void RemoveUserFromTeam()
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }

            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@creatorId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", TeamId);
                SqlParameter userEmail = new SqlParameter("@userEmail", UserEmail);

                var result = db.Database.ExecuteSqlCommand("[Team.RemoveUserFromTeam] @creatorId, @teamId, @userEmail", new[] { userId, teamId, userEmail });
                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    GetTeamUsers();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }
        public void AddUserToTeam()
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }

            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@creatorId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", TeamId);
                SqlParameter userEmail = new SqlParameter("@userEmail", UserEmail);

                var result = db.Database.ExecuteSqlCommand("[Team.AddUserToTeam] @creatorId, @teamId, @userEmail", new[] { userId, teamId, userEmail });
                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    GetTeamUsers();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }

        public void DeleteProject()
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }

            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", TeamId);
                SqlParameter projectId = new SqlParameter("@projectId", ProjectId);

                Stopwatch stopwatch = Stopwatch.StartNew();
                var result = db.Database.ExecuteSqlCommand("[Project.Delete] @userId, @projectId, @teamId", new[] { userId, projectId, teamId });
                stopwatch.Stop();
                MessageBoxResult re = MessageBox.Show(stopwatch.Elapsed.ToString(),
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    GetProjects();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }

        public void CreateProject()
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }

            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter teamId = new SqlParameter("@teamId", TeamId);
                SqlParameter title = new SqlParameter("@title", ProjectTitle);
                SqlParameter about = new SqlParameter("@about", ProjectAbout);


                Stopwatch stopwatch = Stopwatch.StartNew();
                var result = db.Database.ExecuteSqlCommand("[Project.Create] @userId, @teamId, @title, @about", new[] { userId, teamId, title, about });
                stopwatch.Stop();
                MessageBoxResult re = MessageBox.Show(stopwatch.Elapsed.ToString(),
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    GetProjects();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }
        public void GetProjectWindow()
        {
            ProjectView projectWindow = new ProjectView(SelectedProject.Id, TeamId);
            projectWindow.Show();
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
