using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace UT
{
    [TestClass]
    public class SEOViewModelTests
    {
        [TestMethod]
        public void SEOViewModel_TestConstructor()
        {
            SEOViewModel viewModel = new SEOViewModel();
            Assert.IsNotNull(viewModel.GetDataCommand);
            Assert.IsNotNull(viewModel.Input);
        }

        [TestMethod]
        public void SEOViewModel_GetData_Negative()
        {
            SEOViewModel viewModel = new SEOViewModel();
            SEOModel input = new SEOModel();
            input.Keywords = "conveyancing software";
            input.Url = "smokeball.com.au";

            viewModel.GetDataCommand.Execute(input);

            Assert.AreEqual(input.Keywords, viewModel.Input.Keywords);
            Assert.AreEqual(input.Url, viewModel.Input.Url);
            Assert.IsNull(viewModel.Result.Url);
            Assert.IsNull(viewModel.Result.SearchUrls);
            Assert.IsNotNull(viewModel.Result.Information);
        }
    }
}
