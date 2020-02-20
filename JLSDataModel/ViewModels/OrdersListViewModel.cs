using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.ViewModels
{
    public class OrdersListViewModel
    {
        public long Id { get; set; }
        public string OrderReferenceCode { get; set; }

        public string UserName { get; set; }
        public string EntrepriseName { get; set; }

        public float? TotalPrice { get; set; }

        public string StatusReferenceItemLabel { get; set; }

        public DateTime? Date { get; set; }
    }
}
