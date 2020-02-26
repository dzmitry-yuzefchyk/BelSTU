using System.Windows;
using Taskboard.ViewModel;

namespace Taskboard.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        public BoardView(int boardId, int teamId)
        {
            InitializeComponent();
            DataContext = new BoardViewModel(boardId, teamId);
        }
    }
}
