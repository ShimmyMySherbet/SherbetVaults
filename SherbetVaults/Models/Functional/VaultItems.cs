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
        }

        public virtual void EnableSync()
        {
            SyncToDatabase = true;
        }

        public virtual void DisableSync()
        {
            SyncToDatabase = false;
        }

        private ItemHandle GetHandle(byte index)
        {
            var item = getItem(index);
            return new ItemHandle(PlayerID, VaultID, item.x, item.y);
        }

        public new void updateAmount(byte index, byte newAmount)
        {
            base.updateAmount(index, newAmount);

            if (!SyncToDatabase)
                return;

            var handle = GetHandle(index);
            Database.Enqueue(async (table) =>
            {
                await table.UpdateItemAmount(handle, newAmount);
            });
        }

        public new void updateQuality(byte index, byte newQuality)
        {
            base.updateQuality(index, newQuality);

            if (!SyncToDatabase)
                return;

            var handle = GetHandle(index);
            Database.Enqueue(async (table) =>
            {
                await table.UpdateItemQuality(handle, newQuality);
            });
        }

        public new void updateState(byte index, byte[] newState)
        {
            base.updateState(index, newState);

            if (!SyncToDatabase)
                return;

            var handle = GetHandle(index);
            Database.Enqueue(async (table) =>
            {
                await table.UpdateItemState(handle, newState);
            });
        }

        public new void addItem(byte x, byte y, byte rot, Item item)
        {
            base.addItem(x, y, rot, item);

            if (!SyncToDatabase)
                return;

            Database.Enqueue(async (table) =>
            {
                await table.AddItem(PlayerID, VaultID, x, y, rot, item);
            });
        }

        public new bool tryAddItem(Item item)
        {
            if (base.tryAddItem(item))
            {
                if (!SyncToDatabase)
                    return true;

                byte lastIndex = (byte)(getItemCount() - 1);
                var jar = getItem(lastIndex);
                addItem(jar.x, jar.y, jar.rot, item);
                return true;
            }
            return false;
        }

        public new bool tryAddItem(Item item, bool isStateUpdatable)
        {
            if (base.tryAddItem(item, isStateUpdatable))
            {
                if (!SyncToDatabase)
                    return true;

                tryAddItem(item);
                return true;
            }
            return false;
        }

        public new void removeItem(byte index)
        {
            base.removeItem(index);

            if (!SyncToDatabase)
                return;

            var handle = GetHandle(index);
            Database.Enqueue(async (table) =>
            {
                await table.RemoveItem(handle);
            });
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
    }
}