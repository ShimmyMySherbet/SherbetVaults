using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SherbetVaults.Models.Config
{
    [XmlRoot]
    public class VaultConfig
    {
        [XmlAttribute]
        public string VaultID = "default";

        public string Permission = "Vaults.$VaultID";

        public byte Width = 8;

        public byte Height = 8;

        [XmlIgnore]
        public string RealPermission
        {
            get
            {
                var p = Permission;
                var ind = Permission.IndexOf("$vaultid", StringComparison.InvariantCultureIgnoreCase);
                if (ind != -1)
                {
                    p = p.Remove(ind, 8).Insert(ind, VaultID);
                }
                return p;
            }
        }
    }
}