namespace SportHub.Services.Exceptions.ExceptionModels
{
    public class TokenExceptionArgs
    {
        public string Message { get; set; }
        public bool IsReloginRequired { get; set; } = false;
    }
}
