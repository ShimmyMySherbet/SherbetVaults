using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Player;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SherbetVaults.Models;
using UnityEngine;

namespace SherbetVaults.Commands
{
    [CommandName("SpyVault")]
    [CommandInfo("Opens another player's vault", Syntax: "[Target Player] [Vault ID]")]
    [AllowedCaller(AllowedCaller.Player)]
    public class SpyVaultCommand : RocketCommand
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

            var targetVault = context.Arguments.Get(1, Plugin.Config.DefaultVault, paramName: "VaultID");

            var vaultConfig = Plugin.GetVaultConfig(targetVault);

            if (vaultConfig == null)
            {
                await context.ReplyKeyAsync("Vault_Fail_NotFound", targetVault);
                return;
            }

            var vault = await Plugin.VaultManager.GetVault(targetPlayer.playerID, targetVault);

            if (vault == null)
            {
                await context.ReplyKeyAsync("Vault_Fail_CannotLoad", targetVault);
                return;
            }

            await vault.OpenForPlayerAsync(context.LDMPlayer);
        }

        private new SherbetVaultsPlugin Plugin =>
            base.Plugin as SherbetVaultsPlugin;
    }
}