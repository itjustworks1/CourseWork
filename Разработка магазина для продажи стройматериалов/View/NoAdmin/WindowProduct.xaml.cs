using System.Windows;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.ViewModel.NoAdmin;
//using Magaz_Stroitelya.Model;

namespace MVVM.View.NoAdmin
{
    /// <summary>
    /// Логика взаимодействия для WindowProduct.xaml
    /// </summary>
    public partial class WindowProduct : Window
    {
        public WindowProduct(ProductResponse product, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new ProductVM(this, apiClient, product);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
