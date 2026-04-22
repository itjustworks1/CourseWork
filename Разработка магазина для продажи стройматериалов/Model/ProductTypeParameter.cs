using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Разработка_магазина_для_продажи_стройматериалов.Model
{
    public class ProductTypeParameter
    {
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public Parameter Parameter { get; set; }
        public int ParameterId { get; set; }
    }
}
