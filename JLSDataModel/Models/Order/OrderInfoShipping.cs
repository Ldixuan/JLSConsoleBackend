using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.Models.Order
{
    public class OrderInfoShipping:BaseObject
    {
        public string ShippingTelephone { get; set; }
        public string ShippingCity { get; set; }

        public string ShippingPostCode { get; set; }

        public string ShippingAdress { get; set; }
        public string ShippingAdressDetail { get; set; }

        public long CountryReferenceItemId { get; set; }
        public ReferenceItem CountryReferenceItem { get; set; }

        public long OrderInfoId { get; set; }
        public OrderInfo OrderInfo { get; set; }
    }
}
