using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class BoardModel
    {
        private string owner;
        private string bName;
        private int id;

        /// <summary>
        /// Constructs a board model from a boardSL
        /// </summary>
        /// <param name="b"></param>
        public BoardModel(BoardSL b) {

            this.owner = b.owner;
            this.bName = b.name;
            this.id = b.id;
        }

        public string BName { get => bName; set => bName = value; }
        public string Owner { get => owner; set => owner = value; }
        public int Id { get => id; set => id = value; }
    }
}
