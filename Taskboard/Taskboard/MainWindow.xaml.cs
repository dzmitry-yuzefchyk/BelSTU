using System.Windows;
using Taskboard.Model;
using Taskboard.View;

namespace Taskboard
{
    public partial class MainWindow : Window
    {
        public static CurrentUser User { get; set; } = new CurrentUser();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = User;
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = new AccountView();
        }
        private void TaskboardButton_Click(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = new TeamsView();
        }
    }
}
