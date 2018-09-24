﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using VkApiOAuth;

using VkNet;
using VkNet.Enums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

namespace PhotoLiker
{
	public class AutoLiker
	{
		public VkApi Api { get; private set; }
		public long[] Ids { get; private set; }

		public AutoLiker(VkApi api, long[] ids)
		{
			Api = api;
			Ids = ids;
		}

		public async void Begin()
		{
			var t = DateTime.Now;
			var r = new Random();
			
			while (true)
			{
				foreach (var id in Ids)
				{
					try
					{
						var photo = (await Api.Photo.GetAsync(new PhotoGetParams
						{
							OwnerId = id,
							AlbumId = PhotoAlbumType.Saved,
							Extended = true,
							Reversed = true,
							Count = 1
						}))[0];
						if (photo.Likes.UserLikes == false && photo.CreateTime.Value.ToLocalTime() > t)
						{
							await Api.Likes.AddAsync(new LikesAddParams { OwnerId = id, ItemId = photo.Id.Value, Type = LikeObjectType.Photo });
						}
						await Task.Delay(r.Next(333, 1500));
					}
					catch (IndexOutOfRangeException)
					{
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						await Task.Delay(r.Next(1000, 2000));
					}
				}
			}
		}
	}
}