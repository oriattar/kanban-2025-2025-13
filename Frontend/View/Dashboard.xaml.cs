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
        
        private DashboardVM dashboardVM; // the window that context

        

        
        public Dashboard(UserModel u)
        {
            InitializeComponent();

            this.dashboardVM = new DashboardVM(u); //building the datacontext with the usermodel
            this.DataContext = this.dashboardVM;

        }

        /// <summary>
        /// operates the board creation button click, using the text box next to it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateBoard_Click(object sender, RoutedEventArgs e)
        {
            this.dashboardVM.CreateBoard();
        }

        /// <summary>
        /// Operates board deletion from the list of boards.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoardDelete(object sender, RoutedEventArgs e)
        {
            
            Button button = (Button)sender;
            BoardModel b = (BoardModel)(button.DataContext); // casting to the correct object from the list

            this.dashboardVM.DeleteBoard(b.BName); //sends for removal
        }

        /// <summary>
        /// A method that operates the logout button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homepage(object sender, RoutedEventArgs e)
        {
            LoginRegister dashboard = new LoginRegister();// going back to the log in menu
            dashboard.Show();
            this.Close();

        }


        private void openBoard_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement clickedElement = sender as FrameworkElement;

            if (clickedElement != null && clickedElement.DataContext is BoardModel b)
            {
                BoardView bWindow = new BoardView(this.dashboardVM.User, b);
                bWindow.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Operates the details button of board from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Details_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.DataContext is BoardModel board)
            {
                BoardDetails details = new BoardDetails(this.dashboardVM.User, board);
                details.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Operates the join board button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void JoinBoard_Click(object sender, RoutedEventArgs e)
        {
            this.dashboardVM.JoinBoard();
        }

        
    }
}
