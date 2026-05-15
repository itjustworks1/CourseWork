using CommunityToolkit.Mvvm.ComponentModel;

namespace MVVM.Model.DTO.Response
{
    public partial class ProductParameterResponse : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty] private string _meaning = null!;
        //public string Meaning { get; set; } = null!;
        public int ParameterId { get; set; }
        [ObservableProperty] private ParameterResponse _parameter;
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
