using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkLinq.Reports.Creators
{
    public static class ProductTitleSalesRevenueReportLineCreator
    {
        public static ProductTitleSalesRevenueReportLine Create(DbDataReader reader)
        {
            return new ProductTitleSalesRevenueReportLine
            {
                ProductTitleName = reader.GetString(0),
                SalesRevenue = reader.GetDouble(1),
                SalesAmount = (int)reader.GetInt64(2),
            };
        }
    }
}
