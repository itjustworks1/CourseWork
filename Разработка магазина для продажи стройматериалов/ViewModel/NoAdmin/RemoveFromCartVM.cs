//using Magaz_Stroitelya.DB;
//using Magaz_Stroitelya.Model;

using MVVM.Model.DTO.Requests;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.VMTools;

namespace MVVM.ViewModel.NoAdmin
{
    public class RemoveFromCartVM : BaseVM
    {
        private int quantity;

        public int Quantity { get => quantity; set { quantity = value; OnPropertyChanged(); } }
        public CommandMvvm Remove { get; set; }
        public CommandMvvm Cancel { get; set; }
        public RemoveFromCartVM(OrderStructureResponse orderStructure, ApiClient apiClient)
        {
            //...
            Remove = new CommandMvvm(async () =>
            {
                var (list, error) = await apiClient.GetListProduct();
                ProductResponse product = list.FirstOrDefault(d => d.Id == orderStructure.ProductId);
                product.Quantity += Quantity;
                if (orderStructure.Quantity == Quantity)
                    await apiClient.DeleteOrderStructure(orderStructure.Id);
                else
                {
                    orderStructure.Quantity -= Quantity;
                    await apiClient.PatchOrderStructure(orderStructure.Id, orderStructure);
                }
                await apiClient.PatchProduct(product.Id, new ProductRequest
                {
                    Title = product.Title,
                    Quantity = product.Quantity,
                    Value = product.Value,
                    ProductTypeId = product.ProductTypeId
                });
                close();
            }, () =>
            Quantity > 0 &&
            Quantity <= orderStructure.Quantity);

        }

        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
