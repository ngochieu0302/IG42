using System;
using System.Collections.Generic;
using FDI.Base;

namespace FDI.Simple
{
    [Serializable]
    public class ModuleSettingItem : BaseSimple
    {
        public int? ModuleId { get; set; }
        public int? AgencyID { get; set; }
        public string Value { get; set; }
        public string LanguageId { get; set; }
    }

    public class ModelModuleSettingItem : BaseModelSimple
    {
        public IEnumerable<ModuleSettingItem> ListItem { get; set; }
        public string Key { get; set; }
    }
}

