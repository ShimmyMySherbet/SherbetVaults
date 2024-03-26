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

        [XmlAttribute]
        public byte Width = 8;

        [XmlAttribute]
        public byte Height = 8;

        [XmlIgnore]
        public string FormattedPermission => $"Vaults.{VaultID}";

        public bool HasPermission(LDMPlayer player)
        {
            return player.UnturnedPlayer.HasPermission(FormattedPermission);
        }
    }
}