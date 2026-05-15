using MVVM.ViewModel.Admin;
using System.Windows;
using MVVM.Services;

namespace MVVM.View.Admin
{
    /// <summary>
    /// Логика взаимодействия для ListUsersWindow.xaml
    /// </summary>
    public partial class ListUsersWindow : Window
    {
        public ListUsersWindow(ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new ListUsersVM(this, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
