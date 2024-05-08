using System;
using System.Linq;
using System.Text;

namespace Project.Models
{
    public class SEOResponseModel
    {
        public string Information { get; set; }
        public readonly UrlModel[] SearchUrls;
        public readonly string Url;

        public SEOResponseModel() { }

        public SEOResponseModel(string url, UrlModel[] searchUrls)
        {
            Url = url;
            SearchUrls = searchUrls.Where(u => u.Url == Url).ToArray();
            StringBuilder infoBuilder = new StringBuilder($"Your website {Url} has appeared {SearchUrls.Length} time's in the top 100 results.\n");
            for(int i = 0; i < SearchUrls.Length; i++)
            {
                infoBuilder.AppendLine($"{SearchUrls[i].Index + 1}");
            }

            Information = infoBuilder.ToString();
        }
    }
}
