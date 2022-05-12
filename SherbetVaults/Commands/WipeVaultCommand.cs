using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Player;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SherbetVaults.Models;

namespace SherbetVaults.Commands
{
    [CommandName("WipeVault")]
    [CommandInfo("Wipes a player's vault", Syntax: "[TargetPlayer] [VaultID]")]
    [AllowedCaller(AllowedCaller.Player)]
    public class WipeVaultCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            var playerID = context.Arguments.Get<UnturnedPlayer>(0, defaultValue: null)?.CSteamID.m_SteamID
                ?? context.Arguments.Get<ulong>(0, paramName: "Target Player");

            var targetVault = context.Arguments.Get<string>(1, paramName: "VaultID");

            var playerNameTask = playerID.GetPlayerName();

            var items = await Plugin.Database.VaultItems.Clear(playerID, targetVault);

            var name = await playerNameTask;

            await context.ReplyKeyAsync("WipeVault_Wiped", items, name, targetVault);
        }

        private new SherbetVaultsPlugin Plugin =>
            base.Plugin as SherbetVaultsPlugin;
    }
}