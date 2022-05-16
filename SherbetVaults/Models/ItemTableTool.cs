using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using SDG.Unturned;

namespace SherbetVaults.Models
{
    public class ItemTableTool
    {
        public List<RSpawnTable> Tables { get; private set; } = new List<RSpawnTable>();

        public void ReInit()
        {
            Tables = GetSpawntables();
        }

        public ushort[] GetTableIDs(ushort itemID) =>
            Tables.Where(x => x.Items.Contains(itemID)).Select(x => x.TableID).ToArray();

        public ushort[] GetTableIDs(Regex selector) =>
            Tables.Where(x => selector.IsMatch(x.Name)).Select(x => x.TableID).ToArray();

        private List<RSpawnTable> GetSpawntables()
        {
            var tables = new List<RSpawnTable>();

            foreach (var table in LevelItems.tables)
            {
                var items = ResolveItems(table);
                tables.Add(new RSpawnTable(table.tableID, items, table.name));
            }
            return tables;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private HashSet<ushort> ResolveItems(ItemTable table)
        {
            var items = new HashSet<ushort>();

            foreach (var tier in table.tiers)
            {
                foreach (var item in tier.table)
                {
                    if (!items.Contains(item.item))
                    {
                        items.Add(item.item);
                    }
                }
            }

            return items;
        }
    }
}