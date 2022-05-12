//using System;
//using System.Collections.Generic;
//using ShimmyMySherbet.MySQL.EF.Models;
//using ShimmyMySherbet.MySQL.EF.Models.TypeModel;

//namespace SherbetVaults.Database.Models
//{
//    public class PlayerVault
//    {
//        [SQLPrimaryKey]
//        public ulong PlayerID { get; set; }

//        [SQLPrimaryKey, SQLVarChar(64)]
//        public string VaultID { get; set; } = "default";

//        public DateTime LastOpened { get; set; } = DateTime.Now;

//        public PlayerVault()
//        {
//        }

//        public PlayerVault(ulong playerID, string vaultID)
//        {
//            PlayerID = playerID;
//            VaultID = vaultID;
//        }
//    }
//}