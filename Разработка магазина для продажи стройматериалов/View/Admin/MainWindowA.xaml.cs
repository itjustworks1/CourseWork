using System.Windows;
using MVVM.Services;
using MVVM.ViewModel.Admin;

namespace MVVM.View.Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowA : Window
    {
        public MainWindowA(ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new MainAVM(this, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
        }
    }
}