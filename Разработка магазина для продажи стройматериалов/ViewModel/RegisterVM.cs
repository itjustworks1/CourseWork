using MVVM.Model.DTO.Auth;
using MVVM.Services;
using MVVM.View.Admin;
using MVVM.View.NoAdmin;
using MVVM.VMTools;
using System.Windows;
using System.Windows.Controls;

namespace MVVM.ViewModel
{
    public class RegisterVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;
        private Window window;

        private string login = "";
        private string password = string.Empty;
        private string passwordTwo = string.Empty;
        private string error = "";
        private PasswordBox passwordBox;
        private PasswordBox passwordBox2;

        public string Login { get => login; set { login = value; OnPropertyChanged(); } }
        public string Error { get => error; set { error = value; OnPropertyChanged(); } }

        public CommandMvvm RegisterCommand { get; set; }
        public RegisterVM(Window thisWindow, ApiClient apiClient, Window window, PasswordBox password, PasswordBox password2)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;
            this.window = window;
            passwordBox = password;
            passwordBox2 = password2;

            RegisterCommand = new CommandMvvm(RegisterAsync, () => true);
        }
        private async void RegisterAsync()
        {
            if (Login.Length < 3)
            {
                Error = "Длина логина должна быть > 3 символов";
                return;
            }
            if (passwordBox.Password != passwordBox2.Password)
            {
                Error = "Пароли не совпадают";
                return;
            }
            LoginRequest login = new LoginRequest
            {
                Login = Login,
                Password = passwordBox.Password,
            };
            var (data, error) = await apiClient.RegisterAsync(login);
            if (data is null)
            {
                Error = $"{error}";
                return;
            }

            Error = "";
            passwordBox.Password = string.Empty;
            apiClient.SetAccessToken(data.Token);
            var (user, errorMe) = await apiClient.GetMe(data.UserId);
            if (user == null)
            {
                MessageBox.Show(errorMe);
                return;
            }
            apiClient.UserId = user.Id;
            if (user.IsAdmin)
            {
                hide();
                new MainWindowA(apiClient).ShowDialog();
                close();
            }
            else
            {
                hide();
                new MainWindow(apiClient).ShowDialog();
                close();
            }

            //window.Close();
            //new MainWindow(apiClient).Show();
            //close();
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
