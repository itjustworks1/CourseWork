using System.Windows;
using MVVM.Services;
using MVVM.ViewModel.NoAdmin;

namespace MVVM.View.NoAdmin
{
    /// <summary>
    /// Логика взаимодействия для WindowListOrders.xaml
    /// </summary>
    public partial class WindowListOrders : Window
    {
        public WindowListOrders(ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new ListOrdersVM(this, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
