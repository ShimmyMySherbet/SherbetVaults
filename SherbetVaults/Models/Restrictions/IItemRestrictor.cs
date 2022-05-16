using SDG.Unturned;

namespace SherbetVaults.Models.Restrictions
{
    public interface IItemRestrictor
    {
        bool IsMatch(ItemAsset asset);
    }
}