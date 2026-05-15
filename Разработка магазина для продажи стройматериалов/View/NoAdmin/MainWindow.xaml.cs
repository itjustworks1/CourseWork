using System.Windows;
using MVVM.Services;
using MVVM.ViewModel.NoAdmin;

namespace MVVM.View.NoAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new MainVM(this, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
        }
    }
}