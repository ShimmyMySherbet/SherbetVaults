namespace SherbetVaults.Models
{
    public struct ItemHandle
    {
        public ulong PlayerID { get; }
        public string VaultID { get; }

        public byte X { get; }
        public byte Y { get; }

        public ItemHandle(ulong playerID, string vaultID, byte x, byte y)
        {
            PlayerID = playerID;
            VaultID = vaultID;
            X = x;
            Y = y;
        }
    }
}