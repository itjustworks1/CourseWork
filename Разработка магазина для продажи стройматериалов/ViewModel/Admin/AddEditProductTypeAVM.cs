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
    public class AddEditProductTypeAVM : BaseVM
    {
        private ProductTypeResponse selectedProductType;
        private ParameterResponse selectedParameter;
        private ObservableCollection<ProductTypeResponse> productTypes = new();
        private ObservableCollection<ParameterResponse> parameters = new();
        //бебебе ProductTypeParameter не request
        private ObservableCollection<ProductTypeParameter> productTypeParameters = new();
        private ObservableCollection<ProductTypeParameter> selectedProductTypeParametersOnProductType = new();
        private ProductTypeParameter selectedProductTypeParameter;

        public ProductTypeParameter SelectedProductTypeParameter { get => selectedProductTypeParameter; set { selectedProductTypeParameter = value; Signal(); } }
        public ObservableCollection<ProductTypeParameter> ProductTypeParameters { get => productTypeParameters; set { productTypeParameters = value; Signal(); } }
        public ObservableCollection<ProductTypeParameter> SelectedProductTypeParametersOnProductType { get => selectedProductTypeParametersOnProductType; set { selectedProductTypeParametersOnProductType = value; Signal(); } }
        public ObservableCollection<ParameterResponse> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public ObservableCollection<ProductTypeResponse> ProductTypes { get => productTypes; set { productTypes = value; Signal(); } }
        public ParameterResponse SelectedParameter { get => selectedParameter; set { selectedParameter = value; Signal(); } }
        public ProductTypeResponse SelectedProductType { get => selectedProductType; set { selectedProductType = value;
                //if (SelectedProductType != null) SelectedProductTypeParametersOnProductType = new ObservableCollection<ProductTypeParameter>(ProductTypeParameterDB.GetDB().SelectAll().Where(s => s.ProductTypeId == SelectedProductType.Id));
                Signal(); } }

        public CommandMvvm AddProductType { get; set; }
        public CommandMvvm EditProductType { get; set; }
        public CommandMvvm RemoveProductType { get; set; }
        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }
        public CommandMvvm OpenAddEditParameter { get; set; }

        public AddEditProductTypeAVM(WindowAddEditProductType thisWindow)
        {
            SelectAll();
            AddProductType = new CommandMvvm(() =>
            {
                //ProductTypeDB.GetDB().Insert(SelectedProductType);
                SelectAll();
            }, () => SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            EditProductType = new CommandMvvm(() =>
            {
                //ProductTypeDB.GetDB().Update(SelectedProductType);
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
                //ProductTypeParameter productTypeParameter = new ProductTypeParameter() 
                //{
                //    Parameter = SelectedParameter,
                //    ParameterId = SelectedParameter.Id,
                //    ProductType = SelectedProductType,
                //    ProductTypeId = SelectedProductType.Id
                //};
                //ProductTypeParameterDB.GetDB().Insert(productTypeParameter);
                SelectAll();
            }, () => SelectedParameter != null &&
            SelectedProductType != null &&
            !string.IsNullOrEmpty(SelectedProductType.Title));

            EditParameter = new CommandMvvm(() =>
            {
                //Parameter parameter = SelectedProductTypeParameter.Parameter;
                //SelectedProductTypeParameter.Parameter = SelectedParameter;
                //SelectedProductTypeParameter.ParameterId = SelectedParameter.Id;
                //ProductTypeParameterDB.GetDB().UpdateParameter(SelectedProductTypeParameter, parameter);
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
                hide();
                new WindowAddEditParameter().ShowDialog();
                thisWindow.ShowDialog();
                SelectAll();
            }, () => true);

        }

        private void SelectAll()
        {
            //SelectedProductType = new ProductType();
            //ProductTypes = new ObservableCollection<ProductType>(ProductTypeDB.GetDB().SelectAll());
            //Parameters = new ObservableCollection<Parameter>(ParameterDB.GetDB().SelectAll());
            //ProductTypeParameters = new ObservableCollection<ProductTypeParameter>(ProductTypeParameterDB.GetDB().SelectAll());
            //selectedProductTypeParametersOnProductType = new ObservableCollection<ProductTypeParameter>(ProductTypeParameterDB.GetDB().SelectAll());
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
    }
}
