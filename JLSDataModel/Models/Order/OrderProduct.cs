using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.Models.Order
{
    public class OrderProduct:BaseObject
    {
        public int Quantity { get; set; }
        public long OrderId { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public long ReferenceId{ get; set; }
        public ReferenceItem Reference { get; set; }
    }
}
