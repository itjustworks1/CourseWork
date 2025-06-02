using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Разработка_магазина_для_продажи_стройматериалов.Model;
using Разработка_магазина_для_продажи_стройматериалов.View;
using Разработка_магазина_для_продажи_стройматериалов.DB;
using Разработка_магазина_для_продажи_стройматериалов.VMTools;

namespace Разработка_магазина_для_продажи_стройматериалов.ViewModel
{
    public class ProductVM : BaseVM
    {
        private Product selectedProduct;
        private ObservableCollection<Product> products = new();
        private ObservableCollection<ProductParameter> productParameters = new();

        public ObservableCollection<ProductParameter> ProductParameters { get => productParameters; set { productParameters = value; Signal(); } }
        public ObservableCollection<Product> Products { get => products; set { products = value; Signal(); } }
        public Product SelectedProduct { get => selectedProduct; set { selectedProduct = value; Signal(); } }

        public CommandMvvm AddToCart { get; set; }
        public CommandMvvm EditProduct { get; set; }
        public CommandMvvm RemoveProduct { get; set; }
        public CommandMvvm OpenCart { get; set; }

        public ProductVM(Product product)
        {
            SelectedProduct = product;
            SelectAll();
            AddToCart = new CommandMvvm(() =>
            {
                new WindowAddToCart(SelectedProduct).ShowDialog();
                SelectAll();
            }, () => SelectedProduct != null);

            EditProduct = new CommandMvvm(() =>
            {
                new WindowAddEditProduct(SelectedProduct).ShowDialog();
                SelectAll();
            }, () => true);
            //
            RemoveProduct = new CommandMvvm(() =>
            {
                new WindowRemoveProduct(SelectedProduct).ShowDialog();
                SelectAll();
            }, () => SelectedProduct != null);
            
            OpenCart = new CommandMvvm(() =>
            {
                new WindowCart().ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDB().SelectAll());
            ProductParameters = new ObservableCollection<ProductParameter>(ProductParameterDB.GetDB().SelectAll().Where(s => s.ProductId == SelectedProduct.Id));
        }
        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
