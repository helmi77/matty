using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Profile = Macli.Views.Models.Profile;

namespace Macli.Synapse
{
    public class UserManager
    {
        private readonly Dictionary<string, Profile> profileCache;

        public UserManager()
        {
            profileCache = new Dictionary<string, Profile>();
        }

        public async Task<Profile> LookupAsync(string userId)
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
