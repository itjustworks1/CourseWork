using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magaz_Stroitelya.Model;
using Magaz_Stroitelya.View;
using Magaz_Stroitelya.DB;
using Magaz_Stroitelya.VMTools;
using System.Windows;

namespace Magaz_Stroitelya.ViewModel.NoAdmin
{
    /* Список задач
     * Сделать [редактирование заказов (наверное серез корзину)]. + список заказов + состав заказов // я хз что написано
     * Настроить все Remove (особенно Product ?) (кроме WindowListOrders + ...) 
           // стоит ли мне вообще удалять Parameter и ProductType?; думаю можно удалять но при условии что их нигде нет; но тогда придеться страдать с проверками
     * Что-то сделать с поиском ?
     * Разобраться с ценами в заказе // я хз, вроде сделал
     * У всех Листов добавить верт скролл
     * В WindowProduct [не меняется TextBlock] + [доделать вывод инфы] + хотелось бы чуть исправить изменения
     * В WindowAddEditProduct [сломался Parameter] + хотелось что бы ComboBox менялся от выбранных параметров;
           теперь просто не добавляется параметр + почему всё сразу меняется + [при удалении параметра накричали]
           // всё из-за сложности совместить ProductParameter с Parameter
     * В WindowAddEditProductType доделать список Parameter + настроить связи
     */
    /* Готово
     *  [визуал в 3 мелких окнах]
     * [В WindowListOrders сделать отдельно для карзины(false)].
     * [Исправить кнопку добавление/удаления товара в/из корзину/ы, редактирование Order].
     */


    /* ЫЫЫЫЫЫЫЫЫЫЫЫЫЫЫЫЫ
     * No Admin:
     * мейн- MainVM
     * (+в корзину)- AddToCartVM
     * корзина- CartVM
     * (-из корзины)- RemoveFromCartVM
     * заказы- ListOrdersVM
     * заказ- OrderVM
     * инфа продукта- ProductVM
     * 
     * Admin:
     * мейн- MainAVM
     * заказыПо- 
     * заказ- 
     * все изменения:
     *  продукт- AddEditProductAVM
     *  параметр- AddEditParameterAVM
     *  типПр- AddEditProductTypeAVM
     *  заказ- 
     * инфа продукта- 
     * удаление колва продуктов- RemoveProductVM
     * 
     * //и наверное список пользователей
     * 
     * Общие:
     * логин- LoginVM
     * регистрация- RegisterVM
     * 
     */
    public class MainVM : BaseVM
    {
        private Product selectedProduct;
        private ObservableCollection<Product> products = new();
        private string search;

        public ObservableCollection<Product> Products { get => products; set { products = value; Signal(); } }
        public Product SelectedProduct { get => selectedProduct; set { selectedProduct = value; Signal(); } }
        public string Search { get => search; set { search = value; SearchProduct(search); } }

        public CommandMvvm AddToCart { get; set; }
        public CommandMvvm OpenCart { get; set; }
        public CommandMvvm OpenProduct { get; set; }

        public MainVM(MainWindow thisWindow)
        {
            SelectAll();


            AddToCart = new CommandMvvm(() =>
            {
                SelectAll();
            }, () => true);

            OpenCart = new CommandMvvm(() =>
            {
                hide();
                new WindowCart().ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => true);

            OpenProduct = new CommandMvvm(() =>
            {
                hide();
                new WindowProduct(SelectedProduct).ShowDialog();
                SelectAll();
                thisWindow.ShowDialog();
            }, () => SelectedProduct != null);

        }

        private void SelectAll()
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDB().SelectAll().OrderByDescending(t => t.Title));
        }
        private void SearchProduct(string search)
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDB().SearchProduct(search));
        }
        Action hide;

        internal void SetHide(Action hide)
        {
            this.hide = hide;
        }
    }
}
