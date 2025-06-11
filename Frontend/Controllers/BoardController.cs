using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Frontend.Model;
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
    }
}
