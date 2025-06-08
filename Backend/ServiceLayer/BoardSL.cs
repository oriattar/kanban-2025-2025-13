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


        public string name;
        public string owner;
        
        public int id;
        
        /// <summary>
        /// A consturctor for the boardSL
        /// </summary>
        /// <param name="b"></param>
        public BoardSL(BoardBL b)
        { 
            this.owner = b.Owner;
            this.name = b.Name;
            this.id = b.BoardID;
        }   
    }
}
