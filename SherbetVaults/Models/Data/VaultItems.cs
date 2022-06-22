using System.Threading;
using System.Threading.Tasks;
using RocketExtensions.Models;
using RocketExtensions.Utilities;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using SDG.Unturned;
using SherbetVaults.Database;
using SherbetVaults.Database.Tables;

namespace SherbetVaults.Models.Data
{
    public class VaultItems : Items
    {
        public ulong PlayerID { get; }
        public string VaultID { get; }

        public new byte page => 7;

        public DatabaseQueue<VaultItemsTable> Database { get; }
        public SherbetVaultsPlugin Plugin { get; }

        public bool SyncToDatabase { get; private set; } = false;

        public LDMPlayer Player { get; private set; }

        public VaultItems(ulong playerID, string vaultID, SherbetVaultsPlugin plugin) : base(7)
        {
            PlayerID = playerID;
            VaultID = vaultID;
            Plugin = plugin;
            Database = plugin.Database.VaultItems.Queue;
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

            if (!Plugin.RestrictionTool.IsPermitted(jar.item.id, Player, out var group))
            {
                var message = Plugin.Translate(group.TranslationKey, group.GroupID).ReformatColor();
                ThreadPool.QueueUserWorkItem(async (_) => await Player.MessageAsync(message));
                items.Remove(jar);

                Player.Player.inventory.tryAddItem(jar.item, true, false);
                return;
            }

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

        public void OpenForPlayer(LDMPlayer ldm)
        {
            Player = ldm;
            var player = ldm.Player;
            player.inventory.updateItems(7, this);
            player.inventory.sendStorage();
        }

        public async Task OpenForPlayerAsync(LDMPlayer player) =>
            await ThreadTool.RunOnGameThreadAsync(OpenForPlayer, player);
    }
}