using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Magaz_Stroitelya.DTO.Auth;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;

namespace Magaz_Stroitelya.ViewModel
{
    internal class RegisterVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;
        private Window window;

        private string login = "";
        private string password = string.Empty;
        private string passwordTwo = string.Empty;
        private string error = "";

        public string Login { get => login; set { login = value; Signal(); } }
        public string Password { get => password; set { password = value; Signal(); } }
        public string PasswordTwo { get => passwordTwo; set { passwordTwo = value; Signal(); } }
        public string Error { get => error; set { error = value; Signal(); } }

        public ActionCommandMvvm RegisterCommand { get; set; }
        public RegisterVM(Window thisWindow, ApiClient apiClient, Window window)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;
            this.window = window;

            RegisterCommand = new ActionCommandMvvm(RegisterAsync, () => true);
        }
        private async Task RegisterAsync()
        {
            if (Login.Length < 3)
            {
                Error = "Длина логина должна быть > 3 символов";
                return;
            }
            if (Password != PasswordTwo)
            {
                Error = "Пароли не совпадают";
                return;
            }
            LoginRequest login = new LoginRequest
            {
                Login = Login,
                Password = Password,
            };
            var (data, error) = await apiClient.RegisterAsync(login);
            if (data is null)
            {
                Error = $"{error}";
                return;
            }

            Error = "";
            Password = string.Empty;
            apiClient.SetAccessToken(data.Token);
            var (user, errorMe) = await apiClient.GetMe(data.UserId);
            if (user == null)
            {
                MessageBox.Show(errorMe);
                return;
            }
            if (user.IsAdmin)
            {
                window.Close();
                new MainWindowA(apiClient).Show();
                close();
            }
            else
            {
                window.Close();
                new MainWindow(apiClient).Show();
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
