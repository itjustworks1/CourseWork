using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Magaz_Stroitelya.DB;
using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.VMTools;

namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    public class RemoveFromCartVM : BaseVM
    {
        private int quantity;

        public int Quantity { get => quantity; set { quantity = value; Signal(); } }
        public CommandMvvm Remove { get; set; }
        public CommandMvvm Cancel { get; set; }
        public RemoveFromCartVM(OrderStructure orderStructure)
        {
            //...
            Remove = new CommandMvvm(() =>
            {
                Product product = ProductDB.GetDB().SelectAll().FirstOrDefault(d => d.Id == orderStructure.ProductId);
                product.Quantity += Quantity;
                if (orderStructure.Quantity == Quantity)
                    OrderStructureDB.GetDB().Remove(orderStructure);
                else
                {
                    orderStructure.Quantity -= Quantity;
                    OrderStructureDB.GetDB().Update(orderStructure);
                }
                ProductDB.GetDB().Update(product);
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
