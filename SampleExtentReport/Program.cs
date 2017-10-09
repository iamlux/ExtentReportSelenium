using NUnit.Framework;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleExtentReport
{
    [TestFixture]
    public class Program
    {
        public ExtentReports extent;
        public ExtentTest test;

        [OneTimeSetUp]
        public void StartReport()
        {
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath; // project path of your solution

            string reportPath = projectPath + "Reports\\TestReport.html";

            // true if you want to append data to the report.  Replace existing report with new report.  False to create new report each time
            extent = new ExtentReports(reportPath, false);
            extent.AddSystemInfo("Host Name", "localhost")
                .AddSystemInfo("Environment", "QA")
                .AddSystemInfo("User Name", "Lux");

            extent.LoadConfig(projectPath + "extent-config.xml");

        }

        [Test]
        public void DemoReportPass()
        {
            test = extent.StartTest("DemoReportPass");
            Assert.IsTrue(true);
            test.Log(LogStatus.Pass, "Assert Pass as consition is true");

        }

        [Test]
        public void DemoReportFail()
        {
            test = extent.StartTest("DemoReportPass");
            Assert.IsTrue(false);
            test.Log(LogStatus.Fail, "Assert Pass as condition is false");

        }

        [TearDown]
        public void GetResult()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "<pre>" + TestContext.CurrentContext.Result.StackTrace + "</pre>";
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                test.Log(LogStatus.Fail, stackTrace + errorMessage);
            }
            extent.EndTest(test);

        }

        [OneTimeTearDown]
        public void EndReport()
        {
            extent.Flush();
            extent.Close();
        }
    }
}
