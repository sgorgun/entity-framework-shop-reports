using Microsoft.EntityFrameworkCore;
using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services
{
    public class CustomerReportService
    {
        private readonly ShopContext shopContext;

        public CustomerReportService(ShopContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public CustomerSalesRevenueReport GetCustomerSalesRevenueReport()
        {
            var query = this.shopContext.Customers
                .Join(
                    this.shopContext.Orders,
                    c => c.Id,
                    co => co.CustomerId,
                    (c, co) => new
                    {
                        CustomerId = c.Id,
                        PersonFirstName = c.Person.FirstName,
                        PersonLastName = c.Person.LastName,
                        OrderId = co.Id,
                    })
                .Join(
                    this.shopContext.Persons,
                    c => c.CustomerId,
                    p => p.Id,
                    (c, p) => new
                    {
                        Id = c.CustomerId,
                        FirstName = c.PersonFirstName,
                        LastName = c.PersonLastName,
                        OrId = c.OrderId,
                    })
                .Join(
                    this.shopContext.OrderDetails,
                    co => co.OrId,
                    od => od.OrderId,
                    (co, od) => new CustomerSalesRevenueReportLine
                    {
                        CustomerId = co.Id,
                        PersonFirstName = co.FirstName,
                        PersonLastName = co.LastName,
                        SalesRevenue = od.PriceWithDiscount,
                    })
                .GroupBy(c => c.CustomerId)
                .Select(g => new CustomerSalesRevenueReportLine
                {
                    CustomerId = g.Key,
                    PersonFirstName = g.First().PersonFirstName,
                    PersonLastName = g.First().PersonLastName,
                    SalesRevenue = g.Sum(x => x.SalesRevenue),
                })
                .OrderByDescending(c => c.SalesRevenue)
                .ToList();

            var report = new CustomerSalesRevenueReport(query, DateTime.Now);
            return report;
        }
    }
}
