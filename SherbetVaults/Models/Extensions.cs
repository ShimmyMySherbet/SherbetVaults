using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using RocketExtensions.Models;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using SDG.Unturned;

namespace SherbetVaults.Models
{
    public static class Extensions
    {
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
    }
}