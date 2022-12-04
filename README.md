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

### /VaultAlias [list/set/remove] (Vault) (Alias)
Allows players to view, set, and delete aliases for vaults.

Aliases do not create new vaults, but creates a name that redirects to an already existing vault.

For more info on vault aliases, see <a href="https://github.com/ShimmyMySherbet/SherbetVaults/blob/master/VaultAliases.md">Vault Aliases</a>

## Configuration

### Database Settings
The MySQL settings for the plugin to use.

### Vaults
Sets the vault types, their permissions, and size.

`$VaultID` acts as a short-cut to the vault's ID.

### Largest Vault Is Default
When enabled, sets the default vault for a player to the largest one they have access to.

When disabled, the default vault is the one named 'default'.

### Database Table Prefix
Allows the name of the plugin's database tables to be renamed. This allows for multiple servers to store different vaults in the same database.

So you can change this value if you have 2 or more servers using the same database, and you don't want vaults to be synced across them.

### Item Restrictions
This plugin comes with a powerful form of item restrictions. This system provides many different item selectors, weights, whitelisting, blacklisting, custom messages, ect.

See <a href="https://github.com/ShimmyMySherbet/SherbetVaults/blob/master/ItemRestrictions.md">Item Restrictions</a> for more info and documentation.

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
  <LargestVaultIsDefault>true</LargestVaultIsDefault>
  <DefaultVault>default</DefaultVault>
  <VaultAliasesEnabled>false</VaultAliasesEnabled>
  <DatabaseTablePrefix>SherbetVaults</DatabaseTablePrefix>
  <Vaults>
    <Vault VaultID="default" Permission="Vaults.$VaultID" Width="8" Height="8" />
    <Vault VaultID="vip" Permission="Vaults.$VaultID" Width="12" Height="12" />
  </Vaults>
  <Restrictions>
    <AdminsBypassRestrictions>false</AdminsBypassRestrictions>
    <ShowMessages>true</ShowMessages>
    <Enabled>false</Enabled>
    <Groups>
      <RestrictionGroup GroupID="Group1" Weight="1" Blacklist="true">
        <TranslationKey>Restrictions_Blacklisted</TranslationKey>
        <Selectors>
          <ItemSelector>1</ItemSelector>
          <ItemSelector>255-259</ItemSelector>
          <ItemSelector>Type:Throwable</ItemSelector>
          <ItemSelector>Table:Police*</ItemSelector>
          <ItemSelector>Slot:Primary</ItemSelector>
          <ItemSelector>Workshop:2136497468</ItemSelector>
        </Selectors>
      </RestrictionGroup>
    </Groups>
  </Restrictions>
</SherbetVaultsConfig>
```

## Download
Downloads can be found in the <a href="https://github.com/ShimmyMySherbet/SherbetVaults/releases/">Releases</a> page.

## Donate
I work on the project free of charge for everyone to use. Though, if your feeling generous, consider <a href="https://ko-fi.com/ShimmyMySherbet">buying me a coffee</a>
