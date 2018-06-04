using System.Windows;
using MahApps.Metro.Controls;
using InventoryApp.Models;

namespace InventoryApp.Views
{
    public partial class HistoryDialog : MetroWindow
    {
        public HistoryDialog()
        {
            InitializeComponent();
            //for (int i = ListManager.AuditHistory.Count -1; i >= 0; i--)
            //{
            //    lstHistory.Items.Add(ListManager.AuditHistory[i]);
            //}
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
