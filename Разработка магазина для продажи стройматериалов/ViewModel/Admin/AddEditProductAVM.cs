using Magaz_Stroitelya.Model;
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

namespace Magaz_Stroitelya.ViewModel.Admin
{
    internal class AddEditProductAVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private ObservableCollection<ProductTypeResponse> productTypes = new();
        private ProductResponse selectedProduct;
        private ObservableCollection<ProductResponse> products = new();
        private ObservableCollection<ProductParameterResponse> productParameters = new();
        private ProductParameterResponse selectedProductParameter;
        private ObservableCollection<ProductParameterResponse> selectedProductParameters = new();
        private ObservableCollection<ParameterResponse> parameters = new();
        private ObservableCollection<ProductParameterResponse> selectedSelectedProductParameters = new();

        public ObservableCollection<ProductParameterResponse> SelectedSelectedProductParameters { get => selectedSelectedProductParameters; set { selectedSelectedProductParameters = value; Signal(); } }
        public ObservableCollection<ParameterResponse> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public ObservableCollection<ProductParameterResponse> SelectedProductParameters { get => selectedProductParameters; set { selectedProductParameters = value; Signal(); } }
        public ProductParameterResponse SelectedProductParameter { get => selectedProductParameter; set { selectedProductParameter = value; Signal(); } }
        public ObservableCollection<ProductParameterResponse> ProductParameters { get => productParameters; set { productParameters = value; Signal(); } }
        public ObservableCollection<ProductTypeResponse> ProductTypes { get => productTypes; set { productTypes = value; Signal(); } }
        public ObservableCollection<ProductResponse> Products { get => products; set { products = value; Signal(); } }
        public ProductResponse SelectedProduct { get => selectedProduct; set { selectedProduct = value; Signal(); } }

        public CommandMvvm Save { get; set; }
        public CommandMvvm Cancel { get; set; }
        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }
        public CommandMvvm OpenAddEditProductType { get; set; }

        public AddEditProductAVM(Window thisWindow, ApiClient apiClient, ProductResponse product, ref bool isEdit)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            SelectedProduct = product;
            SelectAll();
            SelectedSelectedProductParameters = SelectedProductParameters;
            AddParameter = new CommandMvvm(() =>
            {
                ProductParameterResponse productParameter = new ProductParameterResponse();
                productParameter.Meaning = SelectedProductParameter.Meaning;
                productParameter.Parameter = SelectedProductParameter.Parameter;
                productParameter.ParameterId = productParameter.Parameter.Id;
                productParameter.Product = SelectedProduct;
                productParameter.ProductId = SelectedProduct.Id;
                SelectedSelectedProductParameters.Add(productParameter);
                SelectAll();
            }, () =>
            SelectedProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning)
            );

            EditParameter = new CommandMvvm(() =>
            {
                for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    if (SelectedSelectedProductParameters[i] == SelectedProductParameter)
                        SelectedSelectedProductParameters[i] = SelectedProductParameter;
                SelectAll();
            }, () =>
            SelectedProductParameter != null &&
            SelectedProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning)
            );

            RemoveParameter = new CommandMvvm(() =>
            {
                for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    if (SelectedSelectedProductParameters[i] == SelectedProductParameter)
                        SelectedSelectedProductParameters.Remove(SelectedSelectedProductParameters[i]);
                // при синхронизации удаляет оба
                SelectAll();
                // при обновлении они десинхронизируются
            }, () =>
            SelectedProductParameter != null &&
            SelectedProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning)
            );

            OpenAddEditProductType = new CommandMvvm(() =>
            {
                hide();
                new WindowAddEditProductType().ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => true);

            Save = new CommandMvvm(async () =>
            {
                SelectedProduct.ProductTypeId = SelectedProduct.ProductType.Id;
                if (SelectedProduct.Id > 0)
                {
                    var error = await apiClient.PatchProduct(SelectedProduct.Id, new ProductRequest
                    {
                        ProductTypeId = SelectedProduct.ProductType.Id,
                        Quantity = SelectedProduct.Quantity,
                        Title = SelectedProduct.Title,
                        Value = SelectedProduct.Value
                    });
                }
                else
                {
                    var error = await apiClient.PostProduct(new ProductRequest
                    {
                        ProductTypeId = SelectedProduct.ProductType.Id,
                        Quantity = SelectedProduct.Quantity,
                        Title = SelectedProduct.Title,
                        Value = SelectedProduct.Value
                    });
                    (var list, error) = await apiClient.GetListProduct();
                    Products = [.. list];
                    product = Products.LastOrDefault();
                    SelectedProduct = product;
                    for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    {
                        SelectedSelectedProductParameters[i].Product = SelectedProduct;
                        SelectedSelectedProductParameters[i].ProductId = SelectedProduct.Id;
                    }
                }
                foreach (var s in SelectedSelectedProductParameters)
                {
                    if (s.Id > 0)
                        await apiClient.PatchProductParameter(s.Id, s);
                    else
                        await apiClient.PostProductParameter(s);
                }

                List<int> c = new List<int>();
                for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    c.Add(SelectedSelectedProductParameters[i].Id);

                for (int i = 0; i < SelectedProductParameters.Count; i++)
                    if (!c.Contains(SelectedProductParameters[i].Id)) //MessageBox.Show(SelectedProductParameters[i].Parameter.Title);
                        await apiClient.DeleteProductParameter(SelectedProductParameters[i].Id);
                IsEdit = true;
                //thisWindow
                product = SelectedProduct;
                close();
            }, () =>
            !string.IsNullOrEmpty(SelectedProduct.Title) &&
            SelectedProduct.Value > 0 &&
            SelectedProduct.Quantity > 0 &&
            SelectedProduct.ProductType != null
            );

            Cancel = new CommandMvvm(() =>
            {
                close();
            }, () => true);
        }

        private async Task SelectAll()
        {
            if (SelectedProductParameter == null) SelectedProductParameter = new ProductParameterResponse();
            await SelectProductsAsync();
            await SelectProductTypesAsync();
            if (SelectedProduct.ProductType != null)
                SelectedProduct.ProductType = ProductTypes.FirstOrDefault(s => s.Id == SelectedProduct.ProductTypeId);
            await SelectProductParametersAsync();
            SelectedProductParameters = new ObservableCollection<ProductParameterResponse>(ProductParameters.Where(s => s.ProductId == SelectedProduct.Id));
            await SelectParametersAsync();

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
        public async Task SelectProductTypesAsync()
        {
            var (list, error) = await apiClient.GetListProductType();
            var listProduct = new ObservableCollection<ProductTypeResponse>(list);
            ProductTypes = listProduct;
        }
        public async Task SelectProductParametersAsync()
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
        }
        public async Task SelectParametersAsync()
        {
            var (list, error) = await apiClient.GetListParameter();
            var listProduct = new ObservableCollection<ParameterResponse>(list);
            Parameters = listProduct;
        }
        Action close;
        public bool IsEdit { get; set; } = false;
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
