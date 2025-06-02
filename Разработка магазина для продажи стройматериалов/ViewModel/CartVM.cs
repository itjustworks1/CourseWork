using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Разработка_магазина_для_продажи_стройматериалов.DB;
using Разработка_магазина_для_продажи_стройматериалов.Model;
using Разработка_магазина_для_продажи_стройматериалов.View;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;

namespace Разработка_магазина_для_продажи_стройматериалов.ViewModel
{
    public class CartVM : BaseVM
    {
        private ObservableCollection<Order> orders = new();
        private ObservableCollection<OrderStructure> orderStructures;
        private OrderStructure selectedOrderStructure;
        private ObservableCollection<Product> products = new();
        private string search;

        public ObservableCollection<OrderStructure> OrderStructures { get => orderStructures; set { orderStructures = value; Signal(); } }
        public ObservableCollection<Order> Orders { get => orders; set { orders = value; Signal(); } }
        public ObservableCollection<Product> Products { get => products; set { products = value; Signal(); } }
        public OrderStructure SelectedOrderStructure { get => selectedOrderStructure; set { selectedOrderStructure = value; Signal(); } }
        public string Search { get => search; set { search = value; SearchOrderStructure(search); } }

        public CommandMvvm PlaceAnOrder { get; set; }
        public CommandMvvm RemoveFromCart { get; set; }
        public CommandMvvm OpenOrder { get; set; }

        public CartVM()
        {
            if (Orders.FirstOrDefault(s => s.Status == false) == null)
            {
                NewOrder();
            }
            SelectAll();
            PlaceAnOrder = new CommandMvvm(() =>
            {
                Order order = Orders.FirstOrDefault(s => s.Status == false);
                order.Status = true;
                order.Date = DateTime.Now;
                OrderDB.GetDB().Update(order);
                NewOrder();
                SelectAll();
            }, () => OrderStructures != null);

            RemoveFromCart = new CommandMvvm(() =>
            {
                SelectAll();
            }, () => true);

            OpenOrder = new CommandMvvm(() =>
            {
                new WindowListOrders().ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            OrderStructures = new ObservableCollection<OrderStructure>(OrderStructureDB.GetDB().SelectAll().Where(s => s.Order.Status == false));
            Products = new ObservableCollection<Product>(ProductDB.GetDB().SelectAll());
            Orders = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll());
        }
        private void SearchOrderStructure(string search)
        {
            OrderStructures = new ObservableCollection<OrderStructure>(OrderStructureDB.GetDB().SearchOrderStructure(search).Where(s => s.Order.Status == false));
        }
        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }

        public void NewOrder()
        {
            OrderDB.GetDB().Insert(new Order() { Date = DateTime.Now, Status = false });
        }
    }
}
