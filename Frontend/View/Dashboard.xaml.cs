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
using ControlzEx.Standard;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private UserModel user;
        private DashboardVM dashboardVM;
        

        
        public Dashboard(UserModel u)
        {
            InitializeComponent();
            this.user = u;
            this.dashboardVM = new DashboardVM(u);
            this.DataContext = this.dashboardVM;

        }
        private void CreateBoard_Click(object sender, RoutedEventArgs e)
        {
            this.dashboardVM.CreateBoard();
        }
        private void BoardDelete(object sender, RoutedEventArgs e)
        {
            
            Button button = (Button)sender;
            BoardModel b = (BoardModel)(button.DataContext);

            this.dashboardVM.DeleteBoard(b.BName);
        }

        private void homepage(object sender, RoutedEventArgs e)
        {
            LoginRegister dashboard = new LoginRegister();
            dashboard.Show();
            this.Close();

        }

        
    }
}
