using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Tools;

namespace nugetory.Controllers.Helpers
{
    public static class FindPackage
    {
        public static PackageDAO PackageDAO { get; set; }

        public static HttpResponseMessage Find(HttpRequestMessage request, string filter, string orderby, bool desc,
                                               string top, string id)
        {
            Uri uri = request.RequestUri;

            string title = id.ToLowerInvariant();
            List<Package> packages = PackageDAO.Read(p => p.Title.ToLowerInvariant() == title).Result;
            entry[] entries = packages.Select(p => PackageDetails.GetPackageEntry(p, uri)).ToArray();
            DateTime updated = packages.Any()
                                   ? packages.OrderByDescending(p => p.DateUpdated).First().DateUpdated
                                   : DateTime.UtcNow;

            string baseUri = uri.Scheme + "://" + uri.Host + ":" + uri.Port;
            string apiUri = baseUri + "/api/v2/";

            feed feed = new feed
            {
                @base = apiUri,
                id = WebUtility.HtmlDecode(uri.ToString()),
                title = new feedTitle("FindPackagesById"),
                updated = updated,
                link = new feedLink("FindPackagesById", "FindPackagesById"),
                entry = entries
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(feed));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, feed);
            ms.Position = 0;
            string result = new StreamReader(ms).ReadToEnd();

            HttpResponseMessage res = request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(result);

            res.Content.Headers.Remove("Content-Type");
            res.Content.Headers.Add("Content-Type", "application/atom+xml;type=entry;charset=utf-8");
            res.Headers.Add("DataServiceVersion", "2.0;");

            return res;
        }
    }
}
