using System.Windows;
using System.Windows.Controls;
using MVVM.Services;
using MVVM.View;
using MVVM.View.Admin;
using MVVM.View.NoAdmin;
using MVVM.VMTools;

namespace MVVM.ViewModel
{
    internal class LoginVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;
        private string login = "";
        private PasswordBox passwordBox;
        private string error = "";
        bool isLogin = false;

        public string Login { get => login; set { login = value; OnPropertyChanged(); } }
        public string Error { get => error; set { error = value; OnPropertyChanged(); } }

        public CommandMvvm LoginCommand { get; set; }
        public CommandMvvm RegisterCommand { get; set; }
        public LoginVM(Window thisWindow, ApiClient apiClient, PasswordBox password)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;
            passwordBox = password;

            LoginCommand = new CommandMvvm(LoginAsync, () => true);

            RegisterCommand = new CommandMvvm(RegisterAsync, () => true);
        }
        private async void LoginAsync()
        {
            if (isLogin)
            {
                return;
            }
            isLogin = true;
            var (data, error) = await apiClient.LoginAsync(Login, passwordBox.Password);
            if (data is null)
            {
                Error = $"{error}";
                isLogin = false;
                return;
            }

            Error = "";
            passwordBox.Password = string.Empty; 
            apiClient.SetAccessToken(data.Token);
            var (user, errorMe) = await apiClient.GetMe(data.UserId);
            if (user == null)
            {
                MessageBox.Show(errorMe);
                isLogin = false;
                return;
            }
            apiClient.UserId = user.Id;
            if (user.IsAdmin)
            {
                hide();
                new MainWindowA(apiClient).ShowDialog();
                isLogin = false;
                thisWindow.ShowDialog();
            }
            else
            {
                hide();
                new MainWindow(apiClient).ShowDialog();
                isLogin = false;
                thisWindow.ShowDialog();
            }
            isLogin = false;
        }
        private void RegisterAsync()
        {
            hide();
            new RegisterWindow(thisWindow).ShowDialog();
            thisWindow.ShowDialog();
        }

        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
