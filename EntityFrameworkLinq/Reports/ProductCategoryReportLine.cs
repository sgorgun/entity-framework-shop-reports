using System.Diagnostics;
using System.Xml.Linq;
using EntityFrameworkLinq.Models;

namespace EntityFrameworkLinq.Reports
{
    public class ProductCategoryReportLine : BaseReportLine
    {
        public int CategoryId { get; init; }

        public string CategoryName { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is not ProductCategoryReportLine)
            {
                return false;
            }

            var line = (ProductCategoryReportLine)obj;

            return this.CategoryId == line.CategoryId && this.CategoryName == line.CategoryName;
        }

        public override int GetHashCode()
        {
            return this.CategoryId;
        }

        public override string ToString()
        {
            return $"{this.CategoryId}|{this.CategoryName}";
        }
    }
}
