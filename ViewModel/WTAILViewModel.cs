using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InventoryApp.Model;

namespace InventoryApp.ViewModel
{
    public class WTAILViewModel
    {
        public WTAILViewModel()
        {
            LoadInventory();
            LoadArchive();
            LoadStock();
        }

        public static ObservableCollection<Reagent> Inventory { get; set; }
        public static ObservableCollection<Reagent> InventoryStock { get; set; }
        public static ObservableCollection<Reagent> Archive { get; set; }
        public static ObservableCollection<Stock> Stocks { get; set; }

        public static Dictionary<string, int> Reagents = new Dictionary<string, int> {
            { "C", 1 },
            { "Cw", 1 },
            { "c", 1 },
            { "D", 10 },
            { "E", 1 },
            { "e", 1 },
            { "C+D+E", 10 },
            { "K", 1 },
            { "k", 2 },
            { "Jk-a", 3 },
            { "Jk-b", 10 },
            { "M", 3 },
            { "N", 2 },
            { "Lu-a", 2 },
            { "Lu-b", 2 },
            { "Fy-b", 4 },
            { "s", 1 },
            { "S", 5 },
            { "Kpa", 1 },
            { "2 Cell screen antibody", 15 },
            { "Bromelain", 1 },
            { "Dextran", 1 },
            { "High titre control", 1 },
            { "EXTRAN", 2 },
            { "PK Cleaning Solution", 3 },
            { "Syphilis Kits", 5 },
            { "QC2 Syphilis Control", 5 },
        };

        public static void LoadInventory()
        {
            Inventory = CollectionManager.Get(CollectionManager.WTAILInventoryName);
            InventoryStock = new ObservableCollection<Reagent>();
            foreach (Reagent r in Inventory)
            {
                r.DateWarning = CheckExpiryDate(r.Expiry);
            }
        }

        public static void LoadArchive()
        {
            Archive = CollectionManager.Get(CollectionManager.WTAILArchiveName);
            foreach (Reagent r in Archive)
            {
                r.DateWarning = CheckExpiryDate(r.Expiry);
            }
        }

        public static void LoadStock()
        {
            Stocks = new ObservableCollection<Stock>();
            foreach (KeyValuePair<string, int> kvp in Reagents)
            {
                Stock temp = new Stock()
                {
                    Name = kvp.Key,
                    Min = kvp.Value,
                    Quantity = 0,
                };
                foreach (Reagent r in Inventory)
                {
                    if (r.Name.Equals(temp.Name))
                    {
                        temp.Quantity += r.Quantity;
                    }
                }
                temp.StockWarning = CheckStock(temp.Quantity, temp.Min);
                Stocks.Add(temp);
            }
        }

        public static int CheckStock(int quantity, int min)
        {
            if (quantity == 0)
            {
                //Out of stock
                return 2;
            }
            else if (quantity < min)
            {
                //Stock warning
                return 1;
            }
            else
            {
                //Stock okay
                return 0;
            }
        }

        public static void UpdateStock(string name, int difference)
        {
            foreach (Stock s in Stocks)
            {
                if (s.Name.Equals(name))
                {
                    s.Quantity += difference;
                    s.StockWarning = CheckStock(s.Quantity, s.Min);
                }
            }
        }

        public static int CheckExpiryDate(string expiryDate)
        {
            DateTime reagentDate = DateTime.Parse(expiryDate);
            if (reagentDate.CompareTo(DateTime.Now.Date.AddYears(-2)) < 0)
            {
                //Past two year expiry
                return 3;
            }
            else if (reagentDate.CompareTo(DateTime.Now.Date) < 0)
            {
                //Out of date
                return 2;
            }
            else if (reagentDate.CompareTo(DateTime.Now.Date.AddMonths(1)) <= 0)
            {
                //Within warning date
                return 1;
            }
            else
            {
                //Date okay
                return 0;
            }
        }

        private static Reagent _selectedInventory;

        public static Reagent SelectedInventory
        {
            get
            {
                return _selectedInventory;
            }

            set
            {
                _selectedInventory = value;
            }
        }

        private static Stock _selectedStock;

        public static Stock SelectedStock
        {
            get
            {
                return _selectedStock;
            }

            set
            {
                _selectedStock = value;
            }
        }
    }
}
