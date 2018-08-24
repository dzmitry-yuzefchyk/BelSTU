using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using FileManager.DataBase;
using FileManager.Model;
using System.Collections.ObjectModel;

namespace FileManager.ViewModel
{
    class PasteViewModel : INotifyPropertyChanged
    {
        DataContext db;


        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;

        #region fields
        private long progress;

        private FolderElement source = Mediator.SourceFile;
        private string destination = Mediator.DestionationFile;
        #endregion

        #region properties
        public long Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public string Action
        {
            get
            {
                if (Mediator.Type == Mediator.ActionType.Copy)
                {
                    return "Копирование из " + Mediator.SourceFile.FullName + " в " + Mediator.DestionationFile;
                }

                else
                    return "Перемещение из " + Mediator.SourceFile.FullName + " в " + Mediator.DestionationFile;

            }
        }
        #endregion

        #region commands
        public RelayCommand<Window> CancelCommand { get; set; }
        public RelayCommand<Window> PasteCommand { get; set; }

        #endregion

        public PasteViewModel()
        {
            
            CancelCommand = new RelayCommand<Window>(CancelMethod);
            PasteCommand = new RelayCommand<Window>(Paste);

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

        private async void Paste(Window window)
        {
            string fileName = source.FullName;
            string endDirectory = destination + "\\" + source.Name;

            if (Mediator.Type == Mediator.ActionType.Copy)
            {
                if (source.Type == ElementType.Folder)
                {
                    try
                    {
                        await FolderCopy
                                   (
                                   Mediator.SourceFile.FullName,
                                   Mediator.SourceFile.Name,
                                   endDirectory,
                                   prog => Progress = prog,
                                   window
                                   );
                    }
                    catch (TaskCanceledException ex)
                    {
                        MessageBox.Show(ex.Message);

                        if (window != null)
                        {
                            window.Close();
                        }
                    }
                }

                else
                    try
                    {
                        await CopyFiles
                                        (
                                        new Dictionary<string, string>
                                        {
                                        { fileName, endDirectory}
                                        },
                                        prog => Progress = prog,
                                        window
                                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }

            if (Mediator.Type == Mediator.ActionType.Cut)
            {

                if (source.Type == ElementType.Folder)
                    try
                    {
                        await FolderCut
                                        (
                                        Mediator.SourceFile.FullName,
                                        Mediator.SourceFile.Name,
                                        endDirectory,
                                        prog => Progress = prog,
                                        window
                                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                else
                    try
                    {
                        await CutFiles
                                        (
                                        new Dictionary<string, string>
                                        {
                                        { fileName, endDirectory}
                                        },
                                        prog => Progress = prog,
                                        window
                                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }

        }
        #endregion

        #region Tasks
        public async Task CopyFiles(Dictionary<string, string> files, Action<long> progressCallback, Window windowToClose)
        {

            if (!token.IsCancellationRequested)
            {
                for (var x = 0; x < files.Count; x++)
                {
                    var item = files.ElementAt(x);
                    var from = item.Key;
                    var to = item.Value;

                    using (var outStream = new FileStream(to, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        using (var inStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            await inStream.CopyToAsync(outStream, 128, token);
                        }
                    }

                    progressCallback((long)((x + 1) / files.Count) * 100);
                    db.Logs.Add(new Log
                        (Mediator.SourceFile.FullName,
                        "Файл копирован в " + Mediator.DestionationFile,
                        DateTime.Now.ToString("h:mm:ss tt"))
                        );
                    db.SaveChanges();

                    MainViewModel.GetInstance().AddToCollection(new FolderElement { FullName = to, Type = ElementType.File });
                }
            }

            if (windowToClose != null)
            {
                windowToClose.Close();
            }

        }

        public async Task CutFiles(Dictionary<string, string> files, Action<long> progressCallback, Window windowToClose)
        {
            if (!token.IsCancellationRequested)
            {

                for (var x = 0; x < files.Count; x++)
                {
                    var item = files.ElementAt(x);
                    var from = item.Key;
                    var to = item.Value;

                    using (var outStream = new FileStream(to, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        using (var inStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            await inStream.CopyToAsync(outStream, 128, token);
                        }
                    }

                    progressCallback((long)((x + 1) / files.Count) * 100);
                    File.Delete(from);
                    db.Logs.Add(new Log
                        (Mediator.SourceFile.FullName,
                        "Файл перемещен в " + Mediator.DestionationFile,
                        DateTime.Now.ToString("h:mm:ss tt"))
                        );
                    db.SaveChanges();

                    MainViewModel.GetInstance().AddToCollection(new FolderElement { FullName = to, Type = ElementType.File});

                }
            }

            if (windowToClose != null)
            {
                windowToClose.Close();
            }

        }

        public async Task FolderCopy(string from, string parentDirName, string to, Action<long> progressCallback, Window windowToClose)
        {
            await DirectoryCopy
                        (
                        from,
                        parentDirName,
                        to,
                        prog => Progress = prog
                        );

            db.Logs.Add(new Log
                        (from,
                        "Файл копирован в " + to,
                        DateTime.Now.ToString("h:mm:ss tt"))
                        );
            db.SaveChanges();

            MainViewModel.GetInstance().AddToCollection(new FolderElement { FullName = to, Type = ElementType.Folder });


            if (windowToClose != null)
            {
                windowToClose.Close();
            }
        }

        public async Task FolderCut(string from, string parentDirName, string to, Action<long> progressCallback, Window windowToClose)
        {
            await DirectoryCut
                        (
                        from,
                        parentDirName,
                        to,
                        prog => Progress = prog
                        );
            db.Logs.Add(new Log
                        (from,
                        "Файл перемещен в " + to,
                        DateTime.Now.ToString("h:mm:ss tt"))
                        );
            db.SaveChanges();

            MainViewModel.GetInstance().AddToCollection(new FolderElement { FullName = to, Type = ElementType.Folder });

            try
            {
                Directory.Delete(from, true);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message + " файлы не были удалены");
            }
            if (windowToClose != null)
            {
                windowToClose.Close();
            }
        }

        public async Task DirectoryCopy(string from, string parentDirName, string to, Action<long> progressCallback)
        {
            if (!token.IsCancellationRequested)
            {
                DirectoryInfo directory = new DirectoryInfo(from);

                DirectoryInfo[] dirs = directory.GetDirectories();
                FileInfo[] files = directory.GetFiles();

                if (!Directory.Exists(to + parentDirName))
                {
                    Directory.CreateDirectory(to);
                }

                foreach (FileInfo file in files)
                {
                    using (var outStream = new FileStream(Path.Combine(to, file.Name), FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        using (var inStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            await inStream.CopyToAsync(outStream, 128, token);
                        }
                    }

                    progressCallback((long)(100));
                }

                foreach (DirectoryInfo subDir in dirs)
                {
                    string newPath = Path.Combine(to, subDir.Name);
                    await  DirectoryCopy(subDir.FullName, subDir.Name, newPath, prog => Progress = prog);
                }
            }
        }

        public async Task DirectoryCut(string from, string parentDirName, string to, Action<long> progressCallback)
        {
            if (!token.IsCancellationRequested)
            {
                DirectoryInfo directory = new DirectoryInfo(from);

                DirectoryInfo[] dirs = directory.GetDirectories();
                FileInfo[] files = directory.GetFiles();

                if (!Directory.Exists(to + parentDirName))
                {
                    Directory.CreateDirectory(to);
                }

                foreach (FileInfo file in files)
                {
                    using (var outStream = new FileStream(Path.Combine(to, file.Name), FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        using (var inStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            await inStream.CopyToAsync(outStream, 128, token);
                        }
                    }

                    progressCallback((long)(100));
                    
                }

                
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newPath = Path.Combine(to, subDir.Name);
                    await DirectoryCut(subDir.FullName, subDir.Name, newPath, prog => Progress = prog);
                }
            }
        }
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
