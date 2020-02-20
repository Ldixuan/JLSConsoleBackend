using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.Models.Product
{
    public class Comment:BaseObject
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
