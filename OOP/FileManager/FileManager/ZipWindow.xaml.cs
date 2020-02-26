using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FileManager.ViewModel;
using MahApps.Metro.Controls;

namespace FileManager
{
    /// <summary>
    /// Логика взаимодействия для ZipWindow.xaml
    /// </summary>
    public partial class ZipWindow : MetroWindow
    {
        public ZipWindow()
        {
            InitializeComponent();

            this.DataContext = new ZipViewModel();
        }
    }
}
