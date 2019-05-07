using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using Taskboard.View;

namespace Taskboard.ViewModel
{
    public class TeamResult
    {
        public int TeamId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
    }

    public class TeamsViewModel : INotifyPropertyChanged
    {
        private int pageSize = 30;
        private int currentPage = 0;
        private int teamId;
        private TeamResult selectedTeam;
        private string teamTitle = "Team";
        private List<TeamResult> teams = new List<TeamResult>();

        #region props
        public int TeamId
        {
            get => teamId;
            set
            {
                teamId = value;
                NotifyPropertyChanged("TeamId");
            }
        }
        public string TeamTitle
        {
            get => teamTitle;
            set
            {
                teamTitle = value;
                NotifyPropertyChanged("TeamTitle");
            }
        }

        public TeamResult SelectedTeam
        {
            get => selectedTeam;
            set
            {
                selectedTeam = value;
                NotifyPropertyChanged("SelectedTeam");
            }
        }
        public List<TeamResult> Teams
        {
            get => teams;
            set
            {
                teams = value;
                NotifyPropertyChanged("Teams");
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
        #endregion
        public RelayCommand GetTeamWindowCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }
        public RelayCommand CreateTeamCommand { get; set; }
        public RelayCommand DeleteTeamCommand { get; set; }

        public TeamsViewModel()
        {
            GetUserTeams();
            GetTeamWindowCommand = new RelayCommand(GetTeamWindow);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            CreateTeamCommand = new RelayCommand(CreateTeam);
            DeleteTeamCommand = new RelayCommand(DeleteTeam);
        }

        public void DeleteTeam()
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

                var result = db.Database.ExecuteSqlCommand("[Team.Delete] @creatorId, @teamid", userId, teamId);
                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    GetUserTeams();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }

        public void CreateTeam()
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }

            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter title = new SqlParameter("@teamName", TeamTitle);

                var result = db.Database.ExecuteSqlCommand("[Team.Create] @userId, @teamName", userId, title);
                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    GetUserTeams();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }

        public void NextPage()
        {
            CurrentPage += 1;
            GetUserTeams();
        }

        public void PreviousPage()
        {
            CurrentPage -= CurrentPage == 0 ? 0 : 1;
            GetUserTeams();
        }

        public void GetUserTeams()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter userId = new SqlParameter("@userId", MainWindow.User.Id);
                SqlParameter skip = new SqlParameter("@skip", currentPage * pageSize);
                SqlParameter take = new SqlParameter("@take", pageSize);

                Teams.Clear();
                Teams = db.Database.SqlQuery<TeamResult>("[Team.GetUserTeams] @userId, @skip, @take", new[] { userId, skip, take}).ToList();
            }
        }

        public void GetTeamWindow()
        {
            TeamView teamWindow = new TeamView(SelectedTeam.TeamId);
            teamWindow.Show();
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
