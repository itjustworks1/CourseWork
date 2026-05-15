using MVVM.Model.DTO.Response;
using MVVM.View.Admin;
using System.Collections.ObjectModel;
using System.Windows;
using MVVM.Model.DTO.Auth;
using MVVM.Services;
using MVVM.VMTools;

namespace MVVM.ViewModel.Admin
{
    public class UserVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private UserResponse selectedUser;
        private OrderResponse selectedOrder;
        private ObservableCollection<OrderResponse> orders = new();
        private ObservableCollection<OrderResponse> ordersWithoutCart = new();

        public ObservableCollection<OrderResponse> Orders { get => orders; set { orders = value; OnPropertyChanged(); } }
        public ObservableCollection<OrderResponse> OrdersWithoutCart { get => ordersWithoutCart; set { ordersWithoutCart = value; OnPropertyChanged(); } }

        public OrderResponse SelectedOrder { get => selectedOrder; set { selectedOrder = value; OnPropertyChanged(); } }
        public UserResponse SelectedUser { get => selectedUser; set { selectedUser = value; OnPropertyChanged(); } }

        //public CommandMvvm EditOrder { get; set; }//s?
        public CommandMvvm OpenOrder { get; set; }

        public UserVM(Window thisWindow, UserResponse user, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            SelectedUser = user;
            Task.Run(() => SelectAll());
            //EditOrder = new CommandMvvm(() =>//s
            //{
            //    close();
            //}, () => true);

            OpenOrder = new CommandMvvm(() =>
            {
                hide();
                new WindowOrderA(SelectedOrder, apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => SelectedOrder != null);

        }

        private async void SelectAll()
        {
            await SelectOrdersAsync();
            //s
            await SelectOrdersWithoutCartAsync();
        }
        public async Task SelectOrdersAsync()
        {
            var (list, error) = await apiClient.GetListOrder();
            var listOrder = new ObservableCollection<OrderResponse>(list.Where(s => s.UserId == SelectedUser.Id).OrderByDescending(t => t.Date));
            Orders = listOrder;
        }
        public async Task SelectOrdersWithoutCartAsync()
        {
            var (list, error) = await apiClient.GetListOrder();
            var listOrder = new ObservableCollection<OrderResponse>(list.Where(s => s.UserId == SelectedUser.Id && s.Status == true).OrderByDescending(t => t.Date).Reverse());
            OrdersWithoutCart = listOrder;
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
