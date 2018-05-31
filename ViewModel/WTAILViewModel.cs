using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;
using InventoryApp.Model;
using Dapper;

namespace InventoryApp.ViewModel
{
    public class WTAILViewModel
    {
        public static string connString = ConfigurationManager.ConnectionStrings["InventoryDBConnectionString"].ConnectionString;
        public static string WTAILInventoryName = "WTAILInventory";
        public static string WTAILArchiveName = "WTAILArchive";
        public static ObservableCollection<Reagent> Inventory { get; set; }
        public static ObservableCollection<Reagent> InventoryStock { get; set; }
        public static ObservableCollection<Reagent> Archive { get; set; }
        public static ObservableCollection<Stock> Stocks { get; set; }
        public static Reagent SelectedInventory { get; set; }
        public static Stock SelectedStock { get; set; }

        public WTAILViewModel()
        {
            Inventory = new ObservableCollection<Reagent>();
            InventoryStock = new ObservableCollection<Reagent>();
            Archive = new ObservableCollection<Reagent>();
            Stocks = new ObservableCollection<Stock>();
            LoadInventory();
            LoadArchive();
            LoadStock();
        }

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
            Inventory = Get(WTAILInventoryName);
            foreach (Reagent r in Inventory)
            {
                r.DateWarning = CheckExpiryDate(r.Expiry);
            }
        }

        public static void LoadArchive()
        {
            Archive = Get(WTAILArchiveName);
            foreach (Reagent r in Archive)
            {
                r.DateWarning = CheckExpiryDate(r.Expiry);
            }
        }

        public static void LoadInventoryStock()
        {
            InventoryStock.Clear();
            foreach (Reagent r in Inventory)
            {
                if (r.Name.Equals(SelectedStock.Name))
                {
                    InventoryStock.Add(r);
                }
            }
        }

        public static void LoadStock()
        {
            Stocks.Clear();
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

        //Retrieves List of Reagents from relevant table
        public static ObservableCollection<Reagent> Get(string table)
        {
            string query = "SELECT * FROM " + table;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    return new ObservableCollection<Reagent>(conn.Query<Reagent>(query).ToList());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return new ObservableCollection<Reagent>();
                }

            }
        }

        //Adds reagent item to table
        public static void Add(string table, Reagent r)
        {
            string query = "INSERT INTO " + table + " (Name, Supplier, Batch, Validated1, Validated2, Expiry, Quantity) " +
                "VALUES (@Name, @Supplier, @Batch, @Validated1, @Validated2, @Expiry, @Quantity);";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    conn.Execute(query, r);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        //Updates reagent item in table
        public static void Update(string table, Reagent r)
        {
            string query = "UPDATE " + table +
                " SET Name=@Name, Supplier=@Supplier, Batch=@Batch, Validated1=@Validated1, Validated2=@Validated2, " +
                "Expiry=@Expiry, Quantity=@Quantity WHERE Name=@Name AND Supplier=@Supplier AND Batch=@Batch;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    conn.Execute(query, r);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        //Deletes reagent item from table
        public static void Delete(string table, Reagent r)
        {
            string query = "DELETE FROM " + table + " WHERE Name=@Name AND Supplier=@Supplier AND Batch=@Batch;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    conn.Execute(query, r);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
