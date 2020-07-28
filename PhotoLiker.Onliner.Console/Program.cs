using System;
using System.Text;

using VkNet;

using PhotoLiker.Onliner.Core;

using VkNet.Model;

namespace PhotoLiker.Onliner.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = Encoding.UTF8;
            System.Console.InputEncoding = Encoding.UTF8;
            
            var api = new VkApi();

            api.AuthorizeAsync(new ApiAuthParams()
                {AccessToken = ""});
            
            var ch = new Core.Onliner(api);

            var res = ch.Check().Result;
        }
    }
}
