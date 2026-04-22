using Magaz_Stroitelya.VMTools;

namespace MVVM.Model.DTO.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Value { get; set; }
        public int Quantity { get; set; }
        public int ProductTypeId { get; set; }
        public ProductTypeResponse ProductType { get; set; }

        public CommandMvvm AddToCart { get; set; }

        public ProductResponse()
        {
            AddToCart = new CommandMvvm(() =>
            {
                //new WindowAddToCart(this).ShowDialog();
            }, () => this != null);
        }
    }
}
