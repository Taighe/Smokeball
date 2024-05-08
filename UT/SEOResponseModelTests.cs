using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Models;
using System;
using System.Linq;

namespace UT
{
    [TestClass]
    public class SEOResponseModelTests
    {
        [DataTestMethod]
        [DataRow("smokeball.com.au", DisplayName = "URL test 1")]
        [DataRow("leapconveyancer.com.au", DisplayName = "URL test 2")]
        [DataRow("infotrack.com.au", DisplayName = "URL test 3")]
        public void SEOResponseModel_TestConstructor(string url)
        {
            // Arrange
            UrlModel[] testSearchUrls = new UrlModel[]
            { 
                new UrlModel(0, "smokeball.com.au"),
                new UrlModel(1, "leapconveyancer.com.au"),
                new UrlModel(2, "infotrack.com.au"),
                new UrlModel(3, "smokeball.com.au"),
            };

            // Act
            SEOResponseModel seoResponseModel = new SEOResponseModel(url, testSearchUrls);

            // Assert
            Assert.AreEqual(url, seoResponseModel.Url);
            Assert.IsTrue(seoResponseModel.SearchUrls.SequenceEqual(testSearchUrls.Where(u => u.Url == url)));
            Assert.IsTrue(!string.IsNullOrEmpty(seoResponseModel.Information));
        }
    }
}
