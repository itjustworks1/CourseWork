using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MVVM.Model.DTO.Response;

namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    public class ProductAVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private ProductResponse selectedProduct;
        private ObservableCollection<ProductResponse> products = new();
        private ObservableCollection<ProductParameterResponse> productParameters = new();
        private OrderStructureResponse orderStructure = new();

        public OrderStructureResponse OrderStructure { get => orderStructure; set { orderStructure = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductParameterResponse> ProductParameters { get => productParameters; set { productParameters = value; OnPropertyChanged(); } }
        //public ObservableCollection<ProductResponse> Products { get => products; set { products = value; Signal(); } }
        public ProductResponse SelectedProduct { get => selectedProduct; set { selectedProduct = value; OnPropertyChanged(); } }

        public CommandMvvm AddToCart { get; set; }
        public CommandMvvm EditProduct { get; set; }
        public CommandMvvm RemoveProduct { get; set; }
        public CommandMvvm OpenCart { get; set; }

        public ProductAVM(Window thisWindow, ApiClient apiClient, ProductResponse product)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;
            SelectedProduct = product;

            Task.Run(() => SelectAll());
            //AddToCart = new CommandMvvm(() =>
            //{
            //    //new WindowAddToCart(SelectedProduct).ShowDialog();
            //    Task.Run(() => SelectAll());
            //}, () => SelectedProduct != null);

            EditProduct = new CommandMvvm(async () =>
            {
                ProductResponse product1 = new ProductResponse()
                {
                    Id = SelectedProduct.Id,
                    ProductType = SelectedProduct.ProductType,
                    ProductTypeId = SelectedProduct.ProductTypeId,
                    Quantity = SelectedProduct.Quantity,
                    Title = SelectedProduct.Title,
                    Value = SelectedProduct.Value
                };
                hide();
                new WindowAddEditProduct(SelectedProduct, apiClient).ShowDialog();
                //if (!isEdit) SelectedProduct = product1;
                EditSelectProduct();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => true);
            //
            RemoveProduct = new CommandMvvm(() =>
            {
                //new WindowRemoveProduct(SelectedProduct).ShowDialog();
                Task.Run(() => SelectAll());
            }, () => SelectedProduct != null);
            
            //OpenCart = new CommandMvvm(() =>
            //{
            //    hide();
            //    new WindowCart().ShowDialog();
            //    Task.Run(() => SelectAll());
            //    thisWindow.ShowDialog();
            //}, () => true);
        }

        private async Task EditSelectProduct()
        {
            var (prod, error) = await apiClient.GetProduct(SelectedProduct.Id);
            (var type, error) = await apiClient.GetProductType(prod.ProductTypeId);
            SelectedProduct = new ProductResponse
            {
                Id = prod.Id,
                Title = prod.Title,
                Value = prod.Value,
                Quantity = prod.Quantity,
                ProductTypeId = prod.ProductTypeId,
                ProductType = new ProductTypeResponse
                {
                    Id = type.Id,
                    Title = type.Title,
                }
            };
        }
        private async void SelectAll()
        {
            (var listProductParameter, var error) = await apiClient.GetListProductParameter();
            var list = listProductParameter.Where(s => s.ProductId == SelectedProduct.Id).ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                (var parameter, error) = await apiClient.GetParameter(list[i].ParameterId);
                var param = new ParameterResponse
                {
                    Id = list[i].Id,
                    Title = parameter.Title
                };
                list[i].Parameter = param;
                list[i].Product = SelectedProduct;
            }
            ProductParameters = [.. list];

            (var listOrder, error) = await apiClient.GetListOrder();
            ObservableCollection<OrderResponse> orders = new ObservableCollection<OrderResponse>(listOrder);
            OrderResponse order = orders.FirstOrDefault(s => s.Status == false);

            (var listOrderStructure, error) = await apiClient.GetListOrderStructure();
            ObservableCollection<OrderStructureResponse> orderStructures = new ObservableCollection<OrderStructureResponse>(listOrderStructure.Where(s => s.OrderId == order.Id));
            OrderStructure = orderStructures.FirstOrDefault(s => s.ProductId == SelectedProduct.Id);
            if (OrderStructure == null) OrderStructure = new OrderStructureResponse(){ Quantity = 0 };

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
