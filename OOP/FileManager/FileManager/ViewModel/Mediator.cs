using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModel
{
    public static class Mediator
    {
        public enum ActionType
        {
            Copy,
            Cut,
            Zip,
            UnZip
        }

        private static FolderElement sourceFile;
        private static string destinationFile;
        
        public static FolderElement SourceFile
        {
            get => sourceFile;
            set => sourceFile = value;
        }

        public static string DestionationFile
        {
            get => destinationFile;
            set => destinationFile = value;
        }

        public static FolderElement DeleteFile
        {
            get;
            set;
        }

        public static string CurrentFolder
        {
            get;
            set;
        }

        public static ActionType Type { get; set; }
    }
}
