using SDG.Unturned;

namespace SherbetVaults.Models.Restrictions.Restrictors
{
    [Selector(@"^[0-9]*\-[0-9]*$")]
    public class ItemRangeRestrictor : IItemRestrictor
    {
        public ushort StartID { get; }
        public ushort EndID { get; }
        public bool IsMatch(ItemAsset asset) => asset.id >= StartID && asset.id <= EndID;

        public ItemRangeRestrictor(string selector)
        {
            var parts = selector.Split('-');

            if (!ushort.TryParse(parts[0], out var value1))
                throw new BadSelectorException($"Range: Invalid item ID {parts[0]}");

            if (!ushort.TryParse(parts[1], out var value2))
                throw new BadSelectorException($"Range: Invalid item ID {parts[1]}");

            if (value1 > value2)
            {
                StartID = value2;
                EndID = value1;
            }
            else
            {
                StartID = value1;
                EndID = value2;
            }
        }

    }
}