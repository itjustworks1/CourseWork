using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Разработка_магазина_для_продажи_стройматериалов.View;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;

namespace Разработка_магазина_для_продажи_стройматериалов.Model
{
    public class OrderStructure
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }

        public CommandMvvm RemoveFromCart { get; set; }

        public OrderStructure()
        {
            RemoveFromCart = new CommandMvvm(() =>
            {
               new WindowRemoveFromCart(this).ShowDialog();
            }, () => this != null);
        }
    }
}
