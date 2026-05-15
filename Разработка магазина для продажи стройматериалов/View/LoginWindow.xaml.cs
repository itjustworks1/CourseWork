using System.Windows;
using MVVM.Services;
using MVVM.ViewModel;

namespace MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var vm = new LoginVM(this, new ApiClient(), PBox);//пароль надо
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
