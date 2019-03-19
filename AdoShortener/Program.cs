using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoShortener
{
    class Program
    {
        static void Main(string[] args)
        {
            UserService userService = new UserService();
            userService.Start();
        }
        
    }
}
