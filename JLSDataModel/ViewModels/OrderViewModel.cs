using JLSDataModel.Models.Adress;
using JLSDataModel.Models.User;
using JLSDataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.ViewModels
{
    public class OrderViewModel
    {

        public string OrderReferenceCode { get; set; }

        public string PaymentInfo { get; set; }

        public string ClientRemark { get; set; }

        public string AdminRemark { get; set; }

        public float? TotalPrice { get; set; }

        public float? TaxRate { get; set; }

        // Foreign key 
        public UserViewModel User { get; set; }

        public ReferenceItem StatusReferenceItem { get; set; }

        public string StatusLabel { get; set; }

        public Adress ShippingAdress { get; set; }

        public Adress FacturationAdress { get; set; }

        public List<OrderProductViewModel> Products { get; set; }
    }
}
