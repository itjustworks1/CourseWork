using System;
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
using Разработка_магазина_для_продажи_стройматериалов.ViewModel;

namespace Разработка_магазина_для_продажи_стройматериалов.View
{
    /// <summary>
    /// Логика взаимодействия для WindowRelationshipsParametersToType.xaml
    /// </summary>
    public partial class WindowAddEditProductType : Window
    {
        public WindowAddEditProductType()
        {
            InitializeComponent();
            DataContext = new AddEditProductTypeVM();
        }
    }
}
