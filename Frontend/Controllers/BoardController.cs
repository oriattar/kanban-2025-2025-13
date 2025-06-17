using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ControlzEx.Standard;
using Frontend.Model;
using IntroSE.Kanban.Backend.Buiesness_layer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Controllers
{
    public class BoardController
    {
        private readonly BoardServices bs;
        public BoardController(BoardServices bs) { 
        
            this.bs = bs;
        }

        /// <summary>
        /// Creating a board in the backend, calling the correct service.
        /// </summary>
        /// <param name="boardName"></param>
        /// <param name="email"></param>
        /// <exception cref="Exception"></exception>
        public void CreateBoard(string boardName, string email)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.CreateBoard(email, boardName)); //using create board service
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

        }

        /// <summary>
        /// Locating board in the backend
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <returns></returns>
        public BoardModel GetBoard(string email,string bName)
        {
            Response<BoardSL> res = JsonSerializer.Deserialize<Response<BoardSL>>(this.bs.GetBoard(email, bName));
            if (res.ErrorMessage != null)
                throw new Exception(res.ErrorMessage);

            return new BoardModel(res.ReturnValue);
        }

        /// <summary>
        /// Deleting the board at backend, using the service.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <exception cref="Exception"></exception>
        public void DeleteBoard(string email, string bName)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.DeleteBoard(email, bName)); //calls the board deletion service
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

        }

        /// <summary>
        /// A method that constructs an observable collection from the user boards, getting the list of boardsSL from backend.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ObservableCollection<BoardModel> GetBoards(string email)
        {
            Response<List<BoardSL>> res =JsonSerializer.Deserialize<Response<List<BoardSL>>>(this.bs.GetBoards(email)); //calling backend to get the list.
            if(res.ErrorMessage!= null)
                throw new Exception(res.ErrorMessage);

            return new ObservableCollection<BoardModel>(res.ReturnValue.Select(b => new BoardModel(b))); //constructs and OC list from boardmodel list.

        }

        /// <summary>
        /// Loads the tasks of the input column from backend, using the service.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ObservableCollection<TaskModel> GetTasks(string email,string boardName,int col)
        {
            Response<List<TaskSL>> res = JsonSerializer.Deserialize<Response<List<TaskSL>>>(this.bs.GetColTasks(email,boardName,col));
            if(res.ErrorMessage != null)
                throw new Exception(res.ErrorMessage);

            return new ObservableCollection<TaskModel>(res.ReturnValue.Select(t=>new TaskModel(t)));
        }

        /// <summary>
        /// Calls for the limit set in the backend
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="lim"></param>
        /// <param name="col"></param>
        /// <exception cref="Exception"></exception>
        public void Limit_Col(string email,string boardName, int lim,int col)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.SetLimit(email,boardName,lim,col)); //calls the board deletion service
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// A method that returns a column limit from backend
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetLimit(string email,string boardName,int col)
        {
            Response<int> response = JsonSerializer.Deserialize<Response<int>>(this.bs.GetColLimit(email,boardName,col)); //calls the board deletion service
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

            return response.ReturnValue;
        }

        /// <summary>
        /// Returns the active users from a board from the backend.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ObservableCollection<UserModel> Get_ActiveMemebers(string email,string boardName)
        {
            Response<List<string>> res = JsonSerializer.Deserialize<Response<List<string>>>(this.bs.GetActiveMembers(email,boardName));
            if (res.ErrorMessage != null)
                throw new Exception(res.ErrorMessage);

            return new ObservableCollection<UserModel>(res.ReturnValue.Select(user=>new UserModel(user)));
        }

        /// <summary>
        /// Calls for the join user service
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <param name="boardID"></param>
        /// <exception cref="Exception"></exception>
        public void JoinUser(string inputEmail, int boardID)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.OfflineJoinBoard(inputEmail, boardID));
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// Method responsible removing a user from the board, using leave board service
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardID"></param>
        /// <exception cref="Exception"></exception>
        public void Leave_Board(string email,int boardID)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.LeaveBoard(email, boardID));
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// Operates owner change using the service
        /// </summary>
        /// <param name="currentOwner"></param>
        /// <param name="newOwner"></param>
        /// <param name="boardName"></param>
        /// <exception cref="Exception"></exception>
        public void Change_Owner(string currentOwner,string newOwner,string boardName)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.ChangeOwner(currentOwner,newOwner,boardName));
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// Allows online joining to a board
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <param name="boardID"></param>
        /// <exception cref="Exception"></exception>
        public void JoinUserOnline(string inputEmail, int boardID)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.JoinBoard(inputEmail, boardID));
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// Method uses the get name by id of a board service
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetBoardName(int bId)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.GetBoardName(bId));
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

            return response.ReturnValue;
        }
    }
}
