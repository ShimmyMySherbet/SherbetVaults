using System.Threading.Tasks;
using SherbetVaults.Database.Models;
using ShimmyMySherbet.MySQL.EF.Core;
namespace SherbetVaults.Database.Tables
{
    public class VaultAliasTable : DatabaseTable<VaultAlias>
    {
        public VaultAliasTable(string tableName) : base(tableName)
        {
        }

        public async Task SetAliasAsync(ulong playerID, string vaultID, string alias) =>
            await InsertUpdateAsync(VaultAlias.Create(playerID, vaultID, alias));

        public async Task<string> GetAliasAsync(ulong playerID, string alias) =>
            (await QuerySingleAsync("SELECT * FROM @TABLE WHERE PlayerID=@0 AND Alias=@1;", playerID, alias))?.VaultID;

        public async Task<bool> DeleteAliasAsync(ulong playerID, string alias) =>
            await ExecuteNonQueryAsync("DELETE FROM @TABLE WHERE PlayerID=@0 AND Alias=@1;", playerID, alias) > 0;

        public async Task<VaultAlias[]> GetAliasesAsync(ulong playerID) =>
            (await QueryAsync("SELECT * FROM @TABLE WHERE PlayerID=@0;", playerID)).ToArray();

    }
}