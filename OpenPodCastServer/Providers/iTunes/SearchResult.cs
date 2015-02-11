using System.Collections.Generic;

namespace OpenPodCastServer.Providers.iTunes
{
    public class SearchResult
    {
        public SearchResult()
        {
            Results = new List<Item>();
        }

        public int ResultCount { set; get; }
        public List<Item> Results { set; get; }
    }
}