using System;
using System.Linq;
using System.Threading.Tasks;
using Rocket.Core.Logging;
using SDG.Unturned;
using SherbetVaults.Database;
using SherbetVaults.Models.Caching;

namespace SherbetVaults.Models
{
    public class VaultManager
    {
        public SherbetVaultsPlugin VaultsPlugin { get; }
        public VaultCache VaultsCache { get; }
        public DatabaseManager Database => VaultsPlugin.Database;
        public bool EnableCache { get; } = true;

        public VaultManager(SherbetVaultsPlugin vaultsPlugin)
        {
            VaultsPlugin = vaultsPlugin;
            VaultsCache = new VaultCache();
        }

        public async Task<VaultItems> GetVault(ulong playerID, string vaultID, bool allowCache = true)
        {
            if (EnableCache && allowCache)
            {
                var cached = VaultsCache.GetStorage(playerID, vaultID);
                if (cached != null)
                {
                    return cached;
                }
            }

            var vaultConfig = VaultsPlugin.GetVaultConfig(vaultID);
            if (vaultConfig == null)
            {
                Logger.LogWarning($"Failed to find vault config for {vaultID}");
                return null;
            }

            var items = await Database.VaultItems.OpenVault(playerID, vaultID, vaultConfig);

            if (EnableCache)
            {
                VaultsCache.SetStorage(playerID, vaultID, items);
            }
            if (items == null)
                Logger.LogError("Bad Vault Error");
            return items;
        }
    }
}