using System.Windows;
using MVVM.Services;
using MVVM.ViewModel.Admin;

namespace MVVM.View.Admin
{
    /// <summary>
    /// Логика взаимодействия для WindowRelationshipsParametersToType.xaml
    /// </summary>
    public partial class WindowAddEditProductType : Window
    {
        public WindowAddEditProductType(ApiClient apiClient)
        {
            InitializeComponent();
            var vm = new AddEditProductTypeAVM(this, apiClient);
            DataContext = vm;

            vm.SetHide(Hide);
        }
    }
}
