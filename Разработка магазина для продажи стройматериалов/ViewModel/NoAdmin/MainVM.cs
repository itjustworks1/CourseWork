//using Magaz_Stroitelya.Model;

using CommunityToolkit.Mvvm.Input;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.View.NoAdmin;
using MVVM.VMTools;
using System.Collections.ObjectModel;
using System.Windows;

namespace MVVM.ViewModel.NoAdmin
{
    public partial class MainVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private ProductResponse selectedProduct;
        private ObservableCollection<ProductResponse> searchProducts = new();
        private ObservableCollection<ProductResponse> products = new();
        private string search;
        private ObservableCollection<Filter> filters;

        public ObservableCollection<ProductResponse> Products { get => products; set { products = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductResponse> SearchProducts { get => searchProducts; set { searchProducts = value; OnPropertyChanged(); } }
        public ProductResponse SelectedProduct { get => selectedProduct; set { selectedProduct = value; OnPropertyChanged(); } }
        public string Search { get => search; set { search = value; SearchProduct(search); } }
        public ObservableCollection<Filter> Filters { get => filters; set { filters = value; OnPropertyChanged(); } }

        public CommandMvvm AddToCart { get; set; }
        public CommandMvvm OpenCart { get; set; }
        public CommandMvvm OpenProduct { get; set; }
        public CommandMvvm ApplyFilters { get; set; }

        [RelayCommand]
        private void AddToCart2(ProductResponse product)
        {
            new WindowAddToCart(product, apiClient).ShowDialog();
        }

        public MainVM(MainWindow thisWindow, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            Task.Run(() => SelectAll());

            AddToCart = new CommandMvvm(() =>
            {
                Task.Run(() => SelectAll());
            }, () => true);

            OpenCart = new CommandMvvm(() =>
            {
                hide();
                new WindowCart(apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => true);

            OpenProduct = new CommandMvvm(() =>
            {
                hide();
                new WindowProduct(SelectedProduct, apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => SelectedProduct != null);

            ApplyFilters = new CommandMvvm(() =>
            {
                SearchFilter();
            }, () => true);
        }

        private async void SelectAll()
        {
            await SelectProductsAsync();
            await SelectFiltersAsync();
        }
        public async Task SelectProductsAsync()
        {
            var (list, error) = await apiClient.GetListProduct();
            var listProduct = new ObservableCollection<ProductResponse>(list);
            for (int i = 0; i < list.Count; i++)
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
            SearchProducts = Products;
        }
        public async Task SelectFiltersAsync()
        {
            var (list, error) = await apiClient.GetListProductType();
            var listType = new ObservableCollection<Filter>();
            foreach (var item in list)
            {
                listType.Add(new Filter
                {
                    Text = item,
                    IsChecked = false
                });
            }
            Filters = listType;
        }
        private void SearchProduct(string search)
        {
            if (string.IsNullOrEmpty(Search))
            {
                SearchProducts = Products;
            }
            SearchFilter(search);
        }
        private void SearchFilter(string search = "")
        {
            if (Filters.Count == Filters.Where(s => !s.IsChecked).Count())
            {
                SearchProducts = Products;
            }
            else
            {
                SearchProducts = new ObservableCollection<ProductResponse>();
                foreach (var product in Products)
                {
                    foreach (var filter in Filters)
                    {
                        if (filter.IsChecked && product.ProductType.Id == filter.Text.Id)
                            SearchProducts.Add(product);
                    }

                }
            }
            if (!string.IsNullOrEmpty(Search))
            {
                SearchProducts = [.. SearchProducts.Where(s => s.Title.Contains(Search))];
            }
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
    }
    public class Filter : BaseVM
    {
        public ProductTypeResponse Text { get;set;}
        public bool IsChecked { get; set;}
    }
}
