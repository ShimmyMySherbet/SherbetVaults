# Vault Aliases
Allows players to set an alias for a vault. Similar to renaming a vault.
This allows players to customize their vaults more.
E.g., creating a vault alias for 'MVP' named 'Guns'. So, using `/Vault Guns` opens their 'MVP' vault.
This feature is disabled by default, to enable it, set `VaultAliasesEnabled` in the config to True.

For a player to use a previously set vault alias, the permission `SherbetVaults.Vault.Alias` needs to be given. And for a player to manage their aliases, the `/VaultAlias` command needs to be granted.

Max aliases can be granted with the permission `SherbetVaults.MaxAliases.XXX`, where XXX is the max number of aliases. By default, players do not have a max number of aliases.  

If a player sets aliases and then loses the `SherbetVaults.Vault.Alias` permission, they will lose the ability to use the aliases, but the aliases won't be deleted. If they regain the permission, they can continue to use their previously set aliases.

This feature has been designed to be suitable as a donation rank feature, as a purely QOL feature.