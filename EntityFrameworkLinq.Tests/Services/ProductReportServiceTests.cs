using System.Data.Common;
using System.Reflection;
using System.Resources;
using EntityFrameworkLinq.Reports;
using EntityFrameworkLinq.Reports.Creators;
using EntityFrameworkLinq.Services;
using EntityFrameworkLinq.Tests.Helper;
using NUnit.Framework;

namespace EntityFrameworkLinq.Tests.Services
{
    [TestFixture]
    public sealed class ProductReportServiceTests : IDisposable
    {
        private const int MethodsCount = 4;
        private static ShopContextFactory factory;
        private static ProductReportService service;
        private static List<(string methodName, IReadOnlyList<BaseReportLine> actual, IReadOnlyList<BaseReportLine> expected)> testContainer;

        // [SetUp]
        [OneTimeSetUp]
        public void SetUp()
        {
            factory = new ShopContextFactory();
            service = new ProductReportService(factory.CreateContext());
            testContainer = new List<(string methodName, IReadOnlyList<BaseReportLine> actual, IReadOnlyList<BaseReportLine> expected)>();
            testContainer.Add(("GetReportCategoriesReport", service.GetProductCategories().Lines, factory.ReadEntities(GetSqlQuery("GetReportCategoriesReport"), ProductCategoryReportLineCreator.Create)));
            testContainer.Add(("GetProductReport", service.GetProductReport().Lines, factory.ReadEntities(GetSqlQuery("GetProductReport"), ProductReportLineCreator.Create)));
            testContainer.Add(("GetFullProductReport", service.GetFullProductReport().Lines, factory.ReadEntities(GetSqlQuery("GetFullProductReport"), FullProductReportLineCreator.Create)));
            testContainer.Add(("GetProductTitleSalesRevenueReport", service.GetProductTitleSalesRevenueReport().Lines, factory.ReadEntities(GetSqlQuery("GetProductTitleSalesRevenueReport"), ProductTitleSalesRevenueReportLineCreator.Create)));
        }

        [Test]
        public void ReportService_ReturnsCorrectReportLines([Range(1, MethodsCount)] int index)
        {
            --index;
            var rec = testContainer[index];
            var errorMsg = $"Class: ProductReportService \n  Method {rec.methodName} returns report";
            var rowNumberErrorMsg = errorMsg + $" that must conteins {rec.expected.Count} lines, but {rec.actual.Count} lines given!";
            var dataSetErrorMsg = errorMsg + $" that actual data \n{rec.actual.ConvertToString()}   are not equals to expected data \n{rec.expected.ConvertToString()}";
            //Assert.AreEqual(rec.expected.Count, rec.actual.Count, rowNumberErrorMsg);
            Assert.AreEqual(rec.expected, rec.actual, dataSetErrorMsg);
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
    }
}
