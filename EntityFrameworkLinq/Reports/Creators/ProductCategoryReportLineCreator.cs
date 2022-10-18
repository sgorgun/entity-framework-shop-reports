using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkLinq.Reports.Creators
{
    public class ProductCategoryReportLineCreator
    {
        public static ProductCategoryReportLine Create(DbDataReader reader)
        {
            return new ProductCategoryReportLine
            {
                CategoryId = (int)reader.GetInt64(0),
                CategoryName = reader.GetString(1),
            };
        }
    }
}
