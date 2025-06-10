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
using System.Windows.Controls;

namespace Разработка_магазина_для_продажи_стройматериалов.ViewModel
{
    public class ListOrdersVM : BaseVM
    {
        private Order selectedOrder;
        private ObservableCollection<Order> orders = new();
        private ObservableCollection<Order> ordersWithoutCart = new();

        public ObservableCollection<Order> Orders { get => orders; set { orders = value; Signal(); } }
        public ObservableCollection<Order> OrdersWithoutCart { get => ordersWithoutCart; set { ordersWithoutCart = value; Signal(); } }
        
        public Order SelectedOrder { get => selectedOrder; set { selectedOrder = value; Signal(); } }

        public CommandMvvm EditOrder { get; set; }
        public CommandMvvm OpenOrder { get; set; }

        public ListOrdersVM(WindowListOrders thisWindow)
        {
            SelectAll();
            EditOrder = new CommandMvvm(() =>
            {
                close();
            }, () => true);

            OpenOrder = new CommandMvvm(() =>
            {
                hide();
                new WindowOrder(SelectedOrder).ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => SelectedOrder != null);

        }

        private void SelectAll()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll().OrderByDescending(t => t.Date));
            OrdersWithoutCart = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll().Where(s => s.Status == true).OrderByDescending(t => t.Date).Reverse());
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
