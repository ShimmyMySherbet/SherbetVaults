using System;
using System.Threading.Tasks;
using SDG.Unturned;
using SherbetVaults.Database.Models;
using SherbetVaults.Models.Enums;
using ShimmyMySherbet.MySQL.EF.Core;

namespace SherbetVaults.Database.Tables
{
    public class VaultTransactionsTable : DatabaseTable<VaultTransaction>
    {
        public DatabaseQueue<VaultTransactionsTable> Queue { get; }

        public VaultTransactionsTable(string tableName) : base(tableName)
        {
            Queue = new DatabaseQueue<VaultTransactionsTable>(this);
        }

        public async Task SaveTransaction(VaultItem item, ETransactionType type)
        {
            var transaction = VaultTransaction.FromItem(item, type);
            await InsertAsync(transaction);
        }
        
        public async Task Wipe(ulong playerID, string vaultID)
        {
            var transaction = VaultTransaction.FromWipe(playerID, vaultID);
            await InsertAsync(transaction);
        }

        public async Task Add(ulong playerID, string vaultID, Item item, byte rot, byte x, byte y)
        {
            var transaction = VaultTransaction.FromItem(playerID, vaultID, item, rot, x, y, ETransactionType.Add);
            await InsertAsync(transaction);
        }

        public async Task Remove(ulong playerID, string vaultID, byte x, byte y)
        {
            var transaction = VaultTransaction.Remove(playerID, vaultID, x, y);
            await InsertAsync(transaction);
        }

        public async Task Update(ulong playerID, string vaultID, ItemJar jar)
        {
            var transaction = new VaultTransaction()
            {
                Date = DateTime.Now,
                ItemID = jar.item.id,
                PlayerID = playerID,
                VaultID = vaultID,
                Amount = jar.item.amount,
                Quality = jar.item.quality,
                Rot = jar.rot,
                State = jar.item.state,
                X = jar.x,
                Y = jar.y,
                Type = ETransactionType.Update
            };
            await InsertAsync(transaction);
        }
    }
}