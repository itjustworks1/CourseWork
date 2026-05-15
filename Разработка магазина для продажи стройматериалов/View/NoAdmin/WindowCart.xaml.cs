using System.Windows;
using MVVM.Services;
using MVVM.ViewModel.NoAdmin;

namespace MVVM.View.NoAdmin
{
    /// <summary>
    /// Логика взаимодействия для WindowCart.xaml
    /// </summary>
    public partial class WindowCart : Window
    {
        public WindowCart(ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new CartVM(this, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
        }
    }
}
