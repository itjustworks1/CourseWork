using System.Windows;
using MVVM.Services;
using MVVM.ViewModel.Admin;

namespace MVVM.View.Admin
{
    /// <summary>
    /// Логика взаимодействия для WindowRelationshipsTypesToParameter.xaml
    /// </summary>
    public partial class WindowAddEditParameter : Window
    {
        public WindowAddEditParameter(ApiClient apiClient)
        {
            InitializeComponent();
            DataContext = new AddEditParameterAVM(apiClient);
        }
    }
}
