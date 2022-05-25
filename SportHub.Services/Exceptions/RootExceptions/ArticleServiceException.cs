using System;

namespace SportHub.Services.Exceptions.RootExceptions
{
    public abstract class ArticleServiceException : Exception
    {
        public int StatusCode { get; protected set; }
        protected ArticleServiceException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

