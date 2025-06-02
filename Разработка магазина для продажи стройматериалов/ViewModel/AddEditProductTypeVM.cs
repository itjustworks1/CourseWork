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
    public class AddEditProductTypeVM : BaseVM
    {
        private ProductType selectedProductType;
        private Parameter selectedParameter;
        private ObservableCollection<ProductType> productTypes = new();
        private ObservableCollection<Parameter> parameters = new();
        private ObservableCollection<ProductTypeParameter> selectedParameters = new();

        public ObservableCollection<ProductTypeParameter> SelectedParameters { get => selectedParameters; set { selectedParameters = value; Signal(); } }
        public ObservableCollection<Parameter> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public ObservableCollection<ProductType> ProductTypes { get => productTypes; set { productTypes = value; Signal(); } }
        public Parameter SelectedParameter { get => selectedParameter; set { selectedParameter = value; Signal(); } }
        public ProductType SelectedProductType { get => selectedProductType; set { selectedProductType = value; Signal(); } }

        public CommandMvvm AddProductType { get; set; }
        public CommandMvvm EditProductType { get; set; }
        public CommandMvvm RemoveProductType { get; set; }
        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }
        public CommandMvvm OpenAddEditParameter { get; set; }

        public AddEditProductTypeVM()
        {
            SelectedProductType = new ProductType();
            SelectAll();
            AddParameter = new CommandMvvm(() =>
            {

                SelectAll();
            }, () => SelectedParameter != null);

            EditParameter = new CommandMvvm(() =>
            {

                SelectAll();
            }, () => true);

            RemoveParameter = new CommandMvvm(() =>
            {

                SelectAll();
            }, () => SelectedParameter != null);

            //
            AddParameter = new CommandMvvm(() =>
            {
                ParameterDB.GetDB().Insert(SelectedParameter);
                SelectAll();
            }, () => SelectedParameter != null);

            EditParameter = new CommandMvvm(() =>
            {

                SelectAll();
            }, () => true);

            RemoveParameter = new CommandMvvm(() =>
            {

                SelectAll();
            }, () => SelectedParameter != null);

            //
            OpenAddEditParameter = new CommandMvvm(() =>
            {
                new WindowAddEditParameter().ShowDialog();
                SelectAll();
            }, () => true);

        }

        private void SelectAll()
        {
            ProductTypes = new ObservableCollection<ProductType>(ProductTypeDB.GetDB().SelectAll());
            Parameters = new ObservableCollection<Parameter>(ParameterDB.GetDB().SelectAll());
            //SelectedParameters = new ObservableCollection<ProductTypeParameter>(ProductTypeParameterDB.GetDB().SelectAll().Where(s => s.ProductTypeId == SelectedProductType.Id));
        }
    }
}
