using Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project.Services
{
    /// <summary>
    /// Service responsible for making request to search engine.
    /// </summary>
    public static class SEODataService
    {
        public async static Task<SEOResponseModel> GetDataAsync(string keywords, string url, HttpClient http)
        {
            url = url.Replace("www.", "");
            string[] keywordsArray = keywords.Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            string combinedKeywords = String.Join("+", keywordsArray);
            string combinedUrl = $"{ConfigurationManager.AppSettings["url"]}/search?num=100&q={combinedKeywords}";
            HttpResponseMessage response = null;

            try
            {
                response = await http.GetAsync(combinedUrl);
                string responseString = await response.Content.ReadAsStringAsync();
                int index = 0;
                index = responseString.IndexOf("class=\"kCrYT\"", index);
                index = responseString.LastIndexOf("href=", index);
                List<UrlModel> searches = new List<UrlModel>();
                while (index != -1)
                {
                    int endOfSearchIndex = responseString.IndexOf(';', index);
                    int length = endOfSearchIndex - index + 1;
                    string search = responseString.Substring(index, length);
                    AddCleanUrlLink(search, searches);
                    index = responseString.IndexOf("href=", endOfSearchIndex);
                }

                response.Content.Dispose();
                return new SEOResponseModel(url, searches.ToArray());
            }
            catch(Exception exception)
            {
                return GetErrorResponse(exception, response);
            }
        }

        private static SEOResponseModel GetErrorResponse(Exception exception, HttpResponseMessage response)
        {
            if(response == null)
            {
                return new SEOResponseModel { Information = $"{exception.Message} Please check your connection." };
            }

            switch((int)response.StatusCode)
            {
                case 400:
                    return new SEOResponseModel { Information = response.ReasonPhrase };
                case 429:
                default:
                    return new SEOResponseModel { Information = $"{response.ReasonPhrase}. Try again later." };
            }
        }

        private static void AddCleanUrlLink(string searchString, List<UrlModel> searches)
        {
            // Clean search string to just have the simplified address.
            string url = string.Empty;
            if (searchString.Contains("http"))
            {
                int index = searchString.IndexOf("://") + 3;
                int endIndex = searchString.IndexOf("/", index);
                url = searchString.Substring(index, endIndex - index);
            }
            
            if (searches.Count < 100 && !string.IsNullOrEmpty(url) && !url.Contains("google"))
            {
                // Remove "www." if present.
                UrlModel urlModel = new UrlModel(searches.Count, url.Replace("www.", ""));
                searches.Add(urlModel);
            }
        }
    }
}
