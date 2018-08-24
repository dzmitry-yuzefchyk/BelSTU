using GalaSoft.MvvmLight.Command;
using FileManager.Model;
using FileManager.DataBase;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace FileManager.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region fields
        DataContext db;

        private static MainViewModel instance;

        private ObservableCollection<FolderElement> _file;
        private ObservableCollection<FolderElement> _drive;

        private FolderElement selectedElement;
        private string currentFolderPath;

        private bool copyState = false;
        private bool cutState = false;
        #endregion

        #region properties

        public ObservableCollection<FolderElement> File => _file ?? (_file = new ObservableCollection<FolderElement>());
        public ObservableCollection<FolderElement> _File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged("File");
            }
        }


        public ObservableCollection<FolderElement> Drive
        {
            get => _drive;
            set => _drive = value;
        }

        public string PreviousFolder
        {
            get
            {
                if (currentFolderPath.Split('\\').Count() > 1)
                {
                    var result = from sub in currentFolderPath.Split('\\')
                                 where sub != currentFolderPath.Split('\\').Last()
                                 select sub;
                    if (result.Count() == 1)
                    {
                        return String.Join("\\", result) + "\\";
                    }
                    return String.Join("\\", result);
                }
                else
                    return currentFolderPath = currentFolderPath + '\\';
            }
        }

        public FolderElement SelectedFile
        {
            set
            {
                selectedElement = value;
                OnPropertyChanged("SelectedFile");
            }
        }

        public string CurrentFolder
        {
            get => currentFolderPath;
            set
            {
                currentFolderPath = value;
                OnPropertyChanged("CurrentFolder");
            }
        }
        #endregion

        #region commands
        public RelayCommand<string> LoadCommand { get; set; }
        public RelayCommand ExtendedLoadCommand { get; set; }
        public RelayCommand LoadPreviousCommand { get; set; }
        public RelayCommand<FolderElement> TreeviewSelectedItemChanged { get; set; }
        public RelayCommand CopyCommand { get; set; }
        public RelayCommand CutCommand { get; set; }
        public RelayCommand PasteCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand ZipCommand { get; set; }
        public RelayCommand UnzipCommand { get; set; }
        public RelayCommand<string> SearchCommand { get; set; }
        public RelayCommand PropertyCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand ShowLogCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            instance = this;

            LoadCommand = new RelayCommand<string>(LoadMethod);
            ExtendedLoadCommand = new RelayCommand(ExtendedLoadMethod);
            LoadPreviousCommand = new RelayCommand(LoadPreviousMethod);
            TreeviewSelectedItemChanged = new RelayCommand<FolderElement>(LoadFromTreeMethod);
            CopyCommand = new RelayCommand(CopyMethod);
            CutCommand = new RelayCommand(CutMethod);
            PasteCommand = new RelayCommand(PasteMethod);
            DeleteCommand = new RelayCommand(DeleteMethod);
            ZipCommand = new RelayCommand(ZipMethod);
            UnzipCommand = new RelayCommand(UnzipMethod);
            SearchCommand = new RelayCommand<string>(SearchMethod);
            PropertyCommand = new RelayCommand(PropertyMethod);
            CreateCommand = new RelayCommand(CreateMethod);
            ShowLogCommand = new RelayCommand(ShowLogMethod);

            _drive = new ObservableCollection<FolderElement>();
            db = new DataContext();


            FillTreeMethod();
        }

        public static MainViewModel GetInstance()
        {
            return instance;
        }

        #region methods
        private void LoadMethod(string folderPath)
        {
            if (folderPath != null)
            {
                var folderInf = new FolderData(folderPath);
                _file = new ObservableCollection<FolderElement>(folderInf.GetData());
                currentFolderPath = folderPath;
                OnPropertyChanged(nameof(File));
            }

            else
            {
                _file = _file;
            }
        }

        private void ExtendedLoadMethod()
        {
            try
            {
                if (selectedElement.Type == ElementType.Folder)
                {
                    string folderPath = selectedElement.FullName;
                    var folderInf = new FolderData(folderPath);
                    _file = new ObservableCollection<FolderElement>(folderInf.GetData());
                    CurrentFolder = folderPath;
                    OnPropertyChanged(nameof(File));
                    return;
                }

                if (selectedElement.Type == ElementType.File)
                {
                    string folderPath = selectedElement.FullName;
                    System.Diagnostics.Process.Start(folderPath);
                    return;
                }
            }

            catch
            {
                return;
            }
        }

        private void LoadPreviousMethod()
        {
            try
            {
                string folderPath = PreviousFolder;
                var folderInf = new FolderData(folderPath);
                _file = new ObservableCollection<FolderElement>(folderInf.GetData());
                CurrentFolder = folderPath;
                OnPropertyChanged(nameof(File));
            }

            catch 
            {

            }
            
        }

        private void FillTreeMethod()
        {
            string[] drives = Environment.GetLogicalDrives();
            foreach (string d in drives)
            {
                Drive.Add(new FolderElement { Name = d, Type = ElementType.Folder });
            }
        }

        private void LoadFromTreeMethod(FolderElement folder)
        {
            var folderInf = new FolderData(folder.FullName);
            _file = new ObservableCollection<FolderElement>(folderInf.GetData());
            CurrentFolder = folder.FullName;
            OnPropertyChanged(nameof(File));
        }

        private void CopyMethod()
        {
            try
            {
                Mediator.SourceFile = selectedElement;
                copyState = true;
                cutState = false;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Выберите файл");
            }
        }

        private void CutMethod()
        {
            try
            {
                Mediator.SourceFile = selectedElement;
                copyState = false;
                cutState = true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Выберите файл");
            }
        }

        private async void PasteMethod()
        {
            if (copyState)
            {
                try
                {
                    Mediator.DestionationFile = currentFolderPath;
                    Mediator.Type = Mediator.ActionType.Copy;
                    PasteWindow pasteWindow = new PasteWindow();
                    pasteWindow.Title = "Копирование";

                    await SendMessage
                        (
                        new Log
                        (Mediator.SourceFile.FullName,
                        "Попытка копирования в " + Mediator.DestionationFile,
                        DateTime.Now.ToString("h:mm:ss tt"))
                        );

                    pasteWindow.Show();
                    copyState = false;

                }
                catch (Exception)
                {
                    copyState = false;
                }
            }

            if (cutState)
            {
                try
                {
                    Mediator.DestionationFile = currentFolderPath;
                    Mediator.Type = Mediator.ActionType.Cut;
                    PasteWindow pasteWindow = new PasteWindow();
                    pasteWindow.Title = "Перемещение";

                    await SendMessage
                        (
                        new Log
                        (Mediator.SourceFile.FullName,
                        "Попытка перемещения в " + Mediator.DestionationFile,
                        DateTime.Now.ToString("h:mm:ss tt"))
                        );
                    pasteWindow.Show();
                    cutState = false;

                }
                catch (Exception)
                {
                    cutState = false;
                }
            }
        }

        public async void DeleteMethod()
        {
            try
            {
                Mediator.DeleteFile = selectedElement;
                DialogWindow dialogWindow = new DialogWindow();

                await SendMessage
                    (
                    new Log
                    (Mediator.DeleteFile.FullName,
                    "Попытка удаления",
                    DateTime.Now.ToString("h:mm:ss tt"))
                    );
                dialogWindow.Show();

            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Выберите файл");
            }
        }


        public async void ZipMethod()
        {
            try
            {
                Mediator.SourceFile = selectedElement;
                Mediator.Type = Mediator.ActionType.Zip;

                ZipWindow zipWindow = new ZipWindow();
                zipWindow.Title = "Архивация";

                await SendMessage
                    (
                    new Log
                    (Mediator.SourceFile.FullName,
                    "Попытка Архивации",
                    DateTime.Now.ToString("h:mm:ss tt"))
                    );

                zipWindow.Show();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Выберите файл");
            }
        }

        public async void UnzipMethod()
        {
            try
            {
                Mediator.SourceFile = selectedElement;
                Mediator.Type = Mediator.ActionType.UnZip;

                ZipWindow zipWindow = new ZipWindow();
                zipWindow.Title = "Разархивация";

                await SendMessage
                    (
                    new Log
                    (Mediator.SourceFile.FullName,
                    "Попытка разархивации",
                    DateTime.Now.ToString("h:mm:ss tt"))
                    );

                zipWindow.Show();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Выберите файл");
            }
        }

        public void PropertyMethod()
        {
            try
            {
                Mediator.SourceFile = selectedElement;
                Mediator.CurrentFolder = CurrentFolder;
                PropertyWindow propertyWindow = new PropertyWindow();
                propertyWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось отобразить окно");
            }
        }

        public async void SearchMethod(string filter)
        {
            if (filter != "")
            {
                Regex regex = new Regex(@"\w*" + filter + @"\w*", RegexOptions.IgnoreCase);
                if (currentFolderPath != null)
                {
                    string searchPath = currentFolderPath;
                    File.Clear();
                    try
                    {
                        await RegexSearch(regex, searchPath);
                    }
                    catch (System.IO.IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                } 
            }
        }

        public void CreateMethod()
        {
            try
            {
                if (!System.IO.File.Exists(Path.Combine(CurrentFolder, "Новый файл.txt")))
                {
                    System.IO.File.Create(Path.Combine(CurrentFolder, "Новый файл.txt"));
                    _File.Add(new FolderElement { FullName = Path.Combine(CurrentFolder, "Новый файл.txt"), Type = ElementType.File });
                }
                else
                    MessageBox.Show("Файл " + Path.Combine(CurrentFolder, "Новый файл.txt") + " уже существует");
            }

            catch
            {
            }
        }

        public void ShowLogMethod()
        {

            try
            {
                LogWindow logWindow = new LogWindow();
                logWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось отобразить лог");
            }
        }

        public void RemoveFromCollection(FolderElement folderElement)
        {
            File.Remove(folderElement);
        }

        public void AddToCollection(FolderElement folderElement)
        {
            File.Add(folderElement);
        }

        #endregion

        public async Task RegexSearch(Regex regex, string destinationPath)
        {
            DirectoryInfo directory = new DirectoryInfo(destinationPath);
            DirectoryInfo[] dirs = directory.GetDirectories();
            FileInfo[] files = directory.GetFiles();
            BackgroundWorker worker = new BackgroundWorker();

            foreach (FileInfo file in files)
            {
                if (regex.IsMatch(file.Name))
                {
                    File.Add(new FolderElement { Name = file.FullName, Type = ElementType.File });
                }
            }

            foreach (DirectoryInfo dir in dirs)
            {
                try
                {
                    await RegexSearch(regex, dir.FullName);
                }
                catch (Exception)
                {

                    continue;
                }
                if (regex.IsMatch(dir.Name))
                {
                    File.Add(new FolderElement { Name = dir.FullName, Type = ElementType.Folder });
                }
            }

            CurrentFolder = "";
        }

        public Task SendMessage(Log logInfo)
        {
            return Task.Run(() =>
            {
                db.Logs.Add(logInfo);
                db.SaveChanges();
            });
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