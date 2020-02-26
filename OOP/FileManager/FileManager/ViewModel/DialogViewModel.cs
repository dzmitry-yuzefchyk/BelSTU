using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FileManager.DataBase;
using FileManager.Model;
using GalaSoft.MvvmLight.Command;

namespace FileManager.ViewModel
{
    class DialogViewModel : INotifyPropertyChanged
    {
        #region fields
        DataContext db;

        private FolderElement delete = Mediator.DeleteFile;
        #endregion

        #region properties

        public string Action
        {
            get
            {
                return "Удаление " + delete.FullName;
            }
        }
        #endregion

       
        #region commands
        public RelayCommand<Window> DeleteCommand { get; set; }
        public RelayCommand<Window> CancelCommand { get; set; }
        #endregion

        public DialogViewModel()
        {
            DeleteCommand = new RelayCommand<Window>(DeleteMethod);
            CancelCommand = new RelayCommand<Window>(CancelMethod);

            db = new DataContext();
        }


        #region methods
        public void DeleteMethod(Window window)
        {
            try
            {
                if (delete.Type == ElementType.File)
                    File.Delete(delete.FullName); 
                else
                    Directory.Delete(delete.FullName, true);

                MainViewModel.GetInstance().RemoveFromCollection(Mediator.DeleteFile);

                db.Logs.Add(new Log
                    (Mediator.DeleteFile.FullName,
                    Action,
                    DateTime.Now.ToString("h:mm:ss tt"))
                    );
                db.SaveChanges();

                if (window != null)
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelMethod(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        #endregion

        #region Tasks

        #endregion

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
