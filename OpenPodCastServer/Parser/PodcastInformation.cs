using System.Collections.Generic;
using Model;

namespace OpenPodCastServer.Parser
{
    public class PodcastInformation
    {
        public string ItunesImageUrl { get; set; }
        public string ImageURL { get; set; }
        public string LinkFeedNextPageUrl { get; set; }
        public string LinkFeedFirstPageUrl { get; set; }
        public string LinkFeedLastPageUrl { get; set; }
        public string LinkWebsiteURL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string Copyright { get; set; }
        public string ManagingDirector { get; set; }
        public string WebMaster { get; set; }
        public string Rating { get; set; }
        public string ItunesSubtitle { get; set; }
        public string ItunesAuthor { get; set; }
        public string ItunesSummary { get; set; }
        public List<string> ItunesKeywords { get; set; }
        public List<ItunesCategory> ItunesCategorys { get; set; }
        public ItunesOwner ItunesOwner { get; set; }
        public string PubDate { get; set; }
        public string LastBuildDate { get; set; }
        public string Category { get; set; }
        public List<AlternateFeed> LinkAlternateFeeds { get; set; }
        public List<Payment> LinkPayments { get; set; }
        public List<Contributor> Contributors { get; set; }
    }
}