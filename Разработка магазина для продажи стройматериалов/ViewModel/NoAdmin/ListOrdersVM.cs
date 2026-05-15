using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using MVVM.Model.DTO.Requests;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.View.NoAdmin;
using MVVM.VMTools;
//using Magaz_Stroitelya.Model;
//using Magaz_Stroitelya.DB;

namespace MVVM.ViewModel.NoAdmin
{
    public partial class ListOrdersVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private OrderResponse selectedOrder;
        private ObservableCollection<OrderResponse> orders = new();
        private ObservableCollection<OrderResponse> ordersWithoutCart = new();

        public ObservableCollection<OrderResponse> Orders { get => orders; set { orders = value; OnPropertyChanged(); } }
        public ObservableCollection<OrderResponse> OrdersWithoutCart { get => ordersWithoutCart; set { ordersWithoutCart = value; OnPropertyChanged(); } }
        
        public OrderResponse SelectedOrder { get => selectedOrder; set { selectedOrder = value; OnPropertyChanged(); } }

        public CommandMvvm EditOrder { get; set; }
        public CommandMvvm OpenOrder { get; set; }

        [RelayCommand]
        private async Task EditOrder2(OrderResponse order)
        {
            if (MessageBox.Show("Корзина будет очищена.\nПродолжить?", "Подтверждение", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var (listOrder, error) = await apiClient.GetListOrder();
                (var listOrderStructure, error) = await apiClient.GetListOrderStructure();
                var rOrder = new ObservableCollection<OrderResponse>(listOrder).FirstOrDefault(s => s.Status == false && s.UserId == apiClient.UserId);
                var rOrderStructures = new ObservableCollection<OrderStructureResponse>(listOrderStructure.Where(s => s.OrderId == rOrder.Id));
                foreach (var o in rOrderStructures)
                    await apiClient.DeleteOrderStructure(o.Id);
                await apiClient.DeleteOrder(rOrder.Id);
                order.Status = false;
                await apiClient.PatchOrder(order.Id, new OrderRequest
                {
                    Date = order.Date,
                    UserId = order.UserId,
                    Status = order.Status
                });
                close();
            }
        }

        public ListOrdersVM(Window thisWindow, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            Task.Run(() => SelectAll());
            EditOrder = new CommandMvvm(() =>
            {
                close();
            }, () => true);

            OpenOrder = new CommandMvvm(() =>
            {
                hide();
                new WindowOrder(SelectedOrder, apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => SelectedOrder != null);

        }

        private async void SelectAll()
        {
            await SelectOrdersAsync();
            await SelectOrdersWithoutCartAsync();
        }
        public async Task SelectOrdersAsync()
        {
            var (list, error) = await apiClient.GetListOrder();
            var listOrder = new ObservableCollection<OrderResponse>(list.Where(s => s.UserId == apiClient.UserId).OrderByDescending(t => t.Date));
            Orders = listOrder;
        }
        public async Task SelectOrdersWithoutCartAsync()
        {
            var (list, error) = await apiClient.GetListOrder();
            var listOrder = new ObservableCollection<OrderResponse>(list.Where(s => s.UserId == apiClient.UserId && s.Status == true).OrderByDescending(t => t.Date).Reverse());
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
