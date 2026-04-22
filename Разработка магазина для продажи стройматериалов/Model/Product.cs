using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Разработка_магазина_для_продажи_стройматериалов.View;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;

namespace Разработка_магазина_для_продажи_стройматериалов.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
        public int Quantity { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }


        public CommandMvvm AddToCart { get; set; }

        public Product()
        {
            AddToCart = new CommandMvvm(() =>
            {
                new WindowAddToCart(this).ShowDialog();
            }, () => this != null);
        }
    }
}
