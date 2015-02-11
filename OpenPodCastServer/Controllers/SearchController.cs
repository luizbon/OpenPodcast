using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Model;
using OpenPodCastServer.Providers;

namespace OpenPodCastServer.Controllers
{
    public class SearchController : ApiController
    {
        public async Task<IEnumerable<Podcast>> Get(string q)
        {
            var provider = new iTunesProvider();

            return await provider.Search(q);
        }
    }
}