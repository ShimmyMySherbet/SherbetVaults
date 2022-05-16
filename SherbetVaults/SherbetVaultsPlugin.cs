using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using RocketExtensions.Models;
using SDG.Unturned;
using SherbetVaults.Database;
using SherbetVaults.Models;
using SherbetVaults.Models.Config;
using Steamworks;

namespace SherbetVaults
{
    public class SherbetVaultsPlugin : RocketPlugin<SherbetVaultsConfig>
    {
        public DatabaseManager Database { get; private set; }
        public SherbetVaultsConfig Config => Configuration.Instance;
        public List<VaultConfig> VaultConfigs => Config.Vaults;
        public VaultManager VaultManager { get; private set; }

        public override void LoadPlugin()
        {
            Database = new DatabaseManager(Configuration.Instance);

            if (!Database.Connect(out var failStr))
            {
                Logger.Log($"Failed to connect to database: {failStr}");
                UnloadPlugin(PluginState.Failure);
                return;
            }
            Database.CheckSchema();
            Database.VaultItems.Queue.StartWorker();

            VaultManager = new VaultManager(this);
            Provider.onEnemyDisconnected += OnPlayerDisconnect;
            base.LoadPlugin();
        }

        public override void UnloadPlugin(PluginState state = PluginState.Unloaded)
        {
            base.UnloadPlugin(state);
            Provider.onEnemyDisconnected -= OnPlayerDisconnect;
            Database?.VaultItems?.Queue?.Dispose();
        }

        private void OnPlayerDisconnect(SteamPlayer player)
        {
            VaultManager.RemovePlayerCache(player.playerID.steamID.m_SteamID);
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "Vault_Fail_NotFound", "[color=red]Failed to find a vault by that ID[/color]" },
            { "Vault_Fail_NoPermission", "[color=red]You do not have permission to access vault {0}[/color]"},
            { "Vault_Fail_CannotLoad", "[color=red]Vault {0} is currently unavailable[/color]" },
            { "Vaults_No_Vaults", "[color=yellow]You don't have access to any vaults[/color]" },
            { "Vaults_List", "[color=green]Your vaults: {0}[/color]" },
            { "WipeVault_Wiped", "[color=green]Wiped {0} items from {1}'s vault {2}[/color]" },
            { "VaultAliases_MaxReached", "[color=red]Max vault aliases reached[/color]" },
            { "VaultAliases_Set", "[color=cyan]Vault alias created: {0}➔{1}[/color]" },
            { "VaultAliases_Removed", "[color=cyan]Removed alias {0}[/color]" },
            { "VaultAliases_Remove_NotFound", "[color=cyan]No alias by that name found[/color]" },
            { "VaultAliases_List", "[color=cyan]Aliases: {1}[/color]" },
        };

        public VaultConfig GetDefaultVault(LDMPlayer player, out EVaultAvailability availability, string vaultID = null)
        {
            if (VaultConfigs.Count == 0)
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
                if (Config.LargestVaultIsDefault)
                {
                    availability = EVaultAvailability.VaultAvailable;
                    return LargetVault(player);
                }

                vaultDefaulted = true;
                vaultID = Config.DefaultVault;
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

        /// <summary>
        /// Gets a player's default vault, with alias checking
        /// </summary>
        public async Task<(VaultConfig vault, EVaultAvailability availability)> GetPlayerVault(LDMPlayer player, string vaultID = null)
        {
            if (Config.VaultAliasesEnabled 
                && player.UnturnedPlayer.HasPermission("SherbetVaults.Vault.Alias") 
                && Database.Aliases != null
                && !string.IsNullOrEmpty(vaultID))
            {
                var alias = await Database.Aliases.GetAliasAsync(player.PlayerID, vaultID);
                if (alias != null)
                {
                    vaultID = alias;
                }
            }
            var config = GetDefaultVault(player, out var availability, vaultID);
            return (config, availability);
        }

        public VaultConfig GetVaultConfig(string vaultID) =>
            VaultConfigs.FirstOrDefault(x => x.VaultID.Equals(vaultID));

        public VaultConfig LargetVault(LDMPlayer player) =>
            GetPlayerVaults(player)
            .OrderByDescending(x => x.Width * x.Height)
            .FirstOrDefault();

        public VaultConfig[] GetPlayerVaults(LDMPlayer player) =>
            VaultConfigs.Where(x => x.HasPermission(player)).ToArray();
    }
}