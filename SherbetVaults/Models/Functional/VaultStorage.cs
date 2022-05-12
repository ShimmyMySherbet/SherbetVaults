using System.Reflection;
using System.Threading.Tasks;
using RocketExtensions.Models;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using SDG.Unturned;

namespace SherbetVaults.Models
{
    public class VaultStorage : InteractableStorage
    {
        public VaultItems VaultItems { get; }
        private FieldInfo m_itemsInfo = typeof(InteractableStorage).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
        public new Items items => VaultItems;
        public new bool shouldCloseWhenOutsideRange = false;

        public VaultStorage(VaultItems items)
        {
            VaultItems = items;
            ResetInternalItems();
        }

        private void ResetInternalItems()
        {
            m_itemsInfo.SetValue(this, VaultItems);
        }

        public new void updateState(Asset asset, byte[] state)
        {
            base.updateState(asset, state);
            // Prevent state update overwriting vault items
            ResetInternalItems();
        }

        public void OpenForPlayer(Player player)
        {
            player.inventory.updateItems(7, VaultItems);
            player.inventory.sendStorage();
        }

        public void OpenForPlayer(LDMPlayer ldm) =>
            OpenForPlayer(ldm.Player);

        public async Task OpenForPlayerAsync(LDMPlayer player) =>
            await ThreadTool.RunOnGameThreadAsync(OpenForPlayer, player);
    }
}