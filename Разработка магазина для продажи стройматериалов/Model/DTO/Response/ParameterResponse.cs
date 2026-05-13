namespace MVVM.Model.DTO.Response
{
    public class ParameterResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            if (obj.GetType().Name.Contains(nameof(ParameterResponse)))
            {
                var example = (ParameterResponse)obj;

                return this.Id == example.Id && Title == example.Title;

            }
            return base.Equals(obj);
        }
    }
}
