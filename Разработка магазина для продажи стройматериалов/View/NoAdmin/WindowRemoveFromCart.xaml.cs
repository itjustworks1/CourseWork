using System.Windows;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.ViewModel.NoAdmin;

namespace MVVM.View.NoAdmin
{
    /// <summary>
    /// Логика взаимодействия для WindowRemoveFromCart.xaml
    /// </summary>
    public partial class WindowRemoveFromCart : Window
    {
        public WindowRemoveFromCart(OrderStructureResponse orderStructure, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new RemoveFromCartVM(orderStructure, apiClient);
            DataContext = vm;

            vm.SetClose(Close);
        }
    }
}
