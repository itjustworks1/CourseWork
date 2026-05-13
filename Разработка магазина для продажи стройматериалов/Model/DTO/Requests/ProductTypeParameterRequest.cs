using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model.DTO.Requests
{
    public class ProductTypeParameterRequest
    {
        public int ProductTypeId { get; set; }
        public int ParameterId { get; set; }
    }
}
