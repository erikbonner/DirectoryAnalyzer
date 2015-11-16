using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesktopAnalyzer;
using System.Collections.Generic;
using DesktopAnalyzer.BusinessLogic;
using System.Threading;

namespace UnitTests
{
    [TestClass]
    public class TestDirectoryAnalyzer
    {
        private static TestContext _testContext;


        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        public void TestAnalyze()
        {

            string testPath = @"C:\temp\test";
            DirectoryAnalyzer analyzer = new DirectoryAnalyzer();
            CancellationTokenSource ctSource = new CancellationTokenSource();
             
            IList< DirectoryAnalyzer.DirectoryEntry> results = analyzer.Analyze(testPath, new Progress<int>(), ctSource.Token);

            Assert.AreEqual(results.Count, 3);
            Assert.AreEqual(results[0].Name.ToLower(), @"subdir1");
            Assert.AreEqual(results[1].Name.ToLower(), @"subdir3");
            Assert.AreEqual(results[2].Name.ToLower(), @"subdir2");

        }
    }
}
