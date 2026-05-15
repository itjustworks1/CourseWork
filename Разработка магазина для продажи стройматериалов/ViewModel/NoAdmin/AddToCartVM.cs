using System.Collections.ObjectModel;
using MVVM.Model.DTO.Requests;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.VMTools;
//using Magaz_Stroitelya.DB;
//using Magaz_Stroitelya.Model;

namespace MVVM.ViewModel.NoAdmin
{
    public class AddToCartVM : BaseVM
    {
        private int quantity;
        private ObservableCollection<OrderResponse> orders = new();
        //private ObservableCollection<OrderStructure> orderStructures;

        public int Quantity { get => quantity; set { quantity = value; OnPropertyChanged(); } }
        //public ObservableCollection<OrderStructure> OrderStructures { get => orderStructures; set { orderStructures = value; Signal(); } }
        public CommandMvvm Save { get; set; }
        public CommandMvvm Cancel { get; set; }
        public AddToCartVM(ProductResponse product, ApiClient apiClient)
        {
            Save = new CommandMvvm(async () =>
            {
                //var product = products.FirstOrDefault(d => d.Id == orderStructure.ProductId);
                //List<OrderStructure> OrderStructures = OrderStructureDB.GetDB().SelectAll();

                var (list, error) = await apiClient.GetListOrderStructure();
                (var listOrder, error) = await apiClient.GetListOrder();
                var orderStructure = list.FirstOrDefault(s => s.ProductId == product.Id && listOrder.FirstOrDefault(ss => ss.Id == s.OrderId && ss.UserId == apiClient.UserId) != null && listOrder.FirstOrDefault(ss => ss.Id == s.OrderId && ss.UserId == apiClient.UserId).Status == false);
                if (orderStructure == null)
                {
                    orderStructure = new OrderStructureResponse()
                    {
                        Value = product.Value,
                        Quantity = Quantity,
                        Product = product,
                        ProductId = product.Id
                    };
                    (var Orders, error) = await apiClient.GetListOrder();
                    OrderResponse order = Orders.FirstOrDefault(d => d.Status == false && d.UserId == apiClient.UserId);
                    orderStructure.Order = order;
                    orderStructure.OrderId = order.Id;
                }
                else
                    orderStructure.Quantity += Quantity;

                product.Quantity -= Quantity;
                await apiClient.PatchProduct(product.Id, new ProductRequest
                {
                    Title = product.Title,
                    Value = product.Value,
                    Quantity = product.Quantity,
                    ProductTypeId = product.ProductTypeId
                });


                if (orderStructure.Id > 0)
                    await apiClient.PatchOrderStructure(orderStructure.Id, orderStructure);
                else
                    await apiClient.PostOrderStructure(orderStructure);
                close();
            }, () =>
            Quantity > 0 &&
            Quantity <= product.Quantity);


            Cancel = new CommandMvvm(() =>
            {
                close();
            }, () => true);

        }

        //private void SelectAll()
        //{
        //    Orders = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll());
        //}
        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
