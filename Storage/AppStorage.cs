using Microsoft.Toolkit.Uwp.Helpers;
using Model;
using Model.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Storage
{
    public class AppStorage
    {
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        private static readonly StorageFolder CacheFolder = ApplicationData.Current.LocalCacheFolder;

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

        public static async Task<string> GetThumbnailAsync(string userId)
        {
            StorageFolder thumbnailFolder = await CacheFolder.CreateFolderAsync("Thumbnails", CreationCollisionOption.OpenIfExists);
            string filename = SanitizeFilename(userId);
            if (await thumbnailFolder.FileExistsAsync(filename))
            {
                var file = await thumbnailFolder.GetFileAsync(filename);
                return file.Path;
            }

            return null;
        }

        public static async Task<string> SaveThumbnailAsync(string name, byte[] fileBytes)
        {
            StorageFolder thumbnailFolder = await CacheFolder.CreateFolderAsync("Thumbnails", CreationCollisionOption.OpenIfExists);

            StorageFile file = null;
            string sanitizedFilename = SanitizeFilename(name);

            try
            {
                file = await thumbnailFolder.CreateFileAsync(sanitizedFilename, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBytesAsync(file, fileBytes);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine($"Failed to write thumbnail {sanitizedFilename}");
            }

            return file?.Path;
        }

        private static string SanitizeFilename(string filename)
        {
            string invalid = new string(Path.GetInvalidPathChars()) + new string(Path.GetInvalidFileNameChars());
            foreach (var character in invalid)
            {
                filename = filename.Replace(character, '_');
            }

            return filename;
        }

        public static string LoadLastTimestamp() => LocalSettings.Values["SyncTimestamp"] as string; 
        public static void SaveTimestamp(string timestamp) => LocalSettings.Values["SyncTimestamp"] = timestamp;

        public static async Task<IEnumerable<Model.Client.Room>> LoadRooms()
        {
            StorageFolder roomsFolder = await CacheFolder.CreateFolderAsync("Rooms", CreationCollisionOption.OpenIfExists);
            if (await roomsFolder.FileExistsAsync("rooms.json"))
            {
                var file = await roomsFolder.GetFileAsync("rooms.json");
                using (var stream = await file.OpenStreamForReadAsync())
                using (StreamReader streamReader = new StreamReader(stream))
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return serializer.Deserialize<IEnumerable<Model.Client.Room>>(reader);
                }
            }

            return Enumerable.Empty<Model.Client.Room>();
        }

        public static async void SaveRooms(IEnumerable<Model.Client.Room> rooms)
        { 
            try
            {
                StorageFolder roomsFolder = await CacheFolder.CreateFolderAsync("Rooms", CreationCollisionOption.OpenIfExists); 
                StorageFile roomsFile = await roomsFolder.CreateFileAsync("rooms.json", CreationCollisionOption.ReplaceExisting);
                using (var stream = await roomsFile.OpenStreamForWriteAsync())
                using (StreamWriter streamWriter = new StreamWriter(stream))
                using (JsonWriter writer = new JsonTextWriter(streamWriter))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, rooms);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine($"Failed to write rooms.json file");
            }
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

