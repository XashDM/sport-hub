using SportHub.Services.Exceptions.RootExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportHub.Services.Exceptions.UserServiceExceptions
{
    public class EmailAlreadyInUseException : UserServiceException
    {
        public EmailAlreadyInUseException() : base($"User with such email already exists") { }
    }
}
