using System.Threading.Tasks;
using RocketExtensions.Models;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using SDG.Unturned;

namespace SherbetVaults.Models
{
    public static class TrashUtility
    {
        public static async Task OpenTrashAsync(this LDMPlayer player)
        {
            var items = new Items(7);
            items.resize(15, 15);

            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                player.Player.inventory.updateItems(7, items);
                player.Player.inventory.sendStorage();
            });
        }
    }
}