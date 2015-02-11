using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Model;

namespace OpenPodCastServer.Parser
{
    public static class PodcastParser
    {
        public static Podcast Parse(XElement channel)
        {
            return Transform(Read(channel));
        }

        private static Podcast Transform(PodcastInformation info)
        {
            var podcast = new Podcast();

            podcast.Title = info.Title;
            podcast.Author = info.ItunesAuthor;

            if (!string.IsNullOrEmpty(info.LinkWebsiteURL))
                podcast.WebsiteUrl = info.LinkWebsiteURL.ToLower();

            if (!string.IsNullOrEmpty(info.ItunesImageUrl))
            {
                podcast.ImageUrl = info.ItunesImageUrl.ToLower();
                podcast.ImageUri = new Uri(podcast.ImageUrl);
            }
            else
            {
                if (!string.IsNullOrEmpty(info.ImageURL))
                {
                    podcast.ImageUrl = info.ImageURL.ToLower();
                    podcast.ImageUri = new Uri(podcast.ImageUrl);
                }
            }

            podcast.Language = info.Language;
            podcast.Copyright = info.Copyright;

            DateTime lastbuilddate;
            if (DateTime.TryParse(info.PubDate, out lastbuilddate))
                podcast.LastBuild = lastbuilddate;
            else
                podcast.LastBuild = DateTime.Now;

            podcast.FeedAlt = info.LinkAlternateFeeds;
            if (podcast.HasFeedAlt)
            {
                foreach (var f in podcast.FeedAlt)
                    f.URL = f.URL.ToLower();
            }

            podcast.Subtitle = info.ItunesSubtitle;
            podcast.Summary = info.ItunesSummary;
            podcast.Description = info.Description;
            podcast.Keywords = info.ItunesKeywords;
            podcast.Categorys = info.ItunesCategorys;

            podcast.Contributors = info.Contributors;
            if (podcast.HasContributors)
            {
                foreach (var c in podcast.Contributors)
                {
                    if (!string.IsNullOrEmpty(c.URI))
                        c.URI = c.URI.ToLower();
                }
            }

            podcast.PaymentLinks = info.LinkPayments;
            if (podcast.HasPaymentLinks)
            {
                foreach (var p in podcast.PaymentLinks)
                    p.URL = p.URL.ToLower();
            }

            return podcast;
        }

        private static PodcastInformation Read(XElement channel)
        {
            var info = new PodcastInformation();
            foreach (var e in channel.Elements())
            {
                ParseMeta(e, info);
                ParseMetaAtom(e, info);
                ParseMetaItunes(e, info);
            }

            return info;
        }

        private static void ParseMeta(XElement e, PodcastInformation info)
        {
            switch (e.Name.ToString().ToLower())
            {
                case "title":
                    info.Title = e.Value;
                    break;
                case "link":
                    info.LinkWebsiteURL = e.Value;
                    break;
                case "language":
                    info.Language = e.Value;
                    break;
                case "description":
                    info.Description = e.Value;
                    break;
                case "copyright":
                    info.Copyright = e.Value;
                    break;
                case "managingeditor":
                    info.ManagingDirector = e.Value;
                    break;
                case "webmaster":
                    info.WebMaster = e.Value;
                    break;
                case "rating":
                    info.Rating = e.Value;
                    break;
                case "pubdate":
                    info.PubDate = e.Value;
                    break;
                case "lastbuilddate":
                    info.LastBuildDate = e.Value;
                    break;
                case "category":
                    info.Category = e.Value;
                    break;
                case "image":
                    if (e.HasElements)
                    {
                        var x = e.Elements("url").FirstOrDefault();
                        if (x != null && !x.IsEmpty)
                            info.ImageURL = x.Value;
                    }
                    break;
            }
        }

