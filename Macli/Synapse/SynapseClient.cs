using AutoMapper;
using Macli.Synapse.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Macli.Storage;
using Macli.Views;

namespace Macli.Synapse
{
    sealed class SynapseClient
    {
        public static SynapseClient Instance => instance.Value;
        private static readonly Lazy<SynapseClient> instance = new Lazy<SynapseClient>(() => new SynapseClient());

        public User User { get; set; }
        public string EndpointUrl { get; set; }

        private SynapseClient() { }

        public void Initialize(InitialState state)
        {
            User = state.User;
            EndpointUrl = state.EndpointUrl;
            SynapseAPI.SetHomeserver(state.EndpointUrl);
        }

        public async Task LoginAsync(LoginViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.HomeserverUrl))
            {
                AppStorage.SaveEndpointUrl(viewModel.HomeserverUrl);
                SynapseAPI.SetHomeserver(viewModel.HomeserverUrl);
            }

            var credentials = Mapper.Map<Credentials>(viewModel);
            User = await SynapseAPI.LoginAsync(credentials);
        }

        public async Task<IEnumerable<Views.Models.Room>> LoadRoomsAsync(RoomViewModel viewModel)
        {
            var syncState = await SynapseAPI.SyncAsync(User.AccessToken);
            IEnumerable<Room> rooms = syncState.JoinedRooms.Values;
            return Mapper.Map<IEnumerable<Room>, IEnumerable<Views.Models.Room>>(ProcessMessages(rooms));
        }

        private IEnumerable<Room> ProcessMessages(IEnumerable<Room> rooms)
        {
            var roomList = rooms.ToList();
            foreach (var room in roomList)
            {
                var events = new List<RoomEvent>();
                RoomEvent prevEvent = null;
                foreach (var roomEvent in room.History.Events)
                {
                    if (prevEvent != null)
                    {
                        if (prevEvent.Sender.Equals(roomEvent.Sender))
                        {
                            roomEvent.Preview = roomEvent.Content.Body.Replace('\n', ' ');
                            roomEvent.Content.Body = $"{prevEvent.Content.Body}  \n  \n" +
                                                     $"{roomEvent.Content.Body}";
                        }
                        else
                        {
                            events.Add(prevEvent);
                        }
                    }
                    prevEvent = roomEvent;
                }
                events.Add(prevEvent);
                room.History.Events = events;
            }

            return roomList;
        }

        public string GetPreviewUrl(string mxcUrl, int width, int height)
        {
            var uri = new Uri(mxcUrl);
            return SynapseAPI.GetPreviewUrl(uri.Host, uri.Segments[1], width, height);
        }
    }
}