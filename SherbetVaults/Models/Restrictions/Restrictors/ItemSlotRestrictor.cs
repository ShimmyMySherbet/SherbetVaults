using System;
using SDG.Unturned;

namespace SherbetVaults.Models.Restrictions.Restrictors
{
    [Selector(@"Slot:.*")]
    public class ItemSlotRestrictor : IItemRestrictor
    {
        public ESlotType Slot { get; }

        public bool IsMatch(ItemAsset asset) =>
            asset.slot == Slot;

        public ItemSlotRestrictor(string selector)
        {
            var slotName = selector.Substring(5);
            if (!Enum.TryParse<ESlotType>(slotName.ToUpper(), out var slot))
                throw new BadSelectorException($"Slot: Invalid slot '{slotName}'. Acceptable: None, Primary, Secondary, Tertiary, Any");
            Slot = slot;
        }
    }
}