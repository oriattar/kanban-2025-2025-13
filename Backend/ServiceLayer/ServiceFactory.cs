using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private ForumService FS;
        private PostService PS;
        private UserService US;

        public ServiceFactory(ForumService fS, PostService pS, UserService uS)
        {
            FS = fS;
            PS = pS;
            US = uS;
        }

        public ForumService FS1 { get => FS; set => FS = value; }
        public PostService PS1 { get => PS; set => PS = value; }
        public UserService US1 { get => US; set => US = value; }
    }
}
