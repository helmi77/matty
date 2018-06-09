using System.IO;
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

        public static string GetPreviewUrl(string server, string mediaId, int width, int height)
        {
            var request = new RestRequest("thumbnail/{server}/{mediaId}", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("server", server);
            request.AddUrlSegment("mediaId", mediaId);
            request.AddQueryParameter("width", width.ToString());
            request.AddQueryParameter("height", height.ToString());
            return media.BuildUri(request).ToString();
        }

        public static async Task SendMessageAsync(string accessToken, string roomId, string message)
        {
            var request = new RestRequest("rooms/{roomId}/send/m.room.message", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("roomId", roomId);
            request.AddQueryParameter("access_token", accessToken);
            request.AddBody(new
            {
                msgtype = "m.text",
                body = message,
            });

            await client.ExecuteTaskAsync(request);
        }

        public static async Task<string> UploadFileAsync(string accessToken, string name, string contentType, Stream fileStream)
        {
            var request = new RestRequest("upload", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddQueryParameter("filename", name);
            request.AddQueryParameter("access_token", accessToken);
            request.AddFile("file", fileStream.CopyTo, name, fileStream.Length, contentType);

            var response = await media.ExecuteTaskAsync<UploadResult>(request);
            return response.Data.ContentUri;
        }

        public static async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            var request = new RestRequest("profile/{userId}") { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("userId", userId);

            var response = await client.ExecuteTaskAsync<UserProfile>(request);
            return response.Data;
        }

        public static async Task<DownloadResult> DownloadImageAsync(string imageUrl)
        {
            var request = new RestRequest(imageUrl) { RequestFormat = DataFormat.Json };
            var response = await client.ExecuteTaskAsync(request);
            return new DownloadResult {Content = response.RawBytes, ContentType = response.ContentType};
        }
    }
}
