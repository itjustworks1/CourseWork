using Magaz_Stroitelya.Services;
using MVVM.Model.DTO.Response;
using MVVM.ViewModel.Admin;
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
