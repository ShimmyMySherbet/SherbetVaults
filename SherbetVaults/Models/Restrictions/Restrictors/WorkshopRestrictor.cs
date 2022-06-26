using System.IO;
using System.Linq;
using SDG.Unturned;

namespace SherbetVaults.Models.Restrictions.Restrictors
{
    [Selector(@"^Workshop:.*")]
    public class WorkshopRestrictor : IItemRestrictor
    {
        public uint WorkshopID { get; }

        public bool IsMatch(ItemAsset asset) => GetWorkshopID(asset.absoluteOriginFilePath) == WorkshopID;

        private uint GetWorkshopID(string path)
        {
            var parts = path.Split(Path.DirectorySeparatorChar).ToList();

            var contentIndex = parts.LastIndexOf("content");
            var workshopFolderIndex = contentIndex + 1;
            if (parts.Count < workshopFolderIndex + 1)
            {
                return 0;
            }

            var workshopFolderName = parts[workshopFolderIndex];

            if (uint.TryParse(workshopFolderName, out var workshopID))
            {
                return workshopID;
            }

            return 0;
        }

        public WorkshopRestrictor(string selector)
        {
            var id = selector.Substring(9);
            if (!uint.TryParse(id, out var workshopID))
                throw new BadSelectorException($"Workshop: Invalid workshop ID: {id}");
            WorkshopID = workshopID;
        }
    }
}