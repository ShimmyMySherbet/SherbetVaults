using System.Collections.Generic;
using System.Xml.Serialization;
using Rocket.API;
using SherbetVaults.Models.Config.Restrictions;
using ShimmyMySherbet.MySQL.EF.Models;

namespace SherbetVaults.Models.Config
{
    public class SherbetVaultsConfig : IRocketPluginConfiguration
    {
        public DatabaseSettings DatabaseSettings;
        public bool CacheVaults = true;
        public bool LargestVaultIsDefault = false;
        public string DefaultVault = "default";
        public bool VaultAliasesEnabled = false;

        [XmlArrayItem(ElementName = "Vault")]
        public List<VaultConfig> Vaults = new();

        public RestrictionSettings Restrictions = RestrictionSettings.Default;

        public void LoadDefaults()
        {
            DatabaseSettings = DatabaseSettings.Default;
            Vaults = new List<VaultConfig>() {
                new VaultConfig() { VaultID = "default", Height = 8, Width = 8 },
                new VaultConfig() { VaultID = "vip", Height = 12, Width = 12 }
            };
        }
    }
}