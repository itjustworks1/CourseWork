﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Разработка_магазина_для_продажи_стройматериалов.Model;
using Разработка_магазина_для_продажи_стройматериалов.ViewModel;

namespace Разработка_магазина_для_продажи_стройматериалов.View
{
    /// <summary>
    /// Логика взаимодействия для WindowAddEditProduct.xaml
    /// </summary>
    public partial class WindowAddEditProduct : Window
    {
        public WindowAddEditProduct(Product product, ref bool isEdit)
        {
            InitializeComponent();
            var vm = new AddEditProductVM(this, product, ref isEdit);
            DataContext = vm;

            vm.SetHide(Hide);
            vm.SetClose(Close);
            //((AddAuthorMvvm)this.DataContext).SetAuthor(author);
        }
    }
}
