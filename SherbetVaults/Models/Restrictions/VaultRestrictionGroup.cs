using System.Collections.Generic;
using SherbetVaults.Models.Config.Restrictions;

namespace SherbetVaults.Models.Restrictions
{
    public class VaultRestrictionGroup : RestrictionGroup
    {
        public List<IItemRestrictor> Restrictors = new();

        public VaultRestrictionGroup(RestrictionGroup group, List<IItemRestrictor> restrictors)
        {
            Restrictors = restrictors;
            Blacklist = group.Blacklist;
            GroupID = group.GroupID;
            Selectors = group.Selectors;
            Weight = group.Weight;
            TranslationKey = group.TranslationKey;
        }
    }
}