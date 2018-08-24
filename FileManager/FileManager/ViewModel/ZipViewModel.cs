using System;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FileManager.DataBase;
using FileManager.Model;
using GalaSoft.MvvmLight.Command;

namespace FileManager.ViewModel
{
    class ZipViewModel
    {
        DataContext db;


        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;

        #region fields
        private FolderElement source = Mediator.SourceFile;

        private Mediator.ActionType action = Mediator.Type;
        #endregion

        #region properties

        public string Action
        {
            get
            {
                if (Mediator.Type == Mediator.ActionType.Zip)
                {
                    return "Архивирование " + Mediator.SourceFile.FullName;
                }

                else
                    return "Разархивирование " + Mediator.SourceFile.FullName;

            }
        }
        #endregion

        #region commands
        public RelayCommand<Window> CancelCommand { get; set; }
        public RelayCommand<Window> DoCommand { get; set; }
        

        #endregion

        public ZipViewModel()
        {

            CancelCommand = new RelayCommand<Window>(CancelMethod);
            DoCommand = new RelayCommand<Window>(Zip);

            db = new DataContext();

            token = cancelTokenSource.Token;
        }

        #region methods

        private void CancelMethod(Window window)
        {
            if (window != null)
            {
                cancelTokenSource.Cancel();
                window.Close();
            }
        }

        private async void Zip(Window window)
        {
            if (action == Mediator.ActionType.Zip)
            {
                await Zipper(source.FullName);
                MainViewModel.GetInstance().AddToCollection(new FolderElement { FullName = source.FullName + ".zip", Type = ElementType.File });
            }

            else
            {
                await Unzipper(source.FullName);
                MainViewModel.GetInstance().AddToCollection(new FolderElement { FullName = source.FullName.Split('.').First(), Type = ElementType.File });
            }
            if (window != null)
            {
                window.Close();
            }

        }


        #endregion

        #region Tasks
        public Task Zipper(string filePath)
        {
            return Task.Run(() =>
            {
                if (!token.IsCancellationRequested)
                {
                    try
                    {
                        if (source.Type == ElementType.Folder)
                        {
                            ZipFile.CreateFromDirectory(filePath, filePath + ".zip");
                        }
                        else
                        {
                            using (ZipArchive zip = ZipFile.Open(filePath + ".zip", ZipArchiveMode.Create))
                            {
                                zip.CreateEntryFromFile(filePath, source.Name +"."+ source.Extension);
                            }
                        }

                        db.Logs.Add(new Log
                                (source.FullName,
                                "Файл архивирован",
                                DateTime.Now.ToString("h:mm:ss tt"))
                                );
                        db.SaveChanges();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                
            });

        }

        public Task Unzipper(string filePath)
        {
            return Task.Run(() =>
            {
                if (!token.IsCancellationRequested)
                {
                    try
                    {
                        ZipFile.ExtractToDirectory(filePath, filePath.Split('.').First());
                        db.Logs.Add(new Log
                                (source.FullName,
                                "Файл разархивирован",
                                DateTime.Now.ToString("h:mm:ss tt"))
                                );
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }


            });

        }
        #endregion
    }
}
