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
        private OrderStructure orderStructure = new();

        public OrderStructure OrderStructure { get => orderStructure; set { orderStructure = value; Signal(); } }
        public ObservableCollection<ProductParameter> ProductParameters { get => productParameters; set { productParameters = value; Signal(); } }
        public ObservableCollection<Product> Products { get => products; set { products = value; Signal(); } }
        public Product SelectedProduct { get => selectedProduct; set { selectedProduct = value; Signal(); } }

        public CommandMvvm AddToCart { get; set; }
        public CommandMvvm EditProduct { get; set; }
        public CommandMvvm RemoveProduct { get; set; }
        public CommandMvvm OpenCart { get; set; }

        public ProductVM(WindowProduct thisWindow, Product product)
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
                //Product product1 = new Product() 
                //{ 
                //    Id = SelectedProduct.Id,
                //    ProductType = SelectedProduct.ProductType,
                //    ProductTypeId = SelectedProduct.ProductTypeId,
                //    Quantity = SelectedProduct.Quantity,
                //    Title = SelectedProduct.Title,
                //    Value = SelectedProduct.Value 
                //};
                bool isEdit = false;
                hide();
                new WindowAddEditProduct(SelectedProduct, ref isEdit).ShowDialog();
                //if (!isEdit) SelectedProduct = product1;
                SelectAll();
                thisWindow.ShowDialog();
            }, () => true);
            //
            RemoveProduct = new CommandMvvm(() =>
            {
                new WindowRemoveProduct(SelectedProduct).ShowDialog();
                SelectAll();
            }, () => SelectedProduct != null);
            
            OpenCart = new CommandMvvm(() =>
            {
                hide();
                new WindowCart().ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => true);
        }

        private void SelectAll()
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDB().SelectAll());
            ProductParameters = new ObservableCollection<ProductParameter>(ProductParameterDB.GetDB().SelectAll().Where(s => s.ProductId == SelectedProduct.Id));
            ObservableCollection<Order> orders = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll());
            Order order = orders.FirstOrDefault(s => s.Status == false);
            ObservableCollection<OrderStructure> orderStructures = new ObservableCollection<OrderStructure>(OrderStructureDB.GetDB().SelectAll().Where(s => s.OrderId == order.Id));
            OrderStructure = orderStructures.FirstOrDefault(s => s.ProductId == SelectedProduct.Id);
            if (OrderStructure == null) OrderStructure = new OrderStructure(){ Quantity = 0 };
        }
        Action close;

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
