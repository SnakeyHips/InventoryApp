using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro;

namespace InventoryApp
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabAT.IsSelected)
            {
                ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.Accents.First(x => x.Name == "Crimson"),
                ThemeManager.AppThemes.First(x => x.Name == "BaseLight"));
            }
            else if (tabPDS.IsSelected)
            {
                ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.Accents.First(x => x.Name == "Mauve"),
                ThemeManager.AppThemes.First(x => x.Name == "BaseLight"));
            }
            else if (tabWTAIL.IsSelected)
            {
                ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.Accents.First(x => x.Name == "Olive"),
                ThemeManager.AppThemes.First(x => x.Name == "BaseLight"));
            }
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            //history window here
        }
    }
}
