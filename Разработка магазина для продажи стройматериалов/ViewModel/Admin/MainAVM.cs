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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    public class MainAVM : BaseVM
    {
        private ApiClient apiClient;
        private Window thisWindow;

        private ProductResponse selectedProduct;
        private ObservableCollection<ProductResponse> products = new();

        public ObservableCollection<ProductResponse> Products { get => products; set { products = value; Signal(); } }
        public ProductResponse SelectedProduct { get => selectedProduct; set { selectedProduct = value; Signal(); } }

        public CommandMvvm AddProduct { get; set; }
        public CommandMvvm OpenUsers { get; set; }
        public CommandMvvm OpenProduct { get; set; }

        public MainAVM(Window thisWindow, ApiClient apiClient)
        {
            this.thisWindow = thisWindow;
            this.apiClient = apiClient;

            SelectAll();

            AddProduct = new CommandMvvm(() =>
            {
                ProductResponse product = new ProductResponse();
                bool isEdit = false;
                hide();
                new WindowAddEditProduct(product, apiClient, ref isEdit).ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => true);


            OpenUsers = new CommandMvvm(() =>
            {
                hide();
                new WindowCart().ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => true);

            OpenProduct = new CommandMvvm(() =>
            {
                hide();
                new WindowProductA(SelectedProduct, apiClient).ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => SelectedProduct != null);

        }

        private async Task SelectAll()
        {
            await SelectProductsAsync();
        }
        public async Task SelectProductsAsync()
        {
            var (list, error) = await apiClient.GetListProduct();
            try
            {
                var listProduct = new ObservableCollection<ProductResponse>(list);
                for (int i = 0; i < list.Count; i++)
                {
                    (var productType, error) = await apiClient.GetParameter(listProduct[i].ProductTypeId);
                    try
                    {
                        var type = new ProductTypeResponse
                        {
                            Id = listProduct[i].Id,
                            Title = productType.Title
                        };
                        listProduct[i].ProductType = type;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(error);
                    }
                }
                Products = listProduct;
            }
            catch (Exception ex)
            {
                MessageBox.Show(error);
            }
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
    }
}
