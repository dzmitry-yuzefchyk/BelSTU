using System;
using System.ComponentModel;

namespace Taskboard.Model
{
    public class CurrentUser : INotifyPropertyChanged
    {
        private int id;
        private string email = "";
        private string token = "";
        private string status = "";
        public int Id
        {
            get => id;
            set
            {
                id = value;
                NotifyPropertyChanged("Id");
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
        public string Token
        {
            get => token;
            set
            {
                token = value;
                NotifyPropertyChanged("Token");
            }
        }
        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyPropertyChanged("Status");
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
