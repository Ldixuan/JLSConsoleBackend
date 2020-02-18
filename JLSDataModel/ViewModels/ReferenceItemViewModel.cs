using JLSDataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JLSDataModel.ViewModels
{
    public class ReferenceItemViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long? ParentId { get; set; }
        public string Value { get; set; }
        public int? Order { get; set; }
        public string Category { get; set; }
        public long ReferenceCategoryId { get; set; }
        public List<ReferenceLabel> Labels { get; set; }
        public string Label { get; set; }
        public string Lang { get; set; }
        public bool? Validity { get; set; }
    }
}
