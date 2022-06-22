﻿using System;
using SDG.Unturned;

namespace SherbetVaults.Models.Restrictions.Restrictors
{
    [Selector(@"^(?i)Type\:[a-zA-Z]*$")]
    public class ItemTypeRestrictor : IItemRestrictor
    {
        public EItemType ItemType { get; }

        public bool IsMatch(ItemAsset asset) =>
            asset.type == ItemType;

        public ItemTypeRestrictor(string selector)
        {
            var typeName = selector.Substring(0, 5);
            if (!Enum.TryParse<EItemType>(typeName.ToUpper(), out var type))
                throw new BadSelectorException($"Type: Invalid item type '{typeName}'. Check the wiki for acceptable types.");
            ItemType = type;
        }
    }
}