using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Model;
using OpenPodCastServer.Parser;
using OpenPodCastServer.Providers.iTunes;

namespace OpenPodCastServer.Providers
{
    public class iTunesProvider : BaseProvider
    {
        public async Task<IEnumerable<Podcast>> Search(string term)
        {
            var request = new SearchRequest();

            var result = await request.Search(term);

            var informations = new List<Podcast>();

            foreach (var item in result.Results)
            {
                var podcast = await Parse(item.FeedUrl);

                if (podcast != null)
                    informations.Add(podcast);
            }

            return informations;
        }
    }

    public abstract class BaseProvider
    {
        protected async Task<Podcast> Parse(string feedUrl)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var xml = await client.GetStringAsync(feedUrl);
                    var root = XmlHelper.Parse(xml);
                    if (root == null) return null;
                    var channel = root.Descendants("channel").FirstOrDefault();
                    return PodcastParser.Parse(channel);
                }
                catch (Exception exception)
                {
                    //TODO: Log exception
                    return null;
                }
            }
        }
    }
}