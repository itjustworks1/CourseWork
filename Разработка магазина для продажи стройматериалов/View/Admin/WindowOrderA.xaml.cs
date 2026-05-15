using MVVM.Model.DTO.Response;
using MVVM.ViewModel.Admin;
using System.Windows;
using MVVM.Services;

namespace MVVM.View.Admin
{
    /// <summary>
    /// Логика взаимодействия для WindowOrderA.xaml
    /// </summary>
    public partial class WindowOrderA : Window
    {
        public WindowOrderA(OrderResponse order, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new OrderAVM(this, order, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
