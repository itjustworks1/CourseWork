using System.Windows;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.ViewModel.NoAdmin;
//using Magaz_Stroitelya.Model;

namespace MVVM.View.NoAdmin
{
    /// <summary>
    /// Логика взаимодействия для WindowRemoveProduct.xaml
    /// </summary>
    public partial class WindowRemoveProduct : Window
    {
        public WindowRemoveProduct(ProductResponse product, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new RemoveProductVM(product, apiClient);
            DataContext = vm;

            vm.SetClose(Close);
        }
    }
}
