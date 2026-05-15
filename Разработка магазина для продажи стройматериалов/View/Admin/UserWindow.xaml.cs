using MVVM.ViewModel.Admin;
using System.Windows;
using MVVM.Model.DTO.Auth;
using MVVM.Services;

namespace MVVM.View.Admin
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow(UserResponse user, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new UserVM(this, user, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
