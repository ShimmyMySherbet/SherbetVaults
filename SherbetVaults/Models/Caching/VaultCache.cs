using System.Collections.Concurrent;

namespace SherbetVaults.Models.Caching
{
    public class VaultCache
    {
        private ConcurrentDictionary<ulong, PlayerVaultCache> m_Caches = new ConcurrentDictionary<ulong, PlayerVaultCache>();

        public VaultItems GetStorage(ulong playerID, string vaultID)
        {
            if (!m_Caches.ContainsKey(playerID))
            {
                m_Caches[playerID] = new PlayerVaultCache(playerID);
            }

            return m_Caches[playerID].GetStorage(vaultID);
        }

        public void SetStorage(ulong playerID, string vaultID, VaultItems storage)
        {
            if (!m_Caches.ContainsKey(playerID))
            {
                m_Caches[playerID] = new PlayerVaultCache(playerID);
            }

            m_Caches[playerID].SetStorage(vaultID, storage);
        }

        public void Clear(ulong player)
        {
            if (m_Caches.TryGetValue(player, out var cache))
            {
                cache.Clear();
            }
        }
    }
}