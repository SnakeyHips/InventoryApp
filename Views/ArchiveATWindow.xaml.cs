using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using InventoryApp.Models;
using InventoryApp.ViewModels;

namespace InventoryApp.Views
{
    public partial class ArchiveATWindow : MetroWindow
    {
        public ArchiveATWindow()
        {
            InitializeComponent();
            lstArchive.ItemsSource = ATViewModel.Archive;
            CheckDates();
        }

        private async void CheckDates()
        {
            bool pastTwoYears = false;
            foreach(Reagent r in ATViewModel.Archive)
            {
                if (r.DateWarning == 3)
                {
                    pastTwoYears = true;
                }
            }
            if (pastTwoYears)
            {
                await this.ShowMessageAsync("", "Archive contains items past two years expiry. Please delete these items.");
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstArchive.SelectedItem != null)
            {
                MessageDialogResult choice = await this.ShowMessageAsync("",
                    "Deleting these items from the archive will delete them forever. Are you sure?",
                    MessageDialogStyle.AffirmativeAndNegative);
                if (choice == MessageDialogResult.Affirmative)
                {
                    //Deletes foreverrr mwahahahaha
                    Reagent selected = (Reagent)lstArchive.SelectedItem;
                    ATViewModel.Delete(ATViewModel.ATArchiveName, selected);
                    ATViewModel.Archive.Remove(selected);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