        private static void ParseMetaAtom(XElement e, PodcastInformation info)
        {
            if (e.Name.ToString().ToLower() == "{" + FeedNamespaceCollection.atom + "}link" && e.HasAttributes &&
                e.Attribute("rel") != null)
            {
                switch (e.Attribute("rel").Value)
                {
                    case "alternate":
                        if (info.LinkAlternateFeeds == null)
                            info.LinkAlternateFeeds = new List<AlternateFeed>();
                        info.LinkAlternateFeeds.Add(new AlternateFeed
                                                    {
                                                        Title = e.Attribute("title").Value,
                                                        URL = e.Attribute("href").Value
                                                    });
                        break;

                    case "next":
                        info.LinkFeedNextPageUrl = e.Attribute("href").Value;
                        break;

                    case "first":
                        info.LinkFeedFirstPageUrl = e.Attribute("href").Value;
                        break;

                    case "last":
                        info.LinkFeedLastPageUrl = e.Attribute("href").Value;
                        break;

                    case "payment":
                        if (e.HasAttributes && e.Attribute("href") != null && e.Attribute("title") != null)
                        {
                            if (info.LinkPayments == null)
                                info.LinkPayments = new List<Payment>();
                            info.LinkPayments.Add(new Payment
                                                  {
                                                      URL = e.Attribute("href").Value,
                                                      Title = e.Attribute("title").Value
                                                  });
                        }
                        break;
                }
            }

            if (e.Name.ToString().ToLower() != "{" + FeedNamespaceCollection.atom + "}contributor" || !e.HasElements)
                return;

            var c = new Contributor();
            foreach (var i in e.Elements())
            {
                if (i.Name.ToString().ToLower() == "{" + FeedNamespaceCollection.atom + "}name")
                    c.Name = i.Value;
                if (i.Name.ToString().ToLower() == "{" + FeedNamespaceCollection.atom + "}uri")
                    c.URI = i.Value;
            }

            if (string.IsNullOrEmpty(c.Name)) return;

            if (info.Contributors == null)
                info.Contributors = new List<Contributor>();
            info.Contributors.Add(c);
        }

        private static void ParseMetaItunes(XElement e, PodcastInformation info)
        {
            switch (e.Name.ToString().ToLower())
            {
                case "{" + FeedNamespaceCollection.itunes + "}subtitle":
                    info.ItunesSubtitle = e.Value;
                    break;

                case "{" + FeedNamespaceCollection.itunes + "}author":
                    info.ItunesAuthor = e.Value;
                    break;

                case "{" + FeedNamespaceCollection.itunes + "}summary":
                    info.ItunesSummary = e.Value;
                    break;

                case "{" + FeedNamespaceCollection.itunes + "}keywords":
                    if (e.Value != "")
                    {
                        if (info.ItunesKeywords == null)
                            info.ItunesKeywords = new List<string>();
                        var k = e.Value;
                        var kk = k.Split(',');
                        foreach (var kkk in kk)
                            info.ItunesKeywords.Add(kkk.Trim().ToLower());
                    }
                    break;

                case "{" + FeedNamespaceCollection.itunes + "}category":
                    if (e.HasAttributes && e.Attribute("text") != null && e.Attribute("text").Value != "")
                    {
                        var c = new ItunesCategory {Name = e.Attribute("text").Value};
                        if (e.HasElements)
                        {
                            c.SubCategorys = new List<string>();
                            foreach (var cc in e.Elements())
                            {
                                if (cc.HasAttributes && cc.Attribute("text") != null && cc.Attribute("text").Value != "")
                                    c.SubCategorys.Add(cc.Attribute("text").Value);
                            }
                        }
                        if (info.ItunesCategorys == null)
                            info.ItunesCategorys = new List<ItunesCategory>();
                        info.ItunesCategorys.Add(c);
                    }
                    break;

                case "{" + FeedNamespaceCollection.itunes + "}owner":
                    if (e.HasElements)
                    {
                        var io = new ItunesOwner();
                        foreach (var o in e.Elements())
                        {
                            if (o.Name.ToString().ToLower() == "{" + FeedNamespaceCollection.itunes + "}name")
                                io.Name = o.Value;
                            if (o.Name.ToString().ToLower() == "{" + FeedNamespaceCollection.itunes + "}email")
                                io.EMail = o.Value;
                        }
                        if (!string.IsNullOrEmpty(io.Name))
                            info.ItunesOwner = io;
                    }
                    break;

                case "{" + FeedNamespaceCollection.itunes + "}image":
                    if (e.HasAttributes && e.Attribute("href") != null && e.Attribute("href").Value != "")
                        info.ItunesImageUrl = e.Attribute("href").Value;
                    break;
            }
        }
    }
}