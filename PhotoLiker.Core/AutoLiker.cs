using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using VkNet;
using VkNet.Enums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

using NLog;

namespace PhotoLiker.Core
{
    public class AutoLiker : AbstractVkWorker
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        public IList<long> Ids { get; }

        public AutoLiker(VkApi api, IList<long> ids) : base(api)
        {
            Ids = ids;
        }

        public override async Task Begin()
        {
            Log.Info("Запуск AutoLiker");

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
                            await Api.Likes.AddAsync(new LikesAddParams
                                {OwnerId = id, ItemId = photo.Id.Value, Type = LikeObjectType.Photo});
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
