using System;

namespace SportHub.Services.Exceptions.RootExceptions
{
    public abstract class UserServiceException : Exception
    {
        protected UserServiceException(string message) : base(message) { }
    }
}
