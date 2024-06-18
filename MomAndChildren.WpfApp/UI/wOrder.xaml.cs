using MomAndChildren.Business;
using MomAndChildren.Data.Models;
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

namespace MomAndChildren.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wOrder.xaml
    /// </summary>
    public partial class wOrder : Window
    {
        private readonly OrderBusiness _business;
        public wOrder()
        {
            InitializeComponent();
            this._business ??= new OrderBusiness();
            this.LoadGrdOrders();
        }




        private async void LoadGrdOrders()
        {
            var result = await _business.GetOrdersAsync();

            if (result.Status > 0 && result.Data != null)
            {
                grdOrder.ItemsSource = result.Data as List<Order>;
            }
            else
            {
                grdOrder.ItemsSource = new List<Order>();
            }
        }

        //private async void grdOrder_MouseDouble_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            int idTmp = -1;
            int.TryParse(txtOrderId.Text, out idTmp);
            try
            {
                var item = await _business.GetOrderByIdAsync(idTmp);

                if (item.Data == null)
                {
                    var order = new Order()
                    {
                        OrderDate = DateTime.Parse(dpOrderDate.Text),
                        TotalPrice = Double.Parse(txtTotalPrice.Text),
                        TotalQuantity = int.Parse(txtTotalQuantity.Text),
                        ShippingDate = DateTime.Parse(dpShippingDate.Text),
                        ShippingMethod = txtShippingMethod.Text,
                        OrderNote = txtOrderNote.Text,
                        CreateAt = DateTime.Parse(dpCreateAt.Text),
                        UpdateAt = DateTime.Parse(dpUpdateAt.Text),
                        CustomerId = int.Parse(txtCustomerId.Text),
                    };

                    var result = await _business.CreateOrder(order);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var order = item.Data as Order;
                    //currency.CurrencyCode = txtCurrencyCode.Text;
                    order.OrderDate = DateTime.Parse(dpOrderDate.Text);
                    order.TotalPrice = Double.Parse(txtTotalPrice.Text);
                    order.TotalQuantity = int.Parse(txtTotalQuantity.Text);
                    order.ShippingDate = DateTime.Parse(dpShippingDate.Text);
                    order.ShippingMethod = txtShippingMethod.Text;
                    order.OrderNote = txtOrderNote.Text;
                    order.CreateAt = DateTime.Parse(dpCreateAt.Text);
                    order.UpdateAt = DateTime.Parse(dpUpdateAt.Text);
                    order.CustomerId = int.Parse(txtCustomerId.Text);

                    var result = await _business.UpdateOrder(order);
                    MessageBox.Show(result.Message, "Update");
                }

                txtOrderId.Text = string.Empty;
                dpOrderDate.Text = string.Empty;
                txtTotalPrice.Text = string.Empty;
                txtTotalQuantity.Text = string.Empty;
                dpShippingDate.Text = string.Empty;
                txtShippingMethod.Text = string.Empty;
                txtOrderNote.Text = string.Empty;
                dpCreateAt.Text = string.Empty;
                dpUpdateAt.Text = string.Empty;
                txtCustomerId.Text = string.Empty;
                this.LoadGrdOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }


        }

        private async void grdOrder_MouseDouble_Click(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("Double Click on Grid");
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Order;
                    if (item != null)
                    {
                        var currencyResult = await _business.GetOrderByIdAsync(item.OrderId);

                        if (currencyResult.Status > 0 && currencyResult.Data != null)
                        {
                            item = currencyResult.Data as Order;
                            txtOrderId.Text = item.OrderId.ToString();
                            dpOrderDate.SelectedDate = item.OrderDate;
                            txtTotalPrice.Text = item.TotalPrice.ToString();
                            txtTotalQuantity.Text = item.TotalQuantity.ToString();
                            dpShippingDate.Text = item.ShippingDate.ToString();
                            txtShippingMethod.Text = item.ShippingMethod.ToString();
                            txtOrderNote.Text = item.OrderNote.ToString();
                            dpCreateAt.Text = item.CreateAt.ToString();
                            dpUpdateAt.Text = item.UpdateAt.ToString();
                            txtCustomerId.Text = item.CustomerId.ToString();
                            //item.CustomerId = 1;

                        }
                    }
                }
            }
        }

        private async void grdOrder_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string orderId = btn.CommandParameter.ToString();

            //MessageBox.Show(currencyCode);

            if (!string.IsNullOrEmpty(orderId))
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _business.DeleteOrder(int.Parse(orderId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    this.LoadGrdOrders();
                }
            }
        }

        private void grdOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //Application.Current.Shutdown();
                this.Hide();
            }
        }
    }
}
