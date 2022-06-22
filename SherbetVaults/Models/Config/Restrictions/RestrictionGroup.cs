using System.Collections.Generic;
using System.Xml.Serialization;

namespace SherbetVaults.Models.Config.Restrictions
{
    public class RestrictionGroup
    {
        [XmlAttribute, XmlElement(ElementName = "ID")]
        public string GroupID = "Group1";

        [XmlAttribute]
        public int Weight = 1;

        [XmlAttribute]
        public bool Blacklist = true;

        public string TranslationKey = "Restrictions_Blacklisted";

        [XmlArrayItem(ElementName = "ItemSelector")]
        public List<string> Selectors = new List<string>();
    }
}