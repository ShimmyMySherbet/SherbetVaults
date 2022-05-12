using System.Collections.Generic;
using System.Xml.Serialization;
using Rocket.API;
using ShimmyMySherbet.MySQL.EF.Models;

namespace SherbetVaults.Models.Config
{
    public class SherbetVaultsConfig : IRocketPluginConfiguration
    {
        public DatabaseSettings DatabaseSettings;

        [XmlArrayItem(ElementName = "Vault")]
        public List<VaultConfig> Vaults = new();

        public bool CacheVaults = true;

        public void LoadDefaults()
        {
            DatabaseSettings = DatabaseSettings.Default;
            Vaults = new List<VaultConfig>() { new VaultConfig() { VaultID = "default" } };
        }
    }
}