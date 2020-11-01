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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BrainMRIDesktop
{
    public partial class ResultCtrl : UserControl
    {
        public ResultCtrl()
        {
            InitializeComponent();
        }



        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = (MainWindow)Window.GetWindow(this);

            parentWindow.resGrid.Children.Clear();
        }
    }
}
