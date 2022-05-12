using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using RocketExtensions.Models;
using SherbetVaults.Database;
using SherbetVaults.Models;
using SherbetVaults.Models.Config;

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
            Database = new DatabaseManager(Configuration.Instance.DatabaseSettings);

            if (!Database.Connect(out var failStr))
            {
                Logger.Log($"Failed to connect to database: {failStr}");
                UnloadPlugin(PluginState.Failure);
                return;
            }
            Database.CheckSchema();
            VaultManager = new VaultManager(this);
            Database.VaultItems.Queue.StartWorker();
            base.LoadPlugin();
        }

        public override void UnloadPlugin(PluginState state = PluginState.Unloaded)
        {
            base.UnloadPlugin(state);
            Database?.VaultItems?.Queue?.Dispose();
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "Vault_Fail_NotFound", "[color=red]Failed to find a vault by that ID[/color]" },
            { "Vault_Fail_NoPermission", "[color=red]You do not have permission to access vault {0}[/color]"},
            { "Vault_Fail_CannotLoad", "[color=red]Vault {0} is currently unavailable[/color]" },
            { "Vaults_No_Vaults", "[color=yellow]You don't have access to any vaults[/color]" },
            { "Vaults_List", "[color=green]Your vaults: {0}[/color]" },

        };

        public VaultConfig GetVaultConfig(string vaultID) =>
            VaultConfigs.FirstOrDefault(x => x.VaultID.Equals(vaultID));


        public VaultConfig[] GetPlayerVaults(LDMPlayer player) =>
            VaultConfigs.Where(x => x.HasPermission(player)).ToArray();
    }
}