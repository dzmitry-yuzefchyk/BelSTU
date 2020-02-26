using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Taskboard.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {

        private string email = "d.yuzefchik@gmail.com";
        private string password = "12345";
        private string name = "Dmitry";
        private string take = "10";
        private string skip = "0";

        private List<USER> _users = new List<USER>();
        private List<USER_TOKEN> _tokens = new List<USER_TOKEN>();

        public List<USER> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyPropertyChanged("Users");
            }
        }

        public List<USER_TOKEN> Tokens
        {
            get => _tokens;
            set
            {
                _tokens = value;
                NotifyPropertyChanged("Tokens");
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                NotifyPropertyChanged("Email");
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                NotifyPropertyChanged("Password");
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Take
        {
            get => take;
            set
            {
                take = value;
                NotifyPropertyChanged("Take");
            }
        }

        public string Skip
        {
            get => skip;
            set
            {
                skip = value;
                NotifyPropertyChanged("Skip");
            }
        }


        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }
        public RelayCommand GetUsersCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public AccountViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
            LoginCommand = new RelayCommand(Login);
            GetUsersCommand = new RelayCommand(GetUsers);
            LogoutCommand = new RelayCommand(Logout);
            DeleteCommand = new RelayCommand(DeleteAccount);
        }

        public void Register()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter email = new SqlParameter("@email", Email);
                SqlParameter password = new SqlParameter("@password", Password);
                SqlParameter name = new SqlParameter("@name", Name);

                Stopwatch stopwatch = Stopwatch.StartNew();

                var result = db.Database.ExecuteSqlCommand("[User.Register] @email, @password, @name", new[] { email, password, name });

                stopwatch.Stop();
                MessageBoxResult re = MessageBox.Show(stopwatch.Elapsed.ToString(),
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

                if (result > 0)
                {
                    MainWindow.User.Status = "Успех";
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }

        public void Login()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter email = new SqlParameter("@email", Email);
                SqlParameter password = new SqlParameter("@password", Password);

                Stopwatch stopwatch = Stopwatch.StartNew();

                var result = db.Database.ExecuteSqlCommand("[User.Login] @email, @password", new[] { email, password });

                stopwatch.Stop();
                MessageBoxResult re = MessageBox.Show(stopwatch.Elapsed.ToString(),
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

                USER user = db.USER.SingleOrDefault(x => x.email == Email);
                USER_TOKEN token = null;

                if (user != null)
                {
                    token = db.USER_TOKEN.SingleOrDefault(x => x.userId == user.id);
                }

                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    MainWindow.User.Id = user.id;
                    MainWindow.User.Email = user.email;
                    MainWindow.User.Token = token.token.ToString();
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }

        public void Logout()
        {
            if (MainWindow.User.Email == "")
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }


            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter id = new SqlParameter("@userId", MainWindow.User.Id);
                var result = db.Database.ExecuteSqlCommand("[User.Logout] @userId", id);

                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    MainWindow.User.Id = 0;
                    MainWindow.User.Email = "";
                    MainWindow.User.Token = "";
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
            }
        }

        public void GetUsers()
        {
            using (TaskboardEntities db = new TaskboardEntities())
            {
                Users.Clear();
                Tokens.Clear();

                Stopwatch stopwatch = Stopwatch.StartNew();

                Users = db.USER.OrderBy(x => x.id).Skip(int.Parse(Skip)).Take(int.Parse(Take)).ToList();
                Tokens = db.USER_TOKEN.OrderBy(x => x.id).Skip(int.Parse(Skip)).Take(int.Parse(Take)).ToList();

                stopwatch.Stop();
                MessageBoxResult re = MessageBox.Show(stopwatch.Elapsed.ToString(),
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            }
        }

        public void DeleteAccount()
        {
            if (MainWindow.User.Id == 0)
            {
                MainWindow.User.Status = "Ошибка";
                return;
            }


            using (TaskboardEntities db = new TaskboardEntities())
            {
                SqlParameter id = new SqlParameter("@userId", MainWindow.User.Id);
                var result = db.Database.ExecuteSqlCommand("[User.Delete] @userId", id);

                if (result >= 0)
                {
                    MainWindow.User.Status = "Успех";
                    MainWindow.User.Id = 0;
                    MainWindow.User.Email = "";
                    MainWindow.User.Token = "";
                }

                else
                {
                    MainWindow.User.Status = "Ошибка";
                }
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
