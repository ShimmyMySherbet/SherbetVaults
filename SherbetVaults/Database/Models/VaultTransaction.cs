using System;
using SDG.Unturned;
using SherbetVaults.Models.Enums;
using ShimmyMySherbet.MySQL.EF.Models;
using ShimmyMySherbet.MySQL.EF.Models.TypeModel;

namespace SherbetVaults.Database.Models
{
    public class VaultTransaction
    {
        [SQLPrimaryKey, SQLAutoIncrement]
        public int TransactionID { get; set; }

        [SQLIndex]
        public ulong PlayerID { get; set; }

        [SQLSerialize(ESerializeFormat.JSON)]
        public ETransactionType Type { get; set; }

        public DateTime Date { get; set; }

        [SQLVarChar(64)]
        public string VaultID { get; set; }

        public byte X { get; set; }
        public byte Y { get; set; }
        public ushort ItemID { get; set; }
        public byte Rot { get; set; }
        public byte Quality { get; set; }
        public byte Amount { get; set; }
        public byte[] State { get; set; }

        public static VaultTransaction Remove(ulong playerID, string vaultID, byte x, byte y)
        {
            return new VaultTransaction()
            {
                Date = DateTime.Now,
                ItemID = 0,
                PlayerID = playerID,
                VaultID = vaultID,
                Amount = 0,
                Quality = 0,
                Rot = 0,
                State = new byte[0],
                X = x,
                Y = y,
                Type = ETransactionType.Remove
            };
        }

        public static VaultTransaction FromItem(VaultItem item, ETransactionType type)
        {
            return new VaultTransaction()
            {
                Date = DateTime.Now,
                ItemID = item.ItemID,
                PlayerID = item.PlayerID,
                VaultID = item.VaultID,
                Type = type,
                Amount = item.Amount,
                Quality = item.Quality,
                Rot = item.Rot,
                State = item.State,
                X = item.X,
                Y = item.Y
            };
        }

        public static VaultTransaction FromItem(ulong playerID, string vaultID, Item item, byte rot, byte x, byte y, ETransactionType type)
        {
            return new VaultTransaction()
            {
                Date = DateTime.Now,
                ItemID = item.id,
                PlayerID = playerID,
                VaultID = vaultID,
                Type = type,
                Amount = item.amount,
                Quality = item.quality,
                Rot = rot,
                State = item.state,
                X = x,
                Y = y
            };
        }

        public static VaultTransaction FromWipe(ulong playerID, string vaultID)
        {
            return new VaultTransaction()
            {
                Date = DateTime.Now,
                ItemID = 0,
                PlayerID = playerID,
                VaultID = vaultID,
                Amount = 0,
                Quality = 0,
                Rot = 0,
                State = Array.Empty<byte>(),
                X = 0,
                Y = 0,
                Type = ETransactionType.Wipe
            };
        }
    }
}