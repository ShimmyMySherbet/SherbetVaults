using System.Threading.Tasks;
using SherbetVaults.Database;
using SherbetVaults.Models.Caching;
using SherbetVaults.Models.Data;

namespace SherbetVaults.Models
{
    public class VaultManager
    {
        public SherbetVaultsPlugin VaultsPlugin { get; }
        public VaultCache VaultsCache { get; }
        public DatabaseManager Database => VaultsPlugin.Database;
        public bool EnableCache => VaultsPlugin.Config.CacheVaults;

        public VaultManager(SherbetVaultsPlugin vaultsPlugin)
        {
            VaultsPlugin = vaultsPlugin;
            VaultsCache = new VaultCache();
        }

        public async Task<VaultItems> GetVault(ulong playerID, string vaultID, bool allowCache = true)
        {
            //if (EnableCache && allowCache)
            //{
            //    var cached = VaultsCache.GetStorage(playerID, vaultID);
            //    if (cached != null)
            //    {
            //        return cached;
            //    }
            //}

            var vaultConfig = VaultsPlugin.VaultSelector.GetVaultConfig(vaultID);

            if (vaultConfig == null)
            {
                return null;
            }

            var items = await Database.VaultItems.OpenVault(playerID, vaultID, vaultConfig);

            if (EnableCache)
            {
                VaultsCache.SetStorage(playerID, vaultID, items);
            }
            return items;
        }

        public void RemovePlayerCache(ulong playerID)
        {
            VaultsCache.Clear(playerID);
        }
    }
}