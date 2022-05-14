using Cysharp.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SherbetVaults.Models;

namespace SherbetVaults.Commands
{
    [CommandName("Vault")]
    [CommandInfo("Opens a vault", Syntax: "[Vault Name]")]
    [AllowedCaller(AllowedCaller.Player)]
    public class VaultCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            var targetVault = context.Arguments.Get(0, defaultValue: string.Empty, paramName: "Vault Name");

            var vaultConfig = Plugin.GetDefaultVault(context.LDMPlayer, out var availability, targetVault);

            switch (availability)
            {
                case EVaultAvailability.BadVaultID:
                    await context.ReplyKeyAsync("Vault_Fail_NotFound", targetVault);
                    return;

                case EVaultAvailability.NotAllowed:
                    await context.ReplyKeyAsync("Vault_Fail_NoPermission", vaultConfig.VaultID);
                    return;

                case EVaultAvailability.NoVaults:
                case EVaultAvailability.NoAllowedVaults:
                    await context.ReplyKeyAsync("Vaults_No_Vaults");
                    return;
            }

            if (vaultConfig == null)
            {
                await context.ReplyKeyAsync("Vault_Fail_CannotLoad", targetVault);
                return;
            }

            var vault = await Plugin.VaultManager.GetVault(context.PlayerID, targetVault);

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