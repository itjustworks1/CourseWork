using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;

namespace Magaz_Stroitelya.Model
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
