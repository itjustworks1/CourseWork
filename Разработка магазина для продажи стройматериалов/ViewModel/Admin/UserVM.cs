using Magaz_Stroitelya.DTO.Auth;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;
using MVVM.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public ObservableCollection<OrderResponse> Orders { get => orders; set { orders = value; Signal(); } }
        public ObservableCollection<OrderResponse> OrdersWithoutCart { get => ordersWithoutCart; set { ordersWithoutCart = value; Signal(); } }

        public OrderResponse SelectedOrder { get => selectedOrder; set { selectedOrder = value; Signal(); } }
        public UserResponse SelectedUser { get => selectedUser; set { selectedUser = value; Signal(); } }

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
