using System.Threading.Tasks;
using SDG.Unturned;
using SherbetVaults.Database.Models;
using SherbetVaults.Models;
using SherbetVaults.Models.Config;
using ShimmyMySherbet.MySQL.EF.Core;

namespace SherbetVaults.Database.Tables
{
    public class VaultItemsTable : DatabaseTable<VaultItem>
    {
        public DatabaseQueue<VaultItemsTable> Queue { get; }

        public VaultItemsTable(string tableName) : base(tableName)
        {
            Queue = new DatabaseQueue<VaultItemsTable>(this);
        }

        public async Task UpdateItemAmount(ItemHandle handle, byte amount)
        {
            await ExecuteNonQueryAsync("UPDATE @TABLE SET Amount=@4 WHERE PlayerID=@0 AND VaultID=@1 AND X=@2 AND Y = @3",
                handle.PlayerID, handle.VaultID, handle.X, handle.Y, amount);
        }

        public async Task UpdateItemQuality(ItemHandle handle, byte quality)
        {
            await ExecuteNonQueryAsync("UPDATE @TABLE SET Quality=@4 WHERE PlayerID=@0 AND VaultID=@1 AND X=@2 AND Y = @3",
                handle.PlayerID, handle.VaultID, handle.X, handle.Y, quality);
        }

        public async Task UpdateItemState(ItemHandle handle, byte[] state)
        {
            await ExecuteNonQueryAsync("UPDATE @TABLE SET State=@4 WHERE PlayerID=@0 AND VaultID=@1 AND X=@2 AND Y = @3",
                handle.PlayerID, handle.VaultID, handle.X, handle.Y, state);
        }

        public async Task AddItem(ulong playerID, string vaultID, byte x, byte y, byte rot, Item item)
        {
            var inst = new VaultItem()
            {
                ItemID = item.id,
                PlayerID = playerID,
                VaultID = vaultID,
                Amount = item.amount,
                Quality = item.quality,
                Rot = rot,
                State = item.state,
                X = x,
                Y = y
            };

            await InsertUpdateAsync(inst);
        }

        public async Task RemoveItem(ItemHandle handle)
        {
            await ExecuteNonQueryAsync("DELETE FROM @TABLE WHERE PlayerID=@0 AND VaultID=@1 AND X=@2 AND Y=@3",
                handle.PlayerID, handle.VaultID, handle.X, handle.Y);
        }

        public async Task Clear(ulong playerID, string vaultID)
        {
            await ExecuteNonQueryAsync("DELETE FROM @TABLE WHERE PlayerID=@0 AND VaultID=@1;", playerID, vaultID);
        }

        public async Task<VaultItems> OpenVault(ulong playerID, string vaultID, VaultConfig config)
        {
            var items = await QueryAsync("SELECT * FROM @TABLE WHERE PlayerID=@0 AND VaultID=@1");
            var vi = new VaultItems(playerID, vaultID, Queue);
            vi.loadSize(config.Width, config.Height);
            foreach (var item in items)
            {
                vi.loadItem(item.X, item.Y, item.Rot, item.GetItem());
            }

            vi.EnableSync();

            return vi;
        }
    }
}