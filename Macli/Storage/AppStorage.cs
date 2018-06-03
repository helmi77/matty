using Macli.Synapse;
using Windows.Storage;
using Macli.Synapse.DTO;

namespace Macli.Storage
{
    class AppStorage
    {
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static InitialState LoadInitialState()
        {
            return new InitialState
            {
                User = LoadUser(),
                EndpointUrl = LoadEndpointUrl(),
            };
        }

        public static void SaveUser(User user)
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue
            {
                ["ID"] = user.ID,
                ["Homeserver"] = user.Homeserver,
                ["AccessToken"] = user.AccessToken
            };
            LocalSettings.Values["User"] = composite;
        }

        public static User LoadUser()
        {
            ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)LocalSettings.Values["User"];
            if (composite == null)
                return null;

            return new User
            {
                ID = composite["ID"] as string,
                Homeserver = composite["Homeserver"] as string,
                AccessToken = composite["AccessToken"] as string
            };
        }

        public static void ClearUser()
        {
            if (LocalSettings.Values.ContainsKey("User"))
                LocalSettings.Values.Remove("User");
        }

        public static void SaveEndpointUrl(string homeserverUrl)
        {
            LocalSettings.Values["EndpointUrl"] = homeserverUrl;
        }

        private static string LoadEndpointUrl()
        {
            if (!LocalSettings.Values.ContainsKey("EndpointUrl"))
                return null;
            return LocalSettings.Values["EndpointUrl"] as string;
        }
    }
}

