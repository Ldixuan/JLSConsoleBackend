using System;

namespace JLSDataModel.Models.Product
{
    public class Product:BaseObject
    {
        public float? Price { get; set; }
        public int? QuantityPerBox { get; set; }
        public int? MinQuantity { get; set; }


        /* 不确定需要询问客户 */
        public string Color { get; set; }

        public string Material { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }
        /* 不确定需要询问客户 */

        public long ReferenceItemId { get; set; }
        public ReferenceItem ReferenceItem { get; set; }
    }
}
