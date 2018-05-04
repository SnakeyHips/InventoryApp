using System.ComponentModel;

namespace InventoryApp.Model
{
    public class StockModel
    {
    }

    public class Stock : INotifyPropertyChanged
    {
        private string name { get; set; }
        private int min { get; set; }
        private int quantity { get; set; }
        private int stockWarning { get; set; }

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

        public int Min
        {
            get
            {
                return min;
            }
            set
            {
                if (min != value)
                {
                    min = value;
                    RaisePropertyChanged("Min");
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

        public int StockWarning
        {
            get
            {
                return stockWarning;
            }
            set
            {
                if (stockWarning != value)
                {
                    stockWarning = value;
                    RaisePropertyChanged("StockWarning");
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
