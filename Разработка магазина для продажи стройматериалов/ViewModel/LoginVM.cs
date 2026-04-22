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
    internal class LoginVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;
        private string login = "";
        private string password = string.Empty;
        private string error = "";

        public string Login { get => login; set { login = value; Signal(); } }
        public string Password { get => password; set { password = value; Signal(); } }
        public string Error { get => error; set { error = value; Signal(); } }

        public ActionCommandMvvm LoginCommand { get; set; }
        public ActionCommandMvvm RegisterCommand { get; set; }
        public LoginVM(Window thisWindow, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            LoginCommand = new ActionCommandMvvm(LoginAsync, () => true);
            RegisterCommand = new ActionCommandMvvm(RegisterAsync, () => true);
        }
        private async Task LoginAsync()
        {
            var (data, error) = await apiClient.LoginAsync(Login, Password);
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
                hide();
                new MainWindowA(apiClient).ShowDialog();
                thisWindow.ShowDialog();
            }
            else
            {
                hide();
                new MainWindow(apiClient).ShowDialog();
                thisWindow.ShowDialog();
            }
        }
        private async Task RegisterAsync()
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
