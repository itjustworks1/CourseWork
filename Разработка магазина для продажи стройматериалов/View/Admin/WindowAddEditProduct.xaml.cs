using System.Windows;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.ViewModel.Admin;

namespace MVVM.View.Admin
{
    /// <summary>
    /// Логика взаимодействия для WindowAddEditProduct.xaml
    /// </summary>
    public partial class WindowAddEditProduct : Window
    {
        public WindowAddEditProduct(ProductResponse product, ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new AddEditProductAVM(this, apiClient, product);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
            //((AddAuthorMvvm)this.DataContext).SetAuthor(author);
        }
    }
}
