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
using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.ViewModel;
using Magaz_Stroitelya.ViewModel.NoAdmin;
using MVVM.Model.DTO.Response;

namespace Magaz_Stroitelya.View
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
