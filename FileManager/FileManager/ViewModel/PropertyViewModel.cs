using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;

namespace FileManager.ViewModel
{
    class PropertyViewModel : INotifyPropertyChanged
    {
        private FolderElement _file = Mediator.SourceFile;
        private FolderElement sourceFile;
        private string currentFolder = Mediator.CurrentFolder;

        private bool isReadOnly;
        private bool isHidden;

        public bool IsHidden
        {
            get
            {
                if (sourceFile.Type == ElementType.File)
                {
                    FileInfo file = new FileInfo(sourceFile.FullName);
                    if (file.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        isHidden = true;
                        return true;
                    }
                    else
                    {
                        isHidden = false;
                        return false;
                    }
                }

                else
                {
                    DirectoryInfo directory = new DirectoryInfo(sourceFile.FullName);
                    if (directory.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        isHidden = true;
                        return true;
                    }
                    else
                    {
                        isHidden = false    ;
                        return false;
                    }
                }
            }
            set
            {
                if (sourceFile.Type == ElementType.File)
                {
                    isHidden = value;
                    OnPropertyChanged("IsHidden");
                }

                else
                {
                    isHidden = value;
                    OnPropertyChanged("IsHidden");
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                if (sourceFile.Type == ElementType.File)
                {
                    FileInfo file = new FileInfo(sourceFile.FullName);
                    return file.IsReadOnly;
                }

                else
                {
                    return false;
                }
            }
            set
            {
                if (sourceFile.Type == ElementType.File)
                {
                    isReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
                }

                else
                {
                    isReadOnly = false;
                    OnPropertyChanged("IsReadOnly");
                }
            }
        }

        public string FileName
        {
            get => _file.Name;
            set
            {
                _file.Name = value;
                OnPropertyChanged("FileName");
            }
        }

        public string CurrentFolder
        {
            get => currentFolder;
            set
            {
                currentFolder = value;
                OnPropertyChanged("CurrentFolder");
            }
        }

        public BitmapImage Image
        {
            get => _file.Image;
        }

        public string Size
        {
            get
            {
                if (sourceFile.Type == ElementType.File)
                    return sourceFile.Size +" ("+ new System.IO.FileInfo(sourceFile.FullName).Length.ToString() + " Byte)";
                else
                {
                    long size = GetFolderSize().Result;
                    if (size < 1024)
                    {
                        return size.ToString() + " Byte";
                    }

                    else if (size > 1024 && size < 1024 * 1024)
                    {
                        return (size / 1024).ToString() + " KB"+ "(" +size.ToString() + " Byte)";
                    }

                    else if (size > 1024 * 1024 && size < 1024 * 1024 * 1024)
                    {
                        return (size / (1024 * 1024)).ToString() + " MB" + "(" + size.ToString() + " Byte)"; ;
                    }

                    else
                        return (size / (1024 * 1024 * 1024)).ToString() + " GB" + "(" + size.ToString() + " Byte)"; ;
                }
            }
        }

        public RelayCommand<Window> CancelCommand { get; set; }
        public RelayCommand<Window> AcceptCommand { get; set; }
        public RelayCommand ReadParam { get; set; }
        public RelayCommand HiddenParam { get; set; }

        public PropertyViewModel()
        {
            CancelCommand = new RelayCommand<Window>(CancelMethod);
            AcceptCommand = new RelayCommand<Window>(AcceptMethod);
            ReadParam = new RelayCommand(ReadChange);
            HiddenParam = new RelayCommand(HiddenChange);
            sourceFile = new FolderElement{ Name = _file.FullName, Type = _file.Type };
        }

        private void CancelMethod(Window window)
        { 
            if (window != null)
            {
                window.Close();
            }
        }

        public void ReadChange()
        {
            if (sourceFile.Type == ElementType.File)
            {
                try
                {
                    FileInfo file = new FileInfo(sourceFile.FullName);
                    file.IsReadOnly = isReadOnly;
                    IsReadOnly = !isReadOnly;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
                MessageBox.Show("У папки нельзя изменить этот параметр");
        }

        public void HiddenChange()
        {
            if (sourceFile.Type == ElementType.File)
            {
                try
                {
                    if (IsHidden)
                    {
                        var attr = File.GetAttributes(sourceFile.FullName);
                        File.SetAttributes(sourceFile.FullName, attr & ~FileAttributes.Hidden);
                        IsHidden = isHidden;
                    }
                    else
                    {
                        var attr = File.GetAttributes(sourceFile.FullName);
                        File.SetAttributes(sourceFile.FullName, FileAttributes.Hidden);
                        IsHidden = isHidden;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (sourceFile.Type == ElementType.Folder)
            {
                try
                {
                    if (IsHidden)
                    {
                        DirectoryInfo dir = new DirectoryInfo(sourceFile.FullName);
                        dir.Attributes &= ~FileAttributes.Hidden;
                        IsHidden = isHidden;
                    }
                    else
                    {
                        DirectoryInfo dir = new DirectoryInfo(sourceFile.FullName);
                        dir.Attributes = FileAttributes.Hidden;
                        IsHidden = isHidden;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AcceptMethod(Window window)
        {
            if(sourceFile.Name != FileName)
            {

                if (sourceFile.Type == ElementType.File)
                {
                    try
                    {
                        System.IO.File.Move(sourceFile.FullName, Path.Combine(currentFolder, _file.FullName));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                else
                {
                    try
                    {
                        System.IO.Directory.Move(sourceFile.FullName, Path.Combine(currentFolder, _file.FullName));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                var folderInf = new FolderData(currentFolder);
                MainViewModel.GetInstance()._File = new ObservableCollection<FolderElement>(folderInf.GetData()); ;
            }

            if (window != null)
            {
                window.Close();
            }
        }

        public async Task<long> GetFolderSize()
        {
            return await FolderSize(sourceFile.FullName);
        }

        public async Task<long> FolderSize(string pathToDir)
        {
            try
            {
                long size = 0;
                DirectoryInfo dir = new DirectoryInfo(pathToDir);

                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    size += file.Length;
                }

                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo d in dirs)
                {
                    size += await FolderSize(d.FullName);
                }

                return size;
            }
            catch (Exception)
            {
                return 0;
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
