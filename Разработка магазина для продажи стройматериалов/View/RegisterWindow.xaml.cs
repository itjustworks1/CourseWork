using System.Windows;
using MVVM.Services;
using MVVM.ViewModel;

namespace MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow(Window window)
        {
            InitializeComponent();
            var vm = new RegisterVM(this, new ApiClient(), window, PBox, PBox2);
            DataContext = vm;
            vm.SetHide(Hide);
            vm.SetClose(Close);
        }
    }
}
