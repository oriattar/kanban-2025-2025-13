using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;


namespace IntroSE.Kanban.Backend.ServiceLayer

{
    public class BoardListSL
    {
        private Dictionary<string, BoardSL> boards;
        public BoardListSL(BoardListBL b)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, BoardSL> Boards { get => boards; set => boards = value; }
    }
}
