using MomAndChildren.WpfApp.UI;
using MomAndChildrenWpfApp.UI;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MomAndChildrenWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Open_wOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            var p = new wOrderDetail();
            p.Owner = this;
            p.Show();
        }

        private async void Open_wCategory_Click(object sender, RoutedEventArgs e)
        {
            var p = new wCategory();
            p.Owner = this;
            p.Show();
        }

        private async void Open_wBrand_Click(object sender, RoutedEventArgs e)
        {
            var p = new wBrand();
            p.Owner = this;
            p.Show();
        }

        private async void Open_wProduct_Click(object sender, RoutedEventArgs e)
        {
            var p = new wProduct();
            p.Owner = this;
            p.Show();
        }

        private async void Open_wPayment_Click(object sender, RoutedEventArgs e)
        {
            var p = new wPayment();
            p.Owner = this;
            p.Show();
        }
    }
}