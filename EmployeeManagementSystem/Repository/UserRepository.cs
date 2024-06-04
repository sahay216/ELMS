using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository:IEmployee
    {
        public bool delete()
        {
            return true;
        }
    }
}
