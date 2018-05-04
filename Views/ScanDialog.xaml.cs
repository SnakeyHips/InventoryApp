using MahApps.Metro.Controls;

namespace InventoryApp.Views
{

    public partial class ScanDialog : MetroWindow
    {

        public ScanDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
