using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Разработка_магазина_для_продажи_стройматериалов.DB;
using Разработка_магазина_для_продажи_стройматериалов.Model;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;


namespace Разработка_магазина_для_продажи_стройматериалов.ViewModel
{
    public class RemoveProductVM : BaseVM
    {
        private int quantity;

        public int Quantity { get => quantity; set { quantity = value; Signal(); } }

        public CommandMvvm Remove { get; set; }
        public CommandMvvm Cancel { get; set; }
        public RemoveProductVM(Product product)
        {
            
            Remove = new CommandMvvm(() =>
            {
                if (product.Quantity == Quantity)
                    ProductDB.GetDB().Remove(product);
                else
                {
                    product.Quantity -= Quantity;
                    ProductDB.GetDB().Update(product);
                }
                ProductDB.GetDB().Update(product);
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
