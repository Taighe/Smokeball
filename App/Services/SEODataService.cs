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
    /// Service responsible for making requests to the search engine. 
    /// Primarly it will retrieve the html data from the request.
    /// </summary>
    public static class SEODataService
    {
        /// <summary>
        /// Get the HTML data asyncrounously with the specified parameters.
        /// </summary>
        /// <param name="keywords">The key words that will be used in to narrow down the search.</param>
        /// <param name="url">The url to compare to.</param>
        /// <param name="http">The instance of HttpClient to make the request.</param>
        /// <returns></returns>
        public async static Task<SEOResponseModel> GetDataAsync(string keywords, string url, HttpClient http)
        {
            // Remove the www. if present
            url = url.Replace("www.", "");
            string[] keywordsArray = keywords.Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            string combinedKeywords = String.Join("+", keywordsArray);
            // Specify the top 100 results from the search
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

        /// <summary>
        /// If they are any errors this will return the appropirate response model.
        /// </summary>
        /// <param name="exception">The exception data.</param>
        /// <param name="response">The response data.</param>
        /// <returns></returns>
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

        /// <summary>
        /// This will modify the search string to remove any unnecessary data and 
        /// add to the current search collection if it is a valid.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="searches"></param>
        private static void AddCleanUrlLink(string searchString, List<UrlModel> searches)
        {
            // Clean search string to just have the simplified address
            string url = string.Empty;
            if (searchString.Contains("http"))
            {
                int index = searchString.IndexOf("://") + 3;
                int endIndex = searchString.IndexOf("/", index);
                url = searchString.Substring(index, endIndex - index);
            }
            
            if (searches.Count < 100 && !string.IsNullOrEmpty(url) && !url.Contains("google"))
            {
                // Remove "www." if present
                UrlModel urlModel = new UrlModel(searches.Count, url.Replace("www.", ""));
                searches.Add(urlModel);
            }
        }
    }
}
