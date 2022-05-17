using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SherbetVaults.Models;

namespace SherbetVaults.Commands
{
    [CommandName("VaultAlias")]
    [CommandInfo("Manages vault aliases", Syntax: "[Set/Remove/List] (vault) (alias)")]
    [AllowedCaller(AllowedCaller.Player)]
    public class VaultAliasCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            if (!Plugin.Config.VaultAliasesEnabled || Plugin.Database.Aliases == null)
            {
                await context.ReplyKeyAsync("VaultAliases_Disabled");
                return;
            }

            var option = context.Arguments.Get<string>(0, paramName: "Option").ToLower();

            switch (option)
            {
                case "set":
                    var vaultID = context.Arguments.Get<string>(1, paramName: "VaultName");
                    var vaultAlias = context.Arguments.Get<string>(2, paramName: "Alias");

                    var vaultConfig = Plugin.VaultSelector.GetVaultConfig(vaultID);
                    if (vaultConfig == null)
                    {
                        await context.ReplyKeyAsync("Vault_Fail_NotFound", vaultID);
                        return;
                    }

                    if (!vaultConfig.HasPermission(context.LDMPlayer))
                    {
                        await context.ReplyKeyAsync("Vault_Fail_NoPermission", vaultID);
                        return;
                    }

                    var currentAliases = (await Plugin.Database.Aliases.GetAliasesAsync(context.PlayerID))
                        .Select(x => x.Alias)
                        .ToArray();

                    if (!currentAliases.Contains(vaultAlias, StringComparer.InvariantCultureIgnoreCase))
                    {
                        var aliasMax = context.LDMPlayer.GetMaxAliases();

                        if (aliasMax >= 0 && currentAliases.Length >= aliasMax)
                        {
                            await context.ReplyKeyAsync("VaultAliases_MaxReached", aliasMax);
                            return;
                        }
                    }

                    await Plugin.Database.Aliases.SetAliasAsync(context.PlayerID, vaultID, vaultAlias);

                    await context.ReplyKeyAsync("VaultAliases_Set", Aliases, vaultConfig.VaultID);
                    return;

                case "remove":
                    var removeAlias = context.Arguments.Get<string>(1, paramName: "Alias");

                    var removed = await Plugin.Database.Aliases.DeleteAliasAsync(context.PlayerID, removeAlias);

                    if (removed)
                    {
                        await context.ReplyKeyAsync("VaultAliases_Removed", removeAlias);
                    }
                    else
                    {
                        await context.ReplyKeyAsync("VaultAliases_Remove_NotFound", removeAlias);
                    }
                    return;

                case "list":

                    var aliases = await Plugin.Database.Aliases.GetAliasesAsync(context.PlayerID);

                    var namesOnly = string.Join(", ", aliases.Select(x => x.Alias));
                    var withVault = string.Join(", ", aliases.Select(x => $"{x.Alias} ({x.VaultID})"));

                    await context.ReplyKeyAsync("VaultAliases_List", namesOnly, withVault);

                    return;

                default:
                    await context.ReplyAsync($"Usage: [color=cyan]/{Name} {Syntax}[/color]");
                    return;
            }
        }

        private new SherbetVaultsPlugin Plugin =>
            base.Plugin as SherbetVaultsPlugin;
    }
}