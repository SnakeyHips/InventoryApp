using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ReagentApp.Views
{
    public partial class LogInDialog : MetroWindow
    {
        public LogInDialog()
        {
            InitializeComponent();
        }

        //Here is where it will check is username/password are correct. atm just set as test
        private async void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if(usernameInput.Text.Equals("test"))
            {
                if (passwordInput.Password.ToString().Equals("test"))
                {
                    DialogResult = true;
                }
                else
                {
                    await this.ShowMessageAsync("", "Password incorrect. Please try again.");
                }
            }
            else
            {
                await this.ShowMessageAsync("", "Username incorrect. Please try again.");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
