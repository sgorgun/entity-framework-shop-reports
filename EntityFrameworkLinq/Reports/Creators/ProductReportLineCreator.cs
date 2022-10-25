using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkLinq.Reports.Creators
{
    public static class ProductReportLineCreator
    {
        public static ProductReportLine Create(DbDataReader reader)
        {
            return new ProductReportLine
            {
                ProductId = (int)reader.GetInt64(0),
                ProductTitle = reader.GetString(1),
                Manufacturer = reader.GetString(2),
                Price = reader.GetDecimal(3),
            };
        }
    }
}
