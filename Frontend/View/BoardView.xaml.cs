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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {

        private BoardViewVM bvm;
        public BoardView(UserModel user,BoardModel board)
        {
            InitializeComponent();

            this.bvm = new BoardViewVM(user,board);
            this.DataContext = bvm;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Dashboard dashboard = new Dashboard(this.bvm.User);
            dashboard.Show();
            this.Close();
        }

        private void TaskCreation_Click(object sender, RoutedEventArgs e)
        {
            TaskAdd taskWindow = new TaskAdd(this.bvm.User,this.bvm.Board);
            taskWindow.Show();
            this.Close();
        }

        private void TaskAdvance_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            TaskModel t = (TaskModel)(b.DataContext);



        }

    }
}
