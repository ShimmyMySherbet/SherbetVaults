using Cysharp.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SherbetVaults.Models;
using UnityEngine;

namespace SherbetVaults.Commands
{
    [CommandName("WipeVault")]
    [CommandInfo("Wipes a player's vault", Syntax: "[Target Player] [VaultID]")]
    [AllowedCaller(AllowedCaller.Player)]
    public class WipeVaultCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            var playerHandle = context.Arguments.Get<string>(0, paramName: "Target Player");

            var targetPlayer = await OfflinePlayerUtility.GetPlayer(playerHandle);

            if (targetPlayer.playerID == 0)
            {
                await context.ReplyAsync($"Usage: /{Name} {Syntax}", Color.cyan);
                return;
            }

            var targetVault = context.Arguments.Get<string>(1, paramName: "VaultID");

            var items = await Plugin.Database.VaultItems.Clear(targetPlayer.playerID, targetVault);

            await context.ReplyKeyAsync("WipeVault_Wiped", items, targetPlayer.playerName, targetVault);
        }

        private new SherbetVaultsPlugin Plugin =>
            base.Plugin as SherbetVaultsPlugin;
    }
}