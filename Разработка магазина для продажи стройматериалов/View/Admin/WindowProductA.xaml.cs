using System.Windows;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.ViewModel.Admin;

namespace MVVM.View.Admin
{
    /// <summary>
    /// Логика взаимодействия для WindowProduct.xaml
    /// </summary>
    public partial class WindowProductA : Window
    {
        public WindowProductA(ProductResponse product, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new ProductAVM(this, apiClient, product);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
