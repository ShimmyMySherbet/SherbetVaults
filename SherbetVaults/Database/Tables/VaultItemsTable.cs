using System.Threading.Tasks;
using SDG.Unturned;
using SherbetVaults.Database.Models;
using SherbetVaults.Models.Config;
using SherbetVaults.Models.Data;
using ShimmyMySherbet.MySQL.EF.Core;

namespace SherbetVaults.Database.Tables
{
    public class VaultItemsTable : DatabaseTable<VaultItem>
    {
        public SherbetVaultsPlugin Plugin { get; }

        public VaultItemsTable(SherbetVaultsPlugin plugin, string tableName) : base(tableName)
        {
            Plugin = plugin;
        }

        public async Task<VaultItems> OpenVault(ulong playerID, string vaultID, VaultConfig config)
        {
            var items = await QueryAsync("SELECT * FROM @TABLE WHERE PlayerID=@0 AND VaultID=@1", playerID, vaultID);
            var vi = new VaultItems(playerID, vaultID, Plugin);
            vi.loadSize(config.Width, config.Height);
            foreach (var item in items)
            {
                vi.loadItem(item.X, item.Y, item.Rot, item.GetItem());
            }
            vi.EnableSync();
            return vi;
        }

        public async Task AddItem(ulong playerID, string vaultID, Item item, byte rot, byte x, byte y) =>
            await InsertUpdateAsync(VaultItem.Create(playerID, vaultID, item, rot, x, y));

        public async Task AddItem(ulong playerID, string vaultID, ItemJar jar) =>
            await AddItem(playerID, vaultID, jar.item, jar.rot, jar.x, jar.y);

        public async Task RemoveItem(ulong playerID, string vaultID, byte x, byte y) =>
            await ExecuteNonQueryAsync("DELETE FROM @TABLE WHERE PlayerID=@0 AND VaultID=@1 AND X=@2 AND Y=@3", playerID, vaultID, x, y);

        public async Task<int> Clear(ulong playerID, string vaultID) =>
            await ExecuteNonQueryAsync("DELETE FROM @TABLE WHERE PlayerID=@0 AND VaultID=@1;", playerID, vaultID);

        public async Task UpdateItemState(ulong playerID, string vaultID, ItemJar jar) =>
            await ExecuteNonQueryAsync("UPDATE @TABLE SET State=@4, Quality=@5, Amount=@6 WHERE PlayerID=@0 AND VaultID=@1 AND X=@2 AND Y = @3",
                playerID, vaultID, jar.x, jar.y, jar.item.state, jar.item.quality, jar.item.amount);
    }
}