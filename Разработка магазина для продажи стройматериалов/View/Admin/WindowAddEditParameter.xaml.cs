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
using Magaz_Stroitelya.ViewModel;
using Magaz_Stroitelya.ViewModel.NoAdmin;

namespace Magaz_Stroitelya.View
{
    /// <summary>
    /// Логика взаимодействия для WindowRelationshipsTypesToParameter.xaml
    /// </summary>
    public partial class WindowAddEditParameter : Window
    {
        public WindowAddEditParameter()
        {
            InitializeComponent();
            //DataContext = new AddEditParameterVM();
        }
    }
}
