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
    /// Interaction logic for BoardDetails.xaml
    /// </summary>
    public partial class BoardDetails : Window
    {

        private BoardDetailsVM bdvm;

        /// <summary>
        /// Constructs the board details window
        /// </summary>
        /// <param name="user"></param>
        /// <param name="board"></param>
        public BoardDetails(UserModel user,BoardModel board)
        {
            InitializeComponent();
            this.bdvm = new BoardDetailsVM(user,board);
            this.DataContext = bdvm;
        }

        /// <summary>
        /// Operates the back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_ToBoardView(object sender, EventArgs e)
        {
            BoardView boardView= new BoardView(this.bdvm.User,this.bdvm.Board);
            boardView.Show();
            this.Close();
        }

        /// <summary>
        /// Operates the member adding button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            this.bdvm.AddMember();
        }

        /// <summary>
        /// operates owner change button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeOwner_Click(object sender, RoutedEventArgs e)
        {
            this.bdvm.ChangeOwner();
        }

        /// <summary>
        /// Operates the board leave, if board leaving was success full, leaving the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeaveBoard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.bdvm.LeaveBoard();
                Dashboard dashboard = new Dashboard(this.bdvm.User);
                dashboard.Show();
                this.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
