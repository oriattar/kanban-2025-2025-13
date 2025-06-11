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
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for LoginRegister.xaml
    /// </summary>
    public partial class LoginRegister : Window
    {

        private LoginVM loginVM;

        public LoginRegister()
        {
            InitializeComponent();
            this.loginVM = new LoginVM();
            this.DataContext = loginVM;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel? ret = loginVM.Login();
            if (ret != null)
            {
                Dashboard dashboard = new Dashboard(ret);
                dashboard.Show();
                this.Close();
            }


        }


        private void Register_Click(object sender, RoutedEventArgs e)
        {
            UserModel? ret = loginVM.Register();
    
            if (ret != null)
            {
                Dashboard dashboard = new Dashboard(ret);
                dashboard.Show();
                this.Close();
            }
        }

        }
}
