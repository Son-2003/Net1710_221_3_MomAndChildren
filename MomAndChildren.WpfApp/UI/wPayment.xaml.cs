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
            this.LoadGrdCategories();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            int idTmp = -1;
            int.TryParse(txtPaymentId.Text, out idTmp);
            try
            {
                var item = await _paymentBusiness.GetPaymentByIdAsync(idTmp);

                int status = 0;
                if (chkIsActive.IsChecked == true) { status = 1; }
                else { status = 0; }
                if (item.Data == null)
                {

                    var Payment = new Payment()
                    {
                        PaymentName = txtPaymentName.Text,
                        Description = txtPaymentDescription.Text,
                        Status = status,
                        Image = txtPaymentImage.Text,
                        Note = txtPaymentNote.Text,
                        CreateBy = txtPaymentCreateBy.Text,
                        CreateAt = DateTime.Parse(dpPaymentCreatedAt.Text),
                        UpdateBy = txtPaymentUpdateBy.Text,
                        UpdateAt = DateTime.Parse(dpPaymentUpdatedAt.Text)
                    };

                    var result = await _business.CreatePayment(Payment);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var Payment = item.Data as Payment;
                    Payment.PaymentName = txtPaymentName.Text;
                    Payment.Description = txtPaymentDescription.Text;
                    Payment.Status = status;
                    Payment.Image = txtPaymentImage.Text;
                    Payment.Note = txtPaymentNote.Text;
                    Payment.CreateBy = txtPaymentCreateBy.Text;
                    Payment.CreateAt = DateTime.Parse(dpPaymentCreatedAt.Text);
                    Payment.UpdateBy = txtPaymentUpdateBy.Text;
                    Payment.UpdateAt = DateTime.Parse(dpPaymentUpdatedAt.Text);

                    var result = await _business.UpdatePayment(Payment);
                    MessageBox.Show(result.Message, "Update");
                }

                txtPaymentId.Text = string.Empty;
                txtPaymentName.Text = string.Empty;
                txtPaymentDescription.Text = string.Empty;
                txtPaymentCreateBy.Text = string.Empty;
                txtPaymentUpdateBy.Text = string.Empty;
                txtPaymentImage.Text = string.Empty;
                txtPaymentNote.Text = string.Empty;
                chkIsActive.IsChecked = false;
                dpPaymentCreatedAt.SelectedDate = null;
                dpPaymentUpdatedAt.SelectedDate = null;
                this.LoadGrdCategories();
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
                    this.LoadGrdCategories();
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
                            txtPaymentName.Text = item.PaymentName;
                            txtPaymentDescription.Text = item.Description;
                            chkIsActive.IsChecked = Convert.ToBoolean(item.Status);
                            txtPaymentCreateBy.Text = item.CreateBy;
                            dpPaymentCreatedAt.SelectedDate = item.CreateAt;
                            txtPaymentUpdateBy.Text = item?.UpdateBy;
                            dpPaymentUpdatedAt.SelectedDate = item?.UpdateAt;
                            txtPaymentImage.Text = item?.Image;
                            txtPaymentNote.Text = item?.Note;
                        }
                    }
                }
            }
        }

        private async void LoadGrdCategories()
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
