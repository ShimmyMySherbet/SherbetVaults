using System;
using System.Linq;
using System.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using SherbetVaults.Models.Config;
using SherbetVaults.Models.Enums;

namespace SherbetVaults.Models.Utility
{
    public class VaultSelector
    {
        public SherbetVaultsPlugin Plugin { get; }

        public VaultSelector(SherbetVaultsPlugin plugin)
        {
            Plugin = plugin;
        }

        private VaultConfig GetDefaultVault(LDMPlayer player, out EVaultAvailability availability, string vaultID)
        {
            if (Plugin.VaultConfigs.Count == 0)
            {
                availability = EVaultAvailability.NoVaults;
                return null;
            }

            var permissedVaults = GetPlayerVaults(player);

            if (permissedVaults.Length == 0)
            {
                availability = EVaultAvailability.NoAllowedVaults;
                return null;
            }

            var vaultDefaulted = false;

            if (string.IsNullOrWhiteSpace(vaultID))
            {
                if (Plugin.Config.LargestVaultIsDefault)
                {
                    availability = EVaultAvailability.VaultAvailable;
                    return GetLargetVault(player);
                }

                vaultDefaulted = true;
                vaultID = Plugin.Config.DefaultVault;
            }

            var vault = GetVaultConfig(vaultID);

            if (vault == null)
            {
                if (vaultDefaulted)
                {
                    availability = EVaultAvailability.VaultAvailable;
                    return permissedVaults.FirstOrDefault();
                }
                else
                {
                    availability = EVaultAvailability.BadVaultID;
                    return null;
                }
            }

            if (vault.HasPermission(player))
            {
                availability = EVaultAvailability.VaultAvailable;
                return vault;
            }
            else
            {
                availability = EVaultAvailability.NotAllowed;
                return null;
            }
        }

        public async Task<(VaultConfig vault, EVaultAvailability availability)> GetVault(LDMPlayer player, string vaultID, bool allowAliases = true)
        {
            if (allowAliases
                && Plugin.Config.VaultAliasesEnabled
                && player.UnturnedPlayer.HasPermission("SherbetVaults.Vault.Alias")
                && Plugin.Database.Aliases != null
                && !string.IsNullOrWhiteSpace(vaultID))
            {
                var alias = await Plugin.Database.Aliases.GetAliasAsync(player.PlayerID, vaultID);
                if (alias != null)
                {
                    vaultID = alias;
                }
            }
            var config = GetDefaultVault(player, out var availability, vaultID);
            return (config, availability);
        }

        public VaultConfig GetVaultConfig(string vaultID) =>
            Plugin.VaultConfigs.FirstOrDefault(x => x.VaultID.Equals(vaultID, StringComparison.InvariantCultureIgnoreCase));

        private VaultConfig GetLargetVault(LDMPlayer player) =>
            GetPlayerVaults(player)
            .OrderByDescending(x => x.Width * x.Height)
            .FirstOrDefault();

        public VaultConfig[] GetPlayerVaults(LDMPlayer player) =>
            Plugin.VaultConfigs.Where(x => x.HasPermission(player)).ToArray();
    }
}