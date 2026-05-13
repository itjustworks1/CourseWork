using CommunityToolkit.Mvvm.Input;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;
using MVVM.VMTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magaz_Stroitelya.ViewModel.Admin
{
    public partial class AddEditProductAVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private ObservableCollection<ProductTypeResponse> productTypes = new();
        private ProductResponse selectedProduct;
        private ObservableCollection<ProductResponse> products = new();
        private ObservableCollection<ProductParameterResponse> productParameters = new();
        private ProductParameterResponse selectedProductParameter;
        private ProductParameterResponse editProductParameter;
        private ObservableCollection<ProductParameterResponse> selectedProductParameters = new();
        private ObservableCollection<ParameterResponse> parameters = new();
        private ObservableCollection<ProductParameterResponse> selectedSelectedProductParameters = new();

        public ObservableCollection<ProductParameterResponse> SelectedSelectedProductParameters { get => selectedSelectedProductParameters; set { selectedSelectedProductParameters = value; Signal(); } }
        public ObservableCollection<ParameterResponse> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public ObservableCollection<ProductParameterResponse> SelectedProductParameters { get => selectedProductParameters; set { selectedProductParameters = value; Signal(); } }
        public ProductParameterResponse SelectedProductParameter 
        {
            get => selectedProductParameter; 
            set 
            { 
                selectedProductParameter = value;
                Signal();
                if (value is null)
                {
                    EditProductParameter = null;
                }
                else
                {
                    EditProductParameter = new()
                    {
                        Id = value.Id,
                        Meaning = value.Meaning,
                        Parameter = value.Parameter,
                        ParameterId = value.ParameterId,
                        Product = value.Product,
                        ProductId = value.ProductId
                    };
                }
            } 
        }
        public ProductParameterResponse EditProductParameter { get => editProductParameter; set { editProductParameter = value; Signal(); } }
        public ObservableCollection<ProductParameterResponse> ProductParameters { get => productParameters; set { productParameters = value; Signal(); } }
        public ObservableCollection<ProductTypeResponse> ProductTypes { get => productTypes; 
            set 
            { 
                productTypes = value; 
                Signal();
            } 
        }
        public ObservableCollection<ProductResponse> Products { get => products; set { products = value; Signal(); } }
        public ProductResponse SelectedProduct { get => selectedProduct; 
            set { selectedProduct = value; Signal(); } }

        public CommandMvvm Save { get; set; }
        public CommandMvvm Cancel { get; set; }
        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }
        public CommandMvvm OpenAddEditProductType { get; set; }


        [RelayCommand]
        private async Task EditParameterSecond()
        {
            SelectedProductParameter.ChangeAllProperties(EditProductParameter);
            //if(SelectedProductParameter.Id == EditProductParameter.Id)
            //    for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
            //        if (SelectedSelectedProductParameters[i].Id == EditProductParameter.Id)
            //            SelectedSelectedProductParameters[i] = EditProductParameter;
            await Task.Run(SelectAll);
        }


        public AddEditProductAVM(Window thisWindow, ApiClient apiClient, ProductResponse product)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            SelectedProduct = product;
            Task.Run(() => SelectAll());
            AddParameter = new CommandMvvm(() =>
            {
                ProductParameterResponse productParameter = new ProductParameterResponse();
                productParameter.Meaning = EditProductParameter.Meaning;
                productParameter.Parameter = EditProductParameter.Parameter;
                productParameter.ParameterId = productParameter.Parameter.Id;
                productParameter.Product = SelectedProduct;
                productParameter.ProductId = SelectedProduct.Id;
                SelectedSelectedProductParameters.Add(productParameter);
                Task.Run(() => SelectAll());
            }, () =>
            EditProductParameter != null &&
            EditProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(EditProductParameter.Meaning)
            );
            EditParameter = new CommandMvvm(() =>
            {

                SelectedProductParameter.ChangeAllProperties(EditProductParameter);
                //if(SelectedProductParameter.Id == EditProductParameter.Id)
                //    for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                //        if (SelectedSelectedProductParameters[i].Id == EditProductParameter.Id)
                //            SelectedSelectedProductParameters[i] = EditProductParameter;
                Task.Run(() => SelectAll());
            }, () =>
            SelectedProductParameter != null &&
            SelectedProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning)
            );

            RemoveParameter = new CommandMvvm(() =>
            {
                for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    if (SelectedSelectedProductParameters[i].Id == SelectedProductParameter.Id)
                        SelectedSelectedProductParameters.Remove(SelectedSelectedProductParameters[i]);
                // при синхронизации удаляет оба
                Task.Run(() => SelectAll());
                // при обновлении они десинхронизируются
            }, () =>
            SelectedProductParameter != null &&
            SelectedProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning)
            );

            OpenAddEditProductType = new CommandMvvm(() =>
            {
                hide();
                new WindowAddEditProductType(apiClient).ShowDialog();
                Task.Run(() => SelectAll());
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

        private async void SelectAll()
        {
            var selectedProductTypeId = SelectedProduct?.ProductType?.Id;

            if (SelectedProductParameter == null) SelectedProductParameter = new();
            await SelectProductsAsync();
            await SelectProductTypesAsync();
            
            await SelectProductParametersAsync();
            SelectedProductParameters = new ObservableCollection<ProductParameterResponse>(ProductParameters.Where(s => s.ProductId == SelectedProduct.Id));
            await SelectParametersAsync();
            if (!SelectedSelectedProductParameters.Any())
                SelectedSelectedProductParameters = SelectedProductParameters;

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
        }
        public async Task SelectProductTypesAsync()
        {
            var (list, error) = await apiClient.GetListProductType();
            var listProduct = new ObservableCollection<ProductTypeResponse>(list);
            var productTypeId = SelectedProduct.ProductType.Id;
            ProductTypes = listProduct;

            var prodType = ProductTypes.FirstOrDefault(t => t.Id == productTypeId);
            if (prodType is not null)
            {
                SelectedProduct.ProductType = prodType;
                SelectedProduct.ProductTypeId = productTypeId;
            }
            
            //MessageBox.Show($"{SelectedProduct.ProductType == null}");
            //MessageBox.Show($"{SelectedProduct.ProductType == null}");
            //MessageBox.Show($"{SelectedProduct.ProductType == null}");
            //Thread.Sleep(20); //(╯°□°）╯︵ ┻━━━━━━━━━━━━━━┻
            //SelectedProduct.ProductType = prodType;
            //SelectedProduct.ProductTypeId = prodType.Id;
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
                    Id = parameter.Id,
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
