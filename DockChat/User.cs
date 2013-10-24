using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockChat
{
    public class User
    {
        //public static List<> 
        
        private static User _currentUser = new User();
        public static User CurrentUser { get { return _currentUser; } set { _currentUser = value; } }

        private List<Group> _groups = new List<Group>(); 
        public List<Group> Groups { get{ return _groups; } set { _groups = value; } }

    }
}
