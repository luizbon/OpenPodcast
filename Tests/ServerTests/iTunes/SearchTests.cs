using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPodCastServer.Providers.iTunes;
using Xunit;

namespace ServerTests.iTunes
{
    public class SearchTests
    {
        [Fact]
        public async Task SearchShouldNotThrow()
        {
            var request = new SearchRequest();

            await request.Search("Universal Apps");
        }
    }
}
