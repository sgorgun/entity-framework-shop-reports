using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services
{
    public class ProductReportService
    {
        private readonly ShopContext shopContext;

        public ProductReportService(ShopContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public ProductCategoryReport GetProductCategoryReport()
        {
            var query = this.shopContext.Categories
                .OrderBy(pc => pc.Name)
                .Select(pc => new ProductCategoryReportLine
                {
                    CategoryId = pc.Id,
                    CategoryName = pc.Name,
                })
                .ToList();

            var report = new ProductCategoryReport(query, DateTime.Now);

            return report;
        }

        public ProductReport GetProductReport()
        {
            var query = this.shopContext.Titles
                .OrderByDescending(p => p.Title)
                .Join(
                    this.shopContext.Products,
                    pt => pt.Id,
                    p => p.TitleId,
                    (pt, p) => new
                    {
                        ProductId = p.Id,
                        ProductTitle = pt.Title,
                        Price = p.UnitPrice,
                        ProductManufacturierId = p.ManufacturerId,
                    })
                .Join(
                    this.shopContext.Manufacturers,
                    p => p.ProductManufacturierId,
                    m => m.Id,
                    (p, m) => new ProductReportLine
                    {
                        ProductId = p.ProductId,
                        ProductTitle = p.ProductTitle,
                        Price = p.Price,
                        Manufacturer = m.Name,
                    })
                .ToList();

            var report = new ProductReport(query, DateTime.Now);
            return report;
        }

        public FullProductReport GetFullProductReport()
        {
            var query = this.shopContext.Categories
                .Join(
                    this.shopContext.Titles,
                    pc => pc.Id,
                    pt => pt.CategoryId,
                    (pc, pt) => new
                    {
                        TitleId = pt.Id,
                        CategoryId = pc.Id,
                        CategoryName = pc.Name,
                        ProductTitle = pt.Title,
                    })
                .OrderBy(p => p.ProductTitle)
                .Join(
                    this.shopContext.Products,
                    p => p.TitleId,
                    p => p.TitleId,
                    (pt, p) => new
                    {
                        ProductId = p.Id,
                        Title = pt.ProductTitle,
                        Price = p.UnitPrice,
                        ProductManufacturierId = p.ManufacturerId,
                        CatId = pt.CategoryId,
                        Category = pt.CategoryName,
                    })
                .Join(
                    this.shopContext.Manufacturers,
                    p => p.ProductManufacturierId,
                    m => m.Id,
                    (p, m) => new FullProductReportLine
                    {
                        ProductId = p.ProductId,
                        Name = p.Title,
                        CategoryId = p.CatId,
                        Category = p.Category,
                        Manufacturer = m.Name,
                        Price = p.Price,
                    })
                .ToList();

            var report = new FullProductReport(query, DateTime.Now);
            return report;
        }

        public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReport()
        {
            var query = this.shopContext.Titles
                .Join(
                    this.shopContext.Products,
                    pt => pt.Id,
                    sp => sp.TitleId,
                    (pt, sp) => new
                    {
                        ProductTitleName = pt.Title,
                        ProductID = sp.Id,
                    })
                .Join(
                    this.shopContext.OrderDetails,
                    sp => sp.ProductID,
                    od => od.ProductId,
                    (sp, od) => new ProductTitleSalesRevenueReportLine
                    {
                        ProductTitleName = sp.ProductTitleName,
                        SalesRevenue = od.PriceWithDiscount,
                        SalesAmount = od.ProductAmount,
                    })
                .GroupBy(od => od.ProductTitleName)
                .Select(g => new ProductTitleSalesRevenueReportLine
                {
                    ProductTitleName = g.Key,
                    SalesRevenue = g.Sum(od => od.SalesRevenue),
                    SalesAmount = g.Sum(od => od.SalesAmount),
                })
                .OrderByDescending(od => od.SalesRevenue)
                .ToList();

            var report = new ProductTitleSalesRevenueReport(query, DateTime.Now);
            return report;
        }
    }
}
