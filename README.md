# SherbetVaults
A lag-free reliable vault plugin for Unturned. 

Enjoy vaults hassle free and without random wipes or losses of data, or server stutters when opening a vault.

Full translation support, with easy rich text support.

## Commands
### /Vault *{Vault}*
Opens one of your vaults.

### /Vaults
Lists what vaults you have access to.

### /Trash
Opens a 15x15 trash storage, to discard unwanted items.

### /SpyVault [Player Name/ID] [Vault]
Opens another player's vault, allowing staff members to search and modify the contents of vaults.

### /WipeVault [Player Name/ID] [Vault]
Wipes the contents of another player's vault. Also says how many items were deleted.

## Configuration

### Database Settings
The MySQL settings for the plugin to use.

### Vaults
Sets the vault types, their permissions, and size.

`$VaultID` acts as a short-cut to the vault's ID.

### Cache Vaults
Specifies if the plugin should cache each player's vault fur the duration of their play-session.

Provides a minor speed boost to subsequent uses of the `/vault` command.

This can be disabled to allow for external systems to modify a player's vault while they are playing.

### Largest Vault Is Default
When enabled, sets the default vault for a player to the largest one they have access to.

When disabled, the default vault is the one named 'default'.


### Default Config
```xml
<?xml version="1.0" encoding="utf-8"?>
<SherbetVaultsConfig xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <DatabaseSettings>
    <DatabaseAddress>127.0.0.1</DatabaseAddress>
    <DatabaseUsername>Username</DatabaseUsername>
    <DatabasePassword>SuperSecretPassword</DatabasePassword>
    <DatabaseName>Unturned</DatabaseName>
    <DatabasePort>3306</DatabasePort>
  </DatabaseSettings>
  <Vaults>
    <Vault VaultID="default" Permission="Vaults.$VaultID" Width="8" Height="8" />
    <Vault VaultID="vip" Permission="Vaults.$VaultID" Width="12" Height="12" />
  </Vaults>
  <CacheVaults>true</CacheVaults>
  <LargestVaultIsDefault>false</LargestVaultIsDefault>
</SherbetVaultsConfig>
```

## Download
Downloads can be found in the <a href="https://github.com/ShimmyMySherbet/SherbetVaults/releases/">Releases</a> page.
