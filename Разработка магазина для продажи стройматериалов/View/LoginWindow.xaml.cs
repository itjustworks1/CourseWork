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
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.ViewModel;

namespace Magaz_Stroitelya.View
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var vm = new LoginVM(this, new ApiClient());
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
