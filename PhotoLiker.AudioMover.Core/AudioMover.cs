using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using PhotoLiker.Core;

using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace PhotoLiker.AudioMover.Core
{
    public class AudioMover: AbstractWorker
    {
        private VkApi Api { get; }
        private long Id { get; }
        private string Name { get; }
        
        private int Start { get; }
        private int End { get; }
        
        public AudioMover(VkApi api, long id, string name, int start, int end)
        {
            Api = api;
            Id = id;
            Name = name;
            Start = start;
            End = end;
        }
        
        public override async Task Begin()
        {
            var audios = new List<Audio>();
            const int quan = 100;
            for (var i = Start; i < End; i += quan)
            {
                var got = await Api.Audio.GetAsync(new AudioGetParams {OwnerId = Id, Count = quan, Offset = i});
                audios.AddRange(got);
            }

            await using var sw = new StreamWriter($"{Name}_{Id}_{Start}_{End}.csv");
            await sw.WriteLineAsync(String.Join("\n", audios.Select(au => $"{au.Title};{au.Artist};;")));
        }
    }
}
