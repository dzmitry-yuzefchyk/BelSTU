using System.Windows;
using Taskboard.Model;
using Taskboard.View;

namespace Taskboard
{
    public partial class MainWindow : Window
    {
        public static CurrentUser User { get; set; } = new CurrentUser();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = User;
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new AccountView();
        }

        private void TeamButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new TeamsView();
        }

        private void ProjectButton_Click(object sender, RoutedEventArgs e)
        {
            //this.mainContentControl.Content = new ProjectView();
        }

        //#region Team
        //private void TeamCreate_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CurrentUserEmail.Content.ToString() != "")
        //    {
        //        SqlParameter emailP = new SqlParameter("@userEmail", CurrentUserEmail.Content);
        //        SqlParameter title = new SqlParameter("@teamName", TeamCreateTitle.Text);

        //        using (TaskboardEntities db = new TaskboardEntities())
        //        {
        //            var result = db.Database.ExecuteSqlCommand("[Team.CreateTeam] @userEmail, @teamName", emailP, title);
        //            if (result >= 0)

        //            {
        //                Status.Content = "Команда создана";
        //            }

        //            else
        //            {
        //                Status.Content = "Ошибка создания команды";
        //            }
        //        }
        //    }

        //    else
        //    {
        //        Status.Content = "Ошибка создания команды, пользователь не вошел";
        //    }
        //}

        //private void TeamDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CurrentUserEmail.Content.ToString() != "")
        //    {
        //        SqlParameter emailP = new SqlParameter("@creatorEmail", CurrentUserEmail.Content);
        //        SqlParameter id = new SqlParameter("@teamId", int.Parse(TeamDeleteId.Text));

        //        using (TaskboardEntities db = new TaskboardEntities())
        //        {
        //            var result = db.Database.ExecuteSqlCommand("[Team.DeleteTeam] @creatorEmail, @teamid", emailP, id);
        //            if (result >= 0)

        //            {
        //                Status.Content = "Команда удалена создана";
        //            }

        //            else
        //            {
        //                Status.Content = "Ошибка удаления команды";
        //            }
        //        }
        //    }

        //    else
        //    {
        //        Status.Content = "Ошибка удаления команды, пользователь не вошел";
        //    }
        //}

        //private void TeamAddUser_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CurrentUserEmail.Content.ToString() != "")
        //    {
        //        SqlParameter creatorEmail = new SqlParameter("@creatorEmail", CurrentUserEmail.Content);
        //        SqlParameter teamId = new SqlParameter("@teamId", int.Parse(TeamAddTeamId.Text));
        //        SqlParameter userEmail = new SqlParameter("@addUserEmail", TeamAddUserEmail.Text);

        //        using (TaskboardEntities db = new TaskboardEntities())
        //        {
        //            var result = db.Database.ExecuteSqlCommand("[Team.AddUserToTeam] @creatorEmail, @teamId, @addUserEmail", creatorEmail, teamId, userEmail);
        //            if (result >= 0)

        //            {
        //                Status.Content = "Пользователь добавлен";
        //            }

        //            else
        //            {
        //                Status.Content = "Ошибка";
        //            }
        //        }
        //    }

        //    else
        //    {
        //        Status.Content = "Ошибка добавления в команду, пользователь не вошел";
        //    }
        //}

        //private void GetTeam_Click(object sender, RoutedEventArgs e)
        //{
        //    using (TaskboardEntities db = new TaskboardEntities())
        //    {
        //        List<TEAM> teams = db.TEAM_USER.Where(x => x.userId)
        //        TeamGrid.ItemsSource = teams;

        //        List<USER_TOKEN> tokens = db.USER_TOKEN.OrderBy(x => x.id).Skip(int.Parse(SkipUsers.Text)).Take(int.Parse(TakeUsers.Text)).ToList();
        //        TeamUserGrid.ItemsSource = tokens;
        //    }
        //}
        //#endregion


    }
}
