using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileManager.Model;

namespace FileManager.DataBase
{
    class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<Log> Logs { get; set; }
    }
}
