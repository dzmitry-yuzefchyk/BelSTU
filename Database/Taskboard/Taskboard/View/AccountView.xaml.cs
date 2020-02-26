using System.Windows.Controls;
using Taskboard.ViewModel;

namespace Taskboard.View
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
            this.DataContext = new AccountViewModel();
        }
    }
}
