using Magaz_Stroitelya.Services;
using Magaz_Stroitelya.VMTools;
using System.Collections.ObjectModel;
using System.Windows;

namespace MVVM.Model.DTO.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }

        public CommandMvvm EditOrder { get; set; }

        public OrderResponse()
        {
            ApiClient apiClient = new ApiClient();
            EditOrder = new CommandMvvm(async () =>
            {
                if (MessageBox.Show("Корзина будет очищена.\nПродолжить?", "Подтверждение", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var (listOrder, error) = await apiClient.GetListOrder();
                    (var listOrderStructure, error) = await apiClient.GetListOrderStructure();
                    var rOrder = new ObservableCollection<OrderResponse>(listOrder).FirstOrDefault(s => s.Status == false);
                    var rOrderStructures = new ObservableCollection<OrderStructureResponse>(listOrderStructure.Where(s => s.OrderId == rOrder.Id));
                    foreach (var o in rOrderStructures)
                        await apiClient.DeleteOrderStructure(o.Id);
                    await apiClient.DeleteOrder(rOrder.Id);
                    Status = false;
                    await apiClient.PatchOrder(Id, new OrderRequest
                    {
                        Date = Date,
                        UserId = UserId,
                        Status = Status
                    });
                }

            }, () => true);
        }
    }
}
