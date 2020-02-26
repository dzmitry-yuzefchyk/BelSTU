using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Model
{

    public class Log
    {
        public int ID { get; set; }
        public string FilePath { get; set; }
        public string Action { get; set; }
        public string DateTime { get; set; }

        public Log(string filePath, string action, string dateTime)
        {
            FilePath = filePath;
            Action = action;
            DateTime = dateTime;
        }

        public Log() { }
    }
}
