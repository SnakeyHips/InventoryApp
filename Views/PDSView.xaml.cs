using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using InventoryApp.Model;
using InventoryApp.ViewModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace InventoryApp.Views
{
    public partial class PDSView : UserControl
    {
        MetroWindow mainWindow = (Application.Current.MainWindow as MetroWindow);

        public PDSView()
        {
            InitializeComponent();
            this.DataContext = new PDSViewModel();
        }

        private void lstStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstStock.SelectedItem != null)
            {
                PDSViewModel.InventoryStock = new ObservableCollection<Reagent>(
                    PDSViewModel.Inventory.Where(x => x.Name.Equals(PDSViewModel.SelectedStock.Name)).ToList());
                lstInventory.ItemsSource = PDSViewModel.InventoryStock;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPDSWindow addPDSWindow = new AddPDSWindow();
            addPDSWindow.Owner = mainWindow;
            addPDSWindow.ShowDialog();
            if (addPDSWindow.DialogResult == true)
            {
                lstStock.UnselectAll();
                lstStock.SelectedValue = addPDSWindow.cboReagents.Text;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdatePDSWindow updatePDSWindow = new UpdatePDSWindow(PDSViewModel.SelectedInventory);
            updatePDSWindow.Owner = mainWindow;
            updatePDSWindow.ShowDialog();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstInventory.SelectedIndex > -1)
            {
                MessageDialogResult choice = await mainWindow.ShowMessageAsync("",
                            "Are you sure you want to delete this item?",
                            MessageDialogStyle.AffirmativeAndNegative);
                if (choice == MessageDialogResult.Affirmative)
                {
                    CollectionManager.Delete(CollectionManager.PDSInventoryName, PDSViewModel.SelectedInventory);
                    PDSViewModel.Inventory.Remove(PDSViewModel.SelectedInventory);
                    PDSViewModel.InventoryStock.Remove(PDSViewModel.SelectedInventory);
                    PDSViewModel.LoadStock();
                }
            }
        }

        private void btnArchive_Click(object sender, RoutedEventArgs e)
        {
            ArchivePDSWindow archivePDSWindow = new ArchivePDSWindow();
            archivePDSWindow.Owner = mainWindow;
            archivePDSWindow.ShowDialog();
        }

        private async void btnReport_Click(object sender, RoutedEventArgs e)
        {
            //Allow user to choose save location for report
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Choose Report Save Location";

            //Switch to make sure corresponding letters are on correct report based on tab selected
            saveDialog.FileName = "PDS Report";
            saveDialog.Filter = "PDF document (*.pdf)|*.pdf";
            bool? result = saveDialog.ShowDialog();

            if (result == true)
            {
                await CreateReport(saveDialog.FileName);
            }
        }

        private async Task CreateReport(string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);

                //Document is A4 size with margins of 36 each side
                Document report = new Document(PageSize.A4, 36, 36, 36, 36);

                PdfWriter writer = PdfWriter.GetInstance(report, fs);

                //Table for displaying stock quantities with 2 being amount of columns
                PdfPTable stockTable = new PdfPTable(2);
                stockTable.SpacingBefore = 10f;
                stockTable.SpacingAfter = 10f;

                //Used for creating bold font
                Font bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD);

                //Column titles with bold text for stock table
                stockTable.AddCell(new Phrase(new Chunk("Reagent", bold)));
                stockTable.AddCell(new Phrase(new Chunk("Quantity", bold)));
                foreach (Stock s in PDSViewModel.Stocks)
                {
                    stockTable.AddCell(s.Name);
                    stockTable.AddCell(s.Quantity.ToString());
                }

                //Use below comment for changing cell text alignment
                //stockTable.AddCell(new PdfPCell(new Phrase(x.Quantity.ToString())) { HorizontalAlignment = Element.ALIGN_RIGHT });

                //Table for displaying near or out of date stock with 5 being amount of columns
                PdfPTable expiryTable = new PdfPTable(5);
                expiryTable.SpacingBefore = 10f;
                expiryTable.SpacingAfter = 10f;

                //Column titles with bold text for expiry table
                expiryTable.AddCell(new Phrase(new Chunk("Reagent", bold)));
                expiryTable.AddCell(new Phrase(new Chunk("Supplier", bold)));
                expiryTable.AddCell(new Phrase(new Chunk("Batch", bold)));
                expiryTable.AddCell(new Phrase(new Chunk("Expiry", bold)));
                expiryTable.AddCell(new Phrase(new Chunk("Quantity", bold)));

                //Change OrderBy to what you wish. Name is default atm
                PDSViewModel.Inventory.Where(x => x.DateWarning > 0).OrderBy(x => x.Name).ToList().ForEach(x =>
                {
                    expiryTable.AddCell(x.Name);
                    expiryTable.AddCell(x.Supplier);
                    expiryTable.AddCell(x.Batch);
                    expiryTable.AddCell(x.Expiry);
                    expiryTable.AddCell(x.Quantity.ToString());
                });

                //Title used with date and time when created
                Paragraph titleParagraph = new Paragraph(new Phrase(new Chunk("Reagent Inventory Report: " + DateTime.Now, bold)));
                titleParagraph.Alignment = Element.ALIGN_CENTER;

                //Creates and adds everything to pdf output
                report.Open();
                report.Add(titleParagraph);
                report.Add(stockTable);
                report.Add(expiryTable);
                report.Close();

                await mainWindow.ShowMessageAsync("", "Report created successfully!");
            }
            catch
            {
                //Most common issue for report not producing is that previous file is already open
                await mainWindow.ShowMessageAsync("", "Report failed to create! Please make sure a report is not already open.");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }
    }
}
