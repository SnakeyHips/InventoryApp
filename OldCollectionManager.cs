using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;
using System.IO;
using InventoryApp.Model;
using Dapper;

namespace InventoryApp
{
    public class CollectionManager
    {
        public CollectionManager()
        {
        }

        public static string connString = ConfigurationManager.ConnectionStrings["InventoryDBConnectionString"].ConnectionString;

        //String values used for relevant list
        public static string ATInventoryName = "ATInventory";
        public static string ATArchiveName = "ATArchive";
        public static string PDSInventoryName = "PDSInventory";
        public static string PDSArchiveName = "PDSArchive";
        public static string WTAILInventoryName = "WTAILInventory";
        public static string WTAILArchiveName = "WTAILArchive";

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

        //Retrieves List of Audits from Audit table
        public static List<Audit> GetHistory()
        {
            List<Audit> list = new List<Audit>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM AuditHistory;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Audit temp = new Audit
                                {
                                    Query = reader["Query"].ToString(),
                                    Date = reader["Date"].ToString()
                                };
                                list.Add(temp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
            return list;
        }

        public static void AddAudit(SqlCommand command)
        {
            string commandString = command.CommandText;
            string dateTimeString = DateTime.Now.ToString();
            foreach (SqlParameter p in command.Parameters)
            {
                commandString = commandString.Replace(p.ParameterName, p.Value.ToString());
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO AuditHistory (Query, Date) " +
                        "VALUES (@Query, @Date);";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Query", commandString);
                    cmd.Parameters.AddWithValue("@Date", dateTimeString);
                    cmd.ExecuteNonQuery();
                    //AuditHistory.Add(new Audit() { Query = commandString, Date = dateTimeString});
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        //Methods for creating backup folder and backup of database - not implemented atm
        public static void CreateBackUp()
        {
            if (!Directory.Exists("Backups\\"))
            {
                Directory.CreateDirectory("Backups\\");
                BackUp();
            }
            else
            {
                BackUp();
            }
        }

        public static void BackUp()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "BACKUP DATABASE [" + conn.Database.ToString() + "] TO DISK='" +
                        AppDomain.CurrentDomain.BaseDirectory + "Backups\\Backup_" + DateTime.Now.ToString("yyyy-mm-dd") + ".bak';";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
