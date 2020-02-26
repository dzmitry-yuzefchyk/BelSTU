using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using FileManager.DataBase;
using FileManager.Model;
using GalaSoft.MvvmLight.Command;

namespace FileManager.ViewModel
{


    class LogViewModel : INotifyPropertyChanged
    {

        DataContext db;

        public RelayCommand<Window> ClearLogCommand { get; set; }

        public List<Log> Records { get; set; }

        public LogViewModel()
        {
            ClearLogCommand = new RelayCommand<Window>(ClearLog);

            db = new DataContext();

            try
            {
                Records = db.Logs.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось обратиться к логу, возможно он пуст. " + ex.Message);
            }
        }

        public void ClearLog(Window window)
        {
            try
            {
                foreach (var element in db.Logs)
                {
                    try
                    {
                        db.Logs.Remove(element);
                    }
                    catch (Exception)
                    {
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Очищен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Очистка не удалась. " + ex.Message);
                if (window != null)
                {
                    window.Close();
                }
            }

            if (window != null)
            {
                window.Close();
            }
        }

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
