using Magaz_Stroitelya.ViewModel.Admin;
using Magaz_Stroitelya.VMTools;

namespace MVVM.Model.DTO.Response
{
    public class ProductParameterResponse
    {
        public int Id { get; set; }
        public string Meaning { get; set; } = null!;
        public int ParameterId { get; set; }
        public ParameterResponse Parameter { get; set; }
        public int ProductId { get; set; }
        public ProductResponse Product { get; set; }


        //public static implicit operator ProductParameterVM(ProductParameterResponse obj)
        //{
        //    ProductParameterVM ret = new();
        //    ret.Id = obj.Id;
        //    ret.Meaning = obj.Meaning;  
        //    ret.ParameterId = obj.ParameterId;
        //    ret.Parameter = obj.Parameter;
        //    ret.ProductId = obj.ProductId;
        //    ret.Product = obj.Product;
        //    return ret;
        //}
    }
}
