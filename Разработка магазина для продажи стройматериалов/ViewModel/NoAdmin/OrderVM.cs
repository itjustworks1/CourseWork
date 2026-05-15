//using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
//using Magaz_Stroitelya.DB;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    public class OrderVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private OrderResponse selectedOrder;
        private ObservableCollection<OrderResponse> orders = new();
        private ObservableCollection<OrderStructureResponse> orderStructures = new();
        private decimal value;
        private OrderStructureResponse selectedOrderStructure;

        public OrderStructureResponse SelectedOrderStructure { get => selectedOrderStructure; set { selectedOrderStructure = value; OnPropertyChanged(); } }
        public decimal Value { get => value; set { this.value = value; OnPropertyChanged(); } }
        public ObservableCollection<OrderStructureResponse> OrderStructures { get => orderStructures; set { orderStructures = value; OnPropertyChanged(); } }
        public ObservableCollection<OrderResponse> Orders { get => orders; set { orders = value; OnPropertyChanged(); } }
        public OrderResponse SelectedOrder { get => selectedOrder; set { selectedOrder = value; OnPropertyChanged(); } }

        public CommandMvvm RemoveOrder { get; set; }
        public CommandMvvm OpenProduct { get; set; }
        //public CommandMvvm EditOrder { get; set; }

        public OrderVM(Window thisWindow, OrderResponse order, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            SelectedOrder = order;
            Task.Run(() => SelectAll());

            RemoveOrder = new CommandMvvm(async () =>
            {
                var (list, error) = await apiClient.GetListOrderStructure();
                var rOrderStructures = new ObservableCollection<OrderStructureResponse>(list.Where(s => s.OrderId == SelectedOrder.Id));
                foreach (var o in rOrderStructures)
                    await apiClient.DeleteOrderStructure(o.Id);
                await apiClient.DeleteOrder(SelectedOrder.Id);
                close();
            }, () => SelectedOrder != null);

            OpenProduct = new CommandMvvm(() =>
            {
                hide();
                new WindowProduct(SelectedOrderStructure.Product, apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => SelectedOrderStructure != null);

            //EditOrder = new CommandMvvm(() =>
            //{
            //    SelectAll();
            //}, () => SelectedOrder != null);

        }

        private async void SelectAll()
        {
            await SelectOrdersAsync();
            await SelectOrderStructuresAsync();
            decimal summ = 0;
            foreach (var o in OrderStructures)
                summ += o.Value * o.Quantity;
            Value = summ;
        }
        public async Task SelectOrdersAsync()
        {
            var (list, error) = await apiClient.GetListOrder();
            var listOrder = new ObservableCollection<OrderResponse>(list.Where(s => s.UserId == apiClient.UserId));
            Orders = listOrder;
        }

        public async Task SelectOrderStructuresAsync()
        {
            var (list, error) = await apiClient.GetListOrderStructure();
            var listOrderStructure = new ObservableCollection<OrderStructureResponse>(list);
            for (int i = 0; i < listOrderStructure.Count; i++)
            {
                (var product, error) = await apiClient.GetProduct(listOrderStructure[i].ProductId);
                (var type, error) = await apiClient.GetProductType(product.ProductTypeId);
                var prod = new ProductResponse
                {
                    Id = product.Id,
                    Title = product.Title,
                    Value = product.Value,
                    Quantity = product.Quantity,
                    ProductTypeId = type.Id,
                    ProductType = type,
                };
                (var order, error) = await apiClient.GetOrder(listOrderStructure[i].OrderId);
                var ord = new OrderResponse
                {
                    Id = order.Id,
                    Date = order.Date,
                    Status = order.Status,
                    UserId = order.UserId,
                };
                listOrderStructure[i].Product = prod;
                listOrderStructure[i].Order = ord;
            }
            OrderStructures = [.. listOrderStructure.Where(s => s.OrderId == SelectedOrder.Id).OrderByDescending(t => t.Product.Title)];

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
