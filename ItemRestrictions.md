# Item Restrictions
SherbetVaults now comes with a rather powerful item whitelist/blacklist system. You can create groups that act as a whitelist or blacklist, and assign them to players using permissions.

Restriction groups can be assigned to players with the `SherbetVaults.Restrict.Group` where `Group` is the GroupID. By default any restriction groups you create will not have an effect, you must assign players the permission.

You can also assign different messages to be sent to the player when they try to store a blacklisted item in their vault. This can be done by changing the `TranslationKey` setting of the group and then adding a message for it to the translations file.

Groups can have a weight assigned to them. The greater the weight the earlier it is applied. This means you can create a restriction group for e.g., Lead Staff that whitelists certain items that are blacklisted globally.

Rather than just listing item IDs, you can use item selectors. As of this update, the following selectors are available:
* ### **Single Item**
     Format: _ItemID_
     Example: `15`
     Specifies a single item ID
* ### **Item Range**
   Format: _ItemID_-_ItemID_
   Example: `288-295`
   Specifies a range of items, inclusive.
* ### **Item Slot**
    Format: Slot:_ItemEquipSlot_
    Example: `Slot:Primary`
    Specifies items based on their equitable slot. E.g., `Primary` selects primary weapons.
    Available Options: None _(Cannot be equipped)_, Primary, Secondary, Tertiary _(equipable items, e.g., food, binoculars)_
* ### **Item Type**
    Format: Type:_ItemType_
    Examples: `Type:Gun`, `Type:Medical`, `Type:Food`
    Specifies items based on their type. E.g., `Guns` selects all guns, `Food` selects food items.
    Small Selection of options: _Clothing Slots, e.g., Hat_, Gun,  Food, Water, Trap, Melee, Magazine, _Gun Attachment slots, e.g., Optic_, Structure, and a lot more. A wiki page will be made for this.
* ### **Item Spawn Table**
   Formats: Table:_TableName_, Table:_TableID_
   Examples: `Table:Police*`, `Table:22`
   Specifies an items based on what spawn tables they appear in. Spawn tables are based on the map.
   When using a table name, specifying `Police` matches tables that are exactly named 'Police', so 'Police_Guns' wouldn't be matched.
   The * Symbol matches anything, So `Police*` would match 'Police' as well as 'Police_Guns'
   You can see a list of spawn tables for official maps <a href="https://steamcommunity.com/sharedfiles/filedetails/?id=797414542">Here</a>
* ### **Item Workshop**
   Format: Workshop:_WorkshopID_
   Example: `Workshop:2136497468`
   Specifies items that originate from a workshop mod. E.g., `Workshop:2136497468` specifies Elver Items, since '2136497468' is Elver's workshop ID. These are the same workshop IDs you use in your server's WorkshopDownloadConfig.json


## Default Restriction Config


```xml
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
```

By default item restrictions are disabled. So they need to be enabled, and restriction groups assigned to players before it will work.

