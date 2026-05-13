using Magaz_Stroitelya.DTO.Auth;
using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.ViewModel.NoAdmin;
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
    /// Логика взаимодействия для ListUsersWindow.xaml
    /// </summary>
    public partial class ListUsersWindow : Window
    {
        public ListUsersWindow(ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new ListUsersVM(this, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
