namespace SportHub.OAuthRoot
{
    public interface IExternalAuthHandlerFactory
    {
        IExternalAuthHandler GetAuthHandler(bool isCreationRequired, string authProvider);
    }
}