using SDG.Unturned;

namespace SherbetVaults.Models.Restrictions.Restrictors
{
    [Selector(@"^\d+$")]
    public class ItemRestrictor : IItemRestrictor
    {
        public ushort ItemID { get; }

        public bool IsMatch(ItemAsset asset) => asset.id == ItemID;

        public ItemRestrictor(string id)
        {
            if (!ushort.TryParse(id, out var itemID))
                throw new BadSelectorException($"ItemID: Invalid item ID {itemID}");

            ItemID = itemID;
        }
    }
}