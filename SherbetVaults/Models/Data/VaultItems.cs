using System.Threading;
using System.Threading.Tasks;
using Rocket.Core.Logging;
using RocketExtensions.Models;
using RocketExtensions.Utilities;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using SDG.Unturned;
using SherbetVaults.Database;

namespace SherbetVaults.Models.Data
{
    public class VaultItems : Items
    {
        public ulong PlayerID { get; }
        public string VaultID { get; }

        public new byte page => 7;

        public DatabaseQueue<DatabaseManager> Database { get; }
        public SherbetVaultsPlugin Plugin { get; }

        public bool SyncToDatabase { get; private set; } = false;

        public LDMPlayer Player { get; private set; }

        public VaultItems(ulong playerID, string vaultID, SherbetVaultsPlugin plugin) : base(7)
        {
            PlayerID = playerID;
            VaultID = vaultID;
            Plugin = plugin;
            Database = plugin.Database.Queue;
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
                await table.VaultItems.UpdateItemState(PlayerID, VaultID, jar);
                await table.Transactions.Update(PlayerID, VaultID, jar);
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

                Logger.Log($"({Player.DisplayName}) tried to store blacklisted item ({jar.item.id}) in vault {VaultID}.");
                Logger.Log("Please disregard the following error message.");

                // Throw an error to prevent the item being stored client-side
                throw new VaultStoreDeniedException();
            }

            Database.Enqueue(async (table) =>
            {
                await table.VaultItems.AddItem(PlayerID, VaultID, jar);
                await table.Transactions.Add(PlayerID, VaultID, jar.item, jar.rot, jar.x, jar.y);
            });
        }

        private void ItemRemoved(byte page, byte index, ItemJar jar)
        {
            if (!SyncToDatabase)
                return;

            Database.Enqueue(async (table) =>
            {
                await table.VaultItems.RemoveItem(PlayerID, VaultID, jar.x, jar.y);
                await table.Transactions.Remove(PlayerID, VaultID, jar.x, jar.y);
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
                await table.VaultItems.Clear(PlayerID, VaultID);
            });
        }

        public VaultItems(byte newPage) : base(newPage)
        {
        }

        public void OpenForPlayer(LDMPlayer ldm)
        {
            Player = ldm;
            var player = ldm.Player;
            player.inventory.isStoring = true;
            player.inventory.storage = null;
            player.inventory.updateItems(7, this);
            player.inventory.sendStorage();
        }

        public async Task OpenForPlayerAsync(LDMPlayer player) =>
            await ThreadTool.RunOnGameThreadAsync(OpenForPlayer, player);
    }
}