using System.Diagnostics;
using System.Xml.Linq;
using EntityFrameworkLinq.Models;

namespace EntityFrameworkLinq.Reports
{
    public class ProductTitleSalesRevenueReportLine : BaseReportLine
    {
        public string ProductTitleName { get; init; }

        public double SalesRevenue { get; init; }

        public int SalesAmount { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is not ProductTitleSalesRevenueReportLine)
            {
                return false;
            }

            var line = (ProductTitleSalesRevenueReportLine)obj;

            return this.ProductTitleName == line.ProductTitleName && this.SalesRevenue == line.SalesRevenue && this.SalesAmount == line.SalesAmount;
        }

        public override int GetHashCode()
        {
            return this.ProductTitleName.GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.ProductTitleName}|{this.SalesRevenue}|{this.SalesAmount}";
        }
    }
}
