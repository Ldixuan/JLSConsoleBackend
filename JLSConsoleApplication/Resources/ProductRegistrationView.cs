using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLSConsoleApplication.Resources
{
    public class ProductRegistrationView
    {
        public string Name { get; set; }
        public string ProductReferenceCode { get; set; }
        public float? Price { get; set; }
        public float? TaxRate { get; set; }

        public int? QuantityPerBox { get; set; }
        public int? MinQuantity { get; set; }

        public long Category { get; set; }


        /* 不确定需要询问客户 */
        public string Color { get; set; }

        public string Material { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }
    }
}
