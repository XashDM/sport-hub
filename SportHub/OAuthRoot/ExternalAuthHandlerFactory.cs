using SportHub.Models;
using SportHub.OAuthRoot.Strategies;
using System;
using System.Reflection;
using System.Text;

namespace SportHub.OAuthRoot
{
    public class ExternalAuthHandlerFactory : IExternalAuthHandlerFactory
    {
        public IExternalAuthHandler FetchAuthHandler(bool isCreationRequired, string authProvider)
        {
            string authAction = isCreationRequired ? "Signup" : "Login";

            ExternalAuthProvidersEnum externalProvider;
            if (Enum.TryParse(authProvider, out externalProvider))
            {
                StringBuilder strategyNameStringBuilder = new StringBuilder(typeof(GoogleLogin).Namespace);
                strategyNameStringBuilder.Append(".");
                strategyNameStringBuilder.Append(authProvider);
                strategyNameStringBuilder.Append(authAction);

                string strategyName = strategyNameStringBuilder.ToString();
                Type strategyHolder = Type.GetType(strategyName);

                return (IExternalAuthHandler)Activator.CreateInstance(strategyHolder);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
