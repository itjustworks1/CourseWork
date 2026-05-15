using CommunityToolkit.Mvvm.ComponentModel;
using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;

namespace MVVM.Model.DTO.Response
{
    public partial class OrderStructureResponse : ObservableObject
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public int ProductId { get; set; }
        [ObservableProperty] private ProductResponse _product;
        //public ProductResponse Product { get; set; }
        public int OrderId { get; set; }
        [ObservableProperty] private OrderResponse _order;

        public CommandMvvm RemoveFromCart { get; set; }

        public OrderStructureResponse()
        {
            RemoveFromCart = new CommandMvvm(() =>
            {
                new WindowRemoveFromCart(this, new ApiClient()).ShowDialog();
            }, () => this != null);
        }
    }
}
