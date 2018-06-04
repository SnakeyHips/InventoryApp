using System.ComponentModel;

namespace InventoryApp.Models
{
    public class AuditModel
    {
    }

    public class Audit : INotifyPropertyChanged
    {
        private string query { get; set; }
        private string date { get; set; }

        public string Query
        {
            get
            {
                return query;
            }
            set
            {
                if (query != value)
                {
                    query = value;
                    RaisePropertyChanged("Query");
                }
            }
        }

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                if (date != value)
                {
                    date = value;
                    RaisePropertyChanged("Date");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
