using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls.Dialogs;
using InventoryApp.Model;
using InventoryApp.ViewModel;
using System.Windows.Controls;

namespace InventoryApp.Views
{
    public partial class AddATWindow : MetroWindow
    {
        MetroWindow mainWindow = (Application.Current.MainWindow as MetroWindow);

        public AddATWindow()
        {
            InitializeComponent();
            cboReagents.ItemsSource = ATViewModel.Reagents;
            cboValidate1.Items.Add("Yes");
            cboValidate1.Items.Add("No");
            cboValidate2.Items.Add("Yes");
            cboValidate2.Items.Add("No");
        }

        private async void btnAddReagent_Click(object sender, RoutedEventArgs e)
        {
            //Makes sure each input has an input before being made. Possibly better way to do this but this works and is simple to update
            if (cboReagents.Text == "")
            {
                await this.ShowMessageAsync("", "Please select a Reagent.");
            }
            else if (txtSupplier.Text == "")
            {
                await this.ShowMessageAsync("", "Please enter a Supplier.");
            }
            else if (txtBatch.Text == "")
            {
                await this.ShowMessageAsync("", "Please enter a Batch Number.");
            }
            else if (cboValidate1.Text == "")
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
                Reagent temp = new Reagent()
                {
                    Name = cboReagents.Text,
                    Supplier = txtSupplier.Text,
                    Batch = txtBatch.Text,
                    Validated1 = cboValidate1.Text,
                    Validated2 = cboValidate2.Text,
                    Expiry = dateExpiry.Text,
                    Quantity = int.Parse(txtQuantity.Text),
                    DateWarning = ATViewModel.CheckExpiryDate(dateExpiry.Text)
                };
                CollectionManager.Add(CollectionManager.ATInventoryName, temp);
                ATViewModel.Inventory.Add(temp);             
                ATViewModel.UpdateStock(temp.Name, temp.Quantity);
                this.DialogResult = true;
            }
        }

        //Method which forces only numbers in textbox input
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Use this in the xaml file to only allow numbers in input like for quantity
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void btnCancelReagent_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /*example scan method if scanner feature implemented
        private void btnScanReagent_Click(object sender, RoutedEventArgs e)
        {
            string scanString;

            ScanDialog scanDialog = new ScanDialog();
            scanDialog.Owner = this;
            scanDialog.ShowDialog();

            if(scanDialog.DialogResult == true)
            {
                if (scanDialog.txtScanInput.Text != "")
                {
                    scanString = scanDialog.txtScanInput.Text;
                    //do stuff here. below is just an example
                    txtSupplier.Text = scanString;
                }
            }
        }*/
    }
}
