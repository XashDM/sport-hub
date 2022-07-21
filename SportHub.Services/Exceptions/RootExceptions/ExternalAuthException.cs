using System;

namespace SportHub.Services.Exceptions.RootExceptions
{
    public abstract class ExternalAuthException : Exception
    {
        public int StatusCode { get; protected set; }

        protected ExternalAuthException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}