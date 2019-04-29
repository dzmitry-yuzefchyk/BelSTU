using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace Taskboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter emailP = new SqlParameter("@email", email.Text);
                SqlParameter passwordP = new SqlParameter("@password", password.Text);
                SqlParameter nameP = new SqlParameter("@name", name.Text);
                var result = db.Database.ExecuteSqlCommand("[User.Register] @email, @password, @name", new[] { emailP, passwordP, nameP });

                if (result >= 0)
                {
                    Status.Content = "Успешная регистрация";
                }

                else
                {
                    Status.Content = "Ошибка регистрации";
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter emailP = new SqlParameter("@email", LoginEmail.Text);
                SqlParameter passwordP = new SqlParameter("@password", LoginPassword.Text);
                var result = db.Database.ExecuteSqlCommand("[User.Login] @email, @password", new[] { emailP, passwordP });

                USER user = db.USER.SingleOrDefault(x => x.email == LoginEmail.Text);
                USER_TOKEN token = null;

                if (user != null)
                {
                    token = db.USER_TOKEN.SingleOrDefault(x => x.userId == user.id);
                }

                if (result >= 0)
                {
                    Status.Content = "Успешный вход";
                    CurrentUserEmail.Content = user.email;
                    CurrentUserToken.Content = token.token;
                    TokenLifeTime.Content = $"{token.created}: {token.lifeTime} минут";
                }

                else
                {
                    Status.Content = "Ошибка входа";
                    CurrentUserEmail.Content = "";
                    CurrentUserToken.Content = "";
                    TokenLifeTime.Content = "";
                }
            }
        }

        private void GetUsers_Click(object sender, RoutedEventArgs e)
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                List<USER> users = db.USER.OrderBy(x => x.id).Skip(int.Parse(SkipUsers.Text)).Take(int.Parse(TakeUsers.Text)).ToList();
                UsersGrid.ItemsSource = users;

                List<USER_TOKEN> tokens = db.USER_TOKEN.OrderBy(x => x.id).Skip(int.Parse(SkipUsers.Text)).Take(int.Parse(TakeUsers.Text)).ToList();
                UsersTokenGrid.ItemsSource = tokens;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter emailP = new SqlParameter("@email", CurrentUserEmail.Content);
                var result = db.Database.ExecuteSqlCommand("[User.Logout] @email", emailP);

                if (result >= 0)
                {
                    Status.Content = "Успешный выход";
                    CurrentUserEmail.Content = "";
                    CurrentUserToken.Content = "";
                    TokenLifeTime.Content = "";
                }

                else
                {
                    Status.Content = "Ошибка выхода";
                }
            }
        }
    }
}
