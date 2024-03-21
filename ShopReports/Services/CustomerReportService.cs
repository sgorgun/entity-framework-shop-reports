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
                        CustomerId = c.CustomerId,
                        PersonFirstName = c.PersonFirstName,
                        PersonLastName = c.PersonFirstName,
                    })
                .Join(
                    this.shopContext.OrderDetails,
                    co => co.CustomerId,
                    od => od.OrderId,
                    (co, od) => new CustomerSalesRevenueReportLine
                    {
                        CustomerId = co.CustomerId,
                        PersonFirstName = co.PersonFirstName,
                        PersonLastName = co.PersonLastName,
                        SalesRevenue = od.PriceWithDiscount,
                    }).ToList();

            var report = new CustomerSalesRevenueReport(query, DateTime.Now);
            return report;
        }
    }
}
