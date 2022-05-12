using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<VaultStorage> GetVault(ulong playerID, string vaultID, bool allowCache = true)
        {
            if (EnableCache && allowCache)
            {
                var cached = VaultsCache.GetStorage(playerID, vaultID);
                if (cached != null)
                {
                    return cached;
                }
            }

            var vaultConfig = VaultsPlugin.VaultConfigs.FirstOrDefault
                (x => x.VaultID.Equals(vaultID, StringComparison.InvariantCultureIgnoreCase));

            if (vaultConfig == null)
            {
                return null;
            }

            var items = await Database.VaultItems.OpenVault(playerID, vaultID, vaultConfig);
            var storage = new VaultStorage(items);

            if (EnableCache)
            {
                VaultsCache.SetStorage(playerID, vaultID, storage);
            }

            return storage;
        }
    }
}