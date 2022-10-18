using System.Data.Common;
using System.Reflection;
using System.Resources;
using EntityFrameworkLinq.Reports;
using EntityFrameworkLinq.Reports.Creators;
using EntityFrameworkLinq.Services;
using NUnit.Framework;

namespace EntityFrameworkLinq.Tests.Services
{
    [TestFixture]
    public sealed class ProductReportServiceTests : IDisposable
    {
        private const int methodsCount = 3;
        private static ShopContextFactory factory;
        private static ProductReportService service;
        private static List<(string methodName, IReadOnlyList<BaseReportLine> actual, IReadOnlyList<BaseReportLine> expected)> testContainer;

        [OneTimeSetUp]
        //[SetUp]
        public void SetUp()
        {
            factory = new ShopContextFactory();
            service = new ProductReportService(factory.CreateContext());
            testContainer = new List<(string methodName, IReadOnlyList<BaseReportLine> actual, IReadOnlyList<BaseReportLine> expected)>();
            testContainer.Add(("GetReportCategoriesReport", service.GetProductCategories().Lines, factory.ReadEntities(GetSqlQuery("GetReportCategoriesReport"), ProductCategoryReportLineCreator.Create)));
            testContainer.Add(("GetProductReport", service.GetProductReport().Lines, factory!.ReadEntities(GetSqlQuery("GetProductReport"), ProductReportLineCreator.Create)));
            testContainer.Add(("GetFullProductReport", service.GetFullProductReport().Lines, factory.ReadEntities(GetSqlQuery("GetFullProductReport"), FullProductReportLineCreator.Create)));
            //testContainer.Add(("GetReportCategoriesReport", service.GetProductCategories().Lines, factory.ReadEntities(GetSqlQuery("GetReportCategoriesReport"), ProductCategoryReportLineCreator.Create)));
        }

        [Test]
        public void ReportService_ReturnsCorrectReportLines([Range(1, methodsCount)] int index)
        {
            var rec = testContainer[--index];
            Assert.AreEqual(rec.actual, rec.expected, $"{rec.methodName}");
        }

        public void Dispose()
        {
            factory!.Dispose();
        }

        private static string? GetSqlQuery(string sqlQueryName)
        {
            const string resourceManifestName = "EntityFrameworkLinq.Tests.Properties.Resources.resources";

            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceManifestName);
            ResourceSet set = new ResourceSet(resourceStream!);
            return set.GetString(sqlQueryName);
        }

        private void AssertThatReportHasLines<T>(string sqlQueryName, IReadOnlyList<T> reportItems, Func<DbDataReader, T> builder)
        {
            string? sqlQuery = GetSqlQuery(sqlQueryName);
            var list = factory!.ReadEntities(sqlQuery!, builder);

            Assert.That(reportItems.Count, Is.EqualTo(list.Count));

            for (int i = 0; i < reportItems.Count; i++)
            {
                Assert.That(reportItems[i], Is.EqualTo(list[i]));
            }
        }
    }
}
