using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.Services;
//using Magaz_Stroitelya.DB;
//using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    public class AddToCartVM : BaseVM
    {
        private int quantity;
        private ObservableCollection<OrderResponse> orders = new();
        //private ObservableCollection<OrderStructure> orderStructures;

        public int Quantity { get => quantity; set { quantity = value; Signal(); } }
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
                var orderStructure = list.FirstOrDefault(s => s.ProductId == product.Id && listOrder.FirstOrDefault(ss => ss.Id == s.OrderId).Status == false);
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
                    OrderResponse order = Orders.FirstOrDefault(d => d.Status == false);
                    orderStructure.Order = order;
                    orderStructure.OrderId = order.Id;//кoрзины нет
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
