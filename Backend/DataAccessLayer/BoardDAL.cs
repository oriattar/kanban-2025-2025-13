using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardDAL
    {
        public static readonly int BACKLOG = 0;
        public static readonly int IN_PROGRESS = 1;
        public static readonly int DONE = 2;

        public const string BoardIDColName = "idBoard";
        public const string OwnerColName = "owner";
        public const string NameColName = "name";
        public const string UserIDColName = "email";

        private readonly BoardController controller;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string boardName;
        private int boardID;
        private string boardOwner;
        bool isPresisted;

        private Dictionary<int,ColumnDAL> columns;

        private List<string> activeUsers;

        public BoardDAL(string boardName,int boardID, string boardOwner, List<string> activeUsers,ColumnDAL backlog,ColumnDAL inProgress,ColumnDAL done)
        {
            controller = new BoardController();
            this.boardName = boardName;
            this.boardID = boardID;
            this.BoardOwner = boardOwner;
            this.isPresisted = false;
            this.ActiveUsers = activeUsers;

            this.columns = new Dictionary<int, ColumnDAL>();
            this.SetColumns(backlog,inProgress,done);
        }
        /// <summary>
        /// A contstructor for boardDAL with empty lists - for controller use only
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <param name="name"></param>
        public BoardDAL(int id,string owner,string name)
        {
            controller = new BoardController();
            this.boardName = name;
            this.boardID = id;
            this.BoardOwner = owner;
            this.isPresisted = false;

            this.ActiveUsers = new List<string>();
            this.columns = new Dictionary<int, ColumnDAL>();
        }

        /// <summary>
        /// Helper method that helps loading columns to the dto.
        /// </summary>
        /// <param name="backlog"></param>
        /// <param name="inProgress"></param>
        /// <param name="done"></param>
        public void SetColumns(ColumnDAL backlog, ColumnDAL inProgress, ColumnDAL done)
        {
            this.Columns.Add(BACKLOG, backlog);
            this.Columns.Add(IN_PROGRESS, inProgress);
            this.Columns.Add(DONE, done);

        }

        public string BoardName { get => boardName; }
        public int BoardID { get => boardID; } //fields that are not changed since creation

        public List<string> ActiveUsers { get => activeUsers; set => activeUsers = value; }

        public Dictionary<int, ColumnDAL> Columns { get => columns; set => columns = value; }


        /// <summary>
        /// Setter for the owner field, updates in the db if needed
        /// </summary>
        public string BoardOwner
        {
            get => boardOwner;
            set
            {
            
                if(isPresisted) //if the entry is int the db
                    this.controller.UpdateOwner(value,this.BoardID); //update it
                
                boardOwner = value;
            }
        }


        /// <summary>
        /// Adds a user to the activeUser list, registers in DB
        /// </summary>
        /// <param name="email"></param>
        public void AddUser(string email)
        {
            this.activeUsers.Add(email);
            this.controller.InsertUser(this.BoardID, email);
        }

        /// <summary>
        /// Removes a user from activeUsers, remove from db aswell.
        /// </summary>
        /// <param name="email"></param>
        public void RemoveUser(string email)
        {
            this.activeUsers.Remove(email);
            this.controller.RemoveUser(this, email);
        }

        public void Save()
        {
            if (this.isPresisted)
            {
                Log.Error("Cannot insert an already presisted board - BOARDDAL");
                throw new Exception("Cannot insert an already presisted board");
            }
               

            this.controller.Insert(this);
            this.AddUser(this.BoardOwner);//adds the owner as the first active member
            isPresisted = true;
        }

        /// <summary>
        /// Operates the board deletion
        /// </summary>
        public void Delete()
        {
            this.controller.Delete(this);
        }

    }
}
