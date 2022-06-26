using System;
using System.Linq;
using System.Text.RegularExpressions;
using SDG.Unturned;
using SherbetVaults.Models.Utility;

namespace SherbetVaults.Models.Restrictions.Restrictors
{
    [Selector(@"^Table:.*")]
    public class ItemTableRestrictor : IItemRestrictor
    {
        public ItemTableTool ItemTables { get; }

        public ushort ItemTableID { get; }

        public Regex Regex { get; }

        public bool IsMatch(ItemAsset asset)
        {
            if (Regex == null)
            {
                return ItemTables.GetTableIDs(asset.id).Contains(ItemTableID);
            }
            else
            {
                return ItemTables.GetTableIDs(Regex).Contains(ItemTableID);
            }
        }

        public ItemTableRestrictor(ItemTableTool itemTables, string selector)
        {
            ItemTables = itemTables;
            var table = selector.Substring(6);

            if (ushort.TryParse(table, out var id))
            {
                // Match by table ID
                ItemTableID = id;
                Regex = null;
            }
            else
            {
                // Match by name selector
                ItemTableID = 0;
                var s = table
                    .Replace("*", "§");
                s = Regex.Escape(s)
                    .Replace("§", ".*");
                Regex = new Regex($"^{s}$");
            }
        }
    }
}