using System;
using System.Linq;
using SDG.Unturned;

namespace SherbetVaults.Models.Restrictions.Restrictors
{
    [Selector(@"^Type:.*")]
    public class ItemTypeRestrictor : IItemRestrictor
    {
        public EItemType ItemType { get; }

        public bool IsMatch(ItemAsset asset) =>
            asset.type == ItemType;

        public ItemTypeRestrictor(string selector)
        {
            var typeName = selector.Substring(5);
            if (!Enum.TryParse<EItemType>(typeName.ToUpper(), out var type))
                throw new BadSelectorException($"Type: Invalid item type '{typeName}'. Check the wiki for acceptable types.");
            ItemType = type;
        }
    }
}