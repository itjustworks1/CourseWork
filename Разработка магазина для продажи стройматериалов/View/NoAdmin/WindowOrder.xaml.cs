using System.Windows;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.ViewModel.NoAdmin;

namespace MVVM.View.NoAdmin
{
    /// <summary>
    /// Логика взаимодействия для WindowOrder.xaml
    /// </summary>
    public partial class WindowOrder : Window
    {
        public WindowOrder(OrderResponse order, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new OrderVM(this, order, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
