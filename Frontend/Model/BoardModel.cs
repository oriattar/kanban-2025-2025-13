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
        

        /// <summary>
        /// Constructs a board model from a boardSL
        /// </summary>
        /// <param name="b"></param>
        public BoardModel(BoardSL b) {

            this.owner = b.Owner;
            this.bName = b.Name;
        }

        public BoardModel(string name,string owner)
        {
            this.owner = owner;
            this.bName = name;
        }
        public string BName { get => bName; set => bName = value; }
        public string Owner { get => owner; set => owner = value; }
    }
}
