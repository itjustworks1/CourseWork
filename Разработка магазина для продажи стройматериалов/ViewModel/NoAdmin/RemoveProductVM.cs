using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magaz_Stroitelya.Services;

//using Magaz_Stroitelya.DB;
//using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;


namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    public class RemoveProductVM : BaseVM
    {
        ApiClient apiClient;
        private int quantity;

        public int Quantity { get => quantity; set { quantity = value; OnPropertyChanged(); } }

        public CommandMvvm Remove { get; set; }
        public CommandMvvm Cancel { get; set; }
        public RemoveProductVM(ProductResponse product, ApiClient apiClient)
        {
            this.apiClient = apiClient;
            
            Remove = new CommandMvvm(async () =>
            {
                if (product.Quantity == Quantity)
                    await apiClient.DeleteProduct(product.Id);
                else
                {
                    product.Quantity -= Quantity;
                    await apiClient.PatchProduct(product.Id, new ProductRequest
                    {

                        Title = product.Title,
                        Value = product.Value,
                        Quantity = product.Quantity,
                        ProductTypeId = product.ProductTypeId
                    });
                }
                await apiClient.PatchProduct(product.Id, new ProductRequest
                {

                    Title = product.Title,
                    Value = product.Value,
                    Quantity = product.Quantity,
                    ProductTypeId = product.ProductTypeId
                });//зачем 2?
                close();
            }, () =>
            Quantity > 0 &&
            Quantity <= product.Quantity);

        }

        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
