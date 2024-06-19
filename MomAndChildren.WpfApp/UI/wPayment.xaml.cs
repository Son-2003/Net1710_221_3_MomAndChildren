using Microsoft.IdentityModel.Tokens;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MomAndChildren.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wPayment.xaml
    /// </summary>
    public partial class wPayment : Window
    {
        private readonly IPaymentBusiness _paymentBusiness;
        private readonly IOrderBusiness _orderBusiness;

        public wPayment()
        {
            InitializeComponent();
            this._paymentBusiness ??= new PaymentBusiness();
            this._orderBusiness ??= new OrderBusiness();
            this.LoadGrdPayments();
            this.LoadOrders();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            int idTmp = -1;
            if(!txtPaymentId.Text.IsNullOrEmpty()) int.TryParse(txtPaymentId.Text, out idTmp);
            CheckDateNull(dpPaymentDate.Text, dpPaymentCreatedAt.Text, dpPaymentUpdatedAt.Text);
            try
            {
                if (idTmp >= 0)
                {
                    var item = await _paymentBusiness.GetPaymentByIdAsync(idTmp);
                    if (item.Data == null)
                    {
                        // Create new payment if not found
                        var payment = new Payment()
                        {
                            PaymentMethod = txtPaymentMethod.Text,
                            Status = cbbStatus.Text.Equals("IsPaid") ? 1 : 0,
                            PaymentDate = DateTime.Parse(dpPaymentDate.Text),
                            CreateAt = DateTime.Parse(dpPaymentCreatedAt.Text),
                            UpdateAt = DateTime.Parse(dpPaymentUpdatedAt.Text),
                            Note = txtPaymentNote.Text,
                            BillingAddress = txtBillingAddress.Text,
                            Currency = txtCurrency.Text,
                            OrderId = int.Parse(cbbOrderId.Text)
                        };

                        var result = await _paymentBusiness.CreatePayment(payment);
                        MessageBox.Show(result.Message, "Save");
                    }
                    else
                    {
                        // Update existing payment if found
                        var payment = item.Data as Payment;
                        payment.PaymentMethod = txtPaymentMethod.Text;
                        payment.Status = cbbStatus.Text.Equals("IsPaid") ? 1 : 0;
                        payment.PaymentDate = DateTime.Parse(dpPaymentDate.Text);
                        payment.CreateAt = DateTime.Parse(dpPaymentCreatedAt.Text);
                        payment.UpdateAt = DateTime.Parse(dpPaymentUpdatedAt.Text);
                        payment.Note = txtPaymentNote.Text;
                        payment.BillingAddress = txtBillingAddress.Text;
                        payment.Currency = txtCurrency.Text;
                        payment.OrderId = int.Parse(cbbOrderId.Text);

                        var result = await _paymentBusiness.UpdatePayment(payment);
                        MessageBox.Show(result.Message, "Update");
                    }
                }
                else
                {
                    // Create new payment directly if idTmp is -1
                    var payment = new Payment()
                    {
                        PaymentMethod = txtPaymentMethod.Text,
                        Status = cbbStatus.Text.Equals("IsPaid") ? 1 : 0,
                        PaymentDate = DateTime.Parse(dpPaymentDate.Text),
                        CreateAt = DateTime.Parse(dpPaymentCreatedAt.Text),
                        UpdateAt = DateTime.Parse(dpPaymentUpdatedAt.Text),
                        Note = txtPaymentNote.Text,
                        BillingAddress = txtBillingAddress.Text,
                        Currency = txtCurrency.Text,
                        OrderId = int.Parse(cbbOrderId.Text)
                    };

                    var result = await _paymentBusiness.CreatePayment(payment);
                    MessageBox.Show(result.Message, "Save");
                }

                // Clear form fields
                txtPaymentId.Text = string.Empty;
                txtPaymentMethod.Text = string.Empty;
                cbbStatus.Text = string.Empty;
                dpPaymentDate.Text = string.Empty;
                dpPaymentCreatedAt.SelectedDate = null;
                dpPaymentUpdatedAt.SelectedDate = null;
                txtBillingAddress.Text = string.Empty;
                txtPaymentNote.Text = string.Empty;
                txtCurrency.Text = string.Empty;
                cbbOrderId.Text = string.Empty;

                // Reload data grid
                this.LoadGrdPayments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }



        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Hide();
            }

        }

        private async void grdPayment_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string PaymentId = btn.CommandParameter.ToString();

            //MessageBox.Show(currencyCode);

            if (!string.IsNullOrEmpty(PaymentId))
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _paymentBusiness.RemovePayment(int.Parse(PaymentId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    this.LoadGrdPayments();
                }
            }
        }

        private async void grdPayment_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Payment;
                    if (item != null)
                    {
                        var PaymentResult = await _paymentBusiness.GetPaymentByIdAsync(item.PaymentId);

                        if (PaymentResult.Status > 0 && PaymentResult.Data != null)
                        {
                            item = PaymentResult.Data as Payment;
                            txtPaymentId.Text = item.PaymentId.ToString();
                            txtPaymentMethod.Text = item.PaymentMethod;
                            foreach (ComboBoxItem cbbItem in cbbStatus.Items)
                            {
                                if ((cbbItem.Content.Equals("IsPaid") ? 1 : 0) == item.Status)
                                {
                                    cbbStatus.SelectedItem = cbbItem;
                                    break;
                                }
                            }
                            dpPaymentDate.SelectedDate = item.PaymentDate;
                            dpPaymentCreatedAt.SelectedDate = item.CreateAt;
                            dpPaymentUpdatedAt.SelectedDate = item?.UpdateAt;
                            txtBillingAddress.Text = item?.BillingAddress;
                            txtCurrency.Text = item?.Currency;
                            txtPaymentNote.Text = item?.Note;
                            cbbOrderId.Text = item.OrderId.ToString();
                        }
                    }
                }
            }
        }

        private async void LoadGrdPayments()
        {
            var result = await _paymentBusiness.GetPaymentList();

            if (result.Status > 0 && result.Data != null)
            {
                grdPayment.ItemsSource = result.Data as List<Payment>;
            }
            else
            {
                grdPayment.ItemsSource = new List<Payment>();
            }
        }

        private async void LoadOrders()
        {
            var orders = (List<Order>)(await _orderBusiness.GetOrdersAsync()).Data;

            cbbOrderId.ItemsSource = orders;
            cbbOrderId.IsEnabled = true;
        }

        public void CheckDateNull(string paymentDate, string createdAt, string updatedAt)
        {
            if (paymentDate.IsNullOrEmpty())
            {
                MessageBox.Show("paymentDate is required.", "Date Check", MessageBoxButton.OK, MessageBoxImage.Warning);
            }else if (createdAt.IsNullOrEmpty())
            {
                MessageBox.Show("createdDate is required.", "Date Check", MessageBoxButton.OK, MessageBoxImage.Warning);
            }else if (updatedAt.IsNullOrEmpty())
            {
                MessageBox.Show("updatedDate is required.", "Date Check", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
