using CommunityToolkit.Mvvm.ComponentModel;

namespace MVVM.Model.DTO.Response
{
    public class ProductTypeResponse : ObservableObject
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
