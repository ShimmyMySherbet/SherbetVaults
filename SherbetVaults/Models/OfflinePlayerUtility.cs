using System.Threading.Tasks;
using Rocket.Unturned.Player;

namespace SherbetVaults.Models
{
    public static class OfflinePlayerUtility
    {
        public static async Task<(ulong playerID, string playerName)> GetPlayer(string handle)
        {
            if (ulong.TryParse(handle, out var playerID))
            {
                var playerName = await playerID.GetPlayerName();

                return (playerID, playerName);
            }

            var player = UnturnedPlayer.FromName(handle);

            if (player != null)
            {
                return (player.CSteamID.m_SteamID, player.DisplayName);
            }

            return (0, string.Empty);
        }
    }
}