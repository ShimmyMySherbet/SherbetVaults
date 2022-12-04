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
            VaultItems = new VaultItemsTable(plugin, "SherbetVaults_Items");
            Transactions = new VaultTransactionsTable("SherbetVaults_History");

            if (plugin.Config.VaultAliasesEnabled)
                Aliases = new VaultAliasTable("SherbetVaults_Aliases");

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