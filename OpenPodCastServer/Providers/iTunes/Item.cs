using System.Collections.Generic;

namespace OpenPodCastServer.Providers.iTunes
{
    public class Item
    {
        public Item()
        {
            GenreIDs = new List<string>();
            Genres = new List<string>();
        }

        public string WrapperType { set; get; }
        public string Kind { set; get; }
        public string ArtistID { set; get; }
        public string CollectionID { set; get; }
        public string TrackId { set; get; }
        public string ArtistName { set; get; }
        public string CollectionName { set; get; }
        public string TrackName { set; get; }
        public string CollectionCensoredName { set; get; }
        public string TrackCensoredName { set; get; }
        public string ArtistViewUrl { set; get; }
        public string FeedUrl { set; get; }
        public string TrackViewUrl { set; get; }
        public string PreviewUrl { set; get; }
        public string ArtworkUrl60 { set; get; }
        public string ArtworkUrl100 { set; get; }
        public float CollectionPrice { set; get; }
        public float TrackPrice { set; get; }
        public string CollectionExplicitness { set; get; }
        public string TrackExplicitness { set; get; }
        public string DiscCount { set; get; }
        public string DiscNumber { set; get; }
        public string TrackCount { set; get; }
        public string TrackNumber { set; get; }
        public string TrackTimeMillis { set; get; }
        public string Country { set; get; }
        public string Currency { set; get; }
        public string PrimaryGenreName { set; get; }
        public List<string> GenreIDs { set; get; }
        public List<string> Genres { set; get; }
    }
}