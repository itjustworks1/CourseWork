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
using Magaz_Stroitelya.ViewModel;
using Magaz_Stroitelya.ViewModel.NoAdmin;

namespace Magaz_Stroitelya.View
{
    /// <summary>
    /// Логика взаимодействия для WindowRemoveProduct.xaml
    /// </summary>
    public partial class WindowRemoveProduct : Window
    {
        public WindowRemoveProduct(Product product)
        {
            InitializeComponent();
            var vm = new RemoveProductVM(product);
            DataContext = vm;

            vm.SetClose(Close);
        }
    }
}
