using System.Windows;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.ViewModel.NoAdmin;
//using Magaz_Stroitelya.Model;

namespace MVVM.View.NoAdmin
{
    /// <summary>
    /// Логика взаимодействия для WindowAddToCart.xaml
    /// </summary>
    public partial class WindowAddToCart : Window
    {
        public WindowAddToCart(ProductResponse product, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new AddToCartVM(product, apiClient);
            DataContext = vm;

            vm.SetClose(Close);
        }
    }
}
