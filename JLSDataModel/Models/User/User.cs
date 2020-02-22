using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.Models.User
{
    public class User: IdentityUser<int>
    {
        public User()
        {
            this.CreatedOn = DateTime.Now;
        }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Siret { get; set; }
        public string EntrepriseName { get; set; }

        public long DefaultShippingAdressId { get; set; }
        public Adress.Adress DefaultShippingAdress { get; set; }

        public long FacturationAdressId { get; set; }
        public Adress.Adress FacturationAdress { get; set; }
    }
}
