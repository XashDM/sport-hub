namespace SportHub.OAuthRoot
{
    public interface IExternalAuthHandlerFactory
    {
        IExternalAuthHandler FetchAuthHandler(bool isCreationRequired, string authProvider);
    }
}