using System.Windows;
using Taskboard.ViewModel;

namespace Taskboard.View
{
    /// <summary>
    /// Interaction logic for ProjectView.xaml
    /// </summary>
    public partial class ProjectView : Window
    {
        public ProjectView(int projectId, int teamId)
        {
            InitializeComponent();
            DataContext = new ProjectViewModel(projectId, teamId);
        }
    }
}
