﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLSDataModel.ViewModels
{
    public class ApiResult
    {
        public bool Success { get; set; } = false;
        public string Msg { get; set; } = "";
        public string Type { get; set; } = "";
        public object Data { get; set; } = "";
        public object DataExt { get; set; } = "";
    }
}
