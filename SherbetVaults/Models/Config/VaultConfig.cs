using System;
using System.Xml.Serialization;
using Rocket.API;
using RocketExtensions.Models;

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
        public string FormattedPermission
        {
            get
            {
                var index = Permission.IndexOf("$vaultid", StringComparison.InvariantCultureIgnoreCase);
                if (index != -1)
                {
                    return Permission.Remove(index, 8).Insert(index, VaultID);
                }
                return Permission;
            }
        }

        public bool HasPermission(LDMPlayer player)
            => player.UnturnedPlayer.HasPermission(FormattedPermission);
    }
}