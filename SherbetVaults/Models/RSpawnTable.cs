using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SherbetVaults.Models
{
    public struct RSpawnTable
    {
        public ushort TableID { get; }
        public HashSet<ushort> Items { get; }
        public string Name { get; }

        public RSpawnTable(ushort tableID, HashSet<ushort> items, string name)
        {
            TableID = tableID;
            Items = items;
            Name = name;
        }
    }
}
