using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Разработка_магазина_для_продажи_стройматериалов.DB;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;

namespace Разработка_магазина_для_продажи_стройматериалов.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }


        public CommandMvvm EditOrder { get; set; }

        public Order()
        {
            EditOrder = new CommandMvvm(() =>
            {
                if (MessageBox.Show("Корзина будет очищена.\nПродолжить?", "Подтверждение", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var rOrder = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll()).FirstOrDefault(s => s.Status == false);
                    var rOrderStructures = new ObservableCollection<OrderStructure>(OrderStructureDB.GetDB().SelectAll().Where(s => s.OrderId == rOrder.Id));
                    foreach (var o in rOrderStructures)
                        OrderStructureDB.GetDB().Remove(o);
                    OrderDB.GetDB().Remove(rOrder);
                    Status = false;
                    OrderDB.GetDB().Update(this);
                }

            }, () => true);
        }
    }
}
