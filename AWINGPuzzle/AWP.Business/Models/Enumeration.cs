using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWP.Business.Models
{
    public class Enumeration
    {
        public enum ResultCode
        {
            Failed = 0,
            Success = 1,
            Authenticated = 204,
            UserNotFound = 205,
            PasswordNotCorrect = 206,
            NotTrueParam = 199,
            NotExistFile = 207,
            NotExistFolder = 208,
            NotHaveRight = 209
        }
    }
}
