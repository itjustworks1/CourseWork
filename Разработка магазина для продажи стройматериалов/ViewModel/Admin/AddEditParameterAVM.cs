using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magaz_Stroitelya.DB;
using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.VMTools;
using MVVM.Model.DTO.Response;

namespace Magaz_Stroitelya.ViewModel.Admin
{
    public class AddEditParameterAVM : BaseVM
    {
        private ParameterResponse selectedParameter;
        private ObservableCollection<ParameterResponse> parameters = new();

        public ObservableCollection<ParameterResponse> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public ParameterResponse SelectedParameter { get => selectedParameter; set { selectedParameter = value; Signal(); } }

        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }

        public AddEditParameterAVM()
        {
            SelectedParameter = new ParameterResponse();
            SelectAll();
            AddParameter = new CommandMvvm(() =>
            {
                //ParameterDB.GetDB().Insert(SelectedParameter);
                SelectAll();
            }, () => SelectedParameter != null &&
            !string.IsNullOrEmpty(SelectedParameter.Title));

            EditParameter = new CommandMvvm(() =>
            {
                //ParameterDB.GetDB().Update(SelectedParameter);
                SelectAll();
            }, () => SelectedParameter != null &&
            !string.IsNullOrEmpty(SelectedParameter.Title));
            
            RemoveParameter = new CommandMvvm(() =>
            {
                //ParameterDB.GetDB().Remove(SelectedParameter);
                SelectAll();
            }, () => SelectedParameter != null &&
            !string.IsNullOrEmpty(SelectedParameter.Title));
        }

        private void SelectAll()
        {
            //Parameters = new ObservableCollection<Parameter>(ParameterDB.GetDB().SelectAll());
        }
    }
}
