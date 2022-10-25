using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkLinq.Reports;

namespace EntityFrameworkLinq.Tests.Helper
{
    public static class ReportBaseLineConverter
    {
        public static string ConvertToString(this IEnumerable<BaseReportLine> records)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var line in records)
            {
                builder.AppendLine(line.ToString());
            }

            return builder.ToString();
        }
    }
}
