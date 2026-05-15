using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magaz_Stroitelya.ViewModel.Admin
{
    public class AddEditProductTypeAVM : BaseVM
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();



        private ApiClient apiClient;
        private Window thisWindow;

        private ProductTypeResponse selectedProductType;
        private ParameterResponse selectedParameter;
        private ObservableCollection<ProductTypeResponse> productTypes = new();
        private ObservableCollection<ParameterResponse> parameters = new();
        private ObservableCollection<ProductTypeParameterResponse> productTypeParameters = new();
        private ObservableCollection<ProductTypeParameterResponse> selectedProductTypeParametersOnProductType = new();
        private ProductTypeParameterResponse selectedProductTypeParameter;

        public ProductTypeParameterResponse SelectedProductTypeParameter { get => selectedProductTypeParameter; set { selectedProductTypeParameter = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductTypeParameterResponse> ProductTypeParameters { get => productTypeParameters; set { productTypeParameters = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductTypeParameterResponse> SelectedProductTypeParametersOnProductType { get => selectedProductTypeParametersOnProductType; set { selectedProductTypeParametersOnProductType = value; OnPropertyChanged(); } }
        public ObservableCollection<ParameterResponse> Parameters { get => parameters; set { parameters = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductTypeResponse> ProductTypes { get => productTypes; set { productTypes = value; OnPropertyChanged(); } }
        public ParameterResponse SelectedParameter { get => selectedParameter; set { selectedParameter = value; OnPropertyChanged(); } }
        public ProductTypeResponse SelectedProductType 
        { 
            get => selectedProductType; 
            set 
            { 
                selectedProductType = value;
                if (SelectedProductType != null)
                {
                    SelectPTP(value);
                }
                OnPropertyChanged();
            } 
        }

        public CommandMvvm AddProductType { get; set; }
        public CommandMvvm EditProductType { get; set; }
        public CommandMvvm RemoveProductType { get; set; }
        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }
        public CommandMvvm OpenAddEditParameter { get; set; }

        public AddEditProductTypeAVM(Window thisWindow, ApiClient apiClient)
        {
            //AllocConsole();
            //Console.WriteLine("Hello drodd");
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            Task.Run(() => SelectAll());
            AddProductType = new CommandMvvm(async () =>
            {
                await apiClient.PostProductType(SelectedProductType);
                await Task.Run(() => SelectAll());
            }, () => SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            EditProductType = new CommandMvvm(async () =>
            {
                await apiClient.PatchProductType(SelectedProductType.Id, SelectedProductType);
                await Task.Run(() => SelectAll());
            }, () => SelectedProductType != null &&
             !string.IsNullOrEmpty(SelectedProductType.Title));

            RemoveProductType = new CommandMvvm(() =>
            {

                Task.Run(() => SelectAll());
            }, () => SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            AddParameter = new CommandMvvm(async () =>
            {
                ProductTypeParameterResponse productTypeParameter = new ProductTypeParameterResponse()
                {
                    Parameter = SelectedParameter,
                    ParameterId = SelectedParameter.Id,
                    ProductType = SelectedProductType,
                    ProductTypeId = SelectedProductType.Id
                };
                await apiClient.PostProductTypeParameter(productTypeParameter);
                await Task.Run(() => SelectAll());
            }, () => SelectedParameter != null &&
            SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            EditParameter = new CommandMvvm(async () =>
            {
                ParameterResponse parameter = SelectedProductTypeParameter.Parameter;
                SelectedProductTypeParameter.Parameter = SelectedParameter;
                SelectedProductTypeParameter.ParameterId = SelectedParameter.Id;
                await apiClient.DeleteProductTypeParameter(parameter.Id, SelectedProductTypeParameter.ProductTypeId);
                await apiClient.PostProductTypeParameter(SelectedProductTypeParameter);
                await Task.Run(() => SelectAll());
            }, () =>
            SelectedProductTypeParameter != null &&
            SelectedParameter != null &&
            SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            RemoveParameter = new CommandMvvm(async () =>
            {
                await apiClient.DeleteProductTypeParameter(SelectedProductTypeParameter.ParameterId, SelectedProductTypeParameter.ProductTypeId);
                await Task.Run(() => SelectAll());
            }, () => 
            SelectedProductTypeParameter != null &&
            SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            OpenAddEditParameter = new CommandMvvm(() =>
            {
                hide();
                new WindowAddEditParameter(apiClient).ShowDialog();
                thisWindow.ShowDialog();
                Task.Run(() => SelectAll());
            }, () => true);

        }

        private async void SelectAll()
        {
            SelectedProductType = new ProductTypeResponse();
            await SelectProductTypesAsync();
            await SelectParametersAsync();
            await SelectProductTypeParametersAsync();
            await SelectSelectProductTypeParametersAsync();
        }
        public async Task SelectProductTypesAsync()
        {
            var (list, error) = await apiClient.GetListProductType();
            var listProductType = new ObservableCollection<ProductTypeResponse>(list);
            ProductTypes = listProductType;
        }
        public async Task SelectParametersAsync()
        {
            var (list, error) = await apiClient.GetListParameter();
            var listParameter = new ObservableCollection<ParameterResponse>(list);
            Parameters = listParameter;
        }
        public async Task SelectProductTypeParametersAsync()
        {
            var (list, error) = await apiClient.GetListProductTypeParameter();
            var listProductTP = new ObservableCollection<ProductTypeParameterResponse>(list);
            for (int i = 0; i < list.Count; i++)
            {
                (var productType, error) = await apiClient.GetProductType(listProductTP[i].ProductTypeId);
                var type = new ProductTypeResponse
                {
                    Id = productType.Id,
                    Title = productType.Title
                };
                (var parameter, error) = await apiClient.GetParameter(listProductTP[i].ParameterId);
                var param = new ParameterResponse
                {
                    Id = parameter.Id,
                    Title = parameter.Title
                };
                listProductTP[i].ProductType = type;
                listProductTP[i].Parameter = param;
            }
            ProductTypeParameters = listProductTP;
        }
        public async Task SelectSelectProductTypeParametersAsync()
        {
            //var (list, error) = await apiClient.GetListProductTypeParameter();
            //var listProductTP = new ObservableCollection<ProductTypeParameterResponse>(list);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    (var productType, error) = await apiClient.GetProductType(listProductTP[i].ProductTypeId);
            //    var type = new ProductTypeResponse
            //    {
            //        Id = productType.Id,
            //        Title = productType.Title
            //    };
            //    (var parameter, error) = await apiClient.GetParameter(listProductTP[i].ParameterId);
            //    var param = new ParameterResponse
            //    {
            //        Id = parameter.Id,
            //        Title = parameter.Title
            //    };
            //    listProductTP[i].ProductType = type;
            //    listProductTP[i].Parameter = param;
            //}
            //SelectedProductTypeParametersOnProductType = listProductTP;
            SelectedProductTypeParametersOnProductType = new ObservableCollection<ProductTypeParameterResponse>();
        }
        private async void SelectPTP(ProductTypeResponse selectedPT)
        {
            var (list, error) = await apiClient.GetListProductTypeParameter();
            list = [.. list.Where(s => s.ProductTypeId == selectedPT.Id)];
            var listProductTP = new ObservableCollection<ProductTypeParameterResponse>(list);
            for (int i = 0; i < list.Count; i++)
            {
                (var productType, error) = await apiClient.GetProductType(listProductTP[i].ProductTypeId);
                var type = new ProductTypeResponse
                {
                    Id = productType.Id,
                    Title = productType.Title
                };
                (var parameter, error) = await apiClient.GetParameter(listProductTP[i].ParameterId);
                
                var param = new ParameterResponse
                {
                    Id = parameter.Id,
                    Title = parameter.Title
                };
                listProductTP[i].ProductType = type;
                listProductTP[i].Parameter = param;
            }
            SelectedProductTypeParametersOnProductType = listProductTP;
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
    }
}
