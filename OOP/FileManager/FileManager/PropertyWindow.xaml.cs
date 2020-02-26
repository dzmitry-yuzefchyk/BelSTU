using System.Windows;
using FileManager.ViewModel;
using MahApps.Metro.Controls;

namespace FileManager
{
    /// <summary>
    /// Логика взаимодействия для PropertyView.xaml
    /// </summary>
    public partial class PropertyWindow : MetroWindow
    {
        public PropertyWindow()
        {
            InitializeComponent();

            this.DataContext = new PropertyViewModel();
        }
    }
}
