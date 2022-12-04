using Rocket.API.Collections;

namespace SherbetVaults
{
    public partial class SherbetVaultsPlugin
    {
        public override TranslationList DefaultTranslations => new()
        {
            { "Vault_Fail_NotFound", "[color=red]Failed to find a vault by that ID[/color]" },
            { "Vault_Fail_NoPermission", "[color=red]You do not have permission to access vault {0}[/color]"},
            { "Vault_Fail_CannotLoad", "[color=red]Vault {0} is currently unavailable[/color]" },
            { "Vaults_No_Vaults", "[color=yellow]You don't have access to any vaults[/color]" },
            { "Vaults_List", "[color=green]Your vaults: {0}[/color]" },
            { "WipeVault_Wiped", "[color=green]Wiped {0} items from {1}'s vault {2}[/color]" },
            { "VaultAliases_MaxReached", "[color=red]Max vault aliases reached[/color]" },
            { "VaultAliases_Set", "[color=cyan]Vault alias created: {0}➔{1}[/color]" },
            { "VaultAliases_Removed", "[color=cyan]Removed alias {0}[/color]" },
            { "VaultAliases_Remove_NotFound", "[color=cyan]No alias by that name found[/color]" },
            { "VaultAliases_List", "Aliases: {1}" },
            { "Restrictions_Blacklisted", "[color=red]You cannot store that item in your vault[/color]" },
            { "VaultAliases_Disabled", "[color=red]Vault aliases are disabled on this server[/color]" }
        };
    }
}