using SportHub.Services.Exceptions.ExceptionModels;
using System;

namespace SportHub.Services.Exceptions.RootExceptions
{
    public class TokenServiceException : Exception
    {
        public int StatusCode { get; protected set; }
        public TokenExceptionArgs Args { get; protected set; } = new TokenExceptionArgs();

        public TokenServiceException(string message, int statusCode, bool isReloginRequired = false) : base(message)
        {
            StatusCode = statusCode;
            Args.Message = message;
            Args.IsReloginRequired = isReloginRequired;
        }
    }
}
