using System.Windows;
using System.Windows.Controls;
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

        //TabControl selection handler for saving which tab was last selected and app accent
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabsMain.SelectedIndex)
            {
                case 0:
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Crimson"), ThemeManager.GetAppTheme("BaseLight"));
                    break;
                case 1:
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Mauve"), ThemeManager.GetAppTheme("BaseLight"));
                    break;
                case 2:
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Olive"), ThemeManager.GetAppTheme("BaseLight"));
                    break;
                case 3:
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Sienna"), ThemeManager.GetAppTheme("BaseLight"));
                    break;
            }
        }
    }
}
