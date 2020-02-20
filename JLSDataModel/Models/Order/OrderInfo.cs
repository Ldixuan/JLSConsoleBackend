using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.Models.Order
{
    public class OrderInfo:BaseObject
    {
        public string OrderReferenceCode { get; set; }


        public string ContactTelephone { get; set; }
        public string PaymentInfo { get; set; }

        public string ClientRemark { get; set; }

        public string AdminRemark { get; set; }

        public float? TotalPrice { get; set; }

        public float? TaxRate { get; set; }

        // 外键: 客户
        public int UserId { get; set; }
        public User.User User { get; set; }

        public long StatusReferenceItemId { get; set; }
        public ReferenceItem StatusReferenceItem { get; set; }

        public long AdressId { get; set; }
        public Adress.Adress Adress { get; set; }
    }
}
