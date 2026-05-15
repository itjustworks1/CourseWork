//using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    /* Список задач
     * Сделать [редактирование заказов (наверное серез корзину)]. + список заказов + состав заказов // я хз что написано
     * Настроить все Remove (особенно Product ?) (кроме WindowListOrders + ...) 
           // стоит ли мне вообще удалять Parameter и ProductType?; думаю можно удалять но при условии что их нигде нет; но тогда придеться страдать с проверками
     * Что-то сделать с поиском ?
     * Разобраться с ценами в заказе // я хз, вроде сделал
     * В WindowProduct [не меняется TextBlock] + [доделать вывод инфы] + хотелось бы чуть исправить изменения
     * В WindowAddEditProduct [сломался Parameter] + хотелось что бы ComboBox менялся от выбранных параметров;
           теперь просто не добавляется параметр + почему всё сразу меняется + [при удалении параметра накричали]
           // всё из-за сложности совместить ProductParameter с Parameter
     * В WindowAddEditProductType доделать список Parameter + настроить связи
     */
    /* Готово
     *  [визуал в 3 мелких окнах]
     * [В WindowListOrders сделать отдельно для карзины(false)].
     * [Исправить кнопку добавление/удаления товара в/из корзину/ы, редактирование Order].
     */
    public class MainVM : BaseVM
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
