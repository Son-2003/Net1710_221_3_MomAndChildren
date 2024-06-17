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
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            int idTmp = -1;
            int.TryParse(txtPaymentId.Text, out idTmp);
            try
            {
                var item = await _paymentBusiness.GetPaymentByIdAsync(idTmp);

                if (item.Data == null)
                {

                    var Payment = new Payment()
                    {
                        PaymentMethod = txtPaymentMethod.Text,
                        Status = (int)cbbStatus.SelectedValue,
                        PaymentDate = DateTime.Parse(dpPaymentDate.Text),
                        CreateAt = DateTime.Parse(dpPaymentCreatedAt.Text),
                        UpdateAt = DateTime.Parse(dpPaymentUpdatedAt.Text),
                        Note = txtPaymentNote.Text,
                        BillingAddress = txtBillingAddress.Text,
                        Currency = txtCurrency.Text,
                        OrderId = int.Parse(cbbOrderId.Text)
                    };

                    var result = await _paymentBusiness.CreatePayment(Payment);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var Payment = item.Data as Payment;
                    Payment.PaymentMethod = txtPaymentMethod.Text;
                    Payment.Status = (int)cbbStatus.SelectedValue;
                    Payment.PaymentDate = DateTime.Parse(dpPaymentDate.Text);
                    Payment.CreateAt = DateTime.Parse(dpPaymentCreatedAt.Text);
                    Payment.UpdateAt = DateTime.Parse(dpPaymentUpdatedAt.Text);
                    Payment.Note = txtPaymentNote.Text;
                    Payment.BillingAddress = txtBillingAddress.Text;
                    Payment.Currency = txtCurrency.Text;
                    Payment.OrderId = int.Parse(cbbOrderId.Text);

                    var result = await _paymentBusiness.UpdatePayment(Payment);
                    MessageBox.Show(result.Message, "Update");
                }

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
                            cbbStatus.Text = Convert.ToString(item.Status);
                            dpPaymentDate.SelectedDate = item.PaymentDate;
                            dpPaymentCreatedAt.SelectedDate = item.CreateAt;
                            dpPaymentUpdatedAt.SelectedDate = item?.UpdateAt;
                            txtBillingAddress.Text = item?.BillingAddress;
                            txtCurrency.Text = item?.Currency;
                            txtPaymentNote.Text = item?.Note;
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

    }
}
