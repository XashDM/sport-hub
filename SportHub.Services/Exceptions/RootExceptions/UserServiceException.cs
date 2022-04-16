using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportHub.Services.Exceptions.RootExceptions
{
    public abstract class UserServiceException : Exception
    {
        protected UserServiceException(string message) : base(message) { }
    }
}
