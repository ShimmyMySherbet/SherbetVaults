using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Cysharp.Threading.Tasks;
using Rocket.Core;
using RocketExtensions.Models;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using SDG.Unturned;

namespace SherbetVaults.Models
{
    public static class Extensions
    {
        public static readonly Regex MaxAliasMatch = new Regex(@"^SherbetVaults\.MaxAliases\.[0-9]*$");
        public static readonly Regex MaxAliasValueMatch = new Regex(@"[0-9]*$");
        public static async Task OpenTrashAsync(this LDMPlayer player)
        {
            var items = new Items(7);
            items.resize(15, 15);

            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                player.Player.inventory.updateItems(7, items);
                player.Player.inventory.sendStorage();
            });
        }

        public static async Task<string> GetPlayerName(this ulong player)
        {
            var request = WebRequest.CreateHttp($"http://steamcommunity.com/profiles/{player}?xml=1");
            request.Method = "GET";
            using (var response = await request.GetResponseAsync())
            using (var network = response.GetResponseStream())
            using (var reader = new StreamReader(network))
            {
                var xml = new XmlDocument();
                xml.LoadXml(await reader.ReadToEndAsync());
                return xml["profile"]?["steamID"]?.InnerText ?? "Unknown Player";
            }
        }

        public static int GetMaxAliases(this LDMPlayer player)
        {
            var permissions = R.Permissions.GetPermissions(player.UnturnedPlayer);
            var weights = permissions.Where(x => MaxAliasMatch.IsMatch(x.Name))
                                     .Select(x => MaxAliasValueMatch.Match(x.Name))
                                     .Where(x => x.Success)
                                     .Select(x => int.Parse(x.Value))
                                     .OrderByDescending(x => x)
                                     .ToArray();
            if (weights.Length == 0)
            {
                return -1;
            }
            return weights[0];
        }
    }
}