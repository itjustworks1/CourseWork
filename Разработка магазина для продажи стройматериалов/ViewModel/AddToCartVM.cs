using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Разработка_магазина_для_продажи_стройматериалов.DB;
using Разработка_магазина_для_продажи_стройматериалов.Model;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;

namespace Разработка_магазина_для_продажи_стройматериалов.ViewModel
{
    public class AddToCartVM : BaseVM
    {
        private int quantity;
        private ObservableCollection<Order> orders = new();
        //private ObservableCollection<OrderStructure> orderStructures;

        public int Quantity { get => quantity; set { quantity = value; Signal(); } }
        //public ObservableCollection<OrderStructure> OrderStructures { get => orderStructures; set { orderStructures = value; Signal(); } }
        public CommandMvvm Save { get; set; }
        public CommandMvvm Cancel { get; set; }
        public AddToCartVM(Product product)
        {
            Save = new CommandMvvm(() =>
            {
                //var product = products.FirstOrDefault(d => d.Id == orderStructure.ProductId);
                //List<OrderStructure> OrderStructures = OrderStructureDB.GetDB().SelectAll();

                OrderStructure orderStructure = OrderStructureDB.GetDB().SelectAll().FirstOrDefault(d => d.ProductId == product.Id &&
                    OrderDB.GetDB().SelectAll().FirstOrDefault(dd => dd.Id == d.OrderId).Status == false);
                if (orderStructure == null)
                {
                    orderStructure = new OrderStructure()
                    {
                        Value = product.Value,
                        Quantity = Quantity,
                        Product = product,
                        ProductId = product.Id
                    };
                    List<Order> Orders = OrderDB.GetDB().SelectAll();
                    Order order = Orders.FirstOrDefault(d => d.Status == false);
                    orderStructure.Order = order;
                    orderStructure.OrderId = order.Id;
                }
                else
                    orderStructure.Quantity += Quantity;

                    product.Quantity -= Quantity;
                    ProductDB.GetDB().Update(product);
                

                if (orderStructure.Id > 0)
                    OrderStructureDB.GetDB().Update(orderStructure);
                else
                    OrderStructureDB.GetDB().Insert(orderStructure);
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
