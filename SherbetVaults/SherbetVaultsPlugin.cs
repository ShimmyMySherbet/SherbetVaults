using System.Collections.Generic;
using Rocket.API;
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
        public ItemTableTool ItemTable { get; } = new ItemTableTool();
        public SherbetVaultsConfig Config => Configuration.Instance;
        public List<VaultConfig> VaultConfigs => Config.Vaults;
        #endregion

        public override void LoadPlugin()
        {
            VaultManager = new VaultManager(this);
            RestrictionBuilder = new RestrictionBuilder(this);
            Database = new DatabaseManager(Configuration.Instance);

            if (!Database.Connect(out var errorMessage))
            {
                Logger.Log($"Failed to connect to database: {errorMessage}");
                UnloadPlugin(PluginState.Failure);
                return;
            }

            Database.CheckSchema();
            Database.InitQueue();

            Provider.onEnemyDisconnected += OnPlayerDisconnect;
            Level.onLevelLoaded += OnLevelLoaded;

            base.LoadPlugin();
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