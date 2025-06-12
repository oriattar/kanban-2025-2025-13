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
    
    public partial class TaskAdd : Window
    {
        private TaskAddVM tvm;

        /// <summary>
        /// Constructs the task addition window
        /// </summary>
        /// <param name="user"></param>
        /// <param name="board"></param>
        public TaskAdd(UserModel user,BoardModel board)
        {
            InitializeComponent();
            this.tvm = new TaskAddVM(user, board);
            this.DataContext = tvm;
        }

        /// <summary>
        /// Calls for task creation after the button was pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.tvm.CreateTask();

                BoardView boardView = new BoardView(this.tvm.User, this.tvm.Board); // going back to board view after task creation
                boardView.Show();
                this.Close();
            }
            catch (Exception ex) { };
        }

        /// <summary>
        /// Operates the back to board view operation, opens the board view window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToBoard_Click(object sender, RoutedEventArgs e)
        {
            BoardView boardView = new BoardView(this.tvm.User, this.tvm.Board);
            boardView.Show();
            this.Close();
        }
    }
}
