using System.Linq;
using Cysharp.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using RocketExtensions.Plugins;

namespace SherbetVaults.Commands
{
    [CommandName("Vaults")]
    [CommandInfo("Lists your available vaults")]
    [AllowedCaller(AllowedCaller.Player)]
    public class VaultsCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            var vaults = Plugin.VaultSelector.GetPlayerVaults(context.LDMPlayer);

            if (vaults.Length == 0)
            {
                await context.ReplyKeyAsync("Vaults_No_Vaults");
                return;
            }

            var vaultNames = string.Join(", ", vaults.Select(x => x.VaultID));

            await context.ReplyKeyAsync("Vaults_List", vaultNames, vaults.Length);
        }

        private new SherbetVaultsPlugin Plugin =>
            base.Plugin as SherbetVaultsPlugin;
    }
}