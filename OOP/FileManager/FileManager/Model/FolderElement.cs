using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FileManager.ViewModel
{
    public enum ElementType
    {
        Folder,
        File
    }

    public class FolderElement
    {
        private string name;

        public string Name
        {
            get => name.Split('\\').Last();
            set => name = value ;
        }

        public string Extension
        {
            get
            {
                if (this.Type == ElementType.File)
                    return name.Split('\\', '.').Last();
                else
                    return "Папка с файлами";
            } 
        }
        public ElementType Type { get; set; }

        public string FullName
        {
            get => name;
            set => name = value;
        }

        public BitmapImage Image
        {
            get
            {
                if (this.Type == ElementType.File)
                {
                    string extension = this.Extension;
                    try
                    {
                        return new BitmapImage(new Uri(Environment.CurrentDirectory + "\\png\\" + extension + ".png"));
                    }
                    catch
                    {
                        return new BitmapImage(new Uri(Environment.CurrentDirectory + "\\png\\file.png"));
                    }
                }
                else
                    return new BitmapImage(new Uri(Environment.CurrentDirectory + "\\png\\folder.ico"));
            }
        }

        public string Size
        {
            get
            {
                if (new System.IO.FileInfo(name).Length < 1024)
                {
                    return new System.IO.FileInfo(name).Length.ToString() + " Byte";
                }

                else if (new System.IO.FileInfo(name).Length > 1024 && new System.IO.FileInfo(name).Length < 1024 * 1024)
                {
                    return ((float)(new System.IO.FileInfo(name).Length / 1024)).ToString() + " KB";
                }

                else if (new System.IO.FileInfo(name).Length > 1024 * 1024 && new System.IO.FileInfo(name).Length < 1024 * 1024 * 1024)
                {
                    return ((float)(new System.IO.FileInfo(name).Length / (1024 * 1024))).ToString() + " MB";
                }

                else
                    return ((float)(new System.IO.FileInfo(name).Length / (1024 * 1024 * 1024))).ToString() + " GB";
            } 
        }
    }

    public class FolderData
    {
        private readonly string _folderpath;

        public string FolderPath
        {
            get => _folderpath;
        }

        public FolderData(string folderpath)
        {
            _folderpath = folderpath;
        }

        private IEnumerable<FolderElement> GetFiles()
        {
            return Directory.GetFiles(_folderpath).Select(x => new FolderElement { Name = x, Type = ElementType.File });
        }

        private IEnumerable<FolderElement> GetFolders()
        {
            return Directory.GetDirectories(_folderpath).Select(x => new FolderElement { Name = x, Type = ElementType.Folder });
        }

        public IEnumerable<FolderElement> GetData()
        {
            try
            {
                return GetFiles().Union(GetFolders());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Enumerable.Empty<FolderElement>();
            }
        }
    }
}
