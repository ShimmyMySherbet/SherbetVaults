using SherbetVaults.Database.Tables;
using ShimmyMySherbet.MySQL.EF.Core;
using ShimmyMySherbet.MySQL.EF.Models.ConnectionProviders;

namespace SherbetVaults.Database
{
    public class DatabaseManager : DatabaseClient
    {
        public VaultItemsTable VaultItems { get; }
        public VaultAliasTable Aliases { get; }
        public VaultTransactionsTable Transactions { get; }
        public DatabaseQueue<DatabaseManager> Queue { get; set; }

        public DatabaseManager(SherbetVaultsPlugin plugin) : base(connectionProvider: new TransientConnectionProvider(plugin.Config.DatabaseSettings), autoInit: false)
        {
            Queue = new DatabaseQueue<DatabaseManager>(this);
            VaultItems = new VaultItemsTable(plugin, $"{plugin.Config.DatabaseTablePrefix}_Items");
            Transactions = new VaultTransactionsTable($"{plugin.Config.DatabaseTablePrefix}_History");

            if (plugin.Config.VaultAliasesEnabled)
                Aliases = new VaultAliasTable($"{plugin.Config.DatabaseTablePrefix}_Aliases");

            Init();
        }

        public void InitQueue() =>
            Queue.StartWorker();

        public void Dispose()
        {
            Queue.Dispose();
            Client.Dispose();
        }
    }
}