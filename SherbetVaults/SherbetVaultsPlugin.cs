using System.Collections.Generic;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using SherbetVaults.Database;
using SherbetVaults.Models;
using SherbetVaults.Models.Config;
using SherbetVaults.Models.Restrictions;
using SherbetVaults.Models.Utility;

namespace SherbetVaults
{
    public partial class SherbetVaultsPlugin : RocketPlugin<SherbetVaultsConfig>
    {
        #region "Properties"

        public DatabaseManager Database { get; private set; }
        public VaultManager VaultManager { get; private set; }
        public VaultSelector VaultSelector { get; private set; }
        public RestrictionBuilder RestrictionBuilder { get; private set; }
        public RestrictionTool RestrictionTool { get; private set; }
        public ItemTableTool ItemTable { get; } = new ItemTableTool();
        public SherbetVaultsConfig Config => Configuration.Instance;
        public List<VaultConfig> VaultConfigs => Config.Vaults;
        public List<VaultRestrictionGroup> RestrictionGroups { get; private set; } = new List<VaultRestrictionGroup>();



        #endregion "Properties"

        public override void LoadPlugin()
        {
            var version = typeof(SherbetVaultsPlugin).Assembly.GetName().Version;
            Logger.Log($"Loading Sherbet Vaults v{version}...");
            VaultManager = new VaultManager(this);
            RestrictionBuilder = new RestrictionBuilder(this);
            RestrictionTool = new RestrictionTool(this);
            Database = new DatabaseManager(this);

            if (!Database.Connect(out var errorMessage))
            {
                Logger.Log($"Failed to connect to database: {errorMessage}");
                UnloadPlugin(PluginState.Failure);
                return;
            }

            Database.CheckSchema();
            Database.InitQueue();

            Logger.Log("Loading restriction settings...");
            RestrictionGroups = RestrictionBuilder.BuildGroups(Config.Restrictions, out var errors);

            if (errors > 0)
            {
                Logger.Log($"Loaded {RestrictionGroups.Count} Restriction Groups with no errors.");
            }
            else
            {
                Logger.LogWarning($"Loaded {RestrictionGroups.Count} Restriction Groups with {errors} error/s.");
            }

            Provider.onEnemyDisconnected += OnPlayerDisconnect;
            Level.onLevelLoaded += OnLevelLoaded;

            base.LoadPlugin();
            Logger.Log($"Sherbet Vaults v{version} by ShimmyMySherbet#5694 Loaded!");
        }

        public override void UnloadPlugin(PluginState state = PluginState.Unloaded)
        {
            Provider.onEnemyDisconnected -= OnPlayerDisconnect;
            Level.onLevelLoaded -= OnLevelLoaded;

            Database.Dispose();

            base.UnloadPlugin(state);
        }

        private void OnLevelLoaded(int lvl)
        {
            ItemTable.ReInit();
        }

        private void OnPlayerDisconnect(SteamPlayer player)
        {
            VaultManager.RemovePlayerCache(player.playerID.steamID.m_SteamID);
        }
    }
}