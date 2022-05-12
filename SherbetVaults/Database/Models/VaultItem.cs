using SDG.Unturned;
using ShimmyMySherbet.MySQL.EF.Models;
using ShimmyMySherbet.MySQL.EF.Models.TypeModel;

namespace SherbetVaults.Database.Models
{
    public class VaultItem
    {
        [SQLPrimaryKey]
        public ulong PlayerID { get; set; }

        [SQLPrimaryKey, SQLVarChar(64)]
        public string VaultID { get; set; }

        [SQLPrimaryKey]
        public byte X;

        [SQLPrimaryKey]
        public byte Y;

        public ushort ItemID { get; set; }

        public byte Rot { get; set; }
        public byte Quality { get; set; }
        public byte Amount { get; set; }

        public byte[] State { get; set; }





        public Item GetItem()
        {
            return new Item(ItemID, Amount, Quality, State);
        }
    }
}