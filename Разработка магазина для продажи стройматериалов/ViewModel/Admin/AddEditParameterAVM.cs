using System.Collections.ObjectModel;
using MVVM.Model.DTO.Response;
using MVVM.Services;
using MVVM.VMTools;

namespace MVVM.ViewModel.Admin
{
    public class AddEditParameterAVM : BaseVM
    {
        private ApiClient apiClient;

        private ParameterResponse selectedParameter;
        private ObservableCollection<ParameterResponse> parameters = new();

        public ObservableCollection<ParameterResponse> Parameters { get => parameters; set { parameters = value; OnPropertyChanged(); } }
        public ParameterResponse SelectedParameter { get => selectedParameter; set { selectedParameter = value; OnPropertyChanged(); } }

        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }

        public AddEditParameterAVM(ApiClient apiClient)
        {
            this.apiClient = apiClient;

            SelectedParameter = new ParameterResponse();
            Task.Run(() => SelectAll());
            AddParameter = new CommandMvvm(async () =>
            {
                await apiClient.PostParameter(SelectedParameter);
                await Task.Run(() => SelectAll());
            }, () => SelectedParameter != null &&
            !string.IsNullOrEmpty(SelectedParameter.Title));

            EditParameter = new CommandMvvm(async () =>
            {
                await apiClient.PatchParameter(SelectedParameter.Id,SelectedParameter);
                await Task.Run(() => SelectAll());
            }, () => SelectedParameter != null &&
            !string.IsNullOrEmpty(SelectedParameter.Title));
            
            RemoveParameter = new CommandMvvm(async () =>
            {
                await apiClient.DeleteParameter(SelectedParameter.Id);
                await Task.Run(() => SelectAll());
            }, () => SelectedParameter != null &&
            !string.IsNullOrEmpty(SelectedParameter.Title));
        }

        private async void SelectAll()
        {
            var (list, error) = await apiClient.GetListParameter();
            var listParameter = new ObservableCollection<ParameterResponse>(list);
            Parameters = listParameter;
        }
    }
}
