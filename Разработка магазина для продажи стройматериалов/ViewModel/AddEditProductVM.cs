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
    internal class AddEditProductVM : BaseVM
    {
        private ObservableCollection<ProductType> productTypes = new();
        private Product selectedProduct;
        private ObservableCollection<Product> products = new();
        private string search;
        private ObservableCollection<ProductParameter> productParameters = new();
        private ProductParameter selectedProductParameter;
        private ObservableCollection<ProductParameter> selectedProductParameters = new();
        private ObservableCollection<Parameter> parameters = new();

        public ObservableCollection<Parameter> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public ObservableCollection<ProductParameter> SelectedProductParameters { get => selectedProductParameters; set { selectedProductParameters = value; Signal(); } }
        public ProductParameter SelectedProductParameter { get => selectedProductParameter; set { selectedProductParameter = value; Signal(); } }
        public ObservableCollection<ProductParameter> ProductParameters { get => productParameters; set { productParameters = value; Signal(); } }
        public ObservableCollection<ProductType> ProductTypes { get => productTypes; set { productTypes = value; Signal(); } }
        //public ObservableCollection<Product> Products { get => products; set { products = value; Signal(); }}
        public Product SelectedProduct { get => selectedProduct; set { selectedProduct = value; Signal(); } }

        public CommandMvvm Save { get; set; }
        public CommandMvvm Cancel { get; set; }
        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }
        public CommandMvvm OpenAddEditProductType { get; set; }

        public AddEditProductVM(Product product)
        {
            SelectedProduct = product;
            SelectedProductParameter = new ProductParameter();
            SelectAll();
            AddParameter = new CommandMvvm(() =>
            {
                ProductParameter productParameter = new ProductParameter();
                productParameter.Meaning = SelectedProductParameter.Meaning;
                productParameter.Parameter = SelectedProductParameter.Parameter;
                productParameter.ParameterId = productParameter.Parameter.Id;
                productParameter.Product = SelectedProduct;
                productParameter.ProductId = SelectedProduct.Id;
                ProductParameterDB.GetDB().Insert(productParameter);
                SelectAll();
            }, () =>
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning) &&
            SelectedProductParameter.Parameter != null
            );

            EditParameter = new CommandMvvm(() =>
            {
                ProductParameterDB.GetDB().Update(SelectedProductParameter);
                SelectAll();
            }, () =>
            SelectedProductParameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning) &&
            SelectedProductParameter.Parameter != null
            );

            RemoveParameter = new CommandMvvm(() =>
            {
                ProductParameterDB.GetDB().Remove(SelectedProductParameter);
                SelectAll();
            }, () =>
            SelectedProductParameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning) &&
            SelectedProductParameter.Parameter != null
            );
            //
            OpenAddEditProductType = new CommandMvvm(() => 
            {
                new WindowAddEditProductType().ShowDialog();
                SelectAll();
            }, () => true);
            //
            Save = new CommandMvvm(() =>
            {
                SelectedProduct.ProductTypeId = SelectedProduct.ProductType.Id;
                if (SelectedProduct.Id > 0)
                    ProductDB.GetDB().Update(SelectedProduct);
                else
                    ProductDB.GetDB().Insert(SelectedProduct);
                close();
            }, () =>
            !string.IsNullOrEmpty(SelectedProduct.Title) &&
            SelectedProduct.Value > 0 &&
            SelectedProduct.Quantity > 0 &&
            SelectedProduct.ProductType != null
            );

            Cancel = new CommandMvvm(() =>
            {
                close();
            }, () => true);
        }

        private void SelectAll()
        {
            //Products = new ObservableCollection<Product>(ProductDB.GetDB().SelectAll());
            ProductTypes = new ObservableCollection<ProductType>(ProductTypeDB.GetDB().SelectAll());
            if (SelectedProduct.ProductType != null)
                SelectedProduct.ProductType = ProductTypes.FirstOrDefault(s => s.Id == SelectedProduct.ProductTypeId);
            ProductParameters = new ObservableCollection<ProductParameter>(ProductParameterDB.GetDB().SelectAll());
            SelectedProductParameters = new ObservableCollection<ProductParameter>(ProductParameterDB.GetDB().SelectAll().Where(s => s.ProductId == SelectedProduct.Id));
            Parameters = new ObservableCollection<Parameter>(ParameterDB.GetDB().SelectAll());

        }
        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
