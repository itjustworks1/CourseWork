using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;

namespace MVVM.Model.DTO.Response
{
    public class OrderStructureResponse
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public int ProductId { get; set; }
        public ProductResponse Product { get; set; }
        public int OrderId { get; set; }
        public OrderResponse Order { get; set; }

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
