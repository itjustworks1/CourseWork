using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model.DTO.Response
{
    public class ProductTypeParameterResponse
    {
        public int ProductTypeId { get; set; }
        public ProductTypeResponse ProductType { get; set; }
        public int ParameterId { get; set; }
        public ParameterResponse Parameter { get; set; }
    }
}
