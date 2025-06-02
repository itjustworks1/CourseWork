using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Разработка_магазина_для_продажи_стройматериалов.DB;
using Разработка_магазина_для_продажи_стройматериалов.Model;
using Разработка_магазина_для_продажи_стройматериалов.View;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;

namespace Разработка_магазина_для_продажи_стройматериалов.ViewModel
{
    public class AddEditParameterVM : BaseVM
    {
        private Parameter selectedParameter;
        private ObservableCollection<Parameter> parameters = new();

        public ObservableCollection<Parameter> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public Parameter SelectedParameter { get => selectedParameter; set { selectedParameter = value; Signal(); } }

        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }

        public AddEditParameterVM()
        {
            SelectedParameter = new Parameter();
            SelectAll();
            AddParameter = new CommandMvvm(() =>
            {
                ParameterDB.GetDB().Insert(SelectedParameter);
                SelectAll();
            }, () => !string.IsNullOrEmpty(SelectedParameter.Title));

            EditParameter = new CommandMvvm(() =>
            {
                ParameterDB.GetDB().Update(SelectedParameter);
                SelectAll();
            }, () => !string.IsNullOrEmpty(SelectedParameter.Title));
            
            RemoveParameter = new CommandMvvm(() =>
            {
                //ParameterDB.GetDB().Remove(SelectedParameter);
                SelectAll();
            }, () => !string.IsNullOrEmpty(SelectedParameter.Title));
        }

        private void SelectAll()
        {
            Parameters = new ObservableCollection<Parameter>(ParameterDB.GetDB().SelectAll());
        }
    }
}
