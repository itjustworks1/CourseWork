using MVVM.View.Admin;
using System.Collections.ObjectModel;
using System.Windows;
using MVVM.Model.DTO.Auth;
using MVVM.Services;
using MVVM.VMTools;

namespace MVVM.ViewModel.Admin
{
    public class ListUsersVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow; 
        
        private ObservableCollection<UserResponse> users;
        private UserResponse selectedUser;

        public UserResponse SelectedUser { get => selectedUser; set { selectedUser = value; OnPropertyChanged(); } }
        public ObservableCollection<UserResponse> Users { get => users; set { users = value; OnPropertyChanged(); } }

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
