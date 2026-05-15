using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Model.DTO.Requests;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.View.NoAdmin;
using MVVM.VMTools;
//using Magaz_Stroitelya.DB;
//using Magaz_Stroitelya.Model;

namespace MVVM.ViewModel.NoAdmin
{
    public partial class CartVM : ObservableObject
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private ObservableCollection<OrderResponse> orders = new();
        //private ObservableCollection<OrderStructureResponse> orderStructures;
        private OrderStructureResponse selectedOrderStructure;
        private ObservableCollection<ProductResponse> products = new();

        [ObservableProperty] private ObservableCollection<OrderStructureResponse> _orderStructures;
        //public ObservableCollection<OrderStructureResponse> OrderStructures { get => orderStructures; set { orderStructures = value; Signal(); } }
        public ObservableCollection<OrderResponse> Orders { get => orders; set { orders = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductResponse> Products { get => products; set { products = value; OnPropertyChanged(); } }
        public OrderStructureResponse SelectedOrderStructure { get => selectedOrderStructure; set { selectedOrderStructure = value; OnPropertyChanged(); } }

        public CommandMvvm PlaceAnOrder { get; set; }
        public CommandMvvm RemoveFromCart { get; set; }
        public CommandMvvm OpenOrder { get; set; }
        public CommandMvvm OpenProduct { get; set; }
        public CommandMvvm Close { get; set; }

        [RelayCommand]
        private async Task OpenOrder2()
        {
            hide();
            new WindowListOrders(apiClient).ShowDialog();
            Thread.Sleep(2000);
            Task.Run(() => SelectAll());
            thisWindow.ShowDialog();
        }

        [RelayCommand]
        private async Task RemoveFromCart2(OrderStructureResponse orderStructure)
        {
            hide();
            new WindowRemoveFromCart(orderStructure, apiClient).ShowDialog();
            Thread.Sleep(2000);
            Task.Run(() => SelectAll());
            thisWindow.ShowDialog();
        }

        public CartVM(Window thisWindow, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            Task.Run(() => SelectAll());
            PlaceAnOrder = new CommandMvvm(async () =>
            {
                OrderResponse order = Orders.FirstOrDefault(s => s.Status == false);
                order.Status = true;
                order.Date = DateTime.Now;
                await apiClient.PatchOrder(order.Id, new OrderRequest
                {
                    Date = order.Date,
                    Status = order.Status,
                    UserId = order.UserId,
                });
                //NewOrder();
                await Task.Run(() => SelectAll());
            }, () => OrderStructures != null && OrderStructures.Count != 0);

            RemoveFromCart = new CommandMvvm(() =>
            {
                Task.Run(() => SelectAll());
            }, () => true);

            OpenOrder = new CommandMvvm(() =>
            {
                hide();
                new WindowListOrders(apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => true);

            OpenProduct = new CommandMvvm(() =>
            {
                hide();
                new WindowProduct(SelectedOrderStructure.Product, apiClient).ShowDialog();
                thisWindow.ShowDialog();
                Task.Run(() => SelectAll());
            }, () => SelectedOrderStructure != null);
        }

        private async void SelectAll()
        {
            await SelectOrderStructures();
            await SelectProductsAsync();
            await SelectOrdersAsync();
        }
        public async Task SelectOrderStructures()
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
            OrderStructures = [.. listOrderStructure.Where(s => s.Order.Status == false && s.Order.UserId == apiClient.UserId).OrderByDescending(t => t.Product.Title)];
            OnPropertyChanged(nameof(OrderStructures));

        }
        public async Task SelectProductsAsync()
        {
            var (list, error) = await apiClient.GetListProduct();
            var listProduct = new ObservableCollection<ProductResponse>(list);
            for (int i = 0; i < listProduct.Count; i++)
            {
                (var productType, error) = await apiClient.GetProductType(listProduct[i].ProductTypeId);
                var type = new ProductTypeResponse
                {
                    Id = productType.Id,
                    Title = productType.Title
                };
                listProduct[i].ProductType = type;
            }
            Products = listProduct;
        }
        public async Task SelectOrdersAsync()
        {
            var (list, error) = await apiClient.GetListOrder();
            var listOrder = new ObservableCollection<OrderResponse>(list.Where(s => s.UserId == apiClient.UserId));
            Orders = listOrder;
            if (Orders.FirstOrDefault(s => s.Status == false) == null)
            {
                NewOrder();
            }
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }

        public async void NewOrder()
        {
            await apiClient.PostOrder(new OrderRequest
            {
                Date = DateTime.Now,
                Status = false,
                UserId = apiClient.UserId,
            });
        }
    }
}
