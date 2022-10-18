using System.Data.Common;
using System.Reflection;
using System.Resources;
using EntityFrameworkLinq.Reports;
using EntityFrameworkLinq.Services;
using NUnit.Framework;

namespace EntityFrameworkLinq.Tests.Services
{
    [TestFixture]
    public sealed class ProductReportServiceTests : IDisposable
    {
        private ShopContextFactory? factory;
        private ProductReportService? service;

        [SetUp]
        public void SetUp()
        {
            this.factory = new ShopContextFactory();
            this.service = new ProductReportService(this.factory.CreateContext());
        }

        [Test]
        public void GetReportCategories_ReturnsReport()
        {
            // Act
            ProductCategoryReport report = this.service!.GetProductCategories();

            // Assert
            this.AssertThatReportHasLines("GetReportCategoriesReport", report.Lines, reader =>
            {
                return new ProductCategoryReportLine
                {
                    CategoryId = (int)reader.GetInt64(0),
                    CategoryName = reader.GetString(1),
                };
            });
        }

        [Test]
        public void GetProductReport_ReturnsReport()
        {
            // Act
            ProductReport report = this.service!.GetProductReport();

            // Assert
            this.AssertThatReportHasLines("GetProductReport", report.Lines, reader =>
            {
                return new ProductReportLine
                {
                    ProductId = (int)reader.GetInt64(0),
                    ProductTitle = reader.GetString(1),
                    Manufacturer = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                };
            });
        }

        [Test]
        public void GetFullProductReport_ReturnsReport()
        {
            // Act
            FullProductReport report = this.service!.GetFullProductReport();

            // Assert
            this.AssertThatReportHasLines("GetFullProductReport", report.Lines, reader =>
            {
                return new FullProductReportLine
                {
                    ProductId = (int)reader.GetInt64(0),
                    Name = reader.GetString(1),
                    CategoryId = (int)reader.GetInt64(2),
                    Manufacturer = reader.GetString(3),
                    Price = reader.GetDecimal(4),
                    Category = reader.GetString(5),
                };
            });
        }

        public void Dispose()
        {
            this.factory!.Dispose();
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
            var list = this.factory!.ReadEntities(sqlQuery!, builder);

            Assert.That(reportItems.Count, Is.EqualTo(list.Count));

            for (int i = 0; i < reportItems.Count; i++)
            {
                Assert.That(reportItems[i], Is.EqualTo(list[i]));
            }
        }
    }
}
