using System.ComponentModel;

namespace InventoryApp.Model
{
    public class ReagentModel
    {
    }

    public class Reagent : INotifyPropertyChanged
    {
        private string name { get; set; }
        private string supplier { get; set; }
        private string batch { get; set; }
        private string validated1 { get; set; }
        private string validated2 { get; set; }
        private string expiry { get; set; }
        private int quantity { get; set; }
        private int dateWarning { get; set; }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string Supplier
        {
            get
            {
                return supplier;
            }
            set
            {
                if (supplier != value)
                {
                    supplier = value;
                    RaisePropertyChanged("Supplier");
                }
            }
        }

        public string Batch
        {
            get
            {
                return batch;
            }
            set
            {
                if (batch != value)
                {
                    batch = value;
                    RaisePropertyChanged("Batch");
                }
            }
        }

        public string Validated1
        {
            get
            {
                return validated1;
            }
            set
            {
                if (validated1 != value)
                {
                    validated1 = value;
                    RaisePropertyChanged("Validated1");
                }
            }
        }

        public string Validated2
        {
            get
            {
                return validated2;
            }
            set
            {
                if (validated2 != value)
                {
                    validated2 = value;
                    RaisePropertyChanged("Validated2");
                }
            }
        }

        public string Expiry
        {
            get
            {
                return expiry;
            }
            set
            {
                if (expiry != value)
                {
                    expiry = value;
                    RaisePropertyChanged("Expiry");
                }
            }
        }

        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    RaisePropertyChanged("Quantity");
                }
            }
        }

        public int DateWarning
        {
            get
            {
                return dateWarning;
            }
            set
            {
                if (dateWarning != value)
                {
                    dateWarning = value;
                    RaisePropertyChanged("DateWarning");
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
