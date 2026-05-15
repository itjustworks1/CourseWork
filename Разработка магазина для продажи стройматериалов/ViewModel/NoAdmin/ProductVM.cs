using System.Collections.ObjectModel;
using System.Windows;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.View.NoAdmin;
using MVVM.VMTools;
//using Magaz_Stroitelya.Model;
//using Magaz_Stroitelya.DB;

namespace MVVM.ViewModel.NoAdmin
{
    public class ProductVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private ProductResponse selectedProduct;
        private ObservableCollection<ProductResponse> products = new();
        private ObservableCollection<ProductParameterResponse> productParameters = new();
        private OrderStructureResponse orderStructure = new();

        public OrderStructureResponse OrderStructure { get => orderStructure; set { orderStructure = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductParameterResponse> ProductParameters { get => productParameters; set { productParameters = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductResponse> Products { get => products; set { products = value; OnPropertyChanged(); } }
        public ProductResponse SelectedProduct { get => selectedProduct; set { selectedProduct = value; OnPropertyChanged(); } }

        public CommandMvvm AddToCart { get; set; }
        public CommandMvvm EditProduct { get; set; }
        public CommandMvvm RemoveProduct { get; set; }
        public CommandMvvm OpenCart { get; set; }

        public ProductVM(WindowProduct thisWindow, ApiClient apiClient, ProductResponse product)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            SelectedProduct = product;
            Task.Run(() => SelectAll());
            AddToCart = new CommandMvvm(() =>
            {
                new WindowAddToCart(SelectedProduct, apiClient).ShowDialog();
                Task.Run(() => SelectAll());
            }, () => SelectedProduct != null);

            //EditProduct = new CommandMvvm(() =>
            //{
            //    //Product product1 = new Product() 
            //    //{ 
            //    //    Id = SelectedProduct.Id,
            //    //    ProductType = SelectedProduct.ProductType,
            //    //    ProductTypeId = SelectedProduct.ProductTypeId,
            //    //    Quantity = SelectedProduct.Quantity,
            //    //    Title = SelectedProduct.Title,
            //    //    Value = SelectedProduct.Value 
            //    //};
            //    bool isEdit = false;
            //    hide();
            //    //new WindowAddEditProduct(SelectedProduct, ref isEdit).ShowDialog();
            //    //if (!isEdit) SelectedProduct = product1;
            //    Task.Run(() => SelectAll());
            //    thisWindow.ShowDialog();
            //}, () => true);

            //RemoveProduct = new CommandMvvm(() =>
            //{
            //    new WindowRemoveProduct(SelectedProduct, apiClient).ShowDialog();
            //    Task.Run(() => SelectAll());
            //}, () => SelectedProduct != null);
            
            OpenCart = new CommandMvvm(() =>
            {
                hide();
                new WindowCart(apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => true);
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
            if (OrderStructure == null) OrderStructure = new OrderStructureResponse() { Quantity = 0 };

            //await SelectProductsAsync();
            //await SelectProductParametersAsync();
            //await SelectOrderStructure();
            //if (OrderStructure == null) OrderStructure = new OrderStructureResponse() { Quantity = 0 };
        }
        public async Task SelectProductsAsync()
        {
            var (list, error) = await apiClient.GetListProduct();
            var listProduct = new ObservableCollection<ProductResponse>(list);
            for (int i = 0; i < list.Count; i++)
            {
                (var productType, error) = await apiClient.GetParameter(listProduct[i].ProductTypeId);
                var type = new ProductTypeResponse
                {
                    Id = listProduct[i].Id,
                    Title = productType.Title
                };
                listProduct[i].ProductType = type;
            }
            Products = listProduct;
        }
        public async Task SelectProductParametersAsync()
        {
            var (list, error) = await apiClient.GetListProductParameter();
            var listProductParameter = new ObservableCollection<ProductParameterResponse>(list);

            for (int i = 0; i < list.Count; i++)
            {
                (var listIProductType, error) = await apiClient.GetListProductType();
                var listProductType = new ObservableCollection<ProductTypeResponse>(listIProductType);

                (var product, error) = await apiClient.GetProduct(listProductParameter[i].ProductId);
                var prod = new ProductResponse
                {
                    Id = product.Id,
                    Title = product.Title,
                    Value = product.Value,
                    Quantity = product.Quantity,
                    ProductTypeId = product.ProductTypeId,
                    ProductType = listProductType.FirstOrDefault(s => s.Id == product.ProductTypeId),
                };
                (var parameter, error) = await apiClient.GetParameter(listProductParameter[i].ParameterId);
                var param = new ParameterResponse
                {
                    Id = parameter.Id,
                    Title = parameter.Title
                };
                listProductParameter[i].Product = prod;
                listProductParameter[i].Parameter = param;
            }
            ProductParameters = [..listProductParameter.Where(s => s.ProductId == SelectedProduct.Id)];
        }
        public async Task SelectOrderStructure()
        {
            var (list, error) = await apiClient.GetListOrder();
            ObservableCollection<OrderResponse> orders = new ObservableCollection<OrderResponse>(list);
            OrderResponse order = orders.FirstOrDefault(s => s.Status == false);

            (var listOrderStructure, error) = await apiClient.GetListOrderStructure();
            ObservableCollection<OrderStructureResponse> orderStructures = new ObservableCollection<OrderStructureResponse>(listOrderStructure.Where(s => s.OrderId == order.Id));
            OrderStructure = orderStructures.FirstOrDefault(s => s.ProductId == SelectedProduct.Id);
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
