﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net; 

using VkApiOAuth;

using VkNet;
using VkNet.Enums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

namespace PhotoLiker
{
	public class SavedPhotoSaver
	{
		public VkApi Api { get; private set; }
		public long Id { get; private set; }

		public SavedPhotoSaver(VkApi api, long id)
		{
			Api = api;
			Id = id;
		}

		public async void Begin()
		{
			try
			{
				var t = await Api.Photo.GetAsync(new PhotoGetParams
				{
					OwnerId = Id,
					AlbumId = PhotoAlbumType.Saved,
					PhotoSizes = true
				});
				var wc = new WebClient();
				var dirname = $"Saver {Id} {DateTime.Now.ToShortDateString().Replace("/", "-")} {DateTime.Now.ToLongTimeString().Replace(":", "-")}";
				Directory.CreateDirectory(dirname);
				foreach (var i in t)
				{
					//var uri = i.Sizes.Last().Src;
					var mxw = i.Sizes.Max(j => j.Width);
					var uri = i.Sizes.First(j => j.Width == mxw).Src;
					wc.DownloadFile(uri, $"{dirname}{@"\"}{i.CreateTime.Value.ToShortDateString().Replace("/", "-")} {i.CreateTime.Value.ToLongTimeString().Replace(":", "-")}.jpg");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}