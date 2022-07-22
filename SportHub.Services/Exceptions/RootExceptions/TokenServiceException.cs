using System;

namespace SportHub.Services.Exceptions.RootExceptions
{
    public class TokenServiceException : Exception
    {
        public int StatusCode { get; protected set; }

        public TokenServiceException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
