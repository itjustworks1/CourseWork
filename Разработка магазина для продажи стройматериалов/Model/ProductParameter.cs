using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Разработка_магазина_для_продажи_стройматериалов.Model
{
    public class ProductParameter
    {
        public int Id { get; set; }
        public string Meaning { get; set; }
        public Parameter Parameter { get; set; }
        public int ParameterId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
