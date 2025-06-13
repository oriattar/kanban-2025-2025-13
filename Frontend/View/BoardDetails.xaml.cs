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
        public BoardDetails(UserModel user,BoardModel board)
        {
            InitializeComponent();
            this.bdvm = new BoardDetailsVM(user,board);
            this.DataContext = bdvm;
        }

        private void Back_ToBoardView(object sender, EventArgs e)
        {
            BoardView boardView= new BoardView(this.bdvm.User,this.bdvm.Board);
            boardView.Show();
            this.Close();
        }
        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddUsername_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
