using System.Windows;
using Taskboard.ViewModel;

namespace Taskboard.View
{
    /// <summary>
    /// Interaction logic for TeamView.xaml
    /// </summary>
    public partial class TeamView : Window
    {
        public TeamView(int teamId)
        {
            InitializeComponent();
            DataContext = new TeamViewModel(teamId);
        }
    }
}
