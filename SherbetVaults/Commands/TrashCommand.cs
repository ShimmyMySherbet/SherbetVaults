using Cysharp.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SherbetVaults.Models;

namespace SherbetVaults.Commands
{
    [CommandName("Trash")]
    [CommandInfo("Opens a trash can")]
    [AllowedCaller(AllowedCaller.Player)]
    public class TrashCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            await context.LDMPlayer.OpenTrashAsync();
        }
    }
}