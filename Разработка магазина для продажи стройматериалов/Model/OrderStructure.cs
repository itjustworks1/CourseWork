using MVVM.VMTools;

namespace MVVM.Model
{
    public class OrderStructure
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }

        public CommandMvvm RemoveFromCart { get; set; }

        public OrderStructure()
        {
            RemoveFromCart = new CommandMvvm(() =>
            {
               //new WindowRemoveFromCart(this).ShowDialog();
            }, () => this != null);
        }
    }
}
