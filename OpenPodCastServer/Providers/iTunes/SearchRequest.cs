using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenPodCastServer.Providers.iTunes
{
    public class SearchRequest
    {
        private const string Api = "https://itunes.apple.com/search?{0}";

        public SearchRequest()
        {
            Parameters = new Dictionary<string, string> {{"media", "podcast"}};
        }

        private string LoadParameters()
        {
            return string.Join("&", Parameters.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
        }

        public async Task<SearchResult> Search(string term)
        {
            Parameters.Add("term", Uri.EscapeDataString(term));

            var json = await RequestRaw(string.Format(Api, LoadParameters()));

            return JsonConvert.DeserializeObject<SearchResult>(json);
        }

        private async Task<string> RequestRaw(string url)
        {
            using (var client = new HttpClient())
                return await client.GetStringAsync(url);
        }

        public Dictionary<string, string> Parameters { set; get; }
    }
}