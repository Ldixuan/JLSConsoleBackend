using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.Models.Adress
{
    public class Adress:BaseObject
    {
        public string ContactTelephone { get; set; }

        public string ContactFax { get; set; }

        public string ContactLastName { get; set; }

        public string ContactFirstName { get; set; }

        public string ZipCode { get; set; }

        public string FirstLineAddress { get; set; }

        public string SecondLineAddress { get; set; }
        public string City { get; set; }
        public string Provence { get; set; }

        public string Country { get; set; }
    }
}
