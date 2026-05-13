using Magaz_Stroitelya.DTO.Auth;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;
using MVVM.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModel.Admin
{
    public class ListUsersVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow; 
        
        private ObservableCollection<UserResponse> users;
        private UserResponse selectedUser;

        public UserResponse SelectedUser { get => selectedUser; set { selectedUser = value; Signal(); } }
        public ObservableCollection<UserResponse> Users { get => users; set { users = value; Signal(); } }

        public CommandMvvm OpenUser { get; set; }

        public ListUsersVM(Window thisWindow, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            Task.Run(() => SelectAll());

            OpenUser = new CommandMvvm(() =>
            {
                hide();
                new UserWindow(SelectedUser, apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => SelectedUser != null);
        }

        public async Task SelectAll()
        {
            var (list, error) = await apiClient.GetListUsers();
            Users = new ObservableCollection<UserResponse>(list.Where(s => s.IsAdmin == false));
        }
        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
    }
}
