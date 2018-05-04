using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Linq;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls.Dialogs;
using InventoryApp.Model;
using InventoryApp.ViewModel;

namespace InventoryApp.Views
{
    public partial class UpdateWTAILWindow : MetroWindow
    {
        Reagent Selected;
        int before;
        int after;

        public UpdateWTAILWindow(Reagent r)
        {
            InitializeComponent();
            this.Selected = r;
            txtReagent.Text = Selected.Name;
            txtSupplier.Text = Selected.Supplier;
            txtBatch.Text = Selected.Batch;
            before = Selected.Quantity;
            txtQuantity.Text = Selected.Quantity.ToString();
            cboValidate1.Items.Add("Yes");
            cboValidate1.Items.Add("No");
            cboValidate2.Items.Add("Yes");
            cboValidate2.Items.Add("No");
            if (Selected.Validated1.Equals("No"))
            {
                cboValidate1.SelectedIndex = 1;
            }
            if (Selected.Validated2.Equals("No"))
            {
                cboValidate2.SelectedIndex = 1;
            }
            dateExpiry.Text = Selected.Expiry;
        }

        private async void btnUpdateReagent_Click(object sender, RoutedEventArgs e)
        {
            //Makes sure each input has an input before being made. Possibly better way to do this but this works and is simple to update
            if (cboValidate1.Text == "")
            {
                await this.ShowMessageAsync("", "Please select if Validation 1 performed.");
            }
            else if (cboValidate1.Text == "")
            {
                await this.ShowMessageAsync("", "Please select if Validation 2 performed.");
            }
            else if (dateExpiry.Text == "")
            {
                await this.ShowMessageAsync("", "Please enter an Expiry Date.");
            }
            else if (DateTime.Parse(dateExpiry.Text).CompareTo(DateTime.Now) < 0)
            {
                await this.ShowMessageAsync("", "Please enter a valid Expiry Date.");
            }
            else if (txtQuantity.Text == "")
            {
                await this.ShowMessageAsync("", "Please enter a Quantity.");
            }
            else
            {
                Selected.Validated1 = cboValidate1.Text;
                Selected.Validated2 = cboValidate2.Text;
                Selected.Expiry = dateExpiry.Text;

                //Do quantity stuff
                Selected.Quantity = int.Parse(txtQuantity.Text);
                Selected.DateWarning = WTAILViewModel.CheckExpiryDate(Selected.Expiry);
                //Update with new object using copy method
                CollectionManager.Update(CollectionManager.WTAILInventoryName, Selected);
                after = int.Parse(txtQuantity.Text);
                int difference = before - after;
                WTAILViewModel.UpdateStock(Selected.Name, -difference);
                //Only put into archive if negative change in quantity
                if (before > after)
                {
                    difference = before - after;
                    //Sees if already matching object in archive list
                    try
                    {
                        Reagent temp = WTAILViewModel.Archive.First(
                            x => x.Name == Selected.Name && x.Supplier == Selected.Supplier && x.Batch == Selected.Batch);
                        temp.Quantity += difference;
                        CollectionManager.Update(CollectionManager.WTAILArchiveName, temp);
                    }
                    catch
                    {
                        //If not, create new object in archive
                        Reagent temp = new Reagent()
                        {
                            Name = Selected.Name,
                            Supplier = Selected.Supplier,
                            Batch = Selected.Batch,
                            Validated1 = Selected.Validated1,
                            Validated2 = Selected.Validated2,
                            Expiry = Selected.Expiry,
                            Quantity = difference > 0 ? difference : 0
                        };
                        WTAILViewModel.Archive.Add(temp);
                        CollectionManager.Add(CollectionManager.WTAILArchiveName, temp);
                    }
                }
                this.Close();
            }
        }

        //Method to force only numbers in textbox input
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Use this in the xaml file to only allow numbers in input
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnCancelReagent_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
