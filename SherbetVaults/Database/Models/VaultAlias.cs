using ShimmyMySherbet.MySQL.EF.Models;
using ShimmyMySherbet.MySQL.EF.Models.TypeModel;

namespace SherbetVaults.Database.Models
{
    public class VaultAlias
    {
        [SQLPrimaryKey]
        public ulong PlayerID { get; set; }

        [SQLPrimaryKey, SQLVarChar(64)]
        public string Alias { get; set; }

        public string VaultID { get; set; }

        public static VaultAlias Create(ulong player, string vaultID, string alias)
        {
            return new VaultAlias()
            {
                Alias = alias,
                PlayerID = player,
                VaultID = vaultID
            };
        }
    }
}