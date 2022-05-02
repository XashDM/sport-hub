using System;

namespace SportHub.Services.Exceptions.RootExceptions
{
    public abstract class UserServiceException : Exception
    {
        public int StatusCode { get; protected set; }
        protected UserServiceException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
