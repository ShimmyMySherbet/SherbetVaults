using SherbetVaults.Database.Tables;
using SherbetVaults.Models.Config;
using ShimmyMySherbet.MySQL.EF.Core;

namespace SherbetVaults.Database
{
    public class DatabaseManager : DatabaseClient
    {
        public VaultItemsTable VaultItems { get; } = new VaultItemsTable("SherbetVaults_Items");
        public VaultAliasTable Aliases { get; }

        public DatabaseManager(SherbetVaultsConfig config) : base(config.DatabaseSettings, autoInit: false)
        {
            if (config.VaultAliasesEnabled)
            {
                Aliases = new VaultAliasTable("SherbetVaults_Aliases");
            }

            Init();
        }
    }
}