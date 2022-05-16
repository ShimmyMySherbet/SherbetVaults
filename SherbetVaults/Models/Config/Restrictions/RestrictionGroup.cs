using System.Collections.Generic;
using System.Xml.Serialization;

namespace SherbetVaults.Models.Config.Restrictions
{
    public class RestrictionGroup
    {
        public string GroupID;
        public bool Blacklist = true;
        public string TranslationKey = "Restrictions_Blacklisted";

        [XmlArrayItem(ElementName = "ItemSelector")]
        public List<string> Selectors = new List<string>();
    }
}