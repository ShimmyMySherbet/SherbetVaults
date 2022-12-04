using System;
using System.Collections.Concurrent;
using System.IO;
using Rocket.Core;
using SherbetVaults.Models.Data;

namespace SherbetVaults.Models.Caching
{
    public class PlayerVaultCache
    {
        public ulong PlayerID { get; }
        private readonly ConcurrentDictionary<string, VaultItems> m_Vaults = new(StringComparer.InvariantCultureIgnoreCase);

        public PlayerVaultCache(ulong playerID)
        {
            PlayerID = playerID;
        }

        public void SetStorage(string vaultId, VaultItems storage)
        {
            if (m_Vaults.TryRemove(vaultId, out var oldStorage))
            {
                if (oldStorage != storage)
                {
                    oldStorage.DisableSync();
                }
            }
            m_Vaults[vaultId] = storage;
        }

        public VaultItems GetStorage(string vaultID)
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
                i.DisableSync();
            }
            m_Vaults.Clear();
        }
    }
}