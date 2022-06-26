using SherbetVaults.Database.Tables;
using ShimmyMySherbet.MySQL.EF.Core;

namespace SherbetVaults.Database
{
    public class DatabaseManager : DatabaseClient
    {
        public VaultItemsTable VaultItems { get; }
        public VaultAliasTable Aliases { get; }

        public DatabaseManager(SherbetVaultsPlugin plugin) : base(plugin.Config.DatabaseSettings, autoInit: false, singleConnectionMode: plugin.Config.SingletonDatabaseConnection)
        {
            VaultItems = new VaultItemsTable(plugin, "SherbetVaults_Items");

            if (plugin.Config.VaultAliasesEnabled)
                Aliases = new VaultAliasTable("SherbetVaults_Aliases");

            Init();
        }

        public void InitQueue() =>
            VaultItems.Queue.StartWorker();

        public void Dispose()
        {
            VaultItems.Queue.Dispose();
            Client.Dispose();
        }
    }
}