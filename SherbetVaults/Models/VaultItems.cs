using System.Threading.Tasks;
using Rocket.Core.Logging;
using RocketExtensions.Models;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using SDG.Unturned;
using SherbetVaults.Database;
using SherbetVaults.Database.Tables;

namespace SherbetVaults.Models
{
    public class VaultItems : Items
    {
        public ulong PlayerID { get; }
        public string VaultID { get; }

        public new byte page => 7;

        public DatabaseQueue<VaultItemsTable> Database { get; }

        public bool SyncToDatabase { get; private set; } = false;

        public VaultItems(ulong playerID, string vaultID, DatabaseQueue<VaultItemsTable> database) : base(7)
        {
            PlayerID = playerID;
            VaultID = vaultID;
            Database = database;
            onItemUpdated += ItemUpdated;
            onItemAdded += ItemAdded;
            onItemRemoved += ItemRemoved;
            onItemDiscarded += ItemDiscarded;
        }

        public virtual void EnableSync()
        {
            SyncToDatabase = true;
        }

        public virtual void DisableSync()
        {
            SyncToDatabase = false;
        }

        private void ItemUpdated(byte page, byte index, ItemJar jar)
        {
            if (!SyncToDatabase)
                return;

            Database.Enqueue(async (table) =>
            {
                await table.UpdateItemState(PlayerID, VaultID, jar);
            });
        }

        private void ItemAdded(byte page, byte index, ItemJar jar)
        {
            if (!SyncToDatabase)
                return;

            Database.Enqueue(async (table) =>
            {
                await table.AddItem(PlayerID, VaultID, jar);
            });
        }

        private void ItemRemoved(byte page, byte index, ItemJar jar)
        {
            if (!SyncToDatabase)
                return;

            Database.Enqueue(async (table) =>
            {
                await table.RemoveItem(PlayerID, VaultID, jar.x, jar.y);
            });
        }

        private void ItemDiscarded(byte page, byte index, ItemJar jar)
        {
            ItemRemoved(page, index, jar);
        }

        public new void clear()
        {
            base.clear();

            if (!SyncToDatabase)
                return;

            Database.Enqueue(async (table) =>
            {
                await table.Clear(PlayerID, VaultID);
            });
        }

        public VaultItems(byte newPage) : base(newPage)
        {
        }

        public void OpenForPlayer(Player player)
        {
            player.inventory.updateItems(7, this);
            player.inventory.sendStorage();
        }

        public void OpenForPlayer(LDMPlayer ldm) =>
            OpenForPlayer(ldm.Player);

        public async Task OpenForPlayerAsync(LDMPlayer player) =>
            await ThreadTool.RunOnGameThreadAsync(OpenForPlayer, player);
    }
}