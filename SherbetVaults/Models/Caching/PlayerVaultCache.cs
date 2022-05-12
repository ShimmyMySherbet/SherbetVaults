using System;
using System.Collections.Concurrent;

namespace SherbetVaults.Models.Caching
{
    public class PlayerVaultCache
    {
        public ulong PlayerID { get; }
        private ConcurrentDictionary<string, VaultStorage> m_Vaults = new ConcurrentDictionary<string, VaultStorage>(StringComparer.InvariantCultureIgnoreCase);

        public PlayerVaultCache(ulong playerID)
        {
            PlayerID = playerID;
        }

        public void SetStorage(string vaultId, VaultStorage storage)
        {
            if (m_Vaults.TryRemove(vaultId, out var oldStorage))
            {
                if (oldStorage != storage)
                {
                    oldStorage.VaultItems.DisableSync();
                }
            }
            m_Vaults[vaultId] = storage;
        }

        public VaultStorage GetStorage(string vaultID)
        {
            if (m_Vaults.TryGetValue(vaultID, out var storage))
            {
                return storage;
            }
            return null;
        }

        public void Clear()
        {
            foreach (var i in m_Vaults.Values)
            {
                i.VaultItems.DisableSync();
            }
            m_Vaults.Clear();
        }
    }
}