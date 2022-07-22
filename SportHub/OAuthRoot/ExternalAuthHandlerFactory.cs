using SportHub.Models;
using SportHub.OAuthRoot.Strategies;
using SportHub.Services.Exceptions.ExternalAuthExceptions;
using System;

namespace SportHub.OAuthRoot
{
    public class ExternalAuthHandlerFactory : IExternalAuthHandlerFactory
    {
        public IExternalAuthHandler GetAuthHandler(bool isCreationRequired, string authProvider)
        {
            ExternalAuthProvidersEnum externalProvider;
            if (Enum.TryParse(authProvider, out externalProvider))
            {
                switch (externalProvider)
                {
                    case ExternalAuthProvidersEnum.Facebook:
                        return new FacebookAuth();
                    case ExternalAuthProvidersEnum.Google:
                        return new GoogleAuth();
                    default:
                        throw new Exception();
                }
            }
            else
            {
                throw new InvalidAuthProviderException();
            }
        }
    }
}
