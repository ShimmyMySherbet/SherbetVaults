using System.Collections.Generic;

namespace SherbetVaults.Models.Data
{
    public struct ItemSpawnTable
    {
        public ushort TableID { get; }
        public HashSet<ushort> Items { get; }
        public string Name { get; }

        public ItemSpawnTable(ushort tableID, HashSet<ushort> items, string name)
        {
            TableID = tableID;
            Items = items;
            Name = name;
        }
    }
}
