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
using MVVM.View.Admin;

namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    public class MainAVM : BaseVM
    {
        /* Текущая работа:
         * Окна: списка пользователей+, пользователя+, в админку заказиков+, +
         * Наверное сделать статус у заказов, чтобы админ мог его менять. Тогда и в обычку его пихнуть, и сделать невозможным изменение оплаченных заказов
         * Стоит ли делать удаление заказов?
         * 
         * Поиск+ и котегоризация товаров+
         * Кнопочки "обратно"
         * 
         * 
         * А не стоит ли просто сделать заказы неизменяемыми?
         * 
         * Поиск сделал, теперь категории. а теперь и категории
         */

        private ApiClient apiClient;
        private Window thisWindow;

        private ProductResponse selectedProduct;
        private ObservableCollection<ProductResponse> searchProducts = new();
        private ObservableCollection<ProductResponse> products = new();
        private string search;
        private ObservableCollection<Filter> filters;

        public ObservableCollection<ProductResponse> Products { get => products; set { products = value; OnPropertyChanged(); } }
        public ProductResponse SelectedProduct { get => selectedProduct; set { selectedProduct = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductResponse> SearchProducts { get => searchProducts; set { searchProducts = value; OnPropertyChanged(); } }
        public string Search { get => search; set { search = value; SearchProduct(search); } }
        public ObservableCollection<Filter> Filters { get => filters; set { filters = value; OnPropertyChanged(); } }

        public CommandMvvm AddProduct { get; set; }
        public CommandMvvm OpenUsers { get; set; }
        public CommandMvvm OpenProduct { get; set; }
        public CommandMvvm ApplyFilters { get; set; }

        public MainAVM(Window thisWindow, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            Task.Run(() => SelectAll());

            AddProduct = new CommandMvvm(() =>
            {
                ProductResponse product = new ProductResponse();
                hide();
                new WindowAddEditProduct(product, apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => true);


            OpenUsers = new CommandMvvm(() =>
            {
                hide();
                new ListUsersWindow(apiClient).ShowDialog();
                Task.Run(() => SelectAll());
                thisWindow.ShowDialog();
            }, () => true);

            OpenProduct = new CommandMvvm(() =>
            {
                hide();
                new WindowProductA(SelectedProduct, apiClient).ShowDialog();
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
}
