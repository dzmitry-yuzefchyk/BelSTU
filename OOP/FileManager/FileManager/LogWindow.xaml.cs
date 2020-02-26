using FileManager.ViewModel;
using MahApps.Metro.Controls;

namespace FileManager
{
    /// <summary>
    /// Логика взаимодействия для LogWindow.xaml
    /// </summary>
    public partial class LogWindow : MetroWindow
    {
        public LogWindow()
        {
            InitializeComponent();

            this.DataContext = new LogViewModel();
        }
    }
}
