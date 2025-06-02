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
using System.Windows;

namespace Разработка_магазина_для_продажи_стройматериалов.ViewModel
{
    /* Список задач
     * Сделать [редактирование заказов (наверное серез корзину)]. + список заказов + состав заказов // я хз что написано
     * Настроить все Remove (особенно Product ?) (кроме WindowListOrders + ...) 
           // стоит ли мне вообще удалять Parameter и ProductType?; думаю можно удалять но при условии что их нигде нет; но тогда придеться страдать с проверками
     * Что-то сделать с поиском ?
     * Разобраться с ценами в заказе // я хз, вроде сделал
     * В WindowProduct не меняется TextBlock + [доделать вывод инфы]
     * В WindowAddEditProduct [сломался Parameter] + хотелось что бы ComboBox менялся от выбранных параметров;
           теперь просто не добавляется параметр 
           // всё из-за сложности совместить ProductParameter с Parameter
     * В WindowAddEditProductType доделать список Parameter + настроить связи
     */
    /* На повестке дня
     * Доделал Order
     */
    /* Готово
     * [В WindowListOrders сделать отдельно для карзины(false)].
     * [Исправить кнопку добавление/удаления товара в/из корзину/ы, редактирование Order].
     */
    public class MainVM : BaseVM
    {
        private Product selectedProduct;
        private ObservableCollection<Product> products = new();
        private string search;

        public ObservableCollection<Product> Products { get => products; set { products = value; Signal(); } }
        public Product SelectedProduct { get => selectedProduct; set { selectedProduct = value; Signal(); } }
        public string Search { get => search; set { search = value; SearchProduct(search); } }

        public CommandMvvm AddProduct { get; set; }
        public CommandMvvm AddToCart { get; set; }
        public CommandMvvm OpenCart { get; set; }
        public CommandMvvm OpenProduct { get; set; }

        public MainVM()
        {
            SelectAll();
            //!![#]
            AddProduct = new CommandMvvm(() =>
            {
                Product product = new Product();
                new WindowAddEditProduct(product).ShowDialog();
                SelectAll();
            }, () => true);
            //!!#
            AddToCart = new CommandMvvm(() =>
            {
                //корзина = список OrderStructure в нынешнем Order ?!      \
                // окна под список Order будет !                            } Что Такое Order - это p1-o1;p2-o1;p3-o1; p1-o2;p2-o2
                // Status у Orger это либо оплачен ? либо завершен ?! !    /
                SelectAll();
            }, () => true);
            //!?[.]
            OpenCart = new CommandMvvm(() =>
            {
                new WindowCart().ShowDialog();
                SelectAll();
            }, () => true);
            //!!#
            OpenProduct = new CommandMvvm(() =>
            {
                new WindowProduct(SelectedProduct).ShowDialog();
                SelectAll();
            }, () => SelectedProduct != null);

        }

        private void SelectAll()
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDB().SelectAll());
        }
        private void SearchProduct(string search)
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDB().SearchProduct(search));
        }
    }
}
