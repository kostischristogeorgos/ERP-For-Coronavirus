using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thema3
{
    class Employee
    {
        String username, password;
        public Employee(String user, String pass)
        {
            username = user;
            password = pass;
        }
        public bool CheckLogin(String user, String pass)
        {   // A function that checks for matching username and password.
            if (user.Equals(this.username) && pass.Equals(this.password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
