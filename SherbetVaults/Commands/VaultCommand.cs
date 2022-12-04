using Cysharp.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SherbetVaults.Models;
using SherbetVaults.Models.Enums;

namespace SherbetVaults.Commands
{
    [CommandName("Vault")]
    [CommandInfo("Opens a vault", Syntax: "[Vault Name]")]
    [AllowedCaller(AllowedCaller.Player)]
    public class VaultCommand : RocketCommand
    {
        private VaultPlayerLock PlayerLock { get; } = new VaultPlayerLock();

        public override async UniTask Execute(CommandContext context)
        {
            using (PlayerLock.TryObtainLock(context.PlayerID, out var valid))
            {
                if (!valid)
                {
                    // This player is already opening a vault.
                    return;
                }

                var targetVault = context.Arguments.Get(0, defaultValue: string.Empty, paramName: "Vault Name");

                var (vaultConfig, availability) = await Plugin.VaultSelector.GetVault(context.LDMPlayer, targetVault);

                switch (availability)
                {
                    case EVaultAvailability.BadVaultID:
                        await context.ReplyKeyAsync("Vault_Fail_NotFound", targetVault);
                        return;

                    case EVaultAvailability.NotAllowed:
                        await context.ReplyKeyAsync("Vault_Fail_NoPermission", targetVault);
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

                var vault = await Plugin.VaultManager.GetVault(context.PlayerID, vaultConfig.VaultID);

                if (vault == null)
                {
                    await context.ReplyKeyAsync("Vault_Fail_CannotLoad", vaultConfig.VaultID);
                    return;
                }

                await vault.OpenForPlayerAsync(context.LDMPlayer);
            }
        }

        private new SherbetVaultsPlugin Plugin =>
            base.Plugin as SherbetVaultsPlugin;
    }
}