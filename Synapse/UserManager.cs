using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Model.Server;
using Storage;
using Profile = Model.Client.Profile;

namespace Synapse
{
    public class UserManager
    {
        private readonly Dictionary<string, Profile> profileCache;

        public UserManager()
        {
            profileCache = new Dictionary<string, Profile>();
        }

        public async Task<string> GetDisplayNameAsync(string userId)
        {
            Profile profile = await LookupAsync(userId);
            return profile?.DisplayName;
        }

        public async Task<string> GetAvatarAsync(string userId)
        {
            string thumbnailPath = await AppStorage.GetThumbnailAsync(userId);
            if (thumbnailPath != null) return thumbnailPath;

            Profile profile = await LookupAsync(userId);
            if (profile?.AvatarUrl == null) return null;

            string thumbnailUrl = SynapseClient.Instance.GetPreviewUrl(profile.AvatarUrl, 36, 36);
            DownloadResult thumbnail = await SynapseAPI.DownloadImageAsync(thumbnailUrl);
            return await AppStorage.SaveThumbnailAsync(userId, thumbnail.Content);
        }

        private async Task<Profile> LookupAsync(string userId)
        {
            if (profileCache.ContainsKey(userId))
                return profileCache[userId];

            var profile = await SynapseAPI.GetUserProfileAsync(userId);
            var result = Mapper.Map<Profile>(profile);
            if (!profileCache.ContainsKey(userId))
                profileCache.Add(userId, result);
            return result;
        }
    }
}
