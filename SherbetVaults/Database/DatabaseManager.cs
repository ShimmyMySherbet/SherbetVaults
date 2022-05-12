using SherbetVaults.Database.Tables;
using ShimmyMySherbet.MySQL.EF.Core;
using ShimmyMySherbet.MySQL.EF.Models;

namespace SherbetVaults.Database
{
    public class DatabaseManager : DatabaseClient
    {
        public VaultItemsTable VaultItems { get; } = new VaultItemsTable("SherbetVaults_Items");

        public DatabaseManager(DatabaseSettings settings) : base(settings)
        {
        }
    }
}