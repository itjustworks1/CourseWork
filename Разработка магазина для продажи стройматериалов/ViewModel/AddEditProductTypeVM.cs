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
        private ObservableCollection<ProductTypeParameter> productTypeParameters = new();
        private ObservableCollection<ProductTypeParameter> selectedProductTypeParametersOnProductType = new();
        private ProductTypeParameter selectedProductTypeParameter;

        public ProductTypeParameter SelectedProductTypeParameter { get => selectedProductTypeParameter; set { selectedProductTypeParameter = value; Signal(); } }
        public ObservableCollection<ProductTypeParameter> ProductTypeParameters { get => productTypeParameters; set { productTypeParameters = value; Signal(); } }
        public ObservableCollection<ProductTypeParameter> SelectedProductTypeParametersOnProductType { get => selectedProductTypeParametersOnProductType; set { selectedProductTypeParametersOnProductType = value; Signal(); } }
        public ObservableCollection<Parameter> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public ObservableCollection<ProductType> ProductTypes { get => productTypes; set { productTypes = value; Signal(); } }
        public Parameter SelectedParameter { get => selectedParameter; set { selectedParameter = value; Signal(); } }
        public ProductType SelectedProductType { get => selectedProductType; set { selectedProductType = value; if (SelectedProductType != null) SelectedProductTypeParametersOnProductType = new ObservableCollection<ProductTypeParameter>(ProductTypeParameterDB.GetDB().SelectAll().Where(s => s.ProductTypeId == SelectedProductType.Id)); Signal(); } }

        public CommandMvvm AddProductType { get; set; }
        public CommandMvvm EditProductType { get; set; }
        public CommandMvvm RemoveProductType { get; set; }
        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }
        public CommandMvvm OpenAddEditParameter { get; set; }

        public AddEditProductTypeVM()
        {
            SelectAll();
            AddProductType = new CommandMvvm(() =>
            {
                ProductTypeDB.GetDB().Insert(SelectedProductType);
                SelectAll();
            }, () => SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            EditProductType = new CommandMvvm(() =>
            {
                ProductTypeDB.GetDB().Update(SelectedProductType);
                SelectAll();
            }, () => SelectedProductType != null &&
             !string.IsNullOrEmpty(SelectedProductType.Title));

            RemoveProductType = new CommandMvvm(() =>
            {

                SelectAll();
            }, () => SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            AddParameter = new CommandMvvm(() =>
            {
                ProductTypeParameter productTypeParameter = new ProductTypeParameter() 
                {
                    Parameter = SelectedParameter,
                    ParameterId = SelectedParameter.Id,
                    ProductType = SelectedProductType,
                    ProductTypeId = SelectedProductType.Id
                };
                ProductTypeParameterDB.GetDB().Insert(productTypeParameter);
                SelectAll();
            }, () => SelectedParameter != null &&
            SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            EditParameter = new CommandMvvm(() =>
            {
                Parameter parameter = SelectedProductTypeParameter.Parameter;
                SelectedProductTypeParameter.Parameter = SelectedParameter;
                SelectedProductTypeParameter.ParameterId = SelectedParameter.Id;
                ProductTypeParameterDB.GetDB().UpdateParameter(SelectedProductTypeParameter, parameter);
                SelectAll();
            }, () => SelectedProductTypeParameter != null &&
            SelectedParameter != null &&
            SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            RemoveParameter = new CommandMvvm(() =>
            {
                ProductTypeParameterDB.GetDB().Remove(SelectedProductTypeParameter);
                SelectAll();
            }, () => SelectedProductTypeParameter != null &&
            SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            OpenAddEditParameter = new CommandMvvm(() =>
            {
                new WindowAddEditParameter().ShowDialog();
                SelectAll();
            }, () => true);

        }

        private void SelectAll()
        {
            SelectedProductType = new ProductType();
            ProductTypes = new ObservableCollection<ProductType>(ProductTypeDB.GetDB().SelectAll());
            Parameters = new ObservableCollection<Parameter>(ParameterDB.GetDB().SelectAll());
            ProductTypeParameters = new ObservableCollection<ProductTypeParameter>(ProductTypeParameterDB.GetDB().SelectAll());
            //selectedProductTypeParametersOnProductType = new ObservableCollection<ProductTypeParameter>(ProductTypeParameterDB.GetDB().SelectAll());
        }
    }
}
