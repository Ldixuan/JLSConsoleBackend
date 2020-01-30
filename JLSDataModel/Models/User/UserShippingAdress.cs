using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.Models.User
{
    public class UserShippingAdress:BaseObject
    {
        public string ShippingName { get; set; }
        public string ShippingAdress { get; set; }
        public string ShippingTelephone { get; set; }
        public string ShippingContact { get; set; }
        public int? Order { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
