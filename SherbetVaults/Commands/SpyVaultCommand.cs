using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Player;
using RocketExtensions.Models;
using RocketExtensions.Plugins;

namespace SherbetVaults.Commands
{
    [CommandName("SpyVault")]
    [CommandInfo("Opens another player's vault", Syntax: "[Target Player] [Vault ID]")]
    [AllowedCaller(AllowedCaller.Player)]
    public class SpyVaultCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            var playerID = context.Arguments.Get(0, 0ul);

            if (playerID == 0)
            {
                playerID = context.Arguments.Get<UnturnedPlayer>(0, paramName: "Target Player").CSteamID.m_SteamID;
            }

            var targetVault = context.Arguments.Get(1, "default", paramName: "VaultID");

            var vaultConfig = Plugin.GetVaultConfig(targetVault);

            if (vaultConfig == null)
            {
                await context.ReplyKeyAsync("Vault_Fail_NotFound", targetVault);
                return;
            }

            var vault = await Plugin.VaultManager.GetVault(playerID, targetVault);

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