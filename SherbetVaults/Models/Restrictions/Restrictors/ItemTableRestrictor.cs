using System;
using System.Linq;
using System.Text.RegularExpressions;
using SDG.Unturned;
using SherbetVaults.Models.Utility;

namespace SherbetVaults.Models.Restrictions.Restrictors
{
    [Selector(@"^(?i)Table\:[a-zA-Z0-9]*$")]
    public class ItemTableRestrictor : IItemRestrictor
    {
        public ItemTableTool ItemTables { get; }

        public ushort ItemTableID { get; }

        public Regex Regex { get; }

        public bool IsMatch(ItemAsset asset) =>
            ItemTables.GetTableIDs(asset.id).Contains(ItemTableID);

        public ItemTableRestrictor(ItemTableTool itemTables, string selector)
        {
            ItemTables = itemTables;
            if (ushort.TryParse(selector, out var id))
            {
                // Match by table ID
                ItemTableID = id;
                Regex = null;
            }
            else
            {
                // Match by name selector
                ItemTableID = 0;
                var s = selector
                    .ToLower()
                    .Replace("*", "§");
                s = Regex.Escape(s)
                    .Replace("§", ".*");
                Regex = new Regex($"^{s}$");
            }
        }
    }
}