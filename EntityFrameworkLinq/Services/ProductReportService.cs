using EntityFrameworkLinq.Models;
using EntityFrameworkLinq.Reports;
using Microsoft.EntityFrameworkCore;

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
    }
}
