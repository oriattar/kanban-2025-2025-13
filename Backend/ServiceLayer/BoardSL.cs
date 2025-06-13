using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardSL
    {
        public static readonly int BACKLOG = 0;
        public static readonly int IN_PROGRESS = 1;
        public static readonly int DONE = 2;


        private string name;
        private string owner;



        /// <summary>
        /// A consturctor for the boardSL
        /// </summary>
        /// <param name="b"></param>
        public BoardSL(BoardBL b)
        { 
            this.owner = b.Owner;
            this.name = b.Name;
        }

        public BoardSL()
        {
            
        }

        public string Name { get => name; set => name = value; }
        public string Owner { get => owner; set => owner = value; }
    }
}
