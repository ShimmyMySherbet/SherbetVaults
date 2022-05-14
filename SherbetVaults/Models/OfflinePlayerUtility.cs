using System.Threading.Tasks;
using Rocket.Unturned.Player;

namespace SherbetVaults.Models
{
    public static class OfflinePlayerUtility
    {
        public static async Task<(ulong playerID, string playerName)> GetPlayer(string handle, bool fetchName = true)
        {
            var result = GetPlayerParallel(handle, fetchName);

            if (fetchName)
            {
                return (result.playerID, await result.playerNameTask);
            }
            return (result.playerID, "Unknown Player");
        }

        public static (ulong playerID, Task<string> playerNameTask) GetPlayerParallel(string handle, bool fetchName = true)
        {
            if (ulong.TryParse(handle, out var playerID))
            {
                var nameTask = fetchName ? playerID.GetPlayerName() : Task.FromResult("Unknown Player");

                return (playerID, nameTask);
            }

            var player = UnturnedPlayer.FromName(handle);

            if (player != null)
            {
                return (player.CSteamID.m_SteamID, Task.FromResult(player.DisplayName)));
            }

            return (0ul, Task.FromResult(string.Empty));
        }
    }
}