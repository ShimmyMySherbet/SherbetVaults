using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using RocketExtensions.Models;
using SDG.Unturned;
using SherbetVaults.Models.Config.Restrictions;
using SherbetVaults.Models.Restrictions;

namespace SherbetVaults.Models
{
    public class RestrictionTool
    {
        public SherbetVaultsPlugin Plugin { get; }

        public RestrictionTool(SherbetVaultsPlugin plugin)
        {
            Plugin = plugin;
        }

        public bool IsPermitted(ushort itemID, LDMPlayer player, out RestrictionGroup matchedGroup)
        {
            var settings = Plugin.Config.Restrictions;
            matchedGroup = null;

            if (!settings.Enabled)
            {
                return true;
            }

            if (settings.AdminsBypassRestrictions && player.IsAdmin)
            {
                return true;
            }

            var asset = (ItemAsset)Assets.find(EAssetType.ITEM, itemID);

            if (asset == null)
            {
                return false;
            }

            var groups = GetPlayerGroups(player);

            foreach (var group in groups)
            {
                var isMatch = group.Restrictors.Any(x => x.IsMatch(asset));
                if (isMatch)
                {
                    matchedGroup = group;
                    return !group.Blacklist;
                }
            }

            return true;
        }

        private List<VaultRestrictionGroup> GetPlayerGroups(LDMPlayer player)
        {
            var explicitPermissions = player.UnturnedPlayer.GetPermissions().Select(x => x.Name).ToArray();
            return Plugin.RestrictionGroups
                .Where(x => explicitPermissions.Contains($"SherbetVaults.Restrict.{x.GroupID}"))
                .OrderByDescending(x => x.Weight)
                .ToList();
        }
    }
}