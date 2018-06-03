using System.Diagnostics;
using System.Linq;
using Macli.Synapse.DTO;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

namespace Macli.Synapse
{
    static class SynapseAPI
    {
        private static readonly string DEFAULT_URL = "https://matrix.org:8448";
        private static readonly string CLIENT_SEGMENT = "_matrix/client/r0";
        private static readonly string MEDIA_SEGMENT = "_matrix/media/r0";

        private static RestClient client = new RestClient(string.Join('/', DEFAULT_URL, CLIENT_SEGMENT));
        private static RestClient media = new RestClient(string.Join('/', DEFAULT_URL, MEDIA_SEGMENT));

        static SynapseAPI()
        {
            ServicePointManager.ServerCertificateValidationCallback = (message, cert, chain, errors) => true;
        }

        public static void SetHomeserver(string homeserverUrl)
        {
            if (string.IsNullOrEmpty(homeserverUrl))
                return;

            client = new RestClient(string.Join('/', homeserverUrl.TrimEnd('/'), CLIENT_SEGMENT));
            media = new RestClient(string.Join('/', homeserverUrl.TrimEnd('/'), MEDIA_SEGMENT));
        }

        public static async Task<User> LoginAsync(Credentials credentials)
        {
            var request = new RestRequest("login", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddBody(credentials);

            var result = await client.ExecuteTaskAsync<User>(request);
            return result.Data;
        }

        public static async Task<SyncState> SyncAsync(string accessToken)
        {
            var request = new RestRequest("sync", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddQueryParameter("access_token", accessToken);

            var result = await client.ExecuteTaskAsync<SyncState>(request);
            return result.Data;
        }

        public static async Task<string> GetAvatarUrlAsync(string userId)
        {
            var request = new RestRequest("profile/{userId}", Method.GET) {RequestFormat = DataFormat.Json};
            request.AddUrlSegment("userId", userId);
            var response = await client.ExecuteTaskAsync<Profile>(request);
            return response.Data.AvatarUrl;

        }

        public static string GetPreviewUrl(string server, string mediaId, int width, int height)
        {
            var request = new RestRequest("thumbnail/{server}/{mediaId}", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("server", server);
            request.AddUrlSegment("mediaId", mediaId);
            request.AddQueryParameter("width", width.ToString());
            request.AddQueryParameter("height", height.ToString());
            return media.BuildUri(request).ToString();
        }
    }
}
