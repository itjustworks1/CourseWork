using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private ObservableCollection<ProductParameter> productParameters = new();
        private ProductParameter selectedProductParameter;
        private ObservableCollection<ProductParameter> selectedProductParameters = new();
        private ObservableCollection<Parameter> parameters = new();
        private ObservableCollection<ProductParameter> selectedSelectedProductParameters = new();

        public ObservableCollection<ProductParameter> SelectedSelectedProductParameters { get => selectedSelectedProductParameters; set { selectedSelectedProductParameters = value; Signal(); } }
        public ObservableCollection<Parameter> Parameters { get => parameters; set { parameters = value; Signal(); } }
        public ObservableCollection<ProductParameter> SelectedProductParameters { get => selectedProductParameters; set { selectedProductParameters = value; Signal(); } }
        public ProductParameter SelectedProductParameter { get => selectedProductParameter; set { selectedProductParameter = value; Signal(); } }
        public ObservableCollection<ProductParameter> ProductParameters { get => productParameters; set { productParameters = value; Signal(); } }
        public ObservableCollection<ProductType> ProductTypes { get => productTypes; set { productTypes = value; Signal(); } }
        public ObservableCollection<Product> Products { get => products; set { products = value; Signal(); } }
        public Product SelectedProduct { get => selectedProduct; set { selectedProduct = value; Signal(); } }

        public CommandMvvm Save { get; set; }
        public CommandMvvm Cancel { get; set; }
        public CommandMvvm AddParameter { get; set; }
        public CommandMvvm EditParameter { get; set; }
        public CommandMvvm RemoveParameter { get; set; }
        public CommandMvvm OpenAddEditProductType { get; set; }

        public AddEditProductVM(WindowAddEditProduct thisWindow, Product product, ref bool isEdit)
        {
            SelectedProduct = product;
            SelectAll();
            SelectedSelectedProductParameters = SelectedProductParameters;
            AddParameter = new CommandMvvm(() =>
            {
                ProductParameter productParameter = new ProductParameter();
                productParameter.Meaning = SelectedProductParameter.Meaning;
                productParameter.Parameter = SelectedProductParameter.Parameter;
                productParameter.ParameterId = productParameter.Parameter.Id;
                productParameter.Product = SelectedProduct;
                productParameter.ProductId = SelectedProduct.Id;
                SelectedSelectedProductParameters.Add(productParameter);
                SelectAll();
            }, () =>
            SelectedProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning)
            );

            EditParameter = new CommandMvvm(() =>
            {
                for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    if (SelectedSelectedProductParameters[i] == SelectedProductParameter)
                        SelectedSelectedProductParameters[i] = SelectedProductParameter;
                SelectAll();
            }, () =>
            SelectedProductParameter != null &&
            SelectedProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning)
            );

            RemoveParameter = new CommandMvvm(() =>
            {
                for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    if (SelectedSelectedProductParameters[i] == SelectedProductParameter)
                        SelectedSelectedProductParameters.Remove(SelectedSelectedProductParameters[i]);
                // при синхронизации удаляет оба
                SelectAll();
                // при обновлении они десинхронизируются
            }, () =>
            SelectedProductParameter != null &&
            SelectedProductParameter.Parameter != null &&
            !string.IsNullOrEmpty(SelectedProductParameter.Meaning)
            );

            OpenAddEditProductType = new CommandMvvm(() =>
            {
                hide();
                new WindowAddEditProductType().ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => true);

            Save = new CommandMvvm(() =>
            {
                SelectedProduct.ProductTypeId = SelectedProduct.ProductType.Id;
                if (SelectedProduct.Id > 0)
                    ProductDB.GetDB().Update(SelectedProduct);
                else
                { 
                    ProductDB.GetDB().Insert(SelectedProduct);
                    Products = new ObservableCollection<Product>(ProductDB.GetDB().SelectAll());
                    product = Products.LastOrDefault();
                    SelectedProduct = product;
                    for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    {
                        SelectedSelectedProductParameters[i].Product = SelectedProduct;
                        SelectedSelectedProductParameters[i].ProductId = SelectedProduct.Id;
                    }
                }
                foreach (var s in SelectedSelectedProductParameters)
                {
                    if (s.Id > 0)
                        ProductParameterDB.GetDB().Update(s);
                    else
                        ProductParameterDB.GetDB().Insert(s);
                }

                List<int> c = new List<int>();
                for (int i = 0; i < SelectedSelectedProductParameters.Count; i++)
                    c.Add(SelectedSelectedProductParameters[i].Id);

                for (int i = 0; i < SelectedProductParameters.Count; i++)
                    if (!c.Contains(SelectedProductParameters[i].Id)) //MessageBox.Show(SelectedProductParameters[i].Parameter.Title);
                        ProductParameterDB.GetDB().Remove(SelectedProductParameters[i]);
                IsEdit = true;
                //thisWindow
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
            if (SelectedProductParameter == null) SelectedProductParameter = new ProductParameter();
            Products = new ObservableCollection<Product>(ProductDB.GetDB().SelectAll());
            ProductTypes = new ObservableCollection<ProductType>(ProductTypeDB.GetDB().SelectAll());
            if (SelectedProduct.ProductType != null)
                SelectedProduct.ProductType = ProductTypes.FirstOrDefault(s => s.Id == SelectedProduct.ProductTypeId);
            ProductParameters = new ObservableCollection<ProductParameter>(ProductParameterDB.GetDB().SelectAll());
            SelectedProductParameters = new ObservableCollection<ProductParameter>(ProductParameterDB.GetDB().SelectAll().Where(s => s.ProductId == SelectedProduct.Id));
            Parameters = new ObservableCollection<Parameter>(ParameterDB.GetDB().SelectAll());

        }
        Action close;
        public bool IsEdit { get; set; } = false;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
    }
}
