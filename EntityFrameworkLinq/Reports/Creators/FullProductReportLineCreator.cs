using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkLinq.Reports.Creators
{
    public class FullProductReportLineCreator
    {
        public static FullProductReportLine Create(DbDataReader reader)
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
        }
    }
}
