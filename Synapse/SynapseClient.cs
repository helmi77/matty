using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Windows.Storage;
using AutoMapper;
using Model;
using Model.Client;
using Model.Server;
using Storage;
using Synapse.Processing;
using Synapse.Processing.Comparers;
using ClientRoom = Model.Client.Room;
using ServerRoom = Model.Server.Room;

namespace Synapse
{
    public sealed class SynapseClient
    {
        public static SynapseClient Instance => instance.Value;
        private static readonly Lazy<SynapseClient> instance = new Lazy<SynapseClient>(() => new SynapseClient());

        public User User { get; set; }
        public UserManager UserManager { get; set; }
        public string EndpointUrl { get; set; }

        private string nextBatch;

        private SynapseClient()
        {
            UserManager = new UserManager();
        }

        public void Initialize(InitialState state)
        {
            User = state.User;
            EndpointUrl = state.EndpointUrl;
            SynapseAPI.SetHomeserver(state.EndpointUrl);
        }

        public async Task LoginAsync(string user, string password, string homeserverUrl)
        {
            if (!string.IsNullOrEmpty(homeserverUrl))
            {
                AppStorage.SaveEndpointUrl(homeserverUrl);
                SynapseAPI.SetHomeserver(homeserverUrl);
            }

            User = await SynapseAPI.LoginAsync(new Credentials{user = user, password = password});
        }

        public async Task SendMessageAsync(ClientRoom room)
        {
            if (room == null || string.IsNullOrEmpty(room.NewMessage)) return;
            Debug.WriteLine($"Sending {room.NewMessage} in {room.ID}");
            await SynapseAPI.SendMessageAsync(User.AccessToken, room.ID, room.NewMessage.Replace("\r", "  \n"));
        }

        public string GetPreviewUrl(string mxcUrl, int width, int height)
        {
            var uri = new Uri(mxcUrl);
            return SynapseAPI.GetPreviewUrl(uri.Host, uri.Segments[1], width, height);
        }

        public string GetPreviewUrl(Picture picture)
        {
            var uri = new Uri(picture.ThumbnailUrl ?? picture.Url);
            return SynapseAPI.GetPreviewUrl(uri.Host, uri.Segments[1], picture.Width, picture.Height);
        }

        public async Task<string> GetUserAvatarThumbnailAsync(string userId)
        {
            return await UserManager.GetAvatarAsync(userId);
        }

        public async Task<string> UploadFile(StorageFile file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                return await SynapseAPI.UploadFileAsync(User.AccessToken, file.Name, stream.ContentType,
                    stream.AsStreamForRead());
            }
        }

        public async Task<string> GetDisplayName(string userId)
        {
            return await UserManager.GetDisplayNameAsync(userId);
        }

        public async Task<IEnumerable<ClientRoom>> SynchronizeAsync()
        {
            if (string.IsNullOrEmpty(nextBatch))
                nextBatch = AppStorage.LoadLastTimestamp();

            var syncState = await SynapseAPI.SyncAsync(User.AccessToken, nextBatch);
            IEnumerable<ServerRoom> rooms = syncState.JoinedRooms.Values;
            nextBatch = syncState.NextBatch;
            AppStorage.SaveTimestamp(nextBatch); 

            var messageProcessor = new SequenceProcessor<RoomEvent>();
            messageProcessor.ModifyItems(e => e.IsMine = e.Sender.Equals(User.ID));
            messageProcessor.DefineSequence("FollowupMessages", new FollowupComparer());
            messageProcessor.ModifySequence("FollowupMessages", sequence => sequence.Tail.ForEach(item => item.IsFollowup = true));
            messageProcessor.ModifySequence("FollowupMessages", sequence => sequence.Last.IsLastFollowup = true);

            var roomProcessor = new SequenceProcessor<ServerRoom>();
            roomProcessor.ModifyItems(r => messageProcessor.Process(r.History.Events));
            roomProcessor.Process(rooms.ToList());

            IEnumerable<ClientRoom> result = Mapper.Map<IEnumerable<ServerRoom>, IEnumerable<ClientRoom>>(rooms);
            return result.Zip(syncState.JoinedRooms.Keys, (room, id) =>
            {
                room.ID = id;
                return room;
            });
        }
    }
}