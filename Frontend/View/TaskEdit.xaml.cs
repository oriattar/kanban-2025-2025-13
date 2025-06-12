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
    /// Interaction logic for TaskEdit.xaml
    /// </summary>
    public partial class TaskEdit : Window
    {

        private TaskEditVM tevm;

        /// <summary>
        /// Constructs the window.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="board"></param>
        /// <param name="task"></param>
        public TaskEdit(UserModel user,BoardModel board,TaskModel task)
        {
            InitializeComponent();
            this.tevm = new TaskEditVM(user, board, task);
            this.DataContext = tevm;
        }

        /*
         * 
         * Following method calling for update methods for each task property, when the corresponding button is pressed.
         * 
         */
        private void Title_Click(object sender,RoutedEventArgs e)
        {
            this.tevm.Edit_Title();
        }

        private void Description_Click(object sender, RoutedEventArgs e)
        {
            this.tevm.Edit_Description();
        }

        private void Assignee_Click(object sender, RoutedEventArgs e)
        {
            this.tevm.Edit_Assignee();
        }

        private void DueDate_Click(object sender, RoutedEventArgs e)
        {
            this.tevm.Edit_DueDate();
        }

        /// <summary>
        /// Operates the go back operation, showing the board view again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BoardView view = new BoardView(tevm.User,tevm.Board);
            view.Show();
            this.Close();
        }
    }
}
