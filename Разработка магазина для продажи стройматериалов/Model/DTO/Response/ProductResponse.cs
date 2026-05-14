using CommunityToolkit.Mvvm.ComponentModel;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.ViewModel.Admin;
using Magaz_Stroitelya.VMTools;

namespace MVVM.Model.DTO.Response
{
    public partial class ProductResponse : ObservableObject
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Value { get; set; }
        public int Quantity { get; set; }
        public int ProductTypeId { get; set; }
        [ObservableProperty] private ProductTypeResponse _productType;

        public CommandMvvm AddToCart { get; set; }

        public ProductResponse()
        {
            AddToCart = new CommandMvvm(() =>
            {
                new WindowAddToCart(this, new Magaz_Stroitelya.Services.ApiClient()).ShowDialog();
            }, () => this != null);
        }

        //public static implicit operator ProductVM(ProductResponse obj)
        //{
        //    ProductVM ret = new();
        //    ret.Id = obj.Id;
        //    ret.Title = obj.Title;
        //    ret.Value = obj.Value;
        //    ret.Quantity = obj.Quantity;
        //    ret.ProductTypeId = obj.ProductTypeId;
        //    ret.ProductType = obj.ProductType;
        //    return ret;
        //}
    }
}
