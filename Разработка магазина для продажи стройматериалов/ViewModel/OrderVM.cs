using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Разработка_магазина_для_продажи_стройматериалов.Model;
using Разработка_магазина_для_продажи_стройматериалов.View;
using Разработка_магазина_для_продажи_стройматериалов.DB;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;
using System.Windows;

namespace Разработка_магазина_для_продажи_стройматериалов.ViewModel
{
    public class OrderVM : BaseVM
    {
        private Order selectedOrder;
        private ObservableCollection<Order> orders = new();
        private ObservableCollection<OrderStructure> orderStructures = new();
        private decimal value;

        public decimal Value { get => value; set { this.value = value; Signal(); } }
        public ObservableCollection<OrderStructure> OrderStructures { get => orderStructures; set { orderStructures = value; Signal(); } }
        public ObservableCollection<Order> Orders { get => orders; set { orders = value; Signal(); } }
        public Order SelectedOrder { get => selectedOrder; set { selectedOrder = value; Signal(); } }

        public CommandMvvm RemoveOrder { get; set; }
        //public CommandMvvm EditOrder { get; set; }

        public OrderVM(Order order)
        {
            SelectedOrder = order;
            SelectAll();
            decimal summ = 0;
            foreach (var o in OrderStructures)
                summ += o.Value * o.Quantity;
            Value = summ;

            RemoveOrder = new CommandMvvm(() =>
            {
                var rOrderStructures = new ObservableCollection<OrderStructure>(OrderStructureDB.GetDB().SelectAll().Where(s => s.OrderId == SelectedOrder.Id));
                foreach (var o in rOrderStructures)
                    OrderStructureDB.GetDB().Remove(o);
                OrderDB.GetDB().Remove(SelectedOrder);
                close();
            }, () => SelectedOrder != null);

            //EditOrder = new CommandMvvm(() =>
            //{
            //    SelectAll();
            //}, () => SelectedOrder != null);

        }

        private void SelectAll()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll());
            OrderStructures = new ObservableCollection<OrderStructure>(OrderStructureDB.GetDB().SelectAll().Where(s => s.OrderId == SelectedOrder.Id));
        }
        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
