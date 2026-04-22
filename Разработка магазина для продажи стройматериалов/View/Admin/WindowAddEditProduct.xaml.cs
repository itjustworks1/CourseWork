using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.ViewModel;
using Magaz_Stroitelya.ViewModel.Admin;
using Magaz_Stroitelya.ViewModel.NoAdmin;
using MVVM.Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Magaz_Stroitelya.View
{
    /// <summary>
    /// Логика взаимодействия для WindowAddEditProduct.xaml
    /// </summary>
    public partial class WindowAddEditProduct : Window
    {
        public WindowAddEditProduct(ProductResponse product, ApiClient apiClient, ref bool isEdit)
        {
            InitializeComponent();
            var vm = new AddEditProductAVM(this, apiClient, product, ref isEdit);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
            //((AddAuthorMvvm)this.DataContext).SetAuthor(author);
        }
    }
}
