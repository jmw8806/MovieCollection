using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class User
    {
        public int userID {  get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public bool isActive { get; set; }
        public string imgURL { get; set; }
    }

    public class UserVM : User 
    {
        public string roles { get; set; }
    }

}
