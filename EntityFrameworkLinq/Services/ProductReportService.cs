using EntityFrameworkLinq.Models;
using EntityFrameworkLinq.Reports;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EntityFrameworkLinq.Services
{
    public class ProductReportService
    {
        private readonly ShopContext shopContext;

        public ProductReportService(ShopContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public ProductCategoryReport GetProductCategories()
        {
            var lines = this.shopContext.Categories
                .Select(c => new ProductCategoryReportLine { CategoryId = c.Id, CategoryName = c.Name })
                .OrderBy(l => l.CategoryName)
                .ToArray();

            return new ProductCategoryReport(lines, DateTime.Now);
        }

        public ProductReport GetProductReport()
        {
            var lines = this.shopContext.Products
                .Include(p => p.Title)
                .Include(p => p.Manufacturer)
                .Select(p => new ProductReportLine { ProductId = p.Id, ProductTitle = p.Title.Title, Manufacturer = p.Manufacturer.Name, Price = p.UnitPrice })
                .OrderByDescending(l => l.ProductTitle)
                .ToArray();

            return new ProductReport(lines, DateTime.Now);
        }

        public FullProductReport GetFullProductReport()
        {
            var lines = this.shopContext.Products
                .Include(p => p.Title)
                .Include(p => p.Manufacturer)
                .Select(p => new FullProductReportLine { ProductId = p.Id, Name = p.Title.Title, CategoryId = p.Title.CategoryId, Manufacturer = p.Manufacturer.Name, Price = p.UnitPrice, Category = p.Title.Category.Name })
                .OrderBy(l => l.Name)
                .ToArray();

            return new FullProductReport(lines, DateTime.Now);
        }

        public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReport()
        {
            //this.shopContext.Database.Log = Console.Out;
            var lines = this.shopContext.OrderDetails
                .Include(p => p.Product)
                .Include(p => p.Product.Title)
                /*.Select(p => new ProductTitleSalesRevenueReportLine {
                    ProductTitleName = p.Product.Title.Title,
                    SalesRevenue = (double)p.PriceWithDiscount,
                    SalesAmount = p.ProductAmount,
                })*/
                .GroupBy(x => x.Product.Title.Title)
                .Select(g => new ProductTitleSalesRevenueReportLine
                {
                    ProductTitleName = g.Key,
                    SalesRevenue = g.Sum(x => (double)x.PriceWithDiscount),
                    SalesAmount = g.Sum(x => x.ProductAmount),
                })
            .OrderByDescending(l => l.SalesRevenue)
            .ToArray();
            return new ProductTitleSalesRevenueReport(lines, DateTime.Now);
        }

        public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReportOld()
        {
            var lines = this.shopContext.OrderDetails
                .Include(p => p.Product)
                .Include(p => p.Product.Title)
                .Select(p => new ProductTitleSalesRevenueReportLine
                {
                    ProductTitleName = p.Product.Title.Title,
                    SalesRevenue = (double)p.PriceWithDiscount,
                    SalesAmount = p.ProductAmount,
                })
                .GroupBy(x => x.ProductTitleName)
                .Select(g => new ProductTitleSalesRevenueReportLine
                {
                    ProductTitleName = g.Key,
                    SalesRevenue = g.Sum(x => x.SalesRevenue),
                    SalesAmount = g.Sum(x => x.SalesAmount),
                })
                .OrderByDescending(l => l.SalesRevenue)
                .ToArray();

            return new ProductTitleSalesRevenueReport(lines, DateTime.Now);
        }
    }
}
