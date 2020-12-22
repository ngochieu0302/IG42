﻿using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CountryItem : BaseSimple
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? Show { get; set; }
        public string Description { get; set; }
        public int TotalProducts { get; set; }
    }
    public class ModelCountryItem : BaseModelSimple
    {
        public string LstInt { get; set; } 
        public IEnumerable<CountryItem> ListItem { get; set; }
    }
}
