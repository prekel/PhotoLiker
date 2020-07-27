using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//using VkApiOAuth;

using VkNet;
using VkNet.Enums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

using Newtonsoft.Json;

namespace PhotoLiker
{
	class SavedPhotoLiker
	{
		public VkApi Api { get; private set; }
		public long Id { get; private set; }
		public int Count { get; private set; }
		public TimeSpan WaitOfflineTime { get; private set; }

		public SavedPhotoLiker(VkApi api, long id, int count, TimeSpan waittime)
		{
			Api = api;
			Id = id;
			Count = count;
			WaitOfflineTime = waittime;
		}

		public async void Begin()
		{
			var r = new Random();

			while (true)
			{
				var user1 = Api.Users.Get(new long[] { Id }, ProfileFields.LastSeen);
				var t = user1[0].LastSeen.Time.Value.ToLocalTime();
				if ((DateTime.Now - t).Duration() >= WaitOfflineTime)
				{
					break;
				}
				await Task.Delay(r.Next(10000, 30000));
			}

			var photos = await Api.Photo.GetAsync(new PhotoGetParams
			{
				OwnerId = Id,
				AlbumId = PhotoAlbumType.Saved,
				Extended = true,
				Reversed = true
			});

			var c = 0;
			try
			{
				foreach (var i in photos)
				{
					if (c == Count)
					{
						break;
					}
					if (!i.Likes.UserLikes)
					{
						await Api.Likes.AddAsync(new LikesAddParams { OwnerId = Id, ItemId = i.Id.Value, Type = LikeObjectType.Photo });
						c++;
						await Task.Delay(r.Next(500, 6000));
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}
	}
}
