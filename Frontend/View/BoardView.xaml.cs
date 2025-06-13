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

        /// <summary>
        /// Constructs the board view window,
        /// </summary>
        /// <param name="user"></param>
        /// <param name="board"></param>
        public BoardView(UserModel user,BoardModel board)
        {
            InitializeComponent();

            this.bvm = new BoardViewVM(user,board);
            this.DataContext = bvm;
        }

        /// <summary>
        /// Operates the go back button, to the dashboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Dashboard dashboard = new Dashboard(this.bvm.User);
            dashboard.Show();
            this.Close();
        }

        /// <summary>
        /// Opens the task creation window uppon clicking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskCreation_Click(object sender, RoutedEventArgs e)
        {
            TaskAdd taskWindow = new TaskAdd(this.bvm.User,this.bvm.Board);
            taskWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Advances a task between columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskAdvance_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            TaskModel t = (TaskModel)(b.DataContext); //cast to task model objects, always will be a task model object from the list - safe casting.

            this.bvm.AdvanceTask(t);
        }

        /// <summary>
        /// Opens the details window when clicking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Details_click(object sender, RoutedEventArgs e)
        {
            BoardDetails details = new BoardDetails(this.bvm.User,this.bvm.Board);
            details.Show();
            this.Close();
        }

        /// <summary>
        /// Limits the column selceted in the data context uppon clicking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Limit_Click(object sender, RoutedEventArgs e)
        {
            this.bvm.LimitCol();
        }

        /// <summary>
        /// Opens the task edit window, a button is set for each task in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            TaskModel t = (TaskModel)(b.DataContext); //casting to taskmodel

            TaskEdit edit = new TaskEdit(this.bvm.User,this.bvm.Board,t); //opens the edit window to update the task details
            edit.Show();
            this.Close();
        }
    }
}
