using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        public void CreateBoard(string boardName, string email)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.CreateBoard(email, boardName));
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

        }
        public void DeleteBoard(string email, string bName)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.bs.DeleteBoard(email, bName));
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

        }

        public ObservableCollection<BoardModel> GetBoards(string email)
        {
            Response<List<BoardSL>> res =JsonSerializer.Deserialize<Response<List<BoardSL>>>(this.bs.GetBoards(email));
            if(res.ErrorMessage!= null)
                throw new Exception(res.ErrorMessage);

            return new ObservableCollection<BoardModel>(res.ReturnValue.Select(b => new BoardModel(b)));

        }
    }
}
