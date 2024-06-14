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
    /// Interaction logic for wBrand.xaml
    /// </summary>
    public partial class wBrand : Window


    {
        private readonly BrandBusiness _business;

        public wBrand()
        {
            InitializeComponent();
            this._business ??= new BrandBusiness();
            this.LoadGrdCategories();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            int idTmp = -1;
            int.TryParse(txtBrandId.Text, out idTmp);
            try
            {
                var item = await _business.GetBrandByIdAsync(idTmp);

                int status = 0;
                if (chkIsActive.IsChecked == true) { status = 1; }
                else { status = 0; }
                if (item.Data == null)
                {

                    var brand = new Brand()
                    {
                        BrandName = txtBrandName.Text,
                        Description = txtBrandDescription.Text,
                        Status = status,
                        Image = txtBrandImage.Text,
                        Note = txtBrandNote.Text,
                        CreateBy = txtBrandCreateBy.Text,
                        CreateAt = DateTime.Parse(dpBrandCreatedAt.Text),
                        UpdateBy = txtBrandUpdateBy.Text,
                        UpdateAt = DateTime.Parse(dpBrandUpdatedAt.Text)
                    };

                    var result = await _business.CreateBrand(brand);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var brand = item.Data as Brand;
                    brand.BrandName = txtBrandName.Text;
                    brand.Description = txtBrandDescription.Text;
                    brand.Status = status;
                    brand.Image = txtBrandImage.Text;
                    brand.Note = txtBrandNote.Text;
                    brand.CreateBy = txtBrandCreateBy.Text;
                    brand.CreateAt = DateTime.Parse(dpBrandCreatedAt.Text);
                    brand.UpdateBy = txtBrandUpdateBy.Text;
                    brand.UpdateAt = DateTime.Parse(dpBrandUpdatedAt.Text);

                    var result = await _business.UpdateBrand(brand);
                    MessageBox.Show(result.Message, "Update");
                }

                txtBrandId.Text = string.Empty;
                txtBrandName.Text = string.Empty;
                txtBrandDescription.Text = string.Empty;
                txtBrandCreateBy.Text = string.Empty;
                txtBrandUpdateBy.Text = string.Empty;
                txtBrandImage.Text = string.Empty;
                txtBrandNote.Text = string.Empty;
                chkIsActive.IsChecked = false;
                dpBrandCreatedAt.SelectedDate = null;
                dpBrandUpdatedAt.SelectedDate = null;
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

        private async void grdBrand_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string brandId = btn.CommandParameter.ToString();

            //MessageBox.Show(currencyCode);

            if (!string.IsNullOrEmpty(brandId))
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _business.DeleteBrand(int.Parse(brandId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    this.LoadGrdCategories();
                }
            }
        }

        private async void grdBrand_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Brand;
                    if (item != null)
                    {
                        var brandResult = await _business.GetBrandByIdAsync(item.BrandId);

                        if (brandResult.Status > 0 && brandResult.Data != null)
                        {
                            item = brandResult.Data as Brand;
                            txtBrandId.Text = item.BrandId.ToString();
                            txtBrandName.Text = item.BrandName;
                            txtBrandDescription.Text = item.Description;
                            chkIsActive.IsChecked = Convert.ToBoolean(item.Status);
                            txtBrandCreateBy.Text = item.CreateBy;
                            dpBrandCreatedAt.SelectedDate = item.CreateAt;
                            txtBrandUpdateBy.Text = item?.UpdateBy;
                            dpBrandUpdatedAt.SelectedDate = item?.UpdateAt;
                            txtBrandImage.Text = item?.Image;
                            txtBrandNote.Text = item?.Note;
                        }
                    }
                }
            }
        }

        private async void LoadGrdCategories()
        {
            var result = await _business.GetBrandsAsync();

            if (result.Status > 0 && result.Data != null)
            {
                grdBrand.ItemsSource = result.Data as List<Brand>;
            }
            else
            {
                grdBrand.ItemsSource = new List<Brand>();
            }
        }

    }

}

